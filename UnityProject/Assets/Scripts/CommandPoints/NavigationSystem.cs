using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;
public class NavigationSystem: MonoBehaviour
{
    public GameObject commandPointPrefab;
    public PieceController pieceController;

    private List<CommandPointFsm> availableCommandPoints = new List<CommandPointFsm>();

    private CommandPointFsm selectedCommandPoint;

    bool hasGeneratedThisTurn = false;

    void Awake(){
        EventManager.SubscribeAll(this);
    }

    [EventListener]
    void OnStartNewTurn(GameControllerFsm.Events.NewTurnEvent @event)
    {
        GenerateCommandPoints();
    }

    public void GenerateCommandPoints()
    {
        if (hasGeneratedThisTurn) return;

        if(selectedCommandPoint != null) {
            GameObject.Destroy(selectedCommandPoint.gameObject);
            selectedCommandPoint = null;
        }

        //Standard Destinations

        //Forward Facing (current speed)
        //This is the default selected command point for players
        var defaultSelected = InstantiateCommandPoint(
            pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity),
            pieceController.GetHexDirection(),
            pieceController.GetLevel());

        //Forward Facing (speed up)
        if(pieceController.gamePiece.currentVelocity < pieceController.gamePiece.maxSpeed) {
            InstantiateCommandPoint(
            pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity + 1),
            pieceController.GetHexDirection(),
            pieceController.GetLevel());
        }

        //Forward Facing (slow down)
        if(pieceController.gamePiece.currentVelocity > 2) {
            InstantiateCommandPoint(
            pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity - 1),
            pieceController.GetHexDirection(),
            pieceController.GetLevel());
        }

        //Turn Left (straight bank)
        InstantiateCommandPoint(
            pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity),
            pieceController.GetHexDirection().RotateCounterClockwise(),
            pieceController.GetLevel());

        //Turn Left
        InstantiateCommandPoint(
            pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity - 1).Traverse(pieceController.GetHexDirection().RotateCounterClockwise()),
            pieceController.GetHexDirection().RotateCounterClockwise(),
            pieceController.GetLevel());

        //Turn Right (straight bank)
        InstantiateCommandPoint(
            pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity),
            pieceController.GetHexDirection().RotateClockwise(),
            pieceController.GetLevel());

        //Turn Right
        InstantiateCommandPoint(
            pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity - 1).Traverse(pieceController.GetHexDirection().RotateClockwise()),
            pieceController.GetHexDirection().RotateClockwise(),
            pieceController.GetLevel());

        //Climb Altitude
        if(pieceController.GetLevel() < 6){
            InstantiateCommandPoint(
                pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity - 1),
                pieceController.GetHexDirection(),
                pieceController.GetLevel() + 1);
        }

        //Descend Altitude
        if(pieceController.GetLevel() > 1){
            InstantiateCommandPoint(
                pieceController.GetTilePosition().Traverse(pieceController.GetHexDirection(), pieceController.gamePiece.currentVelocity),
                pieceController.GetHexDirection(),
                pieceController.GetLevel() - 1);
        }

        if(pieceController.isPlayerControlled) {
            defaultSelected.SelectPoint(true);
            selectedCommandPoint = defaultSelected;
        }else{
            //Have enemies fly randomly for now
            selectedCommandPoint = availableCommandPoints[Random.Range(0, availableCommandPoints.Count)];
        }

        hasGeneratedThisTurn = true;
    }

    [EventListener]
    void OnStartPlayingTurn(GameControllerFsm.Events.BeginPlayingOutTurnEvent @event){
        pieceController.SetActivePath(selectedCommandPoint.spline);
        pieceController.gamePiece.SetDestination(selectedCommandPoint.destinationTile, selectedCommandPoint.destinationDirection, selectedCommandPoint.destinationLevel);

        foreach(CommandPointFsm point in availableCommandPoints) {
            if(point != selectedCommandPoint) {
                GameObject.Destroy(point.gameObject);
            }
        }
        availableCommandPoints.Clear();
        hasGeneratedThisTurn = false;
    }

    private CommandPointFsm InstantiateCommandPoint(Vector3Int tile, HexDirection direction, int level){
        CommandPointFsm commandPoint = GameObject.Instantiate(commandPointPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<CommandPointFsm>();
        commandPoint.SetNavigationSystem(this);

        commandPoint.SetSource(pieceController.worldModel.transform.position, pieceController.GetHexDirection());
        commandPoint.SetDestination(tile, direction, level);
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
        selectedCommandPoint = selectedPoint;
    }

}
