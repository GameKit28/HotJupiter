using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileFactory : MonoBehaviour
{
    public static MissileFactory instance;

    public GameObject missilePrefab;
    public Transform gamePieceHolder;

    void Awake(){
        instance = this;
    }

    public static void SpawnMissile(Vector3Int tile, HexDirection direction, int level, BaseGamePiece spawningPiece, MissileStats template) {
        GameObject newMissile = GameObject.Instantiate(
            instance.missilePrefab, 
            Vector3.zero, 
            Quaternion.identity, 
            instance.gamePieceHolder);
        newMissile.GetComponentInChildren<MissileGamePiece>().currentDirection = direction;
        newMissile.GetComponentInChildren<MissileGamePiece>().currentTile = tile;
        newMissile.GetComponentInChildren<MissileGamePiece>().currentLevel = level;
        newMissile.GetComponentInChildren<MissileGamePiece>().PositionAndOrientPiece();

        newMissile.GetComponentInChildren<PieceController>().worldBase.transform.position = HexMapHelper.GetWorldPointFromTile(tile, level);
        newMissile.GetComponentInChildren<PieceController>().worldModel.transform.localEulerAngles = new Vector3(0, HexMapHelper.GetAngleFromDirection(direction), 0);

        newMissile.GetComponentInChildren<NavigationSystem>().GenerateCommandPoints();

        newMissile.GetComponentInChildren<MissileBrain>().FindTarget();
        var selectedCommand = newMissile.GetComponentInChildren<MissileBrain>().SelectCommand();
        newMissile.GetComponentInChildren<PieceController>().SetSelectedCommandPoint(selectedCommand);
    }
}
