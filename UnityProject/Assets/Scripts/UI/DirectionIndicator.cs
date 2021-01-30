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

    private IHaveHexDirection hexDirectionObject;
    private IHaveTilePosition tilePositionObject;

    void Awake()
    {
        if (attachedObject != null){
            hexDirectionObject = attachedObject.GetComponent<IHaveHexDirection>();
            tilePositionObject = attachedObject.GetComponent<IHaveTilePosition>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        triangle.transform.position = HexMapHelper.GetWorldPointFromTile(
            tilePositionObject.GetTilePosition(), tilePositionObject.GetLevel())
            + (HexMapHelper.GetVectorFromDirection(hexDirectionObject.GetHexDirection()) * centerOffset);
        triangle.color = HexMapHelper.GetLevelColor(tilePositionObject.GetLevel());
        triangle.transform.eulerAngles = new Vector3(90, HexMapHelper.GetAngleFromDirection(hexDirectionObject.GetHexDirection()),0);
    }

    //[EventListener]
    //void OnAltitudeUIChange()
}
