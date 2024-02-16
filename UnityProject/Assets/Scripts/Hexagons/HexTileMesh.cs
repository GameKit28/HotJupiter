using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotJupiter
{
public class HexTileMesh
{
    private List<Mesh> polygons = new List<Mesh>();
    private Mesh combinedMesh;
    public Mesh GeneratedMesh { get {return combinedMesh;}}
    private CombineInstance[] combineInstances;

    private static int[] trianglesHex = new int[18]; //6 * 3
    private static int[] trianglesPenta = new int[18]; // 6 * 3
    private static Vector3[] normals = new Vector3[7]; // 6 + 1(center)
    private static Vector2[] uvs = new Vector2[7]; //6 + 1(center)

    static HexTileMesh(){
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

    public HexTileMesh(){
        combinedMesh = new Mesh();
        combinedMesh.MarkDynamic();
    }

    private void Initialize(int polygonCount){
        for(int mIndex = 0; mIndex < polygonCount; mIndex++){
            Mesh polygon = new Mesh();
            polygon.MarkDynamic();
            polygons.Add(polygon);
        }
        combineInstances = new CombineInstance[polygonCount];
    }

    public Mesh GenerateMeshFromTiles<TType>(List<TType> tiles, Transform parent)
        where TType : ITile
    {
        if(polygons.Count == 0) Initialize(tiles.Count);
            
        for(int footPartIndex = 0; footPartIndex < tiles.Count; footPartIndex++){
            ITile footPart = tiles[footPartIndex];
            HexasphereGrid.Tile footTile = HexMapHelper.GetHexasphereTile(footPart);
            if(footTile != null) {
                Vector3[] vertices = new Vector3[7];
                vertices[0] = parent.InverseTransformPoint(footTile.center * 2f * HexMapHelper.GetRadialOffsetFromLevel(footPart.level));
                for(int i = 0; i < footTile.vertices.Length; i++){
                    vertices[i + 1] = parent.InverseTransformPoint(footTile.vertices[i] * 2f * HexMapHelper.GetRadialOffsetFromLevel(footPart.level));
                }

                polygons[footPartIndex].SetVertices(vertices);
                polygons[footPartIndex].SetTriangles(
                    footTile.vertices.Length == 6 ? trianglesHex : trianglesPenta,
                    0);
                polygons[footPartIndex].SetNormals(normals);
                polygons[footPartIndex].SetUVs(0, uvs);

                combineInstances[footPartIndex].mesh = polygons[footPartIndex];
            }
        }

        combinedMesh.CombineMeshes(combineInstances, true, false);
        combinedMesh.RecalculateBounds();

        return combinedMesh;
    }

}
}
