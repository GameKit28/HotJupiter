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
        var defalutSelectedPoint = InstantiateCommandPoint(
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
        pieceController.SetSelectedCommandPoint(selectedPoint);
    }

}
