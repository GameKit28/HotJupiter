using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;
using MeEngine.Events;

namespace HotJupiter {
public abstract class BaseGamePiece : MonoBehaviour, IHaveTilePosition, IHaveTileFacing, IHaveTileFootprint
{
    public static class Events {
        public struct CompletedSetup : IEvent {}
    }
    public EventPublisher eventPublisher {get; private set;} = new EventPublisher();

    public TileWithFacing currentTile;

    private TileWithFacing destinationTile;

    public GameObject gamePieceModel;

    public TileCoords GetPivotTilePosition(){
        return currentTile.position;
    }

    public TileLevel GetPivotTileLevel(){
        return currentTile.level;
    }

    protected DynamicFootprint footprint;

    public FootprintBase GetFootprint(){
        return footprint;
    }

    public TileCoords GetTileFacing(){
        return currentTile.facing;
    }

    protected virtual void Awake() {
        GameControllerFsm.eventPublisher.SubscribeAll(this);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentTile.position = HexMapHelper.GetTileFromWorldPoint(transform.position);
        currentTile.facing = HexMapHelper.GetNeighborTiles(currentTile.position)[0];

        PositionAndOrientPiece();
        eventPublisher.Publish(new Events.CompletedSetup());
    }

    protected virtual void Update(){

    }

    public void SetDestination(TileWithFacing tile) {
        this.destinationTile = tile;
    }

    public TileWithFacing GetDestinationTile(){
        //Used by missiles to intercept
        return this.destinationTile;
    }

    public void PositionAndOrientPiece(){
        transform.position = HexMapHelper.GetWorldPointFromTile(currentTile.position, currentTile.level);
        gamePieceModel.transform.rotation = HexMapHelper.GetRotationFromFacing(currentTile.position, currentTile.facing);

        footprint.SetPivotTile(currentTile);
    }

    [EventListener]
    protected virtual void OnBeginPlayingPhase(GameControllerFsm.Events.BeginPlayingOutTurnState @event) {
        gameObject.SetActive(false);
    }

    [EventListener]
    protected virtual void OnEndPlayingPhase(GameControllerFsm.Events.EndPlayingOutTurnState @event){
        currentTile = destinationTile;

        PositionAndOrientPiece();
        gameObject.SetActive(true);
    }
}
}