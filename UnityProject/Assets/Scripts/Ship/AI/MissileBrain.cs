using System.Collections;
using System.Collections.Generic;
using MeEngine.Events;
using UnityEngine;

namespace HotJupiter
{
	public class MissileBrain : BaseBrain<MissileGamePiece>
	{
		void Awake()
		{
			GameControllerFsm.eventPublisher.SubscribeAll(this);
		}

		public override BaseGamePiece FindTarget()
		{
			List<ShipGamePiece> allShips = ShipManager.GetAllShips();

			//Next Turn Hex
			TileWithFacing startVec = myGamePiece.currentTile;
			TileWithFacing headingTile = startVec.TraversePlanar(
				HexDirection.Forward,
				myGamePiece.currentVelocity
			);

			//Find the ship closest to where I will be if I move forward. Exclude the ship that fired me.
			ShipGamePiece closestShip = null;
			float closestShipDistance = float.MaxValue;

			foreach (ShipGamePiece ship in allShips)
			{
				if (ship == myGamePiece.motherGamePiece)
					continue;

				float distance = HexMapHelper.CrowFlyDistance(
					new Tile(headingTile.position, headingTile.level),
					new Tile(ship.currentTile.position, ship.currentTile.level)
				);
				if (distance < closestShipDistance)
				{
					closestShipDistance = distance;
					closestShip = ship;
				}
			}

			currentTarget = closestShip;
			Debug.DrawLine(
				HexMapHelper.GetWorldPointFromTile(myGamePiece.currentTile.position),
				HexMapHelper.GetWorldPointFromTile(currentTarget.currentTile.position),
				Color.yellow,
				5f
			);
			return currentTarget;
		}

		public override CommandPointController SelectCommand()
		{
			var availableCommands = pieceController.navigationSystem.GetAvailableCommandPoints();

			if (currentTarget != null)
			{
				// Missiles know where their target will be
				TileWithFacing targetDestinationTile = currentTarget.GetDestinationTile();
				Vector3 targetDestinationWorldSpace = HexMapHelper.GetWorldPointFromTile(
					targetDestinationTile.position,
					targetDestinationTile.level
				);

				CommandPointController closestPoint = null;
				float closestPointDist = float.MaxValue;

				foreach (CommandPointController point in availableCommands)
				{
					if (
						point.model.destinationTile.position == targetDestinationTile.position
						&& point.model.destinationTile.level == targetDestinationTile.level
					)
						return point;

					float distance = Vector3.Distance(
						targetDestinationWorldSpace,
						HexMapHelper.GetWorldPointFromTile(
							point.model.destinationTile.position,
							point.model.destinationTile.level
						)
					);
					if (distance < closestPointDist)
					{
						closestPoint = point;
						closestPointDist = distance;
					}
				}

				return closestPoint;
			}
			else
			{
				return base.SelectCommand();
			}
		}

		[EventListener]
		void OnNewTurn(GameControllerFsm.Events.BeginCommandSelectionState @event)
		{
			FindTarget();
		}

		[EventListener]
		void OnEndOfTurn(GameControllerFsm.Events.BeginIntentDeclarationState @event)
		{
			Debug.Log("Missile Selecing Command");
			pieceController.SetSelectedCommandPoint(SelectCommand());
		}
	}
}
