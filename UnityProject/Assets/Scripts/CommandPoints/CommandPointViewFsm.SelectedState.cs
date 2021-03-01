using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;
using MeEngine.Events;

public partial class CommandPointViewFsm {
    public class SelectedState : MeFsmState<CommandPointViewFsm>
    {
        private const float colorSwapRate = 0.5f; // seconds

        private float swapCountdown = 0f;
        private bool isBaseColor = true;

        protected override void EnterState()
        {
            ParentFsm.model.spline.gameObject.SetActive(true);
        }

        protected override void ExitState()
        {
            ParentFsm.spriteRenderer.color = HexMapUI.GetLevelColor(ParentFsm.model.destinationTile.level);
        }

        // Update is called once per frame
        void Update()
        {
            //Swap Color between (Cyan selected color and level color)
            swapCountdown -= TimeManager.UIDeltaTime;
            if(swapCountdown < 0) {
                ParentFsm.spriteRenderer.color = isBaseColor ? Color.cyan : HexMapUI.GetLevelColor(ParentFsm.model.destinationTile.level);
                swapCountdown += colorSwapRate;
                isBaseColor = !isBaseColor;
            }
            
        }
    }
}