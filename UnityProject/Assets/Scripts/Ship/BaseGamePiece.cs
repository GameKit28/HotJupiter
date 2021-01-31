using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;
using MeEngine.Events;
public class BaseGamePiece : MonoBehaviour, IHaveTilePosition, IHaveHexDirection
{
    public Vector3Int currentTile;
    public HexDirection currentDirection;
    public int currentLevel;

    private Vector3Int destinationTile;
    private HexDirection destinationDirection;
    private int destinationLevel;


    public GameObject gamePieceModel;

    public Vector3Int GetTilePosition(){
        return currentTile;
    }

    public int GetLevel(){
        return currentLevel;
    }

    public HexDirection GetHexDirection(){
        return currentDirection;
    }

    protected virtual void Awake() {
        EventManager.SubscribeAll(this);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentTile = HexMapHelper.GetTileFromWorldPoint(transform.position);
        currentLevel = HexMapHelper.GetLevelFromAltitude(transform.position.y);

        PositionAndOrientPiece();
    }

    protected virtual void Update(){

    }

    public void SetDestination(Vector3Int destinationTile, HexDirection destinationDirection, int destinationLevel) {
        Debug.Log("Destination Set");
        this.destinationTile = destinationTile;
        this.destinationDirection = destinationDirection;
        this.destinationLevel = destinationLevel;
    }

    public void PositionAndOrientPiece(){
        transform.position = HexMapHelper.GetWorldPointFromTile(currentTile, currentLevel);
        gamePieceModel.transform.localEulerAngles = new Vector3(0, HexMapHelper.GetAngleFromDirection(currentDirection), 0);
    }

    [EventListener]
    protected virtual void OnBeginPlayingPhase(GameControllerFsm.Events.BeginPlayingOutTurnEvent @event) {
        gameObject.SetActive(false);
    }

    [EventListener]
    protected virtual void OnEndPlayingPhase(GameControllerFsm.Events.EndPlayingOutTurnEvent @event){
        Debug.Log("Applying Destination");
        currentTile = destinationTile;
        currentDirection = destinationDirection;
        currentLevel = destinationLevel;

        PositionAndOrientPiece();
        gameObject.SetActive(true);
    }
}
