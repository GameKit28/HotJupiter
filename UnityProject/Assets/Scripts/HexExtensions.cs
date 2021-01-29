using UnityEngine;

public static class HexExtensions {
    public static Vector3Int Traverse(this Vector3Int currentTile, HexDirection direction, int steps = 1) {
        switch(direction){
            case HexDirection.NorthEast:
                return currentTile + (new Vector3Int(0,1,0) * steps);
            case HexDirection.East:
                return currentTile + (new Vector3Int(1,0,0) * steps);
            case HexDirection.SouthEast:
                return currentTile + (new Vector3Int(0,-1,0) * steps);
            case HexDirection.SouthWest:
                return currentTile + (new Vector3Int(-1,-1,0) * steps);
            case HexDirection.West:
                return currentTile + (new Vector3Int(-1,0,0) * steps);
            case HexDirection.NorthWest:
                return currentTile + (new Vector3Int(-1,1,0) * steps);
            default:
                return currentTile;
        }
    }
}