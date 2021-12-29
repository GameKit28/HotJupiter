using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;

public class DirectionIndicator : MonoBehaviour
{
    const bool hideWhenExecuting = true;

    private const float centerOffset = 0.55f;

    [SerializeReference]
    public GameObject attachedObject; // Must derive from IHaveHexDirection and IHaveTilePosition
    public SpriteRenderer triangle;

    private IHaveTileFacing hexDirectionObject;
    private IHaveTilePosition tilePositionObject;

    void Awake()
    {
        if (attachedObject != null){
            hexDirectionObject = attachedObject.GetComponent<IHaveTileFacing>();
            tilePositionObject = attachedObject.GetComponent<IHaveTilePosition>();
        }
    }

    void Start(){
        GameControllerFsm.eventPublisher.SubscribeAll(this);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = HexMapHelper.GetWorldPointFromTile(
            tilePositionObject.GetPivotTilePosition(), tilePositionObject.GetPivotTileLevel())
            + (HexMapHelper.GetFacingVector(tilePositionObject.GetPivotTilePosition(), hexDirectionObject.GetTileFacing()) * centerOffset);
        triangle.color = HexMapUI.GetLevelColor(tilePositionObject.GetPivotTileLevel());
        transform.rotation = HexMapHelper.GetRotationFromFacing(tilePositionObject.GetPivotTilePosition(), hexDirectionObject.GetTileFacing());
    }

    [EventListener]
    private void OnPlayingOutTurnStart(GameControllerFsm.Events.BeginPlayingOutTurnState @event){
        GetComponentInChildren<SpriteRenderer>().enabled = !hideWhenExecuting;
    }

    [EventListener]
    private void OnPlayingOutTurnEnd(GameControllerFsm.Events.EndPlayingOutTurnState @event){
        GetComponentInChildren<SpriteRenderer>().enabled = true;
    }
}
