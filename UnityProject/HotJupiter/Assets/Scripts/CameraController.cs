using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float cameraSpeedX = 5f;
    float cameraSpeedZ = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            this.transform.Translate(Vector3.left * cameraSpeedX * Time.deltaTime);
        }else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            this.transform.Translate(Vector3.right * cameraSpeedX * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            this.transform.Translate(Vector3.forward * cameraSpeedZ * Time.deltaTime);
        }else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            this.transform.Translate(Vector3.back * cameraSpeedZ * Time.deltaTime);
        }
    }
}
