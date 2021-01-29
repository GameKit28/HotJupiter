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

    protected override void Start()
    {
        base.Start();

        //sprite = transform.Find("Sprite").gameObject;

    }

    public void SetDestination(Vector3Int tile, HexDirection direction, int level) {

        //Position the sprite within the Hex
        sprite.transform.position = HexMapHelper.GetWorldPointFromTile(tile) + (HexMapHelper.GetVectorFromDirection(direction) * HexMapHelper.HexWidth * 0.3f);

        //Face the sprite the correct direction
        sprite.transform.eulerAngles = new Vector3(90, HexMapHelper.GetAngleFromDirection(direction), 0);

        //Color the sprite based on height
        sprite.GetComponent<SpriteRenderer>().color = HexMapHelper.GetLevelColor(level);
    }
}
