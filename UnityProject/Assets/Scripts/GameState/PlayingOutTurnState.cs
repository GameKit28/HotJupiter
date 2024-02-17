using System.Collections;
using System.Collections.Generic;
using MeEngine.Events;
using MeEngine.FsmManagement;
using UnityEngine;

namespace HotJupiter
{
	public partial class GameControllerFsm : MeFsm
	{
		//The player watches as the events of the turn unfold
		public class PlayingOutTurnState : MeFsmState<GameControllerFsm>
		{
			protected override void EnterState()
			{
				Debug.Log("Entering PlayingOutTurnState");
				GameControllerFsm.eventPublisher.Publish(new Events.BeginPlayingOutTurnState());
			}

			protected override void ExitState()
			{
				GameControllerFsm.eventPublisher.Publish(new Events.EndPlayingOutTurnState());
			}

			// Update is called once per frame
			void Update()
			{
				if (TimeManager.TurnTimeNormalized >= 1.0f)
				{
					SwapState<CommandSelectionState>();
				}
			}
		}
	}
}
