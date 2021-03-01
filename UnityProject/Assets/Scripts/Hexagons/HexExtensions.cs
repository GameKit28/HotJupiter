using UnityEngine;
using System.Collections.Generic;

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

    public static TileWithFacing TraversePlanar(this TileWithFacing startVec, HexDirection direction, int steps = 1, PentagonTraversalStrategy strategy = PentagonTraversalStrategy.LeanLeft) {
        TilePath path = new TilePath(startVec);
        return path.TraversePlanar(direction, steps, strategy).GetEndTile();
    }

    public static TilePath TraversePlanar(this TilePath activePath, HexDirection direction, int steps = 1, PentagonTraversalStrategy strategy = PentagonTraversalStrategy.LeanLeft){
        TileWithFacing currentTile = activePath.GetEndTile();
        for(int step = 0; step < steps; step++) 
        {
            TileCoords newTileCoords;
            if(HexMapHelper.GetTileShape(currentTile.position) == TileShape.Hexagon) {
                newTileCoords = HexMapHelper.GetTileInHexDirection(currentTile.position, currentTile.facing, direction);
            }else{
                //We're stepping out of a Pentagon using HexDirection. What do we do?
                newTileCoords = GetNewTileWhenLeavingPentagon(currentTile.position, currentTile.facing, direction, strategy);
            }
            direction = HexDirection.Forward;

            if(HexMapHelper.GetTileShape(newTileCoords) == TileShape.Hexagon){
                currentTile.facing = HexMapHelper.GetTileInHexDirection(newTileCoords, currentTile.position, HexDirection.Backward);
            }else{
                //We're stepping into a Pentagon with Hex Directions. What do we do?
                currentTile.facing = GetFacingWhenEnteringPentagon(currentTile.position, newTileCoords, strategy);
            }

            currentTile = new TileWithFacing(newTileCoords, currentTile.facing, currentTile.level);
            activePath.AppendTile(currentTile);
        }
        return activePath;
    }

    public static TileWithFacing TraverseVertical(this TileWithFacing startVec, int relativeAscention){
        TilePath path = new TilePath(startVec);
        return path.TraverseVertical(relativeAscention).GetEndTile();
    }

    public static TilePath TraverseVertical(this TilePath activePath, int relativeAscention){
        TileWithFacing currentTile = activePath.GetEndTile();
        for(int step = 0; step < Mathf.Abs(relativeAscention); step++){
            TileWithFacing newTile;
            if(relativeAscention > 0 && currentTile.level < TileLevel.MAX){
                newTile = new TileWithFacing(currentTile.position, currentTile.facing, currentTile.level + 1);
            }else if(relativeAscention < 0 && currentTile.level > TileLevel.MIN) {
                newTile = new TileWithFacing(currentTile.position, currentTile.facing, currentTile.level - 1);
            }else{
                throw new System.Exception($"Attempted to traverse to a TileLevel outside of bounds. {currentTile} Attempted Level: {currentTile.level + Mathf.Clamp(relativeAscention, -1, 1)}");
            }
            currentTile = newTile;
            activePath.AppendTile(currentTile);
        }
        return activePath;
    }

    public static TileWithFacing Face(this TileWithFacing startVec, HexDirection direction){
        TilePath path = new TilePath(startVec);
        return path.Face(direction).GetEndTile();
    }

    public static TilePath Face(this TilePath activePath, HexDirection direction){
        TileWithFacing currentTile = activePath.GetEndTile();
        TileCoords newFacing;
        if(HexMapHelper.GetTileShape(currentTile.position) == TileShape.Hexagon){
            newFacing = HexMapHelper.GetTileInHexDirection(currentTile.position, currentTile.facing, direction);
        }else{
            newFacing = GetNewTileWhenLeavingPentagon(currentTile.position, currentTile.facing, direction, PentagonTraversalStrategy.LeanLeft);
        }
        activePath.AppendTile(new TileWithFacing(currentTile.position, newFacing, currentTile.level));
        return activePath;
    }

    public static HexDirection RotateClockwise(this HexDirection startingDirection, int steps = 1) {
        return (HexDirection)(MathHelper.Mod(((int)startingDirection) + steps, 6));
    }

    public static HexDirection RotateCounterClockwise(this HexDirection startingDirection, int steps = 1) {
        return (HexDirection)(MathHelper.Mod(((int)startingDirection) - steps, 6));
    }
}
