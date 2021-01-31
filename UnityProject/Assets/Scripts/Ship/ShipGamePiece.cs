﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;
using MeEngine.Events;
public class ShipGamePiece : NavigatingGamePiece
{
    public ShipStats shipTemplete;

    int missileCount;
    int missileCooldown;

    bool willFireMissileThisTurn = false;

    protected override void Awake() {
        base.Awake();

        GameObject.Destroy(gamePieceModel.transform.Find("PlaceholderShip").gameObject);

        GameObject newModel = GameObject.Instantiate(shipTemplete.model, gamePieceModel.transform, false);
        newModel.transform.localPosition = Vector3.zero;
        newModel.transform.localScale = Vector3.one;
        newModel.transform.localRotation = Quaternion.identity;

        missileCount = shipTemplete.missileCount;
        missileCooldown = 0;
    }

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.M)) {
            Debug.Log("Firing Missile");
            QueueMissile(true);
        }
    }

    public void QueueMissile(bool fireThisTurn){
        Debug.Log("Will Fire Missile: " + fireThisTurn);
        willFireMissileThisTurn = fireThisTurn;
    }

    public bool CanFireMissile() {
        return missileCooldown < 1 && missileCount > 0;
    }

    public int GetMissileCount() {
        return missileCount;
    }

    private void FireMissile(){
        if(CanFireMissile()) {
            Debug.Log("Firing Missile");
            missileCount -= 1;
            missileCooldown = shipTemplete.missileFireCooldownTurns;

            MissileFactory.SpawnMissile(currentTile, currentDirection, currentLevel, this, shipTemplete.missileTemplate);
        }
    }

    [EventListener]
    public void OnProcessEndTurn(GameControllerFsm.Events.ProcessEndTurnEvent @event) {
        Debug.Log("On Process End Turn");
        if(willFireMissileThisTurn){
            FireMissile();
            willFireMissileThisTurn = false;
        }
    }


    [EventListener]
    protected override void OnEndPlayingPhase(GameControllerFsm.Events.EndPlayingOutTurnEvent @event){
        base.OnEndPlayingPhase(@event);

        missileCooldown = Mathf.Max(0, missileCooldown - 1);
    }
}
