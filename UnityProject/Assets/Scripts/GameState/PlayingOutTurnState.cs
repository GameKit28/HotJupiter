﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;
using MeEngine.Events;

public partial class GameControllerFsm : MeFsm
{
    //The player watches as the events of the turn unfold
    public class PlayingOutTurnState : MeFsmState<GameControllerFsm>
    {
        protected override void EnterState()
        {
            Debug.Log("Entering PlayingOutTurnState");
            EventManager.Publish(new Events.BeginPlayingOutTurnEvent());
        }

        protected override void ExitState()
        {
            EventManager.Publish(new Events.EndPlayingOutTurnEvent());
        }

        // Update is called once per frame
        void Update()
        {
            if(TimeManager.TurnRatio >= 1.0f){
                SwapState<CommandSelectionState>();
            }
        }
    }
}