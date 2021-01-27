using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;

public class PreviewShipFlight : MonoBehaviour
{
    // Start is called before the first frame update
    BGCurve curve;

    void Start()
    {
        curve = gameObject.GetComponent<BGCurve>();
    }

    // Update is called once per frame
    void Update()
    {
        //curve.Fields.
    }
}
