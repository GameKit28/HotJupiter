using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;

public class DirectionIndicator : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        triangle.transform.position = HexMapHelper.GetWorldPointFromTile(
            tilePositionObject.GetTilePosition(), tilePositionObject.GetLevel())
            + (HexMapHelper.GetFacingVector(tilePositionObject.GetTilePosition(), hexDirectionObject.GetTileFacing()) * centerOffset);
        triangle.color = HexMapHelper.GetLevelColor(tilePositionObject.GetLevel());
        triangle.transform.rotation = HexMapHelper.GetRotationFromFacing(tilePositionObject.GetTilePosition(), hexDirectionObject.GetTileFacing());
    }

    //[EventListener]
    //void OnAltitudeUIChange()
}
