using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour, IHaveTilePosition
{
    public Vector3Int currentTile;
    public HexDirection currentDirection;
    public int currentLevel;

    public int currentSpeed;

    public const int maxSpeed = 6;

    public Vector3Int GetTilePosition(){
        return currentTile;
    }

    public int GetLevel(){
        return currentLevel;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTile = HexMapHelper.GetTileFromWorldPoint(transform.position);
        currentLevel = HexMapHelper.GetLevelFromAltitude(transform.position.y);

        transform.position = HexMapHelper.GetWorldPointFromTile(currentTile, currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
