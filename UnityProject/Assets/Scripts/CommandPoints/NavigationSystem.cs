using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;

namespace HotJupiter{
    public class NavigationSystem: MonoBehaviour
    {
        public static class Events {
            public struct NewPointSelected : IEvent {public CommandPointController SelectedPoint; }
        }
        public EventPublisher eventPublisher = new EventPublisher();

        public GameObject commandPointPrefab;
        public PieceController pieceController;

        private List<CommandPointController> availableCommandPoints = new List<CommandPointController>();


        bool hasGeneratedThisTurn = false;

        void Awake(){
            GameControllerFsm.eventPublisher.SubscribeAll(this);
        }

        public void GenerateCommandPoints()
        {
            if (hasGeneratedThisTurn) return;

            int gForceModifier = 0;
            if(pieceController.gamePiece.currentVelocity == pieceController.pieceTemplate.TopSpeed) gForceModifier = 1;
            if(pieceController.gamePiece.currentVelocity == 2) gForceModifier = -1;

            //Standard Destinations

            //Forward Facing (current speed)
            //This is the default selected command point for players
            TileWithFacing startingVec = new TileWithFacing { position = pieceController.GetPivotTilePosition(), facing = pieceController.GetTileFacing(), level = pieceController.GetPivotTileLevel() };

            var defalutSelectedPoint = InstantiateCommandPoint(
                new TilePath(startingVec).TraversePlanar(HexDirection.Forward, pieceController.gamePiece.currentVelocity),
                pieceController.gamePiece.currentVelocity,
                0);

            //Forward Facing (speed up)
            if(pieceController.gamePiece.currentVelocity < pieceController.pieceTemplate.TopSpeed && pieceController.pieceTemplate.canAccelerate) {
                InstantiateCommandPoint(
                    new TilePath(startingVec).TraversePlanar(HexDirection.Forward, pieceController.gamePiece.currentVelocity + 1),
                    pieceController.gamePiece.currentVelocity + 1,
                    1);
            }

            //Forward Facing (slow down)
            if(pieceController.gamePiece.currentVelocity > 2 && pieceController.pieceTemplate.canDecelerate) {
                InstantiateCommandPoint(
                    new TilePath(startingVec).TraversePlanar(HexDirection.Forward, pieceController.gamePiece.currentVelocity - 1),
                    pieceController.gamePiece.currentVelocity - 1,
                    1);
            }

            //Turn Left (straight bank)
            InstantiateCommandPoint(
                new TilePath(startingVec).TraversePlanar(HexDirection.Forward, pieceController.gamePiece.currentVelocity).Face(HexDirection.ForwardLeft),
                pieceController.gamePiece.currentVelocity,
                0 + gForceModifier);


            //Turn Right (straight bank)
            InstantiateCommandPoint(
                new TilePath(startingVec).TraversePlanar(HexDirection.Forward, pieceController.gamePiece.currentVelocity).Face(HexDirection.ForwardRight),
                pieceController.gamePiece.currentVelocity,
                0 + gForceModifier);

            //Turn Radius
            for(int manu = 1; manu <= pieceController.pieceTemplate.Maneuverability; manu++){
                int baseGForce = manu - 1;
                if(pieceController.gamePiece.currentVelocity - manu >= 1) {
                    if(pieceController.pieceTemplate.canStrafe){
                        //Strafe Left
                        InstantiateCommandPoint(
                            new TilePath(startingVec).TraversePlanar(HexDirection.Forward, pieceController.gamePiece.currentVelocity - manu).TraversePlanar(HexDirection.ForwardLeft, manu).Face(HexDirection.ForwardRight),
                            pieceController.gamePiece.currentVelocity,
                            baseGForce + gForceModifier);

                        //Strafe Right
                        InstantiateCommandPoint(
                            new TilePath(startingVec).TraversePlanar(HexDirection.Forward, pieceController.gamePiece.currentVelocity - manu).TraversePlanar(HexDirection.ForwardRight, manu).Face(HexDirection.ForwardLeft),
                            pieceController.gamePiece.currentVelocity,
                            baseGForce + gForceModifier);
                    }

                    //Turn Left
                    InstantiateCommandPoint(
                        new TilePath(startingVec).TraversePlanar(HexDirection.Forward, pieceController.gamePiece.currentVelocity - manu).TraversePlanar(HexDirection.ForwardLeft, manu),
                        pieceController.gamePiece.currentVelocity,
                        baseGForce + gForceModifier);

                    //Turn Right
                    InstantiateCommandPoint(
                        new TilePath(startingVec).TraversePlanar(HexDirection.Forward, pieceController.gamePiece.currentVelocity - manu).TraversePlanar(HexDirection.ForwardRight, manu),
                        pieceController.gamePiece.currentVelocity,
                        baseGForce + gForceModifier);
                }
            }

            //Climb Altitude
            if(pieceController.pieceTemplate.effortlessClimb){
                if(pieceController.GetPivotTileLevel() < TileLevel.MAX && pieceController.gamePiece.currentVelocity >= 1){
                    InstantiateCommandPoint(
                        new TilePath(startingVec).TraversePlanar(HexDirection.Forward, pieceController.gamePiece.currentVelocity - 1).TraverseVertical(1).TraversePlanar(HexDirection.Forward, 1),
                        pieceController.gamePiece.currentVelocity,
                        1 + gForceModifier);
                    }

            }else{
                if(pieceController.GetPivotTileLevel() < TileLevel.MAX && pieceController.gamePiece.currentVelocity >= 2){
                    InstantiateCommandPoint(
                        new TilePath(startingVec).TraversePlanar(HexDirection.Forward, pieceController.gamePiece.currentVelocity - 2).TraverseVertical(1).TraversePlanar(HexDirection.Forward, 1),
                        pieceController.gamePiece.currentVelocity,
                        1 + gForceModifier);
                }
            }

            //Descend Altitude
            if(pieceController.GetPivotTileLevel() > 1 && pieceController.gamePiece.currentVelocity >= 1){
                InstantiateCommandPoint(
                        new TilePath(startingVec).TraversePlanar(HexDirection.Forward, pieceController.gamePiece.currentVelocity - 1).TraverseVertical(-1).TraversePlanar(HexDirection.Forward, 1),
                        pieceController.gamePiece.currentVelocity,
                        1 + gForceModifier);
            }
            
            eventPublisher.Publish(new Events.NewPointSelected() { SelectedPoint = defalutSelectedPoint });

            hasGeneratedThisTurn = true;
        }

