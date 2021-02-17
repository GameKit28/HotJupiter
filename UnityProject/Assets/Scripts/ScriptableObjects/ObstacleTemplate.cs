using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTemplate : ScriptableObject
{
    public RelativeFootprintTemplate footprint;
    public List<GameObject> modelPrefabs;
}
