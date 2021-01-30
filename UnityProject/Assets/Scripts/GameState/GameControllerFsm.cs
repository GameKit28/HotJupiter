using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;
using MeEngine.Events;
public partial class GameControllerFsm : MeFsm
{
    public static class Events{
        public struct NewTurnEvent : IEvent {}
        public struct BeginPlayingOutTurnEvent : IEvent {}
        public struct EndPlayingOutTurnEvent : IEvent {}
    }

    public void OnEndTurnClicked() {
        DoEndTurn();
    }

    private void DoEndTurn(){
        Debug.Log("end turn");
        SwapState<CalculatingInterceptsState>();
    }
}
