using UnityEngine;

public static class HexExtensions {

    public enum PentagonTraversalStrategy
    {
        LeanLeft,
        LeanRight,
        RandomForward
    }

    private static TileCoords GetFacingWhenEnteringPentagon(TileCoords currentHex, TileCoords newPenta, PentagonTraversalStrategy strategy){
        if(strategy == PentagonTraversalStrategy.LeanLeft){
            return HexMapHelper.GetTileInPentaDirection(newPenta, currentHex, PentaDirection.BackwardRight);
        }else{
            return currentHex; //Face backwards as default
        }
    }

    private static TileCoords GetNewTileWhenLeavingPentagon(TileCoords currentPenta, TileCoords currentFacing, HexDirection hexDirection, PentagonTraversalStrategy strategy){
        PentaDirection pentaDirection = PentaDirection.Forward;//default
        if(strategy == PentagonTraversalStrategy.LeanLeft){
            switch(hexDirection) {
                case HexDirection.Forward:
                    pentaDirection = PentaDirection.Forward;
                    break;
                case HexDirection.ForwardRight:
                    pentaDirection = PentaDirection.ForwardRight;
                    break;
                case HexDirection.BackwardRight:
                    pentaDirection = PentaDirection.BackwardRight;
                    break;    
                case HexDirection.Backward:
                case HexDirection.BackwardLeft:
                    pentaDirection = PentaDirection.BackwardLeft;
                    break;
                case HexDirection.ForwardLeft:
                    pentaDirection = PentaDirection.ForwardLeft;
                    break;
            }
        }else{
            pentaDirection = PentaDirection.Forward;
        }
        return HexMapHelper.GetTileInPentaDirection(currentPenta, currentFacing, pentaDirection);
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
                //We're stepping out of a Pentagon using HexDirection. What do we do?
                newTile = GetNewTileWhenLeavingPentagon(currentTile, currentFacing, direction, strategy);
            }
            direction = HexDirection.Forward;

            if(HexMapHelper.GetTileShape(newTile) == TileShape.Hexagon){
                currentFacing = HexMapHelper.GetTileInHexDirection(newTile, currentTile, HexDirection.Backward);
            }else{
                //We're stepping into a Pentagon with Hex Directions. What do we do?
                currentFacing = GetFacingWhenEnteringPentagon(currentTile, newTile, strategy);
            }

            currentTile = newTile;
        }
        return new TileWithFacing() { position = currentTile, facing = currentFacing };
    }

    public static TileWithFacing Face(this TileWithFacing startVec, HexDirection direction){
        TileCoords newFacing;
        if(HexMapHelper.GetTileShape(startVec.position) == TileShape.Hexagon){
            newFacing = HexMapHelper.GetTileInHexDirection(startVec.position, startVec.facing, direction);
        }else{
            newFacing = GetNewTileWhenLeavingPentagon(startVec.position, startVec.facing, direction, PentagonTraversalStrategy.LeanLeft);
        }
        return new TileWithFacing() { position = startVec.position, facing = newFacing };
    }

    public static HexDirection RotateClockwise(this HexDirection startingDirection, int steps = 1) {
        return (HexDirection)(MathHelper.Mod(((int)startingDirection) + steps, 6));
    }

    public static HexDirection RotateCounterClockwise(this HexDirection startingDirection, int steps = 1) {
        return (HexDirection)(MathHelper.Mod(((int)startingDirection) - steps, 6));
    }
}
