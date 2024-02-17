using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotJupiter
{
	[CreateAssetMenu(
		fileName = "PathingLineSchema",
		menuName = "HotJupiter/PathingLineSchema",
		order = 0
	)]
	public class PathingMaterialScheme : ScriptableObject
	{
		[System.Serializable]
		public struct PathIndicatorMaterial
		{
			public PathIndicatorType type;
			public Material material;
		}

		public List<PathIndicatorMaterial> pathMaterials;

		public Dictionary<PathIndicatorType, Material> pathMaterialsDict;

		public Material GetMaterialFromIndicator(PathIndicatorType indicatorType)
		{
			if (pathMaterialsDict == null)
			{
				pathMaterialsDict = new Dictionary<PathIndicatorType, Material>();
				foreach (var indicator in pathMaterials)
				{
					pathMaterialsDict.Add(indicator.type, indicator.material);
				}
			}

			return pathMaterialsDict[indicatorType];
		}
	}

	public enum PathIndicatorType
	{
		Default,
		Selected,
		G1,
		G2,
		G3,
		Collision,
	}
}
