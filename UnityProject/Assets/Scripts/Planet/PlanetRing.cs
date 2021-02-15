using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRing : MonoBehaviour
{
    public LineRenderer line;
    public int level;
    public int facingIndex = 0;

    public int maxLength = 20;

    TileWithFacing startVec;

    // Start is called before the first frame update
    void Start()
    {
        TileCoords startTile = HexMapHelper.GetTileFromWorldPoint(transform.position);
        startVec = new TileWithFacing() {
            position = startTile,
            facing = HexMapHelper.GetNeighborTiles(startTile)[facingIndex]
            };

        List<Vector3> lineNodes = new List<Vector3>();
        TileWithFacing currentVec = startVec;
        for(int nodeIndex = 0; nodeIndex < maxLength; nodeIndex++){
            currentVec = currentVec.Traverse(HexDirection.Forward);
            if(currentVec.position == startVec.position) {
                break;
            }
            lineNodes.Add(HexMapHelper.GetWorldPointFromTile(currentVec.position, level));
        }
        line.positionCount = lineNodes.Count;
        line.SetPositions(lineNodes.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
