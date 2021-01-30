using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour, IHaveTilePosition
{
    public bool isPlayerControlled = false;

    public ShipStats shipTemplete;

    public Vector3Int currentTile;
    public HexDirection currentDirection;
    public int currentLevel;

    public int currentSpeed;

    public const int maxSpeed = 6;

    public GameObject shipPieceModel;
    public GameObject shipWorldModel;

    public GameObject shipGamePiece;
    public GameObject worldShip;

    public Vector3Int GetTilePosition(){
        return currentTile;
    }

    public int GetLevel(){
        return currentLevel;
    }

    void Awake() {
        GameObject.Destroy(shipPieceModel.transform.Find("PlaceholderShip").gameObject);
        
        GameObject.Instantiate(shipTemplete.shipModel, shipWorldModel.transform, false);
        GameObject.Instantiate(shipTemplete.shipModel, shipPieceModel.transform, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTile = HexMapHelper.GetTileFromWorldPoint(transform.position);
        currentLevel = HexMapHelper.GetLevelFromAltitude(transform.position.y);

        shipGamePiece.transform.position = HexMapHelper.GetWorldPointFromTile(currentTile, currentLevel);
        worldShip.transform.position = HexMapHelper.GetWorldPointFromTile(currentTile, currentLevel);

        shipPieceModel.transform.localEulerAngles = new Vector3(0, HexMapHelper.GetAngleFromDirection(currentDirection), 0);
        worldShip.transform.localEulerAngles = new Vector3(0, HexMapHelper.GetAngleFromDirection(currentDirection), 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
