using UnityEngine;
using BansheeGz.BGSpline.Curve;
using MeEngine.Events;

public class CommandPointModel : MonoBehaviour{

    public TileCoords destinationTile;
    public TileCoords destinationFacingTile;
    public int destinationLevel;
    public int endVelocity;
    public Vector3 sourcePosition;
    public Vector3 sourceHeading;

    public BGCurve spline;
}
