using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cursor : MonoBehaviour
{
    public Camera cursorCamera;

    private static Vector3 planeCenter = Vector3.zero;
    private Plane plane = new Plane(Vector3.up, planeCenter);
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cursorWorldPos = GetPlaneIntersection();
        Vector3Int selectedTile = HexMapHelper.GetTileFromWorldPoint(cursorWorldPos);
        Debug.Log("SelectedTile = " + selectedTile);
        transform.position = HexMapHelper.GetWorldPointFromTile(selectedTile);
    }

    Vector3 GetPlaneIntersection(){
        Ray ray = cursorCamera.ScreenPointToRay(Input.mousePosition);
        float delta = ray.origin.y - planeCenter.y;
        Vector3 dirNorm = ray.direction / ray.direction.y;
        return ray.origin - dirNorm * delta;
    }
}
