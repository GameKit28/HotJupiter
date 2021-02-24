using MeEngine.FsmManagement;
using MeEngine.Events;
using UnityEngine;
using BansheeGz.BGSpline.Curve;

public class CommandPointController : MonoBehaviour
{
    public static class Events {
        public struct DestinationSet : IEvent {}
    }
    public EventPublisher eventPublisher = new EventPublisher();

    public CommandPointModel model;
    public CommandPointViewFsm view;

    public PathingMaterialScheme pathingMaterialScheme;

    void Awake(){
        if(view != null) {
            eventPublisher.SubscribeAll(view);
        }
        model.spline.gameObject.SetActive(false);
    }

    public void SetSource(Vector3 sourcePosition, Vector3 forwardVector) {
        model.sourcePosition = sourcePosition;
        model.sourceHeading = forwardVector;
    }

    public void SetDestination(TileCoords tileCoords, TileCoords facingTile, TileLevel level) {
        model.destinationTile = tileCoords;
        model.destinationFacingTile = facingTile;
        model.destinationLevel = level;

        SetSpline(model.sourcePosition, model.sourceHeading,
            HexMapHelper.GetWorldPointFromTile(tileCoords, level), HexMapHelper.GetFacingVector(tileCoords, facingTile));

        eventPublisher.Publish(new Events.DestinationSet());

    }
    public void SetEndVelocity(int velocity) {
        model.endVelocity = velocity;
    }
    protected void SetSpline(Vector3 startPosition, Vector3 startHeading, Vector3 endPosition, Vector3 endHeading){
        model.spline.Clear();

        model.spline.AddPoint(new BGCurvePoint(model.spline, startPosition, BGCurvePoint.ControlTypeEnum.BezierSymmetrical,
            startPosition - startHeading, startPosition + startHeading, true));
        model.spline.AddPoint(new BGCurvePoint(model.spline, endPosition, BGCurvePoint.ControlTypeEnum.BezierSymmetrical,
            endPosition - endHeading, endPosition + endHeading, true));
    }
}