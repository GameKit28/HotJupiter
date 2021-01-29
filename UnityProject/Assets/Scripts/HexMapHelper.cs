using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum HexDirection {
    NorthEast = 0,
    East = 1,
    SouthEast = 2,
    SouthWest = 3,
    West = 4,
    NorthWest = 5
}

public class HexMapHelper : MonoBehaviour
{
    public Tilemap baseTileMap;
    static HexMapHelper instance;

    public const float HexWidth = 1.0f;

    void Awake(){
        instance = this;
    }

    public static Vector3Int GetTileFromWorldPoint(Vector3 worldPos){
        return instance.baseTileMap.WorldToCell(worldPos);
    }

    public static Vector3 GetWorldPointFromTile(Vector3Int tilePos){
        return instance.baseTileMap.CellToWorld(tilePos);
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
        return Mathf.Clamp(Mathf.RoundToInt((altitude + 0.25f) / 0.5f), 0, 6);
    }

    public static float GetAltitudeFromLevel(int level) {
        return Mathf.Max(0, 0.25f + ((level - 1) * 0.5f));
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
}