        public List<CommandPointController> GetAvailableCommandPoints(){
            return availableCommandPoints;
        }

        [EventListener]
        void OnStartNewTurn(GameControllerFsm.Events.BeginCommandSelectionState @event)
        {
            foreach(CommandPointController point in availableCommandPoints) {
                GameObject.Destroy(point.gameObject);
            }
            availableCommandPoints.Clear();

            hasGeneratedThisTurn = false;
            GenerateCommandPoints();
        }

        private CommandPointController InstantiateCommandPoint(TilePath path, int endVelocity, int gForce){
            CommandPointController commandPoint = GameObject.Instantiate(commandPointPrefab, transform.position, transform.rotation, transform).GetComponent<CommandPointController>();

            commandPoint.SetSource(pieceController.worldModel.transform.position, HexMapHelper.GetFacingVector(pieceController.gamePiece.currentTile.position, pieceController.gamePiece.currentTile.facing));
            commandPoint.SetEndVelocity(endVelocity);
            commandPoint.SetGForce(Mathf.Max(0, gForce));
            commandPoint.SetTilePath(path); //Hidden knowledge, must be called after SetGForce
            commandPoint.SubscribeNavigationEvents(this);

            availableCommandPoints.Add(commandPoint);
            return commandPoint;
        }

        [EventListener]
        private void OnCommandPointClicked(CommandPointViewFsm.Events.CommandPointClicked @event){
            Debug.Log("Selecting Command Point: " + @event.CommandPoint);
            eventPublisher.Publish(new Events.NewPointSelected() { SelectedPoint = @event.CommandPoint });
        }
    }
}