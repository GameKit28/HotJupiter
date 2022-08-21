using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;
using MeEngine.Events;

public partial class GameControllerFsm : MeFsm
{
    //
    public class CalculatingInterceptsState : MeFsmState<GameControllerFsm>
    {
        protected override void EnterState()
        {
            base.EnterState();
            Debug.Log("Entering CalculatingInterceptsState");

            SwapState<PlayingOutTurnState>();
        }
    }
}