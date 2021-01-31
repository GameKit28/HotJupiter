using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;
using MeEngine.Events;
public class ShipGamePiece : NavigatingGamePiece
{
    public ShipStats shipTemplete;

    protected override void Awake() {
        base.Awake();

        GameObject.Destroy(gamePieceModel.transform.Find("PlaceholderShip").gameObject);

        GameObject.Instantiate(shipTemplete.shipModel, gamePieceModel.transform, false);
    }
}
