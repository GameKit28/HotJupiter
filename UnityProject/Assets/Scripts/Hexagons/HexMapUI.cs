using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Tilemaps;
using MeEngine.Events;
using HexasphereGrid;

public class HexMapUI : MonoBehaviour
{
    public static class Events {
        public struct UIMapLevelChanged : IEvent { public int previousMapLevel; public int newMapLevel; }
    }

    public static EventPublisher eventPublisher { get; private set; } = new EventPublisher();

    public List<Hexasphere> tileSpheres = new List<Hexasphere>();
    static HexMapUI instance;

    public float scrollThreshold = 0.1f;

    public int startingUIMapLevel = 2;

    public static int currentUIMapLevel {
        get {
            return instance._UIMapLevel;
        }
    }
    private int _UIMapLevel;

    public static Hexasphere currentTilemap {
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
        instance._UIMapLevel = Mathf.Clamp(newLevel, 0, instance.tileSpheres.Count);

        instance.HideAllButCurrentUILevel();
        eventPublisher.Publish(new Events.UIMapLevelChanged() { previousMapLevel = previousLevel, newMapLevel = newLevel });
    }

    void Awake(){
        instance = this;
        _UIMapLevel = startingUIMapLevel;
    }

    void Start(){
        HideAllButCurrentUILevel();
    }

    void Update(){
        if((Input.mouseScrollDelta.y > scrollThreshold || Input.GetKeyDown(KeyCode.KeypadPlus)) && _UIMapLevel < instance.tileSpheres.Count - 1){
            SetUIMapLevel(_UIMapLevel + 1);
        }else if((Input.mouseScrollDelta.y < -scrollThreshold  || Input.GetKeyDown(KeyCode.KeypadMinus)) && _UIMapLevel > 0) {
            SetUIMapLevel(_UIMapLevel - 1);
        }
    }

    private void HideAllButCurrentUILevel(){
        //The grids can be simply enabled or disabled
        for(int levelIndex = 0; levelIndex < tileSpheres.Count; levelIndex++){
            tileSpheres[levelIndex].gameObject.SetActive(levelIndex == currentUIMapLevel);
        }
    }
    public static Color GetLevelColor(int level) {
        return instance.tileSpheres[Mathf.Clamp(level, 0, instance.tileSpheres.Count - 1)].wireframeColor;
    }

    public static Tile GetHexasphereTile(TileCoords tilePosition, int level){
        Hexasphere hexasphere = instance.tileSpheres[Mathf.Clamp(level, 0, instance.tileSpheres.Count - 1)];
        if(hexasphere.tiles != null){ 
            return hexasphere.tiles[tilePosition.index];
        }else{
            return null;
        }
    }
}



