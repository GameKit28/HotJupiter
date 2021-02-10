using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;

public class AltitudeIndicator : MonoBehaviour
{
    public Transform attachedObject;
    public GameObject heightCylinder;
    public GameObject baseSprite;

    private Vector3 centerPoint = Vector3.zero; //Assume Planet is at Vector3.zero for now

    private const float xzScale = 0.05f;

    // Start is called before the first frame update
    void Awake()
    {
        //xzScale = heightCylinder.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        float objectRelativeAltitude = attachedObject.position.y - HexMapUI.currentUIMapAltitude;

        //normal
        Vector3 normal = (attachedObject.position - centerPoint).normalized;
        Vector3 gridIntercectPoint = centerPoint + (normal * HexMapHelper.GetRadialOffsetFromLevel(HexMapUI.currentUIMapLevel));

        //Set the cylinder position to midway between object and grid
        heightCylinder.transform.position = (attachedObject.position + gridIntercectPoint) / 2f;
        heightCylinder.transform.localScale = new Vector3(xzScale, xzScale, Vector3.Distance(attachedObject.position, gridIntercectPoint));
        heightCylinder.transform.rotation = Quaternion.LookRotation(normal);

        //Set the little target sprite to grid height
        baseSprite.transform.position = gridIntercectPoint;
        baseSprite.transform.rotation = Quaternion.LookRotation(normal);
        baseSprite.GetComponent<SpriteRenderer>().color = HexMapHelper.GetLevelColor(HexMapUI.currentUIMapLevel);
    }
}
