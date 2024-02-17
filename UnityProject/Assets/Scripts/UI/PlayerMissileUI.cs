using System.Collections;
using System.Collections.Generic;
using MeEngine.Events;
using UnityEngine;
using UnityEngine.UI;

namespace HotJupiter
{
	public class PlayerMissileUI : MonoBehaviour
	{
		public ShipGamePiece playerGamePiece;

		public Toggle toggleButton;
		public Text missileCountText;

		void Awake()
		{
			GameControllerFsm.eventPublisher.SubscribeAll(this);
		}

		// Start is called before the first frame update
		void Start()
		{
			UpdateMissileCountText();
		}

		public void FireMissileToggleClicked()
		{
			playerGamePiece.QueueMissile(toggleButton.isOn);
		}

		private void UpdateMissileCountText()
		{
			missileCountText.text = "Missiles: " + playerGamePiece.GetMissileCount();
		}

		[EventListener]
		void OnNewTurn(GameControllerFsm.Events.BeginCommandSelectionState @event)
		{
			toggleButton.isOn = false;
			UpdateMissileCountText();
			if (playerGamePiece.CanFireMissile())
			{
				toggleButton.interactable = true;
				toggleButton.GetComponentInChildren<Text>().text = "Fire Missile";
			}
			else
			{
				toggleButton.interactable = false;
				toggleButton.GetComponentInChildren<Text>().text = "Reloading";
			}
		}
	}
}
