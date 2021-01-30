using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;

public class AltitudeIndicator : MonoBehaviour
{
    public Transform attachedObject;
    public List<MeshRenderer> cubes;
    public GameObject cubeHolder;
    public GameObject heightCylinder;
    public GameObject baseSprite;

    private float xzScale;

    private float primitiveHeight = 2f; //The default height of our cylinder. If we switch out for another mesh, we will need to adjust this.

    // Start is called before the first frame update
    void Awake()
    {
        xzScale = heightCylinder.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        float objectRelativeAltitude = attachedObject.position.y - HexMapUI.currentUIMapAltitude;

        //Set the cylinder position to midway between object and grid
        heightCylinder.transform.position = new Vector3(attachedObject.position.x, HexMapUI.currentUIMapAltitude + (objectRelativeAltitude / 2f), attachedObject.position.z);
        heightCylinder.transform.localScale = new Vector3(xzScale, objectRelativeAltitude / primitiveHeight, xzScale);
        cubeHolder.transform.position = new Vector3(attachedObject.position.x, 0, attachedObject.position.z);

        //Set the little target sprite to grid height
        baseSprite.transform.position = new Vector3(attachedObject.position.x, HexMapUI.currentUIMapAltitude, attachedObject.position.z);
        baseSprite.GetComponent<SpriteRenderer>().color = HexMapHelper.GetLevelColor(HexMapUI.currentUIMapLevel);

        /*float lowBounds = Mathf.Min(attachedObject.position.y, HexMapUI.currentUIMapAltitude);
        float highBounds = Mathf.Max(attachedObject.position.y, HexMapUI.currentUIMapAltitude);

        //Hide or show height cubes
        for(int cubeI = 0; cubeI < cubes.Count; cubeI++){
            int level = cubeI + 1;
            if(HexMapHelper.GetAltitudeFromLevel(level) > lowBounds && HexMapHelper.GetAltitudeFromLevel(level) < highBounds){
                cubes[cubeI].enabled = true;
            }else{
                cubes[cubeI].enabled = false;
            }
        }
        */
    }

    //[EventListener]
    //void OnAltitudeUIChange()
}
