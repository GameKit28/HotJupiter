using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;
using MeEngine.Events;
public class ShipController : MonoBehaviour, IHaveTilePosition, IHaveHexDirection
{
    public bool isPlayerControlled = false;

    public ShipStats shipTemplete;


    public Vector3Int currentTile;
    public HexDirection currentDirection;
    public int currentLevel;

    public Vector3Int destinationTile;
    public HexDirection destinationDirection;
    public int destinationLevel;


    public int currentSpeed;

    public const int maxSpeed = 6;


    public GameObject shipPieceModel;
    public GameObject shipWorldModel;

    public GameObject shipGamePiece;
    public GameObject worldShip;


    private BGCurve activeWorldPath;


    public Vector3Int GetTilePosition(){
        return currentTile;
    }

    public int GetLevel(){
        return currentLevel;
    }

    public HexDirection GetHexDirection(){
        return currentDirection;
    }

    void Awake() {
        EventManager.SubscribeAll(this);

        GameObject.Destroy(shipPieceModel.transform.Find("PlaceholderShip").gameObject);
        
        GameObject.Instantiate(shipTemplete.shipModel, shipWorldModel.transform, false);
        GameObject.Instantiate(shipTemplete.shipModel, shipPieceModel.transform, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTile = HexMapHelper.GetTileFromWorldPoint(transform.position);
        currentLevel = HexMapHelper.GetLevelFromAltitude(transform.position.y);

        worldShip.transform.position = HexMapHelper.GetWorldPointFromTile(currentTile, currentLevel);
        worldShip.transform.localEulerAngles = new Vector3(0, HexMapHelper.GetAngleFromDirection(currentDirection), 0);

        PositionAndOrientPiece();
    }

    public void SetActivePath(BGCurve path) {
        activeWorldPath = path;
    }
    public void SetDestination(Vector3Int destinationTile, HexDirection destinationDirection, int destinationLevel) {
        this.destinationTile = destinationTile;
        this.destinationDirection = destinationDirection;
        this.destinationLevel = destinationLevel;
    }

    private void PositionAndOrientPiece(){
        shipGamePiece.transform.position = HexMapHelper.GetWorldPointFromTile(currentTile, currentLevel);
        shipPieceModel.transform.localEulerAngles = new Vector3(0, HexMapHelper.GetAngleFromDirection(currentDirection), 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(activeWorldPath != null)
        {
            BGCcMath math = activeWorldPath.GetComponent<BGCcMath>();

            Vector3 tangent;
            worldShip.transform.position = math.CalcPositionAndTangentByDistanceRatio(TimeManager.TurnRatio, out tangent);
            shipWorldModel.transform.rotation = Quaternion.LookRotation(tangent);
        }
    }

    [EventListener]
    void OnBeginPlayingPhase(GameControllerFsm.Events.BeginPlayingOutTurnEvent @event) {
        shipGamePiece.SetActive(false);
    }

    [EventListener]
    void OnEndPlayingPhase(GameControllerFsm.Events.EndPlayingOutTurnEvent @event){
        currentTile = destinationTile;
        currentDirection = destinationDirection;
        currentLevel = destinationLevel;

        PositionAndOrientPiece();
        shipGamePiece.SetActive(true);
    }
}
