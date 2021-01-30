using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;
using MeEngine.Events;
public partial class GameControllerFsm : MeFsm
{
    public static class Events{
        public struct NewTurnEvent : IEvent {}
        public struct PlayingOutTurnEvent : IEvent {}
    }

    // Start is called before the first frame update
    protected override void Start()
    {

    }

    public void OnEndTurnClicked() {
        DoEndTurn();
    }

    private void DoEndTurn(){
        Debug.Log("end turn");
    }
}
