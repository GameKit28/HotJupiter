using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;
using HexasphereGrid;

public enum HexDirection {
    NorthEast = 0,
    East = 1,
    SouthEast = 2,
    SouthWest = 3,
    West = 4,
    NorthWest = 5
}

public struct TileCoords {
    public int index;

    public bool Equals(TileCoords other)
    {
        return Equals(other, this);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var objectToCompareWith = (TileCoords) obj;

        return index == objectToCompareWith.index;
    }

    public override int GetHashCode()
    {
        return index.GetHashCode();
    }

    public static bool operator ==(TileCoords o1, TileCoords o2) 
    {
        return o1.Equals(o2);
    }

    public static bool operator !=(TileCoords o1, TileCoords o2) 
    {
        return !o1.Equals(o2);
    }
}

public class HexMapHelper : MonoBehaviour
{
    private static HexMapHelper instance;

    public const float gridFirstAltitudeOffset = 0.25f;
    public const float gridAltitudeOffsets = 0.5f;

    public Hexasphere baseHexasphere;

    public const float HexWidth = 1.0f; //The incircle diameter of a hexagon

    void Awake(){
        instance = this;
    }

    public static TileCoords GetTileFromWorldPoint(Vector3 worldPos){
        return new TileCoords(){ index = instance.baseHexasphere.GetTileAtPos(worldPos) };
    }

    public static Vector3 GetTileNormal(TileCoords tileCoords){
        return (instance.baseHexasphere.tiles[tileCoords.index].center - instance.baseHexasphere.transform.position).normalized;
    }

    public static Vector3 GetWorldPointFromTile(TileCoords tileCoords, int level = 0){
        Tile tile = instance.baseHexasphere.tiles[tileCoords.index];
        if(level == 0){
            return tile.center;
        }else{
            return tile.center + GetTileNormal(tileCoords) * GetAltitudeFromLevel(level);
        }
    }

    public static float GetAngleFromDirection(HexDirection direction) {
        return 30f + ((int)direction) * 60f;
    }

    public static Vector3 GetVectorFromDirection(HexDirection direction){
        switch(direction){
            case HexDirection.NorthEast:
                return new Vector3(0.5f, 0, 0.87f);
            case HexDirection.East:
                return new Vector3(1f, 0, 0);
            case HexDirection.SouthEast:
                return new Vector3(0.5f, 0, -0.87f);
            case HexDirection.SouthWest:
                return new Vector3(-0.5f, 0, -0.87f);
            case HexDirection.West:
                return new Vector3(-1f, 0, 0);
            case HexDirection.NorthWest:
                return new Vector3(-0.5f, 0, 0.87f);
            default:
                return Vector3.up;
        }
    }

    public static int GetLevelFromAltitude(float altitude){
        return Mathf.Clamp(Mathf.RoundToInt((altitude + gridFirstAltitudeOffset) / gridAltitudeOffsets), 0, 6);
    }

    public static float GetAltitudeFromLevel(int level) {
        return Mathf.Max(0, gridFirstAltitudeOffset + ((level - 1) * gridAltitudeOffsets));
    }

    public static Color GetLevelColor(int level) {
        switch(level) {
            case 0: 
                return Color.white;
            
            case 1:
                return Color.red;

            case 2:
                return new Color(1, 0.5f, 0, 1); //Orange

            case 3:
                return Color.yellow;

            case 4:
                return Color.green;

            case 5:
                return Color.blue;

            case 6:
                return new Color(1, 0, 1, 1); //Purple

            default:
                return Color.white;
        }
    }

    public static float CrowFlyDistance(TileCoords hex1Tile, int hex1Height, TileCoords hex2Tile, int hex2Height){
        Vector3 pos1 = GetWorldPointFromTile(hex1Tile, hex1Height);
        Vector3 pos2 = GetWorldPointFromTile(hex2Tile, hex2Height);
        return Vector3.Distance(pos1, pos2);
    }
}



