using UnityEngine;
using BansheeGz.BGSpline.Curve;
using MeEngine.Events;

public class CommandPointModel : MonoBehaviour{

    public TileWithFacing destinationTile;

    public int endVelocity;
    public Vector3 sourcePosition;
    public Vector3 sourceHeading;

    public TilePath tilePath;

    public BGCurve spline;
}
