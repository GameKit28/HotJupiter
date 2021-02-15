using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Footprint", menuName = "HotJupiter/Footprint", order = 0)]
public class Footprint : ScriptableObject 
{
    public List<RelativeFootprint> footprintParts = new List<RelativeFootprint>();
    public TileObstacleType obstacleType;

    public List<TileWithLevel> GetAllTilesInFootprint(TileWithFacing pivotVec, int pivotLevel){
        List<TileWithLevel> footprintList = new List<TileWithLevel>();
        foreach (var part in footprintParts)
        {
            TileWithFacing newVec = pivotVec;
            if(part.relativePosStep1.step > 0){
                newVec = newVec.Traverse(part.relativePosStep1.direction, part.relativePosStep1.step);
            }
            if(part.relativePosStep2.step > 0){
                newVec = newVec.Traverse(part.relativePosStep2.direction, part.relativePosStep2.step);
            }
            footprintList.Add(new TileWithLevel(){position = newVec.position, level = pivotLevel + part.relativeLevel} );
        }
        return footprintList;
    }
}

[System.Serializable]
public struct TileWithLevel {
    public TileCoords position;
    public int level;
}

[System.Serializable]
public struct DirectionStep
{
    public HexDirection direction;
    public int step;
}

[System.Serializable]
public struct RelativeFootprint
{
    public int relativeLevel;
    public DirectionStep relativePosStep1;
    public DirectionStep relativePosStep2;
}

public enum TileObstacleType{
    solid, //Only one object can occupy this space. Other objects like ships cannot pass through this tile.
    semisolid, //Only one object can occupy this space, but other objects can pass through.
    empty //Does not preclude other objects from occupying this tile.
}