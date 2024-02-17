using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using MeEngine.Events;
using UnityEngine;

namespace HotJupiter
{
	public class PieceController : MonoBehaviour, IHaveTilePosition, IHaveTileFacing
	{
		public BaseManuStats pieceTemplate;

		public NavigatingGamePiece gamePiece;

		private CommandPointController selectedCommandPoint;

		public NavigationSystem navigationSystem;

		public GameObject worldModel;
		public GameObject worldBase;

		private BGCurve activeWorldPath;

		public TileCoords GetPivotTilePosition()
		{
			return gamePiece.currentTile.position;
		}

		public RelativeFootprintTemplate GetFootprint()
		{
			return pieceTemplate.footprint;
		}

		public TileLevel GetPivotTileLevel()
		{
			return gamePiece.currentTile.level;
		}

		public TileCoords GetTileFacing()
		{
			return gamePiece.currentTile.facing;
		}

		void Awake()
		{
			GameControllerFsm.eventPublisher.SubscribeAll(this);
			navigationSystem.eventPublisher.SubscribeAll(this);
			gamePiece.eventPublisher.SubscribeAll(this);

			GameObject worldObject = GameObject.Instantiate(
				pieceTemplate.model,
				worldModel.transform,
				false
			);
			worldObject.transform.localPosition = Vector3.zero;
			worldObject.transform.localScale = Vector3.one;
			worldObject.transform.localRotation = Quaternion.identity;
		}

		public void SetSelectedCommandPoint(CommandPointController point)
		{
			selectedCommandPoint = point;
			gamePiece.SetDestination(selectedCommandPoint.model.destinationTile);
			gamePiece.currentVelocity = selectedCommandPoint.model.endVelocity;
			Debug.DrawLine(
				HexMapHelper.GetWorldPointFromTile(gamePiece.currentTile.position),
				HexMapHelper.GetWorldPointFromTile(
					selectedCommandPoint.model.destinationTile.position
				),
				Color.cyan,
				5f
			);
		}

		public void SetActivePath(BGCurve path)
		{
			activeWorldPath = path;
		}

		// Update is called once per frame
		void Update()
		{
			if (activeWorldPath != null)
			{
				BGCcMath math = activeWorldPath.GetComponent<BGCcMath>();

				Vector3 tangent;
				worldBase.transform.position = math.CalcPositionAndTangentByDistanceRatio(
					TimeManager.TurnTimeNormalized,
					out tangent
				);
				worldModel.transform.rotation = Quaternion.LookRotation(
					tangent,
					worldBase.transform.position
				);
			}
		}

		void ResetToGamePiecePosition()
		{
			worldBase.transform.position = HexMapHelper.GetWorldPointFromTile(
				gamePiece.currentTile.position,
				gamePiece.currentTile.level
			);
			worldModel.transform.rotation = HexMapHelper.GetRotationFromFacing(
				gamePiece.currentTile.position,
				gamePiece.currentTile.facing
			);
		}

		[EventListener]
		void OnNewPointSelected(NavigationSystem.Events.NewPointSelected @event)
		{
			SetSelectedCommandPoint(@event.SelectedPoint);
		}

		[EventListener]
		void OnGamePieceCompletedSetup(BaseGamePiece.Events.CompletedSetup @event)
		{
			ResetToGamePiecePosition();
		}

		[EventListener]
		void OnStartTurn(GameControllerFsm.Events.BeginCommandSelectionState @event)
		{
			ResetToGamePiecePosition();
		}

		[EventListener]
		void OnStartPlayingOutTurn(GameControllerFsm.Events.BeginPlayingOutTurnState @event)
		{
			SetActivePath(selectedCommandPoint.model.spline);
		}
	}
}
