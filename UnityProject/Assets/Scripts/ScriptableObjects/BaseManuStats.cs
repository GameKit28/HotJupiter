using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotJupiter
{
	public class BaseManuStats : ScriptableObject
	{
		public RelativeFootprintTemplate footprint;
		public int numThrusters = 3;
		public int numVectorThrusters = 2;

		public bool canAccelerate = true;
		public bool canDecelerate = true;

		public bool canStrafe = true;

		public bool effortlessClimb = false;

		public GameObject model;

		public int TopSpeed
		{
			get { return numThrusters; }
		}

		public int Maneuverability
		{
			get { return numVectorThrusters; }
		}
	}
}
