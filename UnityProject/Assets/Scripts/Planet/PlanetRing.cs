using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRing : MonoBehaviour, IHaveTileFootprint
{
    public LineRenderer line;
    public TileLevel level;
    public int facingIndex = 0;

    public int maxLength = 20;

    StaticFootprint footprint;

    // Start is called before the first frame update
    void Start()
    {
        TileCoords startTile = HexMapHelper.GetTileFromWorldPoint(transform.position);
        TileWithFacing startVec = new TileWithFacing() {
            position = startTile,
            facing = HexMapHelper.GetNeighborTiles(startTile)[facingIndex],
            level = level
            };

        List<FootprintTile> footprintParts = new List<FootprintTile>();
        footprintParts.Add(new FootprintTile(startVec.position, startVec.level, TileObstacleType.Solid));

        List<Vector3> lineNodes = new List<Vector3>();
        TileWithFacing currentVec = startVec;
        for(int nodeIndex = 0; nodeIndex < maxLength; nodeIndex++){
            currentVec = currentVec.TraversePlanar(HexDirection.Forward);
            if(currentVec.position == startVec.position) {
                break;
            }
            lineNodes.Add(HexMapHelper.GetWorldPointFromTile(currentVec.position, level));
            footprintParts.Add(new FootprintTile(currentVec.position, level, TileObstacleType.Solid));
        }
        line.positionCount = lineNodes.Count;
        line.SetPositions(lineNodes.ToArray());

        footprint = new StaticFootprint(this, footprintParts);
    }
    
    public FootprintBase GetFootprint()
    {
        return footprint;
    }
}
