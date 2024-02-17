using BansheeGz.BGSpline.Curve;
using MeEngine.Events;
using UnityEngine;

namespace HotJupiter
{
	public class CommandPointModel : MonoBehaviour
	{
		public TileWithFacing destinationTile;

		public int endVelocity;
		public int gForce;
		public Vector3 sourcePosition;
		public Vector3 sourceHeading;

		public TilePath tilePath;

		public BGCurve spline;
	}
}
