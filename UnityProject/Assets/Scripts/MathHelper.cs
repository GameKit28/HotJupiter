using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper {
    public static int Mod(int startingNumber, int mod){
        return ((startingNumber % mod) + mod) % mod;
    }
}