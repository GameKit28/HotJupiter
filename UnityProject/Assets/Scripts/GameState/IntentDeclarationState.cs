using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;
using MeEngine.Events;

public partial class GameControllerFsm : MeFsm
{
    //All active pieces commit to an action
    public class IntentDeclarationState : MeFsmState<GameControllerFsm>
    {
        protected override void EnterState()
        {
            base.EnterState();
            Debug.Log("Entering ProcessingCommandsState");

            GameControllerFsm.eventPublisher.Publish(new Events.BeginProcessingCommandsState());

            SwapState<CalculatingInterceptsState>();
        }
    }
}