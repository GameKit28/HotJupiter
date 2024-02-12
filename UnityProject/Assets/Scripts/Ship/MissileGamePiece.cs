using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;
using MeEngine.Events;

namespace HotJupiter {
public class MissileGamePiece : NavigatingGamePiece
{
    public MissileStats missileTemplate;

    public BaseGamePiece motherGamePiece;

    protected override void Awake() {
        base.Awake();

        GameObject.Destroy(gamePieceModel.transform.Find("PlaceholderMissile").gameObject);

        GameObject newModel = GameObject.Instantiate(missileTemplate.model, gamePieceModel.transform, false);
        newModel.transform.localPosition = Vector3.zero;
        newModel.transform.localScale = Vector3.one;
        newModel.transform.rotation = Quaternion.identity;

        footprint = new DynamicFootprint(this, missileTemplate.footprint);
    }
}
}