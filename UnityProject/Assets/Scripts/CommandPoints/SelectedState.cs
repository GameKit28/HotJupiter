using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;
using MeEngine.Events;

public partial class CommandPointFsm {
    public class SelectedState : MeFsmState<CommandPointFsm>
    {
        private const float colorSwapRate = 0.5f; // seconds

        private float swapCountdown = 0f;
        private bool isBaseColor = true;

        protected override void EnterState()
        {
            ParentFsm.myNavigationSystem.NewPointSelected(ParentFsm);
        }

        protected override void ExitState()
        {
            base.ExitState();

            ParentFsm.sprite.GetComponentInChildren<SpriteRenderer>().color = HexMapUI.GetLevelColor(ParentFsm.destinationLevel);
        }

        // Update is called once per frame
        void Update()
        {
            //Swap Color between (Cyan selected color and level color)
            swapCountdown -= TimeManager.UIDeltaTime;
            if(swapCountdown < 0) {
                ParentFsm.sprite.GetComponentInChildren<SpriteRenderer>().color = isBaseColor ? Color.white : HexMapUI.GetLevelColor(ParentFsm.destinationLevel);
                swapCountdown += colorSwapRate;
                isBaseColor = !isBaseColor;
            }
            
        }
    }
}