using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRing : MonoBehaviour, IHaveTileFootprint
{
    public LineRenderer line;
    public int level;
    public int facingIndex = 0;

    public int maxLength = 20;

    StaticFootprint footprint;

    // Start is called before the first frame update
    void Start()
    {
        TileCoords startTile = HexMapHelper.GetTileFromWorldPoint(transform.position);
        TileWithFacing startVec = new TileWithFacing() {
            position = startTile,
            facing = HexMapHelper.GetNeighborTiles(startTile)[facingIndex]
            };

        List<TileWithLevel> footprintParts = new List<TileWithLevel>();
        footprintParts.Add(new TileWithLevel() {position = startVec.position, level = level});

        List<Vector3> lineNodes = new List<Vector3>();
        TileWithFacing currentVec = startVec;
        for(int nodeIndex = 0; nodeIndex < maxLength; nodeIndex++){
            currentVec = currentVec.Traverse(HexDirection.Forward);
            if(currentVec.position == startVec.position) {
                break;
            }
            lineNodes.Add(HexMapHelper.GetWorldPointFromTile(currentVec.position, level));
            footprintParts.Add(new TileWithLevel(){position = currentVec.position, level = level});
        }
        line.positionCount = lineNodes.Count;
        line.SetPositions(lineNodes.ToArray());

        footprint = new StaticFootprint(footprintParts);
    }
    
    public FootprintBase GetFootprint()
    {
        return footprint;
    }
}
