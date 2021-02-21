using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;
using MeEngine.Events;
public partial class CommandPointFsm : MeFsm
{


    public TileCoords destinationTile;
    public TileCoords destinationFacingTile;
    public int destinationLevel;

    public int endVelocity;

    public PathingMaterialScheme pathingMaterialScheme;

    public GameObject sprite;
    public BGCurve spline;

    private Vector3 sourcePosition;
    private Vector3 sourceHeading;

    private NavigationSystem myNavigationSystem;

    protected override void Start()
    {
        base.Start();
    }

    /*private void OnEnable() {
        SwapState<WaitingState>();
    }*/

    public void SetNavigationSystem(NavigationSystem navigationSystem){
        myNavigationSystem = navigationSystem;
    }

    public void SetSource(Vector3 sourcePosition, Vector3 forwardVector) {
        this.sourcePosition = sourcePosition;
        this.sourceHeading = forwardVector;
    }

    public void SetDestination(TileCoords tileCoords, TileCoords facingTile, int level) {
        destinationTile = tileCoords;
        destinationFacingTile = facingTile;
        destinationLevel = level;

        //Position the sprite within the Hex
        sprite.transform.position = HexMapHelper.GetWorldPointFromTile(tileCoords, level) + ((HexMapHelper.GetFacingVector(tileCoords, facingTile) * HexMapHelper.HexWidth * 0.3f));

        //Face the sprite the correct direction
        sprite.transform.rotation = HexMapHelper.GetRotationFromFacing(tileCoords, facingTile);

        //Color the sprite based on height
        sprite.GetComponentInChildren<SpriteRenderer>().color = HexMapUI.GetLevelColor(level);

        //Set line material
        if(PlayfieldManager.GetTileObstacleType(new TileWithLevel() {position = tileCoords, level = level}) == TileObstacleType.Solid){
            spline.GetComponent<LineRenderer>().material = pathingMaterialScheme.GetMaterialFromIndicator(PathIndicatorType.Collision);
        }else{
            spline.GetComponent<LineRenderer>().material = pathingMaterialScheme.GetMaterialFromIndicator(PathIndicatorType.Selected);
        }

        SetSpline(sourcePosition, sourceHeading,
            HexMapHelper.GetWorldPointFromTile(tileCoords, level), HexMapHelper.GetFacingVector(tileCoords, facingTile));
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
