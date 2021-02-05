using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;
using MeEngine.Events;
public class PieceController : MonoBehaviour, IHaveTilePosition, IHaveHexDirection
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

    public HexDirection GetHexDirection(){
        return gamePiece.currentDirection;
    }

    void Awake() {
        EventManager.SubscribeAll(this);

        GameObject worldObject = GameObject.Instantiate(pieceTemplate.model, worldModel.transform, false);
        worldObject.transform.localPosition = Vector3.zero;
        worldObject.transform.localScale = Vector3.one;
        worldObject.transform.localRotation = Quaternion.identity;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Kit - Potential race condition here as the gamePiece is setting up it's position in Start as well
        var assumedTile = HexMapHelper.GetTileFromWorldPoint(transform.position);
        var assumedHeight = HexMapHelper.GetLevelFromAltitude(transform.position.y);

        worldBase.transform.position = HexMapHelper.GetWorldPointFromTile(assumedTile, assumedHeight);
        worldModel.transform.localEulerAngles = new Vector3(0, HexMapHelper.GetAngleFromDirection(GetHexDirection()), 0);
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
            worldModel.transform.rotation = Quaternion.LookRotation(tangent);
        }

        //Hacky Missile Detonation
        /*if(TimeManager.TurnDeltaTime != 0){
            MissileGamePiece missile = transform.GetComponentInChildren<MissileGamePiece>();
            if(missile != null){
                var allShips = ShipManager.GetAllShips();
                foreach(ShipGamePiece ship in allShips){
                    if(ship == missile.motherGamePiece) continue;
                    
                    PieceController shipController = ship.transform.GetComponentInParent<PieceController>();
                    if(shipController != null) {
                        GameObject shipWorld = shipController.worldModel;
                        Vector3 shipWorldPos = shipWorld.transform.position;

                        float distance = Vector3.Distance(worldModel.transform.position, shipWorldPos);
                        if(distance < 2f){
                            Debug.Log("KABOOM!");
                            activeWorldPath.Delete(1);
                            activeWorldPath.AddPoint(new BGCurvePoint(activeWorldPath, shipWorldPos, true));
                        }
                    }
                }
            }
        }*/
    }

    [EventListener]
    void OnStartPlayingTurn(GameControllerFsm.Events.BeginPlayingOutTurnEvent @event){
        SetActivePath(selectedCommandPoint.spline);
        gamePiece.SetDestination(selectedCommandPoint.destinationTile, selectedCommandPoint.destinationDirection, selectedCommandPoint.destinationLevel);
        gamePiece.currentVelocity = selectedCommandPoint.endVelocity;
    }
}
