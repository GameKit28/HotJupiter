using System.Collections;
using System.Collections.Generic;
using MeEngine.Events;
using MeEngine.FsmManagement;
using UnityEngine;

namespace HotJupiter
{
	public class TimeManager : MeFsm
	{
		public const float TurnDuration = 2.5f;

		public static float TurnTime { get; private set; } = 0;
		public static float TurnDeltaTime { get; private set; } = 0;

		public static float TurnTimeNormalized
		{
			get { return Mathf.Clamp01(TurnTime / TurnDuration); }
		}

		public static float UITime { get; private set; } = 0;
		public static float UIDeltaTime { get; private set; } = 0;

		public static float SimulationTime { get; private set; } = 0;
		public static float SimulationDeltaTime { get; private set; } = 0;

		public static float SimulationTimeNormalized
		{
			get { return Mathf.Clamp01(SimulationTime / TurnDuration); }
		}

		public static float ElapsedGameTime { get; private set; } = 0;

		protected override void Start()
		{
			GameControllerFsm.eventPublisher.SubscribeAll(this);
		}

		void Update()
		{
			UIDeltaTime = Time.deltaTime;
			UITime += UIDeltaTime;

			SimulationDeltaTime = Time.deltaTime;
			SimulationTime += SimulationDeltaTime;
		}

		[EventListener]
		void OnPlaymodeStart(GameControllerFsm.Events.BeginPlayingOutTurnState @event)
		{
			SwapState<PlayingState>();
		}

		[EventListener]
		void OnPlanModeStart(GameControllerFsm.Events.BeginCommandSelectionState @event)
		{
			SwapState<SimulatingState>();
		}

		class SimulatingState : MeFsmState<TimeManager>
		{
			protected override void EnterState()
			{
				TurnTime = 0;
				TurnDeltaTime = 0;
				Debug.Log("Entering SimulatingState");
			}
		}

		class PlayingState : MeFsmState<TimeManager>
		{
			protected override void EnterState()
			{
				TurnTime = 0;
				Debug.Log("Entering PlayingState");
			}

			void Update()
			{
				TurnDeltaTime = Time.deltaTime;
				TurnTime += TurnDeltaTime;
				ElapsedGameTime += TurnDeltaTime;
			}
		}
	}
}
