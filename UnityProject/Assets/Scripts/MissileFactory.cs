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

    public static void SpawnMissile(TileCoords tileCoords, TileCoords tileFacing, TileLevel level, BaseGamePiece spawningPiece, MissileStats template) {
        GameObject newMissile = SimplePool.Spawn(
            instance.missilePrefab, 
            Vector3.zero, 
            Quaternion.identity, 
            instance.gamePieceHolder);
        newMissile.GetComponentInChildren<MissileGamePiece>().currentTileFacing = tileFacing;
        newMissile.GetComponentInChildren<MissileGamePiece>().currentTile = tileCoords;
        newMissile.GetComponentInChildren<MissileGamePiece>().currentLevel = level;
        newMissile.GetComponentInChildren<MissileGamePiece>().PositionAndOrientPiece();

        newMissile.GetComponentInChildren<PieceController>().worldBase.transform.position = HexMapHelper.GetWorldPointFromTile(tileCoords, level);
        newMissile.GetComponentInChildren<PieceController>().worldModel.transform.rotation = HexMapHelper.GetRotationFromFacing(tileCoords, tileFacing);

        newMissile.GetComponentInChildren<NavigationSystem>().GenerateCommandPoints();

        newMissile.GetComponentInChildren<MissileBrain>().FindTarget();
        var selectedCommand = newMissile.GetComponentInChildren<MissileBrain>().SelectCommand();
        newMissile.GetComponentInChildren<PieceController>().SetSelectedCommandPoint(selectedCommand);
    }
}
