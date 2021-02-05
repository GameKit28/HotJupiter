using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float cameraSpeedX = 5f;
    float cameraSpeedZ = 3f;

    const float planetRadius = 19.92694f;
    const float level1Altitude = 0.25f;
    const float focalPointRadius = planetRadius + level1Altitude;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Kit. Try RotateAround


        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            this.transform.Translate(Vector3.left * cameraSpeedX * TimeManager.UIDeltaTime);
        }else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            this.transform.Translate(Vector3.right * cameraSpeedX * TimeManager.UIDeltaTime);
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            this.transform.Translate(Vector3.forward * cameraSpeedZ * TimeManager.UIDeltaTime);
        }else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            this.transform.Translate(Vector3.back * cameraSpeedZ * TimeManager.UIDeltaTime);
        }
    }
}
