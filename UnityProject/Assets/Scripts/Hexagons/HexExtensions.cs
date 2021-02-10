using UnityEngine;

public static class HexExtensions {
    public static TileCoords Traverse(this TileCoords startTile, TileCoords startTileFacing, HexDirection direction, out TileCoords endFacing, int steps = 1) {
        
        TileCoords currentTile = startTile;
        TileCoords currentFacing = startTileFacing;
        for(int step = 0; step < steps; step++) 
        {
            TileCoords newTile = HexMapHelper.GetTileInDirection(currentTile, HexMapHelper.GetFacingVector(currentTile, currentFacing), direction);
            currentFacing = HexMapHelper.GetTileInDirection(newTile, HexMapHelper.GetFacingVector(currentTile, newTile), HexDirection.Forward);
            currentTile = newTile;
            Debug.DrawRay(HexMapHelper.GetWorldPointFromTile(currentTile), HexMapHelper.GetFacingVector(currentTile, currentFacing), Color.cyan, 100);
            Debug.Log("Pathing to " + newTile);
        }
        endFacing = currentFacing;
        return currentTile;
    }

    public static HexDirection RotateClockwise(this HexDirection startingDirection, int steps = 1) {
        return (HexDirection)(MathHelper.Mod(((int)startingDirection) + steps, 6));
    }

    public static HexDirection RotateCounterClockwise(this HexDirection startingDirection, int steps = 1) {
        return (HexDirection)(MathHelper.Mod(((int)startingDirection) - steps, 6));
    }
}
