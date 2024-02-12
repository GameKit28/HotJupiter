using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotJupiter {
public class CameraController : MonoBehaviour
{
    float cameraSpeedX = 5f;
    float cameraSpeedZ = 15f;

    public PlanetSizer focalPlanet;
    public GameObject cameraFocalPoint;
    public Transform focalTargetTransform;

    private float focalPointRadius {
        get {
            return focalPlanet.planetRadius + HexMapHelper.gridFirstAltitudeOffset;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 targetNormal = Vector3.Normalize(focalTargetTransform.position - focalPlanet.transform.position);
        cameraFocalPoint.transform.position = targetNormal * focalPointRadius;
        cameraFocalPoint.transform.rotation = Quaternion.LookRotation(focalTargetTransform.forward, targetNormal);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newFocalPointPos = cameraFocalPoint.transform.position;

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            newFocalPointPos += Camera.main.transform.forward * cameraSpeedZ * TimeManager.UIDeltaTime;
        }else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            newFocalPointPos += Camera.main.transform.forward * -cameraSpeedZ * TimeManager.UIDeltaTime;
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            newFocalPointPos += cameraFocalPoint.transform.right * -cameraSpeedX * TimeManager.UIDeltaTime;
        }else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            newFocalPointPos += cameraFocalPoint.transform.right * cameraSpeedX * TimeManager.UIDeltaTime;
        }

        Vector3 normal = (newFocalPointPos - this.transform.position).normalized;
        Vector3 lookAtNormal = ((newFocalPointPos + Camera.main.transform.forward) - this.transform.position).normalized;
   
        cameraFocalPoint.transform.position = this.transform.position + (normal * focalPointRadius);
        Vector3 lookAtPos = this.transform.position + (lookAtNormal * focalPointRadius);

        cameraFocalPoint.transform.LookAt(lookAtNormal, normal);
    }
}
}