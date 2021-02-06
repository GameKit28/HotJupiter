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

    public List<Hexasphere> tilemaps = new List<Hexasphere>();
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
        //The base sphere (the planet) behaves a little differently
        //tilemaps[0].style = currentUIMapLevel == 0 ? STYLE.ShadedWireframe : STYLE.Shaded;
        //tilemaps[0].GetComponent<SphereCollider>().enabled = currentUIMapLevel == 0;

        //The grids can be simply enabled or disabled
        for(int levelIndex = 0; levelIndex < tilemaps.Count; levelIndex++){
            tilemaps[levelIndex].gameObject.SetActive(levelIndex == currentUIMapLevel);
        }
    }
}



