using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotJupiter
{
	public class ExhaustTrailAnimator : MonoBehaviour
	{
		// Start is called before the first frame update
		ParticleSystem exhaustParticles;

		void Start()
		{
			exhaustParticles = GetComponent<ParticleSystem>();
		}

		// Update is called once per frame
		void Update()
		{
			if (TimeManager.TurnDeltaTime > 0)
			{
				exhaustParticles.Simulate(TimeManager.TurnDeltaTime, true, false);
			}
		}
	}
}
