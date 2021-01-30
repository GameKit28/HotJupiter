using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;

public partial class CommandPointFsm {
    public class HoverState : MeFsmState<CommandPointFsm>
    {
        protected override void EnterState()
        {
            base.EnterState();

            ParentFsm.spline.gameObject.SetActive(true);
        }

        void OnMouseExit(){
            Debug.Log("Hover Ended");
            SwapState<WaitingState>();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(0)){
                //The player selected this command point
                Debug.Log("Command Selected");
                SwapState<SelectedState>();
            }
        }
    }
}