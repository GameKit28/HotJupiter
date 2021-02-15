using UnityEngine;

public interface IHaveTileFootprint{
    Footprint GetFootprint();
    TileCoords GetPivotTilePosition();
    int GetPivotTileLevel();
}