using UnityEngine;

public static class HexExtensions {
    public static Vector3Int Traverse(this Vector3Int startTile, HexDirection direction, int steps = 1) {
        
        Vector3Int currentTile = startTile;
        for(int step = 0; step < steps; step++) 
        {
            if(currentTile.y % 2 == 0) {
                switch(direction){
                    case HexDirection.NorthEast:
                        currentTile += new Vector3Int(0,1,0);
                        break;
                    case HexDirection.East:
                        currentTile +=  new Vector3Int(1,0,0);
                        break;
                    case HexDirection.SouthEast:
                        currentTile +=  new Vector3Int(0,-1,0);
                        break;
                    case HexDirection.SouthWest:
                        currentTile +=  new Vector3Int(-1,-1,0);
                        break;
                    case HexDirection.West:
                        currentTile +=  new Vector3Int(-1,0,0);
                        break;
                    case HexDirection.NorthWest:
                        currentTile +=  new Vector3Int(-1,1,0);
                        break;
                    default: break;
                }
            }else{
                switch(direction){
                    case HexDirection.NorthEast:
                        currentTile +=  new Vector3Int(1,1,0);
                        break;
                    case HexDirection.East:
                        currentTile +=  new Vector3Int(1,0,0);
                        break;
                    case HexDirection.SouthEast:
                        currentTile +=  new Vector3Int(1,-1,0);
                        break;
                    case HexDirection.SouthWest:
                        currentTile +=  new Vector3Int(0,-1,0);
                        break;
                    case HexDirection.West:
                        currentTile +=  new Vector3Int(-1,0,0);
                        break;
                    case HexDirection.NorthWest:
                        currentTile +=  new Vector3Int(0,1,0);
                        break;
                    default: break;
                }
            }
        }
        return currentTile;
    }

    public static HexDirection RotateClockwise(this HexDirection startingDirection, int steps = 1) {
        return (HexDirection)((((int)startingDirection) + steps) % 6);
    }

    public static HexDirection RotateCounterClockwise(this HexDirection startingDirection, int steps = 1) {
        return (HexDirection)((((int)startingDirection) - steps) % 6); //Kit Start Here. Mod of negative number doesn't work as expected
    }
}
