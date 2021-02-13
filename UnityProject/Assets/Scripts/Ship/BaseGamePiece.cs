using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;
using MeEngine.Events;
public class BaseGamePiece : MonoBehaviour, IHaveTilePosition, IHaveTileFacing
{
    public static class Events {
        public struct CompletedSetup : IEvent {}
    }
    public EventPublisher eventPublisher {get; private set;} = new EventPublisher();

    public TileCoords currentTile;
    public TileCoords currentTileFacing;
    public int currentLevel;

    private TileCoords destinationTile;
    private TileCoords destinationTileFacing;
    private int destinationLevel;


    public GameObject gamePieceModel;

    public TileCoords GetTilePosition(){
        return currentTile;
    }

    public int GetLevel(){
        return currentLevel;
    }

    public TileCoords GetTileFacing(){
        return currentTileFacing;
    }

    protected virtual void Awake() {
        GameControllerFsm.eventPublisher.SubscribeAll(this);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentTile = HexMapHelper.GetTileFromWorldPoint(transform.position);
        currentTileFacing = HexMapHelper.GetNeighborTiles(currentTile)[0];

        PositionAndOrientPiece();
        eventPublisher.Publish(new Events.CompletedSetup());
    }

    protected virtual void Update(){

    }

    public void SetDestination(TileCoords destinationTile, TileCoords destinationTileFacing, int destinationLevel) {
        this.destinationTile = destinationTile;
        this.destinationTileFacing = destinationTileFacing;
        this.destinationLevel = destinationLevel;
    }

    public TileCoords GetDestinationTile(){
        //Used by missiles to intercept
        return this.destinationTile;
    }

    public int GetDestinationLevel(){
        //Used by missiles to intercept
        return this.destinationLevel;
    }

    public void PositionAndOrientPiece(){
        transform.position = HexMapHelper.GetWorldPointFromTile(currentTile, currentLevel);
        gamePieceModel.transform.rotation = HexMapHelper.GetRotationFromFacing(currentTile, currentTileFacing);
    }

    [EventListener]
    protected virtual void OnBeginPlayingPhase(GameControllerFsm.Events.BeginPlayingOutTurnState @event) {
        gameObject.SetActive(false);
    }

    [EventListener]
    protected virtual void OnEndPlayingPhase(GameControllerFsm.Events.EndPlayingOutTurnState @event){
        currentTile = destinationTile;
        currentTileFacing = destinationTileFacing;
        currentLevel = destinationLevel;

        PositionAndOrientPiece();
        gameObject.SetActive(true);
    }
}
