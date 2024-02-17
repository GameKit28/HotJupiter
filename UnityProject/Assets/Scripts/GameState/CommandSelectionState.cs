using System.Collections;
using System.Collections.Generic;
using MeEngine.Events;
using MeEngine.FsmManagement;
using UnityEngine;

namespace HotJupiter
{
	public partial class GameControllerFsm : MeFsm
	{
		//The player must select a destination tile and any other actions for their turn
		public class CommandSelectionState : MeFsmState<GameControllerFsm>
		{
			protected override void EnterState()
			{
				base.EnterState();
				Debug.Log("Entering CommandSelectionState");

				GameControllerFsm.eventPublisher.Publish(new Events.BeginCommandSelectionState());
				GameControllerFsm.eventPublisher.Publish(
					new Events.BeginCommandSelectionStatePost()
				); //Some systems need to do their thing after others have
			}

			// Update is called once per frame
			void Update()
			{
				if (Input.GetKeyDown(KeyCode.Space))
				{
					ParentFsm.DoCommitTurn();
				}
			}
		}
	}
}
