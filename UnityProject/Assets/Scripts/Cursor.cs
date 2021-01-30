using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cursor : MonoBehaviour
{
    public Camera cursorCamera;

    public float scrollThreshold = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cursorWorldPos = GetPlaneIntersection();
        Vector3Int selectedTile = HexMapHelper.GetTileFromWorldPoint(cursorWorldPos);
        //Debug.Log("SelectedTile = " + selectedTile);
        transform.position = HexMapHelper.GetWorldPointFromTile(selectedTile, HexMapUI.currentUIMapLevel);
    }

    Vector3 GetPlaneIntersection(){
        Ray ray = cursorCamera.ScreenPointToRay(Input.mousePosition);
        float delta = ray.origin.y - HexMapUI.currentUIMapAltitude;
        Vector3 dirNorm = ray.direction / ray.direction.y;
        return ray.origin - dirNorm * delta;
    }
}
