using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObjects/Ship", order = 1)]
public class ShipStats : BaseManuStats
{
    public int health = 3;

    public int missileCount = 10;
    public int missileFireCooldownTurns = 2;

    public MissileStats missileTemplate;
}
