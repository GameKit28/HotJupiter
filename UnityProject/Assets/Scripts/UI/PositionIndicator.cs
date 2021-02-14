using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;
using HexasphereGrid;

public class PositionIndicator : MonoBehaviour
{
    [SerializeReference]
    public GameObject attachedObject; // Must derive from IHaveTilePosition
    private Mesh polygon;

    private int colorBlockID;

    private IHaveTilePosition tilePositionObject;

    private int[] trianglesHex = new int[18];
    private int[] trianglesPenta = new int[18];
    private Vector3[] normals = new Vector3[7];
    private Vector2[] uvs = new Vector2[7];

    private TileCoords lastTilePos;
    private int lastLevel;

    void Awake()
    {
        polygon = new Mesh();
        polygon.MarkDynamic();
        GetComponent<MeshFilter>().mesh = polygon;

        if (attachedObject != null)
            tilePositionObject = attachedObject.GetComponent<IHaveTilePosition>();

        //Generate Triangles for Hex Shaped Polygon
        for(int i = 0; i < 6; i++){
            trianglesHex[i * 3] = 0;
            trianglesHex[(i * 3) + 1] = (i % 6) + 1;
            trianglesHex[(i * 3) + 2] = ((i + 1) % 6) + 1;
        }

        //Generate Triangles for Pentagon Shaped Polygon
        for(int i = 0; i < 5; i++){
            trianglesPenta[i * 3] = 0;
            trianglesPenta[(i * 3) + 1] = (i % 5) + 1;
            trianglesPenta[(i * 3) + 2] = ((i + 1) % 5) + 1;
        }
        for(int i = 0; i < 7; i++){
            normals[i] = Vector3.up;
        }
        uvs[0] = new Vector2(0.5f, 0);
        for(int i = 1; i < 7; i++){
            uvs[i] = new Vector2(0.5f, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(lastTilePos != tilePositionObject.GetTilePosition() || lastLevel != tilePositionObject.GetLevel()){
            Tile tile = HexMapUI.GetHexasphereTile(tilePositionObject.GetTilePosition(), tilePositionObject.GetLevel());

            if(tile != null) {
                Vector3[] vertices = new Vector3[7];
                vertices[0] = transform.InverseTransformPoint(tile.center * 2f * HexMapHelper.GetRadialOffsetFromLevel(tilePositionObject.GetLevel()));
                for(int i = 0; i < tile.vertices.Length; i++){
                    vertices[i + 1] = transform.InverseTransformPoint(tile.vertices[i] * 2f * HexMapHelper.GetRadialOffsetFromLevel(tilePositionObject.GetLevel()));
                }

                polygon.SetVertices(vertices);
                polygon.SetTriangles(
                    tile.vertices.Length == 6 ? trianglesHex : trianglesPenta,
                    0);
                polygon.SetNormals(normals);
                polygon.SetUVs(0, uvs);
                polygon.RecalculateBounds();

                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                GetComponent<MeshRenderer>().material.color = HexMapUI.GetLevelColor(tilePositionObject.GetLevel());
            }

            lastLevel = tilePositionObject.GetLevel();
            lastTilePos = tilePositionObject.GetTilePosition();
        }
    }
}
