using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;
using MeEngine.Events;

public partial class GameControllerFsm : MeFsm
{
    //The player must select a destination tile and any other actions for their turn
    public class CommandSelectionState : MeFsmState<GameControllerFsm>
    {
        protected override void EnterState()
        {
            base.EnterState();
            Debug.Log("Entering CommandSelectionState");

            EventManager.Publish(new Events.NewTurnEvent());
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space)) {
                ParentFsm.DoEndTurn();
            }
        }
    }
}