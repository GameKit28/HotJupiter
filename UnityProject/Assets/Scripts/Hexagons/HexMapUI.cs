using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using MeEngine.Events;

public class HexMapUI : MonoBehaviour
{
    public static class Events {
        public struct UIMapLevelChanged : IEvent { public int previousMapLevel; public int newMapLevel; }
    }

    public List<Tilemap> tilemaps = new List<Tilemap>();
    static HexMapUI instance;

    public float scrollThreshold = 0.1f;

    public int startingUIMapLevel = 2;

    public static int currentUIMapLevel {
        get {
            return instance._UIMapLevel;
        }
    }
    private int _UIMapLevel;

    public static Tilemap currentTilemap {
        get {
            return instance.tilemaps[instance._UIMapLevel];
        }
    }

    public static float currentUIMapAltitude {
        get {
            return HexMapHelper.GetAltitudeFromLevel(instance._UIMapLevel);
        }
    }

    public static void SetUIMapLevel(int newLevel){

        int previousLevel = instance._UIMapLevel;
        instance._UIMapLevel = Mathf.Clamp(newLevel, 0, instance.tilemaps.Count);

        instance.HideAllButCurrentUILevel();
        EventManager.Publish(new Events.UIMapLevelChanged() { previousMapLevel = previousLevel, newMapLevel = newLevel });
    }

    void Awake(){
        instance = this;
        _UIMapLevel = startingUIMapLevel;
    }

    void Start(){
        HideAllButCurrentUILevel();
    }

    void Update(){
        if(Input.mouseScrollDelta.y > scrollThreshold && _UIMapLevel < instance.tilemaps.Count - 1){
            SetUIMapLevel(_UIMapLevel + 1);
        }else if(Input.mouseScrollDelta.y < -scrollThreshold && _UIMapLevel > 0) {
            SetUIMapLevel(_UIMapLevel - 1);
        }
    }

    private void HideAllButCurrentUILevel(){
        for(int levelI = 0; levelI < tilemaps.Count; levelI++){
            tilemaps[levelI].gameObject.SetActive(levelI == currentUIMapLevel);
        }
    }
}



