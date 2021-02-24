using UnityEngine;

public interface IHaveTilePosition
{
    TileCoords GetPivotTilePosition();
    TileLevel GetPivotTileLevel();
}