using System.Collections;
using System.Collections.Generic;
using MeEngine.FsmManagement;
using UnityEngine;

namespace HotJupiter
{
	public partial class CommandPointViewFsm
	{
		public class HoverState : MeFsmState<CommandPointViewFsm>
		{
			protected override void EnterState()
			{
				base.EnterState();

				ParentFsm.model.spline.gameObject.SetActive(true);
			}

			void OnMouseExit()
			{
				Debug.Log("Hover Ended");
				SwapState<WaitingState>();
			}

			// Update is called once per frame
			void Update()
			{
				if (Input.GetMouseButtonDown(0))
				{
					//The player selected this command point
					ParentFsm.eventPublisher.Publish(
						new Events.CommandPointClicked() { CommandPoint = ParentFsm.controller }
					);
				}
			}
		}
	}
}
