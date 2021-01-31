using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;
using MeEngine.Events;
public class PieceController : MonoBehaviour, IHaveTilePosition, IHaveHexDirection
{
    public bool isPlayerControlled = false;

    public ShipStats shipTemplete;

    public NavigatingGamePiece gamePiece;


    public GameObject worldModel;
    public GameObject worldBase;

    private BGCurve activeWorldPath;


    public Vector3Int GetTilePosition(){
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

        GameObject.Instantiate(shipTemplete.shipModel, worldModel.transform, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Kit - Potential race condition here as the gamePiece is setting up it's position in Start as well
        worldBase.transform.position = HexMapHelper.GetWorldPointFromTile(GetTilePosition(), GetLevel());
        worldModel.transform.localEulerAngles = new Vector3(0, HexMapHelper.GetAngleFromDirection(GetHexDirection()), 0);
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
    }
}
