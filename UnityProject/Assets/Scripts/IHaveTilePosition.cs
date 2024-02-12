using UnityEngine;

namespace HotJupiter{
    public interface IHaveTilePosition
    {
        TileCoords GetPivotTilePosition();
        TileLevel GetPivotTileLevel();
    }
}