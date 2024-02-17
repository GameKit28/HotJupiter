using System.Collections;
using System.Collections.Generic;
using MeEngine.FsmManagement;
using UnityEngine;

namespace HotJupiter
{
	public partial class CommandPointViewFsm
	{
		public class WaitingState : MeFsmState<CommandPointViewFsm>
		{
			protected override void EnterState()
			{
				base.EnterState();

				ParentFsm.model.spline.gameObject.SetActive(false);
			}

			void OnMouseEnter()
			{
				Debug.Log("Mouse Entered");
				SwapState<HoverState>();
			}
		}
	}
}
