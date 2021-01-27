using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_Y_Movement : MonoBehaviour
{
	public Spaceship_Selector_Controller ssc;
    // Start is called before the first frame update
	public void Reset_Y_M(){
		ssc.Reset_Y_Axis_Lock();
	}
}
