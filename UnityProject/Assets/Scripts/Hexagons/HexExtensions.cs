using UnityEngine;

public static class HexExtensions {
    public static TileCoords Traverse(this TileCoords startTile, ref Vector3 forwardVector, HexDirection direction, int steps = 1) {
        
        TileCoords currentTile = startTile;
        for(int step = 0; step < steps; step++) 
        {
            TileCoords newTile = HexMapHelper.GetTileInDirection(startTile, forwardVector, direction);
            forwardVector = HexMapHelper.GetFacingVector(currentTile, newTile);
            currentTile = newTile;
        }
        return currentTile;
    }

    public static HexDirection RotateClockwise(this HexDirection startingDirection, int steps = 1) {
        return (HexDirection)(MathHelper.Mod(((int)startingDirection) + steps, 6));
    }

    public static HexDirection RotateCounterClockwise(this HexDirection startingDirection, int steps = 1) {
        return (HexDirection)(MathHelper.Mod(((int)startingDirection) - steps, 6));
    }
}
