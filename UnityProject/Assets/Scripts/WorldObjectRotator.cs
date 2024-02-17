using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotJupiter
{
	public class WorldObjectRotator : MonoBehaviour
	{
		const float minRotationRate = 15f;
		const float maxRotationRate = 5f;

		Vector3 rotationPole;
		float rotationRate;

		// Start is called before the first frame update
		void Start()
		{
			rotationPole = Random.insideUnitSphere;
			rotationRate = Random.Range(minRotationRate, maxRotationRate);
		}

		// Update is called once per frame
		void Update()
		{
			transform.Rotate(rotationPole, rotationRate * TimeManager.TurnDeltaTime, Space.Self);
		}
	}
}
