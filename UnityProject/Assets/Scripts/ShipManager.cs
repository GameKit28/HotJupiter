﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HotJupiter
{
	public class ShipManager : MonoBehaviour
	{
		private static List<ShipGamePiece> allShips;

		// Start is called before the first frame update
		void Start()
		{
			allShips = GameObject.FindObjectsOfType<ShipGamePiece>().ToList();
		}

		public static List<ShipGamePiece> GetAllShips()
		{
			return allShips;
		}
	}
}
