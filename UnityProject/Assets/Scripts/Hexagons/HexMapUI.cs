using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Tilemaps;
using MeEngine.Events;
using HexasphereGrid;

namespace HotJupiter
{
    public class HexMapUI : MonoBehaviour
    {
        public static class Events {
            public struct UIMapLevelChanged : IEvent { public int previousMapLevel; public int newMapLevel; }
        }
        static HexMapUI instance;

        public static EventPublisher eventPublisher { get; private set; } = new EventPublisher();

        public List<HexGridSphere> tileSpheres = new List<HexGridSphere>();
        private List<Material> tileSphereMaterials = new List<Material>();
        public Material gridMaterial;
        
        public WorldCursor cursor;
        

        public float scrollThreshold = 0.1f;

        public int startingUIMapLevel = 2;

        private const string shaderWorldPosVariable = "_Center";

        public static int currentUIMapLevel {
            get {
                return instance._UIMapLevel;
            }
        }
        private int _UIMapLevel;

        public static HexGridSphere currentTilemap {
            get {
                return instance.tileSpheres[instance._UIMapLevel];
            }
        }

        public static float currentUIMapAltitude {
            get {
                return HexMapHelper.GetAltitudeFromLevel(instance._UIMapLevel);
            }
        }

        public static void SetUIMapLevel(int newLevel){

            int previousLevel = instance._UIMapLevel;
            instance._UIMapLevel = Mathf.Clamp(newLevel, 0, instance.tileSpheres.Count - 1);

            instance.HideAllButCurrentUILevel();
            eventPublisher.Publish(new Events.UIMapLevelChanged() { previousMapLevel = previousLevel, newMapLevel = newLevel });
        }

        void Awake(){
            instance = this;
            _UIMapLevel = startingUIMapLevel;
        }

        void Start(){
            HideAllButCurrentUILevel();

            if(TileLevel.MAX != instance.tileSpheres.Count - 1){
                Debug.LogWarning($"HexMapUI expects to have {TileLevel.MAX + 1} tileSperes to match the HexMapHelper MaxLevel.");
            }

            foreach(HexGridSphere sphere in tileSpheres){
                MeshRenderer renderer = sphere.GetComponentInChildren<MeshRenderer>();
                renderer.material = new Material(gridMaterial);
                renderer.material.color = sphere.Color;
                tileSphereMaterials.Add(renderer.material);
            }
        }

        void Update(){
            if((Input.mouseScrollDelta.y > scrollThreshold || Input.GetKeyDown(KeyCode.KeypadPlus)) && _UIMapLevel < instance.tileSpheres.Count - 1){
                SetUIMapLevel(_UIMapLevel + 1);
            }else if((Input.mouseScrollDelta.y < -scrollThreshold  || Input.GetKeyDown(KeyCode.KeypadMinus)) && _UIMapLevel > 0) {
                SetUIMapLevel(_UIMapLevel - 1);
            }

            //Update Grid Shader Based on Mouse Position
            Vector3 cursorWorldPosition = cursor.CursorWorldPosition;
            tileSphereMaterials[_UIMapLevel].SetVector(shaderWorldPosVariable, cursorWorldPosition);
        }

        private void HideAllButCurrentUILevel(){
            //The grids can be simply enabled or disabled
            for(int levelIndex = 0; levelIndex < tileSpheres.Count; levelIndex++){
                tileSpheres[levelIndex].gameObject.SetActive(levelIndex == currentUIMapLevel);
            }
        }
        public static Color GetLevelColor(int level) {
            return instance.tileSpheres[Mathf.Clamp(level, 0, instance.tileSpheres.Count - 1)].Color;
        }
    }
}