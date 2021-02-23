using UnityEngine;
using MeEngine.Events;
using MeEngine.FsmManagement;

public partial class CommandPointViewFsm : MeFsm {
    public GameObject spriteHolder;
    public SpriteRenderer spriteRenderer;
    public CommandPointModel model;
    public CommandPointController controller;

    public PathingMaterialScheme materialScheme;

    protected override void Start()
    {
        base.Start();
    }

    [EventListener]
    void OnDestinationSet(CommandPointController.Events.DestinationSet @event){
        Debug.Log("View recieved destination set event");
        //Position the sprite within the Hex
        spriteHolder.transform.position = HexMapHelper.GetWorldPointFromTile(model.destinationTile, model.destinationLevel) + ((HexMapHelper.GetFacingVector(model.destinationTile, model.destinationFacingTile) * HexMapHelper.HexWidth * 0.3f));

        //Face the sprite the correct direction
        spriteHolder.transform.rotation = HexMapHelper.GetRotationFromFacing(model.destinationTile, model.destinationFacingTile);

        //Color the sprite based on height
        spriteRenderer.color = HexMapUI.GetLevelColor(model.destinationLevel);

        if(PlayfieldManager.GetTileObstacleType(new TileWithLevel() {position = model.destinationTile, level = model.destinationLevel}) == TileObstacleType.Solid){
            model.spline.GetComponent<LineRenderer>().material = materialScheme.GetMaterialFromIndicator(PathIndicatorType.Collision);
        }else{
            model.spline.GetComponent<LineRenderer>().material = materialScheme.GetMaterialFromIndicator(PathIndicatorType.Selected);
        }
        
    }

    [EventListener]
    void OnSelectedSet(CommandPointController.Events.SelectedSet @event){
        if(@event.isSelected) {
            SwapState<SelectedState>();
        }else{
            SwapState<WaitingState>();
        }
    }
}
