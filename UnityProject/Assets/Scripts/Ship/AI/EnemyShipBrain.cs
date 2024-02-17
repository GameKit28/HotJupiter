using System.Collections;
using System.Collections.Generic;
using MeEngine.Events;
using UnityEngine;

namespace HotJupiter
{
	public class EnemyShipBrain : BaseBrain<ShipGamePiece>
	{
		void Awake()
		{
			GameControllerFsm.eventPublisher.SubscribeAll(this);
		}

		public override BaseGamePiece FindTarget()
		{
			//Player Ship
			if (currentTarget != null)
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
			var availableCommandPoints =
				pieceController.navigationSystem.GetAvailableCommandPoints();
			var chosenCommandPoint = availableCommandPoints[
				Random.Range(0, availableCommandPoints.Count)
			];
			return chosenCommandPoint;
		}

		[EventListener]
		void OnNewTurn(GameControllerFsm.Events.BeginCommandSelectionState @event)
		{
			FindTarget();
			TileWithFacing startVec = myGamePiece.currentTile;
			TileCoords missileOkayZone = startVec
				.TraversePlanar(
					HexDirection.Forward,
					myGamePiece.shipTemplete.missileTemplate.TopSpeed
				)
				.position;
			if (
				HexMapHelper.CrowFlyDistance(
					new Tile(missileOkayZone, myGamePiece.currentTile.level),
					new Tile(currentTarget.currentTile.position, currentTarget.currentTile.level)
				) < 4f
			)
			{
				myGamePiece.QueueMissile(true);
			}
		}

		[EventListener]
		void OnNewTurnPost(GameControllerFsm.Events.BeginCommandSelectionStatePost @event)
		{
			pieceController.SetSelectedCommandPoint(SelectCommand());
		}
	}
}
