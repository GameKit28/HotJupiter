using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;
using MeEngine.Events;
public class PieceController : MonoBehaviour, IHaveTilePosition, IHaveTileFacing
{
    public bool isPlayerControlled = false;

    public BaseManuStats pieceTemplate;

    public NavigatingGamePiece gamePiece;

    private CommandPointFsm selectedCommandPoint;

    public NavigationSystem navigationSystem;

    public GameObject worldModel;
    public GameObject worldBase;

    private BGCurve activeWorldPath;

    public TileCoords GetTilePosition(){
        return gamePiece.currentTile;
    }

    public int GetLevel(){
        return gamePiece.currentLevel;
    }

    public TileCoords GetTileFacing(){
        return gamePiece.currentTileFacing;
    }

    void Awake() {
        GameControllerFsm.eventPublisher.SubscribeAll(this);
        gamePiece.eventPublisher.SubscribeAll(this);

        GameObject worldObject = GameObject.Instantiate(pieceTemplate.model, worldModel.transform, false);
        worldObject.transform.localPosition = Vector3.zero;
        worldObject.transform.localScale = Vector3.one;
        worldObject.transform.localRotation = Quaternion.identity;
    }

    public void SetSelectedCommandPoint(CommandPointFsm point){
        selectedCommandPoint = point;
    }
    public void SetActivePath(BGCurve path) {
        activeWorldPath = path;
    }

    // Update is called once per frame
    void Update()
    {
        if(activeWorldPath != null)
        {
            BGCcMath math = activeWorldPath.GetComponent<BGCcMath>();

            Vector3 tangent;
            worldBase.transform.position = math.CalcPositionAndTangentByDistanceRatio(TimeManager.TurnRatio, out tangent);
            worldModel.transform.rotation = Quaternion.LookRotation(tangent, worldBase.transform.position);
        }
    }

    void ResetToGamePiecePosition(){
        worldBase.transform.position = HexMapHelper.GetWorldPointFromTile(gamePiece.currentTile, gamePiece.currentLevel);
        worldModel.transform.rotation = HexMapHelper.GetRotationFromFacing(gamePiece.currentTile, gamePiece.currentTileFacing);
    }

    [EventListener]
    void OnGamePieceCompletedSetup(BaseGamePiece.Events.CompletedSetup @event){
        ResetToGamePiecePosition();
    }

    [EventListener]
    void OnStartTurn(GameControllerFsm.Events.NewTurnEvent @event){
        ResetToGamePiecePosition();
    }

    [EventListener]
    void OnStartPlayingTurn(GameControllerFsm.Events.BeginPlayingOutTurnEvent @event){
        SetActivePath(selectedCommandPoint.spline);
        gamePiece.SetDestination(selectedCommandPoint.destinationTile, selectedCommandPoint.destinationFacingTile, selectedCommandPoint.destinationLevel);
        gamePiece.currentVelocity = selectedCommandPoint.endVelocity;
    }
}
