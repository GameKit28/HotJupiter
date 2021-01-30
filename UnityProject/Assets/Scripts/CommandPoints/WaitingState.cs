using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;

public partial class CommandPointFsm {
    public class WaitingState : MeFsmState<CommandPointFsm>
    {
        protected override void EnterState()
        {
            base.EnterState();

            ParentFsm.spline.gameObject.SetActive(false);
        }

        void OnMouseEnter(){
            Debug.Log("Mouse Entered");
            SwapState<HoverState>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}