using System.Collections;
using System.Collections.Generic;
using HexasphereGrid;
using MeEngine.Events;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HotJupiter {
    public class WorldCursor : MonoBehaviour
    {
        public Camera cursorCamera;

        public float scrollThreshold = 0.1f;

        private TileCoords _highlightedTile;
        public TileCoords HighlightedTile {
            get{
                return _highlightedTile;
            }
            private set{
                if(value != _highlightedTile){
                    shouldRegenerateMesh = true;
                }
                _highlightedTile = value;
            }
        }

        private Vector3 _cursorWorldPos = Vector3.zero;
        public Vector3 CursorWorldPosition {
            get {return _cursorWorldPos;}
        }

        [SerializeField] private MeshFilter hexMeshFilter;
        [SerializeField] private MeshRenderer hexMeshRenderer;
        private HexTileMesh hexTileMesh;
        private bool shouldRegenerateMesh = false;

        // Start is called before the first frame update
        void Start()
        {
            hexTileMesh = new HexTileMesh();
            hexMeshFilter.mesh = hexTileMesh.GeneratedMesh;

            HexMapUI.eventPublisher.SubscribeAll(this);
        }

        // Update is called once per frame
        void Update()
        {
            _cursorWorldPos = GetPlaneIntersection();
            HighlightedTile = HexMapHelper.GetTileFromWorldPoint(_cursorWorldPos);
            transform.position = HexMapHelper.GetWorldPointFromTile(_highlightedTile, HexMapUI.currentUIMapLevel);
        
            if(shouldRegenerateMesh){
                UpdateHexTile();
                shouldRegenerateMesh = false;
            }
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

        private void UpdateHexTile(){
            hexTileMesh.GenerateMeshFromTile(new Tile(HighlightedTile, HexMapUI.currentUIMapLevel), hexMeshFilter.transform);
            hexMeshRenderer.material.color = HexMapUI.GetLevelColor(HexMapUI.currentUIMapLevel);
        }

        [EventListener]
        void OnUILevelChanged(HexMapUI.Events.UIMapLevelChanged @event){
            shouldRegenerateMesh = true;
        }
    }
}
