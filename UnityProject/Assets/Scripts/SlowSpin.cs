using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSpin : MonoBehaviour
{
    public float rotationRate = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.up, ((Mathf.PI * 2)/rotationRate) * TimeManager.TurnDeltaTime);
    }
}
