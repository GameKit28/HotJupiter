using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float cameraSpeedX = 15f;
    float cameraSpeedZ = 15f;

    public PlanetSizer focalPlanet;
    public GameObject cameraFocalPoint;

    private float focalPointRadius {
        get {
            return focalPlanet.planetRadius + HexMapHelper.gridFirstAltitudeOffset;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraFocalPoint.transform.position = Vector3.up * focalPointRadius;
    }

    // Update is called once per frame
    void Update()
    {
        // Kit. Try RotateAround


        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            this.transform.Rotate(transform.forward, cameraSpeedX * TimeManager.UIDeltaTime);
        }else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            this.transform.Rotate(transform.forward, -cameraSpeedX * TimeManager.UIDeltaTime);
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            this.transform.Rotate(transform.right, cameraSpeedZ * TimeManager.UIDeltaTime);
        }else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            this.transform.Rotate(transform.right, -cameraSpeedZ * TimeManager.UIDeltaTime);
        }
    }
}
