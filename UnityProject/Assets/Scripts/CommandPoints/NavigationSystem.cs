using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;
public class NavigationSystem: MonoBehaviour
{
    public GameObject commandPointPrefab;
    public PieceController pieceController;

    private List<CommandPointFsm> availableCommandPoints = new List<CommandPointFsm>();

    bool hasGeneratedThisTurn = false;

    void Awake(){
        EventManager.SubscribeAll(this);
    }

    public void GenerateCommandPoints()
    {
        if (hasGeneratedThisTurn) return;

        //Standard Destinations

        //Forward Facing (current speed)
        //This is the default selected command point for players
        TileWithFacing startingVec = new TileWithFacing { position = pieceController.GetTilePosition(), facing = pieceController.GetTileFacing() };

        var defalutSelectedPoint = InstantiateCommandPoint(
            startingVec.Traverse(HexDirection.Forward, pieceController.gamePiece.currentVelocity),
            pieceController.GetLevel(),
            pieceController.gamePiece.currentVelocity);

        //Forward Facing (speed up)
        /*if(pieceController.gamePiece.currentVelocity < pieceController.pieceTemplate.TopSpeed && pieceController.pieceTemplate.canAccelerate) {
            InstantiateCommandPoint(
                pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity + 1),
                pieceController.GetHexDirection(),
                pieceController.GetLevel(),
                pieceController.gamePiece.currentVelocity + 1);
        }

        //Forward Facing (slow down)
        if(pieceController.gamePiece.currentVelocity > 2 && pieceController.pieceTemplate.canDecelerate) {
            InstantiateCommandPoint(
                pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity - 1),
                pieceController.GetHexDirection(),
                pieceController.GetLevel(),
                pieceController.gamePiece.currentVelocity - 1);
        }*/

        //Turn Left (straight bank)
        InstantiateCommandPoint(
            startingVec.Traverse(HexDirection.Forward, pieceController.gamePiece.currentVelocity).Face(HexDirection.ForwardLeft),
            pieceController.GetLevel(),
            pieceController.gamePiece.currentVelocity);

        //Temp Turn Left
        InstantiateCommandPoint(
            startingVec.Traverse(HexDirection.Forward, pieceController.gamePiece.currentVelocity - 1).Traverse(HexDirection.ForwardLeft),
            pieceController.GetLevel(),
            pieceController.gamePiece.currentVelocity);

/*
        //Turn Right (straight bank)
        InstantiateCommandPoint(
            pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity),
            pieceController.GetHexDirection().RotateClockwise(),
            pieceController.GetLevel(),
            pieceController.gamePiece.currentVelocity);

        for(int manu = 1; manu <= pieceController.pieceTemplate.Maneuverability; manu++){
            if(pieceController.gamePiece.currentVelocity - manu >= 1) {
                if(pieceController.pieceTemplate.canStrafe){
                    //Strafe Left
                    InstantiateCommandPoint(
                        pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity - manu).Traverse(pieceController.GetHexDirection().RotateCounterClockwise(), manu),
                        pieceController.GetHexDirection(),
                        pieceController.GetLevel(),
                        pieceController.gamePiece.currentVelocity);

                    //Strafe Right (straight bank)
                    InstantiateCommandPoint(
                        pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity - manu).Traverse(pieceController.GetHexDirection().RotateClockwise(), manu),
                        pieceController.GetHexDirection(),
                        pieceController.GetLevel(),
                        pieceController.gamePiece.currentVelocity);
                }

                //Turn Left
                InstantiateCommandPoint(
                    pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity - manu).Traverse(pieceController.GetHexDirection().RotateCounterClockwise(), manu),
                    pieceController.GetHexDirection().RotateCounterClockwise(),
                    pieceController.GetLevel(),
                    pieceController.gamePiece.currentVelocity);

                //Turn Right
                InstantiateCommandPoint(
                    pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity - manu).Traverse(pieceController.GetHexDirection().RotateClockwise(), manu),
                    pieceController.GetHexDirection().RotateClockwise(),
                    pieceController.GetLevel(),
                    pieceController.gamePiece.currentVelocity);
            }
        }

        

        //Climb Altitude
        if(pieceController.pieceTemplate.effortlessClimb){
            if(pieceController.GetLevel() < 6){
                InstantiateCommandPoint(
                    pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity),
                    pieceController.GetHexDirection(),
                    pieceController.GetLevel() + 1,
                    pieceController.gamePiece.currentVelocity);
                }

        }else{
            if(pieceController.GetLevel() < 6 && pieceController.gamePiece.currentVelocity >= 2){
                InstantiateCommandPoint(
                    pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity - 1),
                    pieceController.GetHexDirection(),
                    pieceController.GetLevel() + 1,
                    pieceController.gamePiece.currentVelocity);
            }
        }

        //Descend Altitude
        if(pieceController.GetLevel() > 1){
            InstantiateCommandPoint(
                pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity),
                pieceController.GetHexDirection(),
                pieceController.GetLevel() - 1,
                pieceController.gamePiece.currentVelocity);
        }
        */
        
        if(pieceController.isPlayerControlled) defalutSelectedPoint.SelectPoint(true);

        hasGeneratedThisTurn = true;
    }

    public List<CommandPointFsm> GetAvailableCommandPoints(){
        return availableCommandPoints;
    }

    [EventListener]
    void OnStartNewTurn(GameControllerFsm.Events.NewTurnEvent @event)
    {
        foreach(CommandPointFsm point in availableCommandPoints) {
            GameObject.Destroy(point.gameObject);
        }
        availableCommandPoints.Clear();

        hasGeneratedThisTurn = false;
        GenerateCommandPoints();
    }

    private CommandPointFsm InstantiateCommandPoint(TileWithFacing tileVec, int level, int endVelocity){
        CommandPointFsm commandPoint = GameObject.Instantiate(commandPointPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<CommandPointFsm>();
        commandPoint.SetNavigationSystem(this);

        commandPoint.SetSource(pieceController.worldModel.transform.position, HexMapHelper.GetFacingVector(tileVec.position, tileVec.facing));
        commandPoint.SetDestination(tileVec.position, tileVec.facing, level);
        commandPoint.SetEndVelocity(endVelocity);
        if(!pieceController.isPlayerControlled) commandPoint.gameObject.SetActive(false);

        availableCommandPoints.Add(commandPoint);
        return commandPoint;
    }

    public void NewPointSelected(CommandPointFsm selectedPoint){
        Debug.Log("Selecting Command Point: " + selectedPoint);
        foreach (var point in availableCommandPoints)
        {
            if(point != selectedPoint) point.SelectPoint(false);
        }
        pieceController.SetSelectedCommandPoint(selectedPoint);
    }

}
