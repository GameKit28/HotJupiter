using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;
public class EnemyShipBrain : BaseBrain<ShipGamePiece>
{
    void Awake() {
        EventManager.SubscribeAll(this);
    }

    public override BaseGamePiece FindTarget()
    {
        //Player Ship
        return currentTarget;
    }

    public override CommandPointFsm SelectCommand(){
        var availableCommandPoints = pieceController.navigationSystem.GetAvailableCommandPoints();
        return availableCommandPoints[Random.Range(0, availableCommandPoints.Count)];
    }

    [EventListener]
    void OnNewTurn(GameControllerFsm.Events.NewTurnEvent @event){
        FindTarget();
        Vector3Int missileOkayZone = myGamePiece.currentTile.Traverse(myGamePiece.currentDirection, myGamePiece.shipTemplete.missileTemplate.TopSpeed);
        if(HexMapHelper.CrowFlyDistance(missileOkayZone, myGamePiece.currentLevel, currentTarget.currentTile, currentTarget.currentLevel) < 4f){
            myGamePiece.QueueMissile(true);
        }
    }

    [EventListener]
    void OnNewTurnPost(GameControllerFsm.Events.NewTurnEventPost @event){
        pieceController.SetSelectedCommandPoint(SelectCommand());
    }
}
