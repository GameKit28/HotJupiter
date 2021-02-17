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
    Solid, //Only one object can occupy this space. Other objects like ships cannot pass through this tile.
    Semisolid, //Only one object can occupy this space, but other objects can pass through.
    Empty //Does not preclude other objects from occupying this tile.
}