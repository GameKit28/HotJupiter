using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;
public class EnemyShipBrain : BaseBrain<ShipGamePiece>
{
    void Awake() {
        GameControllerFsm.eventPublisher.SubscribeAll(this);
    }

    public override BaseGamePiece FindTarget()
    {
        //Player Ship
        if(currentTarget != null) Debug.DrawLine(HexMapHelper.GetWorldPointFromTile(myGamePiece.currentTile), HexMapHelper.GetWorldPointFromTile(currentTarget.currentTile), Color.yellow, 5f);
        return currentTarget;
    }

    public override CommandPointController SelectCommand(){
        var availableCommandPoints = pieceController.navigationSystem.GetAvailableCommandPoints();
        var chosenCommandPoint = availableCommandPoints[Random.Range(0, availableCommandPoints.Count)];
        return chosenCommandPoint;
    }

    [EventListener]
    void OnNewTurn(GameControllerFsm.Events.BeginCommandSelectionState @event){
        FindTarget();
        TileWithFacing startVec = new TileWithFacing() {position = myGamePiece.currentTile, facing = myGamePiece.currentTileFacing};
        TileCoords missileOkayZone = startVec.Traverse(HexDirection.Forward, myGamePiece.shipTemplete.missileTemplate.TopSpeed).position;
        if(HexMapHelper.CrowFlyDistance(missileOkayZone, myGamePiece.currentLevel, currentTarget.currentTile, currentTarget.currentLevel) < 4f){
            myGamePiece.QueueMissile(true);
        }
    }

    [EventListener]
    void OnNewTurnPost(GameControllerFsm.Events.BeginCommandSelectionStatePost @event){
        pieceController.SetSelectedCommandPoint(SelectCommand());
    }
}
