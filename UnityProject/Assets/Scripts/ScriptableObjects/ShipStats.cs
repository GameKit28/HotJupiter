using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObjects/Ship", order = 1)]
public class ShipStats : ScriptableObject
{
    public int numThrusters = 3;
    public int numVectorThrusters = 2;

    public int TopSpeed { get {
        return numThrusters;
    }}
}
