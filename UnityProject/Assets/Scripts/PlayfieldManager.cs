using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotJupiter
{
	public class PlayfieldManager : MonoBehaviour
	{
		//Path Conflict Resolution Order
		//Sort pieces in order of slowest to fastest, (then biggest to smallest?, then what?)
		//Piece places reservations for all tiles on path (semi-solid, or solid)
		//* If a reservation already exists
		//* * Inventory surounding tiles for those without reservations
		//* * Randomly select free tile in place of planned path tile
		//* * Evasive action has taken place. Add one G-Force to manuever
		//* * Continue reserving tiles in path

		//TODO: Open Question. How do we respond to an event and still place reservations in a specific order?


		/*public interface IPathConflictResolver{
		    IPathTraveler DeterminePrevailingClaimant(List<IPathTraveler> claimants, Tile contestedTile);
		    bool IsAnyConflict(List<IPathTraveler> claimants, Tile contestedTile);
		}*/

		/*public interface IPathTraveler: IHaveTilePosition, IHaveTileFacing, IHaveTileFootprint {
		    //bool HasConflictOverTile(List<IPathTraveler> otherClaimants, Tile contestedTile);
		    void YieldReservation(IPathTraveler prevailingObject, Tile contestedTile, float simulationTime);
		    //void OnPrevailingReservation(List<IPathTraveler> yieldingObjects, Tile contestedTile);
		}*/

		public struct SimulationTimePeriod
		{
			public float start; //inclusive
			public float end; //exclusive
			float duration
			{
				get { return end - start; }
			}

			public SimulationTimePeriod(float start, float end)
			{
				this.start = start;
				if (end < start)
					throw new System.ArgumentException(
						"Provided end time must be greater than start time."
					);
				this.end = end;
			}

			public bool WithinPeriod(float time)
			{
				return this.start <= time && time < this.end;
			}

			public bool HasOverlap(SimulationTimePeriod otherTimePeriod)
			{
				return this.start < otherTimePeriod.end && otherTimePeriod.start < this.end;
			}

			public override bool Equals(object obj)
			{
				if (!(obj is SimulationTimePeriod))
					return false;

				SimulationTimePeriod otherStruct = (SimulationTimePeriod)obj;
				return this.start == otherStruct.start && this.end == otherStruct.end;
			}

			public override int GetHashCode()
			{
				return start.GetHashCode().WrapShift(2) ^ end.GetHashCode();
			}
		}

		public struct TileReservation
		{
			public IHaveTileFootprint reservationHolder;
			public SimulationTimePeriod reservationPeriod;
			public TileObstacleType obstacleType;

			public override bool Equals(object obj)
			{
				if (!(obj is TileReservation))
					return false;

				TileReservation otherStruct = (TileReservation)obj;

				return this.reservationHolder == otherStruct.reservationHolder
					&& this.reservationPeriod.Equals(otherStruct.reservationPeriod);
			}

			public override int GetHashCode()
			{
				return reservationHolder.GetHashCode().WrapShift(2)
					^ reservationPeriod.GetHashCode();
			}
		}

		public static PlayfieldManager instance;

		private Dictionary<Tile, HashSet<TileReservation>> tileReservations =
			new Dictionary<Tile, HashSet<TileReservation>>();
		private Dictionary<
			IHaveTileFootprint,
			HashSet<HashSet<TileReservation>>
		> reservationHolderMap =
			new Dictionary<IHaveTileFootprint, HashSet<HashSet<TileReservation>>>();

		private void Awake()
		{
			instance = this;
		}

		public static TileObstacleType GetTileObstacleTypeAtTime(Tile tile, float simulationTime)
		{
			HashSet<TileReservation> tileReservations;
			TileObstacleType foundObstacle = TileObstacleType.Empty;
			if (instance.tileReservations.TryGetValue(tile, out tileReservations))
			{
				foreach (TileReservation reservation in tileReservations)
				{
					if (
						reservation.reservationPeriod.WithinPeriod(simulationTime)
						&& (int)reservation.obstacleType > (int)foundObstacle
					)
					{
						foundObstacle = reservation.obstacleType;
					}
				}
			}
			return foundObstacle;
		}

		public static List<IHaveTileFootprint> GetTileOccupantsAtTime(
			Tile tile,
			float simulationTime
		)
		{
			HashSet<TileReservation> tileReservations;
			List<IHaveTileFootprint> tileOccupants = new List<IHaveTileFootprint>();
			if (instance.tileReservations.TryGetValue(tile, out tileReservations))
			{
				foreach (TileReservation reservation in tileReservations)
				{
					if (reservation.reservationPeriod.WithinPeriod(simulationTime))
					{
						tileOccupants.Add(reservation.reservationHolder);
					}
				}
			}
			return tileOccupants;
		}

		public static bool TryReserveTile(
			FootprintTile tile,
			IHaveTileFootprint claimant,
			SimulationTimePeriod timePeriod
		)
		{
			HashSet<TileReservation> tileReservations;
			if (instance.tileReservations.TryGetValue(tile.tile, out tileReservations))
			{
				if (tileReservations == null)
					tileReservations = new HashSet<TileReservation>();
				if (tile.obstacleType != TileObstacleType.Empty)
				{
					foreach (TileReservation currentReservation in tileReservations)
					{
						if (currentReservation.reservationPeriod.HasOverlap(timePeriod))
						{ //There is an overlapping reservation
							if (currentReservation.obstacleType == TileObstacleType.Solid)
							{ //No passing through solids
								return false;
							}
							else if (
								currentReservation.obstacleType == TileObstacleType.Semisolid
								&& timePeriod.WithinPeriod(TimeManager.TurnDuration)
							)
							{ //No stopping at semi-solids at end of turn
								return false;
							}
						}
					}
				}
			}
			else
			{
				tileReservations = new HashSet<TileReservation>();
				instance.tileReservations.Add(tile.tile, tileReservations);
			}
			tileReservations.Add(
				new TileReservation()
				{
					reservationHolder = claimant,
					obstacleType = tile.obstacleType,
					reservationPeriod = timePeriod
				}
			);

			HashSet<HashSet<TileReservation>> reservationSets;
			if (instance.reservationHolderMap.TryGetValue(claimant, out reservationSets))
			{
				reservationSets.Add(tileReservations);
			}
			else
			{
				reservationSets = new HashSet<HashSet<TileReservation>>();
				reservationSets.Add(tileReservations);
				instance.reservationHolderMap.Add(claimant, reservationSets);
			}

			return true;
		}

		public static void RemoveTileReservations(Tile tile, IHaveTileFootprint reservationHolder)
		{
			HashSet<TileReservation> currentTileReservations;
			if (instance.tileReservations.TryGetValue(tile, out currentTileReservations))
			{
				int result = currentTileReservations.RemoveWhere(
					(x) => x.reservationHolder == reservationHolder
				);
				if (result == 0)
					Debug.LogWarning(
						$"Reservation Holder {reservationHolder} has no reservations on tile {tile}."
					);

				HashSet<HashSet<TileReservation>> reservationSets;
				if (
					instance.reservationHolderMap.TryGetValue(
						reservationHolder,
						out reservationSets
					)
				)
				{
					bool setResult = reservationSets.Remove(currentTileReservations);
					if (setResult == false)
						Debug.LogWarning(
							$"Reservation Holder {reservationHolder} is missing reservation on tile {tile}."
						);
					if (reservationSets.Count == 0)
					{
						instance.reservationHolderMap.Remove(reservationHolder);
					}
				}
				else
				{
					Debug.LogWarning(
						$"Reservation Holder {reservationHolder} has no reservations."
					);
				}
			}
			else
			{
				Debug.LogWarning($"Tile {tile} has no reservations.");
			}
		}

		public static void RemoveAllReservations(IHaveTileFootprint reservationHolder)
		{
			HashSet<HashSet<TileReservation>> tileReservationSets;
			if (
				instance.reservationHolderMap.TryGetValue(
					reservationHolder,
					out tileReservationSets
				)
			)
			{
				foreach (HashSet<TileReservation> set in tileReservationSets)
				{
					int result = set.RemoveWhere((x) => x.reservationHolder == reservationHolder);
					if (result == 0)
						Debug.LogWarning(
							$"Reservation Holder {reservationHolder} is missing a reservation."
						);
				}
				tileReservationSets.Clear();
				instance.reservationHolderMap.Remove(reservationHolder);
			}
			else
			{
				Debug.LogWarning(
					$"Reservation Holder {reservationHolder} has no active reservations."
				);
			}
		}
	}
}
