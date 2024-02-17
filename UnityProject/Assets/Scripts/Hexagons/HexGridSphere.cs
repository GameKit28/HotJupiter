using UnityEngine;

//using UnityEngine.Tilemaps;

namespace HotJupiter
{
	[RequireComponent(typeof(SphereCollider))]
	[RequireComponent(typeof(MeshFilter))]
	public class HexGridSphere : MonoBehaviour
	{
		public Color Color;
		public float Radius { get; private set; }

		void Awake()
		{
			SphereCollider collider = GetComponent<SphereCollider>();
			MeshFilter meshFilter = GetComponent<MeshFilter>();
			float localRadius = meshFilter.mesh.bounds.size.x / 2f;
			Radius = transform.localScale.x * localRadius;
			collider.radius = localRadius;
		}
	}
}
