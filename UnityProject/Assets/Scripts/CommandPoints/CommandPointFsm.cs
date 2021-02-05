﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;
using MeEngine.Events;
public partial class CommandPointFsm : MeFsm
{
    public TileCoords destinationTile;
    public HexDirection destinationDirection;
    public int destinationLevel;

    public int endVelocity;


    public GameObject sprite;
    public BGCurve spline;

    private Vector3 sourcePosition;
    private Vector3 sourceHeading;

    private NavigationSystem myNavigationSystem;

    protected override void Start()
    {
        base.Start();

        //sprite = transform.Find("Sprite").gameObject;

    }

    public void SetNavigationSystem(NavigationSystem navigationSystem){
        myNavigationSystem = navigationSystem;
    }

    public void SetSource(Vector3 sourcePosition, HexDirection sourceDirection) {
        this.sourcePosition = sourcePosition;
        this.sourceHeading = HexMapHelper.GetVectorFromDirection(sourceDirection);
    }

    public void SetDestination(TileCoords tileCoords, HexDirection direction, int level) {
        destinationTile = tileCoords;
        destinationDirection = direction;
        destinationLevel = level;

        //Position the sprite within the Hex
        sprite.transform.position = HexMapHelper.GetWorldPointFromTile(tileCoords, level) + (HexMapHelper.GetVectorFromDirection(direction) * HexMapHelper.HexWidth * 0.3f);

        //Face the sprite the correct direction
        sprite.transform.eulerAngles = new Vector3(90, HexMapHelper.GetAngleFromDirection(direction), 0);

        //Color the sprite based on height
        sprite.GetComponent<SpriteRenderer>().color = HexMapHelper.GetLevelColor(level);

        SetSpline(sourcePosition, sourceHeading,
            HexMapHelper.GetWorldPointFromTile(tileCoords) + new Vector3(0, HexMapHelper.GetAltitudeFromLevel(level), 0), HexMapHelper.GetVectorFromDirection(direction));
    }

    public void SetEndVelocity(int velocity) {
        endVelocity = velocity;
    }

    public void SelectPoint(bool selected){
        if(selected) {
            SwapState<SelectedState>();
        }else{
            SwapState<WaitingState>();
        }
    }

    protected void SetSpline(Vector3 startPosition, Vector3 startHeading, Vector3 endPosition, Vector3 endHeading){
        spline.Clear();

        spline.AddPoint(new BGCurvePoint(spline, startPosition, BGCurvePoint.ControlTypeEnum.BezierSymmetrical,
            startPosition - startHeading, startPosition + startHeading, true));
        spline.AddPoint(new BGCurvePoint(spline, endPosition, BGCurvePoint.ControlTypeEnum.BezierSymmetrical,
            endPosition - endHeading, endPosition + endHeading, true));
    }
}
