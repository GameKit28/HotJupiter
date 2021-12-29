using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FootprintTemplate", menuName = "HotJupiter/FootprintTemplate", order = 0)]
public class RelativeFootprintTemplate : ScriptableObject
{
    public List<RelativeFootprint> footprintParts = new List<RelativeFootprint>();
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
    public TileObstacleType obstacleType;
}

public enum TileObstacleType{
    Empty = 0, //Does not preclude other objects from occupying this tile.
    Semisolid = 1, //Only one object can occupy this space at the end of a turn, but other objects can pass through.
    Solid = 2, //Only one object can occupy this space at any moment. Other objects like ships cannot pass through this tile.
}