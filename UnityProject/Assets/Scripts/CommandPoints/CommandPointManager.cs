using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;
public class CommandPointManager : MonoBehaviour
{
    public GameObject commandPointPrefab;
    public ShipController playerShipPiece;

    private HashSet<CommandPointFsm> activeCommandPoints = new HashSet<CommandPointFsm>();

    void Awake(){
        EventManager.SubscribeAll(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [EventListener]
    void OnStartNewTurn(GameControllerFsm.Events.NewTurnEvent @event)
    {
        //Standard Positions

        //Forward Facing (current speed)
        InstantiateCommandPoint(
            playerShipPiece.currentTile.Traverse(playerShipPiece.currentDirection, playerShipPiece.currentSpeed),
            playerShipPiece.currentDirection,
            playerShipPiece.currentLevel);

        //Forward Facing (speed up)
        if(playerShipPiece.currentSpeed < ShipController.maxSpeed) {
            InstantiateCommandPoint(
            playerShipPiece.currentTile.Traverse(playerShipPiece.currentDirection, playerShipPiece.currentSpeed + 1),
            playerShipPiece.currentDirection,
            playerShipPiece.currentLevel);
        }

        //Forward Facing (slow down)
        if(playerShipPiece.currentSpeed > 2) {
            InstantiateCommandPoint(
            playerShipPiece.currentTile.Traverse(playerShipPiece.currentDirection, playerShipPiece.currentSpeed - 1),
            playerShipPiece.currentDirection,
            playerShipPiece.currentLevel);
        }

        //Turn Left (straight bank)
        InstantiateCommandPoint(
            playerShipPiece.currentTile.Traverse(playerShipPiece.currentDirection, playerShipPiece.currentSpeed),
            playerShipPiece.currentDirection.RotateCounterClockwise(),
            playerShipPiece.currentLevel);

        //Turn Left
        InstantiateCommandPoint(
            playerShipPiece.currentTile.Traverse(playerShipPiece.currentDirection, playerShipPiece.currentSpeed - 1).Traverse(playerShipPiece.currentDirection.RotateCounterClockwise()),
            playerShipPiece.currentDirection.RotateCounterClockwise(),
            playerShipPiece.currentLevel);

        //Turn Right (straight bank)
        InstantiateCommandPoint(
            playerShipPiece.currentTile.Traverse(playerShipPiece.currentDirection, playerShipPiece.currentSpeed),
            playerShipPiece.currentDirection.RotateClockwise(),
            playerShipPiece.currentLevel);

        //Turn Right
        InstantiateCommandPoint(
            playerShipPiece.currentTile.Traverse(playerShipPiece.currentDirection, playerShipPiece.currentSpeed - 1).Traverse(playerShipPiece.currentDirection.RotateClockwise()),
            playerShipPiece.currentDirection.RotateClockwise(),
            playerShipPiece.currentLevel);

        //Climb Altitude
        if(playerShipPiece.currentLevel < 6){
            InstantiateCommandPoint(
                playerShipPiece.currentTile.Traverse(playerShipPiece.currentDirection, playerShipPiece.currentSpeed - 1),
                playerShipPiece.currentDirection,
                playerShipPiece.currentLevel + 1);
        }

        //Descend Altitude
        if(playerShipPiece.currentLevel > 1){
            InstantiateCommandPoint(
                playerShipPiece.currentTile.Traverse(playerShipPiece.currentDirection, playerShipPiece.currentSpeed),
                playerShipPiece.currentDirection,
                playerShipPiece.currentLevel - 1);
        }
    }

    private void InstantiateCommandPoint(Vector3Int tile, HexDirection direction, int level){
        CommandPointFsm commandPoint = GameObject.Instantiate(commandPointPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<CommandPointFsm>();
        
        commandPoint.SetSource(playerShipPiece.transform.position + new Vector3(0, HexMapHelper.GetAltitudeFromLevel(playerShipPiece.currentLevel), 0), playerShipPiece.currentDirection);
        commandPoint.SetDestination(tile, direction, level);

        activeCommandPoints.Add(commandPoint);
    }
}
