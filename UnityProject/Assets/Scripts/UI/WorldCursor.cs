using System.Collections;
using System.Collections.Generic;
using HexasphereGrid;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldCursor : MonoBehaviour
{
    public Camera cursorCamera;

    public float scrollThreshold = 0.1f;

    private TileCoords _highlightedTile;
    public TileCoords HighlightedTile {
        get{
            return _highlightedTile;
        }
    }

    private Vector3 _cursorWorldPos = Vector3.zero;
    public Vector3 CursorWorldPosition {
        get {return _cursorWorldPos;}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _cursorWorldPos = GetPlaneIntersection();
        _highlightedTile = HexMapHelper.GetTileFromWorldPoint(_cursorWorldPos);
        transform.position = HexMapHelper.GetWorldPointFromTile(_highlightedTile, HexMapUI.currentUIMapLevel);
    }

    Vector3 GetPlaneIntersection(){
        RaycastHit hitData;
        Ray ray = cursorCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitData)){
            return hitData.point;
        }else{
            return Vector3.zero;
        }
    }
}
