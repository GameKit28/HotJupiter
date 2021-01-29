using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;
using BansheeGz.BGSpline.Curve;
public partial class CommandPointFsm : MeFsm
{
    public Vector3Int destinationTile;
    public HexDirection destinationDirection;
    public int destinationLevel;


    public GameObject sprite;
    public BGCurve spline;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
