using UnityEngine;
using System.Collections;

namespace HexasphereGrid {

	[ExecuteInEditMode]
	public class HexasphereConfig : MonoBehaviour {

		[Tooltip ("Help")]
		[TextArea]
		public string info = "To load this configuration, just activate this component or call LoadConfiguration() method of this script.";

		[Tooltip ("User-defined name for this configuration")]
		[TextArea]
		public string title = "Optionally name this configuration editing this text.";

		[HideInInspector]
		public string config;

		[HideInInspector]
		public Texture2D[] textures;

		// Use this for initialization
		void OnEnable () {
			if (!Application.isPlaying)
				LoadConfiguration ();
		}

		void Start () {
			LoadConfiguration ();
		}

		/// <summary>
		/// Call this method to force a configuration load.
		/// </summary>
		public void LoadConfiguration () {
			if (config == null)
				return;

			Hexasphere hexa = GetComponent<Hexasphere> ();
			if (hexa == null) {
				Debug.Log ("Hexasphere Grid System not found in this game object!");
				return;
			}
			hexa.textures = textures;
			hexa.SetTilesConfigurationData (config);
		}

	}

}