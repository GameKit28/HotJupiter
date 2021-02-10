using UnityEngine;

public static class HexExtensions {
    public static TileWithFacing Traverse(this TileWithFacing startVec, HexDirection direction, int steps = 1) {
        
        TileCoords currentTile = startVec.position;
        TileCoords currentFacing = startVec.facing;
        for(int step = 0; step < steps; step++) 
        {
            TileCoords newTile = HexMapHelper.GetTileInDirection(currentTile, HexMapHelper.GetFacingVector(currentTile, currentFacing), direction);
            currentFacing = HexMapHelper.GetTileInDirection(newTile, HexMapHelper.GetFacingVector(currentTile, newTile), HexDirection.Forward);
            currentTile = newTile;
            Debug.DrawRay(HexMapHelper.GetWorldPointFromTile(currentTile), HexMapHelper.GetFacingVector(currentTile, currentFacing), Color.cyan, 100);
            Debug.Log("Pathing to " + newTile);
        }
        return new TileWithFacing() { position = currentTile, facing = currentFacing };
    }

    public static TileWithFacing Face(this TileWithFacing startVec, HexDirection direction){
        TileCoords newFacing = HexMapHelper.GetTileInDirection(startVec.position, HexMapHelper.GetFacingVector(startVec.position, startVec.facing), direction);
        return new TileWithFacing() { position = startVec.position, facing = newFacing };
    }

    public static HexDirection RotateClockwise(this HexDirection startingDirection, int steps = 1) {
        return (HexDirection)(MathHelper.Mod(((int)startingDirection) + steps, 6));
    }

    public static HexDirection RotateCounterClockwise(this HexDirection startingDirection, int steps = 1) {
        return (HexDirection)(MathHelper.Mod(((int)startingDirection) - steps, 6));
    }
}
