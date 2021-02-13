using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;

public class MissileBrain : BaseBrain<MissileGamePiece>
{   
    void Awake() {
        GameControllerFsm.eventPublisher.SubscribeAll(this);
    }

    public override BaseGamePiece FindTarget()
    {
        List<ShipGamePiece> allShips = ShipManager.GetAllShips();

        //Next Turn Hex
        TileWithFacing startVec = new TileWithFacing() {position = myGamePiece.currentTile, facing = myGamePiece.currentTileFacing};
        TileWithFacing headingTile = startVec.Traverse(HexDirection.Forward, myGamePiece.currentVelocity);

        //Find the ship closest to where I will be if I move forward. Exclude the ship that fired me.
        ShipGamePiece closestShip = null;
        float closestShipDistance = float.MaxValue;

        foreach(ShipGamePiece ship in allShips){
            if(ship == myGamePiece.motherGamePiece) continue;
            
            float distance = HexMapHelper.CrowFlyDistance(headingTile.position, myGamePiece.currentLevel, ship.currentTile, ship.currentLevel);
            if (distance < closestShipDistance) {
                closestShipDistance = distance;
                closestShip = ship;
            }
        }

        currentTarget = closestShip;
        
        return currentTarget;
    }

    public override CommandPointFsm SelectCommand()
    {
        var availableCommands = pieceController.navigationSystem.GetAvailableCommandPoints();

        if(currentTarget != null){
            // Missile know where their target will be
            TileCoords targetDestinationTile = currentTarget.GetDestinationTile();
            int targetDestinationLevel = currentTarget.GetDestinationLevel();
            Vector3 targetDestinationWorldSpace = HexMapHelper.GetWorldPointFromTile(targetDestinationTile, targetDestinationLevel);

            CommandPointFsm closestPoint = null;
            float closestPointDist = float.MaxValue;

            foreach(CommandPointFsm point in availableCommands){
                if(point.destinationTile == targetDestinationTile && point.destinationLevel == targetDestinationLevel) return point;

                float distance = Vector3.Distance(targetDestinationWorldSpace, HexMapHelper.GetWorldPointFromTile(point.destinationTile, point.destinationLevel));
                if(distance < closestPointDist){
                    closestPoint = point;
                    closestPointDist = distance;
                }
            }

            return closestPoint;

        }else{
            return base.SelectCommand();
        }
    }
 
 
    [EventListener]
    void OnNewTurn(GameControllerFsm.Events.NewTurnEvent @event){
        FindTarget();
    }

    [EventListener]
    void OnEndOfTurn(GameControllerFsm.Events.ProcessEndTurnEvent @event){
        Debug.Log("Missile Selecing Command");
        pieceController.SetSelectedCommandPoint(SelectCommand());
    }
}
