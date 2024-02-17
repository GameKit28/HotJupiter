using System.Collections;
using System.Collections.Generic;
using MeEngine.Events;
using MeEngine.FsmManagement;
using UnityEngine;

namespace HotJupiter
{
	public partial class GameControllerFsm : MeFsm
	{
		//
		public class CalculatingInterceptsState : MeFsmState<GameControllerFsm>
		{
			protected override void EnterState()
			{
				base.EnterState();
				Debug.Log("Entering CalculatingInterceptsState");

				SwapState<PlayingOutTurnState>();
			}
		}
	}
}
