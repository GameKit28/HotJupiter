using UnityEngine;

public static class HexExtensions {

    public enum PentagonTraversalStrategy
    {
        LeanLeft,
        LeanRight,
        RandomForward
    }

    public static TileWithFacing Traverse(this TileWithFacing startVec, HexDirection direction, int steps = 1, PentagonTraversalStrategy strategy = PentagonTraversalStrategy.LeanLeft) {
        
        TileCoords currentTile = startVec.position;
        TileCoords currentFacing = startVec.facing;
        for(int step = 0; step < steps; step++) 
        {
            TileCoords newTile;
            if(HexMapHelper.GetTileShape(currentTile) == TileShape.Hexagon) {
                newTile = HexMapHelper.GetTileInHexDirection(currentTile, currentFacing, direction);
            }else{
                //Uh oh. It's a pentagon. What do we do?
                PentaDirection pentaDirection;
                if(strategy == PentagonTraversalStrategy.LeanLeft){
                    switch(direction) {
                        case HexDirection.Forward:
                        case HexDirection.ForwardLeft:
                            pentaDirection = PentaDirection.ForwardLeft;
                            break;
                        case HexDirection.ForwardRight :
                            pentaDirection = PentaDirection.ForwardRight;
                            break;
                        case HexDirection.BackwardLeft :
                            pentaDirection = PentaDirection.BackwardLeft;
                            break;
                        case HexDirection.BackwardRight :
                            pentaDirection = PentaDirection.BackwardRight;
                            break;
                        case HexDirection.Backward :
                        default :
                            pentaDirection = PentaDirection.Backward;
                            break;
                    }
                }else{
                    pentaDirection = PentaDirection.Backward;
                }
                newTile = HexMapHelper.GetTileInPentaDirection(currentTile, HexMapHelper.GetFacingVector(currentFacing, currentFacing), pentaDirection);
            }

            currentFacing = HexMapHelper.GetTileInHexDirection(newTile, currentTile, HexDirection.Backward);
            currentTile = newTile;
            //Debug.DrawRay(HexMapHelper.GetWorldPointFromTile(currentTile), HexMapHelper.GetFacingVector(currentTile, currentFacing), Color.cyan, 100);
            //Debug.Log("Pathing to " + newTile);
        }
        return new TileWithFacing() { position = currentTile, facing = currentFacing };
    }

    public static TileWithFacing Face(this TileWithFacing startVec, HexDirection direction){
        TileCoords newFacing = HexMapHelper.GetTileInHexDirection(startVec.position, startVec.facing, direction);
        return new TileWithFacing() { position = startVec.position, facing = newFacing };
    }

    public static HexDirection RotateClockwise(this HexDirection startingDirection, int steps = 1) {
        return (HexDirection)(MathHelper.Mod(((int)startingDirection) + steps, 6));
    }

    public static HexDirection RotateCounterClockwise(this HexDirection startingDirection, int steps = 1) {
        return (HexDirection)(MathHelper.Mod(((int)startingDirection) - steps, 6));
    }
}
