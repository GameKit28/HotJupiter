using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;
using HexasphereGrid;

public class PositionIndicator : MonoBehaviour
{
    [SerializeReference]
    public GameObject attachedObject; // Must derive from IHaveTilePosition
    private List<Mesh> polygons = new List<Mesh>();
    private Mesh combinedMesh;

    private IHaveTileFootprint tilePositionObject;
    private IHaveTileFacing tileFacingObject;

    private int[] trianglesHex = new int[18]; //6 * 3
    private int[] trianglesPenta = new int[18]; // 6 * 3
    private Vector3[] normals = new Vector3[7]; // 6 + 1(center)
    private Vector2[] uvs = new Vector2[7]; //6 + 1(center)

    CombineInstance[] combineInstances;

    private TileCoords lastTilePos;
    private int lastLevel;

    void Awake()
    {
        combinedMesh = new Mesh();
        combinedMesh.MarkDynamic();

        GetComponent<MeshFilter>().mesh = combinedMesh;

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

    private void Start() {
        if (attachedObject != null) {
            tilePositionObject = attachedObject.GetComponent<IHaveTileFootprint>();
            tileFacingObject = attachedObject.GetComponent<IHaveTileFacing>();
        }
    }

    void Initialize(int size){
        for(int mIndex = 0; mIndex < size; mIndex++){
            Mesh polygon = new Mesh();
            polygon.MarkDynamic();
            polygons.Add(polygon);
        }
        combineInstances = new CombineInstance[size];
    }

    // Update is called once per frame
    void Update()
    {
        if(lastTilePos != tilePositionObject.GetPivotTilePosition() || lastLevel != tilePositionObject.GetPivotTileLevel()){
            Tile pivotTile = HexMapUI.GetHexasphereTile(tilePositionObject.GetPivotTilePosition(), tilePositionObject.GetPivotTileLevel());

            if(pivotTile != null) {
                TileWithFacing pivotVec = new TileWithFacing(){ position = tilePositionObject.GetPivotTilePosition(), facing = tileFacingObject.GetTileFacing() };
                List<TileWithLevel> footParts = tilePositionObject.GetFootprint().GetAllTilesInFootprint(pivotVec, tilePositionObject.GetPivotTileLevel());
                
                if(polygons.Count == 0) Initialize(footParts.Count);
                
                for(int footPartIndex = 0; footPartIndex < footParts.Count; footPartIndex++){
                    TileWithLevel footPart = footParts[footPartIndex];
                    Tile footTile = HexMapUI.GetHexasphereTile(footPart.position, footPart.level);
                    Vector3[] vertices = new Vector3[7];
                    vertices[0] = transform.InverseTransformPoint(footTile.center * 2f * HexMapHelper.GetRadialOffsetFromLevel(footPart.level));
                    for(int i = 0; i < footTile.vertices.Length; i++){
                        vertices[i + 1] = transform.InverseTransformPoint(footTile.vertices[i] * 2f * HexMapHelper.GetRadialOffsetFromLevel(footPart.level));
                    }

                    polygons[footPartIndex].SetVertices(vertices);
                    polygons[footPartIndex].SetTriangles(
                        footTile.vertices.Length == 6 ? trianglesHex : trianglesPenta,
                        0);
                    polygons[footPartIndex].SetNormals(normals);
                    polygons[footPartIndex].SetUVs(0, uvs);
                    //polygon.RecalculateBounds();

                    combineInstances[footPartIndex].mesh = polygons[footPartIndex];
                }

                combinedMesh.CombineMeshes(combineInstances, true, false);
                combinedMesh.RecalculateBounds();

                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                GetComponent<MeshRenderer>().material.color = HexMapUI.GetLevelColor(tilePositionObject.GetPivotTileLevel());
            }

            lastLevel = tilePositionObject.GetPivotTileLevel();
            lastTilePos = tilePositionObject.GetPivotTilePosition();
        }
    }
}
