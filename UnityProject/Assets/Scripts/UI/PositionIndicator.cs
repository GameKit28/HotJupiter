using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;

public class PositionIndicator : MonoBehaviour
{
    [SerializeReference]
    public GameObject attachedObject; // Must derive from IHaveTilePosition
    public SpriteRenderer hexagon;

    private IHaveTilePosition tilePositionObject;

    void Awake()
    {
        if (attachedObject != null)
            tilePositionObject = attachedObject.GetComponent<IHaveTilePosition>();
    }

    // Update is called once per frame
    void Update()
    {
        hexagon.transform.position = HexMapHelper.GetWorldPointFromTile(tilePositionObject.GetTilePosition(), tilePositionObject.GetLevel());
        hexagon.color = HexMapHelper.GetLevelColor(tilePositionObject.GetLevel());
    }

    //[EventListener]
    //void OnAltitudeUIChange()
}
