﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;
using MeEngine.Events;
public partial class GameControllerFsm : MeFsm
{
    public static class Events{
        public struct BeginCommandSelectionState : IEvent {}
        public struct BeginCommandSelectionStatePost : IEvent {} //Hacky, but some systems need to do their thing after others.
        public struct BeginPlayingOutTurnState : IEvent {}
        public struct EndPlayingOutTurnState : IEvent {}

        public struct BeginProcessingCommandsState : IEvent {}
    }

    public static EventPublisher eventPublisher { get; private set; } = new EventPublisher();

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }

    public void OnEndTurnClicked() {
        DoEndTurn();
    }

    private void DoEndTurn(){
        Debug.Log("end turn");
        SwapState<ProcessingCommandsState>();
    }
}
