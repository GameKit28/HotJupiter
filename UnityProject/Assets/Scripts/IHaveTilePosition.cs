using UnityEngine;

public interface IHaveTilePosition
{
    TileCoords GetPivotTilePosition();
    int GetPivotTileLevel();
}