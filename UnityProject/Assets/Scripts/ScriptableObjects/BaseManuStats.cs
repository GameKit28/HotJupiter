using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManuStats : ScriptableObject
{
    public int numThrusters = 3;
    public int numVectorThrusters = 2;

    public GameObject model;

    public int TopSpeed { get {
        return numThrusters * 2;
    }}

    public int Maneuverability { get {
        return numVectorThrusters;
    }}
}
