using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle", menuName = "HotJupiter/Obstacle", order = 1)]

public class ObstacleTemplate : ScriptableObject
{
    public RelativeFootprintTemplate footprint;
    public List<GameObject> modelPrefabs;
}
