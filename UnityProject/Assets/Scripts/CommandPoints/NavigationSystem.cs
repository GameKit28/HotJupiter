using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;
public class NavigationSystem: MonoBehaviour
{
    public GameObject commandPointPrefab;
    public ShipController shipPiece;

    private HashSet<CommandPointFsm> availableCommandPoints = new HashSet<CommandPointFsm>();

    private CommandPointFsm selectedCommandPoint;

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
        if(selectedCommandPoint != null) {
            GameObject.Destroy(selectedCommandPoint.gameObject);
            selectedCommandPoint = null;
        }

        //Standard Positions

        //Forward Facing (current speed)
        //This is the default selected command point
        var defaultSelected = InstantiateCommandPoint(
            shipPiece.currentTile.Traverse(shipPiece.currentDirection, shipPiece.currentSpeed),
            shipPiece.currentDirection,
            shipPiece.currentLevel);
        defaultSelected.SelectPoint(true);
        selectedCommandPoint = defaultSelected;

        //Forward Facing (speed up)
        if(shipPiece.currentSpeed < ShipController.maxSpeed) {
            InstantiateCommandPoint(
            shipPiece.currentTile.Traverse(shipPiece.currentDirection, shipPiece.currentSpeed + 1),
            shipPiece.currentDirection,
            shipPiece.currentLevel);
        }

        //Forward Facing (slow down)
        if(shipPiece.currentSpeed > 2) {
            InstantiateCommandPoint(
            shipPiece.currentTile.Traverse(shipPiece.currentDirection, shipPiece.currentSpeed - 1),
            shipPiece.currentDirection,
            shipPiece.currentLevel);
        }

        //Turn Left (straight bank)
        InstantiateCommandPoint(
            shipPiece.currentTile.Traverse(shipPiece.currentDirection, shipPiece.currentSpeed),
            shipPiece.currentDirection.RotateCounterClockwise(),
            shipPiece.currentLevel);

        //Turn Left
        InstantiateCommandPoint(
            shipPiece.currentTile.Traverse(shipPiece.currentDirection, shipPiece.currentSpeed - 1).Traverse(shipPiece.currentDirection.RotateCounterClockwise()),
            shipPiece.currentDirection.RotateCounterClockwise(),
            shipPiece.currentLevel);

        //Turn Right (straight bank)
        InstantiateCommandPoint(
            shipPiece.currentTile.Traverse(shipPiece.currentDirection, shipPiece.currentSpeed),
            shipPiece.currentDirection.RotateClockwise(),
            shipPiece.currentLevel);

        //Turn Right
        InstantiateCommandPoint(
            shipPiece.currentTile.Traverse(shipPiece.currentDirection, shipPiece.currentSpeed - 1).Traverse(shipPiece.currentDirection.RotateClockwise()),
            shipPiece.currentDirection.RotateClockwise(),
            shipPiece.currentLevel);

        //Climb Altitude
        if(shipPiece.currentLevel < 6){
            InstantiateCommandPoint(
                shipPiece.currentTile.Traverse(shipPiece.currentDirection, shipPiece.currentSpeed - 1),
                shipPiece.currentDirection,
                shipPiece.currentLevel + 1);
        }

        //Descend Altitude
        if(shipPiece.currentLevel > 1){
            InstantiateCommandPoint(
                shipPiece.currentTile.Traverse(shipPiece.currentDirection, shipPiece.currentSpeed),
                shipPiece.currentDirection,
                shipPiece.currentLevel - 1);
        }
    }

    [EventListener]
    void OnStartPlayingTurn(GameControllerFsm.Events.BeginPlayingOutTurnEvent @event){
        shipPiece.SetActivePath(selectedCommandPoint.spline);
        shipPiece.SetDestination(selectedCommandPoint.destinationTile, selectedCommandPoint.destinationDirection, selectedCommandPoint.destinationLevel);

        foreach(CommandPointFsm point in availableCommandPoints) {
            if(point != selectedCommandPoint) {
                GameObject.Destroy(point.gameObject);
            }
        }
        availableCommandPoints.Clear();
    }

    private CommandPointFsm InstantiateCommandPoint(Vector3Int tile, HexDirection direction, int level){
        CommandPointFsm commandPoint = GameObject.Instantiate(commandPointPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<CommandPointFsm>();
        commandPoint.SetNavigationSystem(this);

        commandPoint.SetSource(shipPiece.worldShip.transform.position, shipPiece.currentDirection);
        commandPoint.SetDestination(tile, direction, level);
        if(!shipPiece.isPlayerControlled) commandPoint.gameObject.SetActive(false);

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
