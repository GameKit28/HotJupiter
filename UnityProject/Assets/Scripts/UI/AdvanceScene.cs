﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdvanceScene : MonoBehaviour
{
	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
		{
			SceneManager.LoadScene("Planetoid");
		}
	}
}
