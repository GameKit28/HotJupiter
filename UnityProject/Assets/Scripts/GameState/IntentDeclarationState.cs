﻿using System.Collections;
using System.Collections.Generic;
using MeEngine.Events;
using MeEngine.FsmManagement;
using UnityEngine;

namespace HotJupiter
{
	public partial class GameControllerFsm : MeFsm
	{
		//All active pieces commit to an action
		public class IntentDeclarationState : MeFsmState<GameControllerFsm>
		{
			protected override void EnterState()
			{
				base.EnterState();
				Debug.Log("Entering IntentDeclarationState");

				GameControllerFsm.eventPublisher.Publish(new Events.BeginIntentDeclarationState());

				SwapState<CalculatingInterceptsState>();
			}
		}
	}
}
