using MeEngine.Events;
using MeEngine.FsmManagement;
using UnityEngine;

namespace HotJupiter
{
	public partial class CommandPointViewFsm : MeFsm
	{
		public static class Events
		{
			public struct CommandPointClicked : IEvent
			{
				public CommandPointController CommandPoint;
			}
		}

		public EventPublisher eventPublisher = new EventPublisher();

		public GameObject spriteHolder;
		public SpriteRenderer spriteRenderer;
		public CommandPointModel model;
		public CommandPointController controller;

		public PathingMaterialScheme materialScheme;

		protected override void Start()
		{
			base.Start();
		}

		[EventListener]
		void OnDestinationSet(CommandPointController.Events.DestinationSet @event)
		{
			Debug.Log("View recieved destination set event");
			//Position the sprite within the Hex
			spriteHolder.transform.position =
				HexMapHelper.GetWorldPointFromTile(
					model.destinationTile.position,
					model.destinationTile.level
				)
				+ (
					(
						HexMapHelper.GetFacingVector(
							model.destinationTile.position,
							model.destinationTile.facing
						)
						* HexMapHelper.HexWidth
						* 0.3f
					)
				);

			//Face the sprite the correct direction
			spriteHolder.transform.rotation = HexMapHelper.GetRotationFromFacing(
				model.destinationTile.position,
				model.destinationTile.facing
			);

			//Color the sprite based on height
			spriteRenderer.color = HexMapUI.GetLevelColor(model.destinationTile.level);

			//Is any point on path colliding with a static object?
			bool collisionEncountered = false;
			foreach (var node in model.tilePath.GetTilesInPath())
			{
				if (
					PlayfieldManager.GetTileObstacleTypeAtTime((Tile)node, 0)
					== TileObstacleType.Solid
				)
				{ //TODO: Maybe use path time to check this instead of time 0
					collisionEncountered = true;
					Debug.Log("Collision Encountered");
					break;
				}
			}

			PathIndicatorType pathIndicator = PathIndicatorType.Selected;

			if (collisionEncountered)
			{
				pathIndicator = PathIndicatorType.Collision;
			}
			else
			{
				if (model.gForce <= 0)
				{
					pathIndicator = PathIndicatorType.Selected;
				}
				else if (model.gForce == 1)
				{
					pathIndicator = PathIndicatorType.G1;
				}
				else if (model.gForce == 2)
				{
					pathIndicator = PathIndicatorType.G2;
				}
				else if (model.gForce >= 3)
				{
					pathIndicator = PathIndicatorType.G3;
				}
			}

			model.spline.GetComponent<LineRenderer>().material =
				materialScheme.GetMaterialFromIndicator(pathIndicator);
		}

		[EventListener]
		void OnNewPointSelected(NavigationSystem.Events.NewPointSelected @event)
		{
			if (@event.SelectedPoint == controller)
			{
				SwapState<SelectedState>();
			}
			else
			{
				SwapState<WaitingState>();
			}
		}
	}
}
