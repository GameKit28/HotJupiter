using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;
using MeEngine.Events;
public partial class GameControllerFsm : MeFsm
{
    public static class Events{
        public struct NewTurnEvent : IEvent {}
        public struct NewTurnEventPost : IEvent {} //Hacky, but some systems need to do their thing after others.
        public struct BeginPlayingOutTurnEvent : IEvent {}
        public struct EndPlayingOutTurnEvent : IEvent {}

        public struct ProcessEndTurnEvent : IEvent {}
    }

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
