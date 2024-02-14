using UnityEngine;
//using UnityEngine.Tilemaps;

namespace HotJupiter
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(MeshFilter))]
    public class HexGridSphere : MonoBehaviour {
        public Color Color;
        public float Radius {get; private set;}

        void Start(){
            SphereCollider collider = GetComponent<SphereCollider>();
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Radius = meshFilter.mesh.bounds.size.x / 2f;
            collider.radius = Radius;
        }
    }
}