using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FootprintTemplate", menuName = "HotJupiter/FootprintTemplate", order = 0)]
public class RelativeFootprintTemplate : ScriptableObject
{
    public List<RelativeFootprint> footprintParts = new List<RelativeFootprint>();
    public TileObstacleType obstacleType;
}

[System.Serializable]
public struct TileWithLevel {
    public TileCoords position;
    public int level;

    public override bool Equals(object obj)
    {
        if (!(obj is TileWithLevel))
          return false;

        TileWithLevel otherStruct = (TileWithLevel)obj;

        return this.level == otherStruct.level && this.position == otherStruct.position;
    }

    public override int GetHashCode()
    {
        return position.GetHashCode().WrapShift(2) ^ level.GetHashCode();
    }
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
    Empty = 0, //Does not preclude other objects from occupying this tile.
    Semisolid = 1, //Only one object can occupy this space, but other objects can pass through.
    Solid = 2, //Only one object can occupy this space. Other objects like ships cannot pass through this tile.
}