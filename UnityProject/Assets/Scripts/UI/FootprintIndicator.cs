using System;
using System.Collections;
using System.Collections.Generic;
using HexasphereGrid;
using MeEngine.Events;
using UnityEngine;

namespace HotJupiter
{
	public class FootprintIndicator : MonoBehaviour
	{
		const bool hideWhenExecuting = true;

		[SerializeReference]
		public GameObject attachedObject; // Must derive from IHaveTilePosition
		private IHaveTileFootprint tileFootprintObject;

		private HexTileMesh tileMesh;
		private bool isDirty = true;

		void Awake()
		{
			tileMesh = new HexTileMesh();
			GetComponent<MeshFilter>().mesh = tileMesh.GeneratedMesh;
		}

		private void Start()
		{
			if (attachedObject != null)
			{
				tileFootprintObject = attachedObject.GetComponent<IHaveTileFootprint>();
			}

			GameControllerFsm.eventPublisher.SubscribeAll(this);
			StartCoroutine(
				DelayInitialize(() =>
				{
					if (tileFootprintObject.GetFootprint() != null)
					{
						tileFootprintObject.GetFootprint().FootprintUpdatedEvent +=
							OnFootprintUpdated;
						return true;
					}
					else
					{
						return false;
					}
				})
			);
		}

		IEnumerator DelayInitialize(Func<bool> action)
		{
			while (action.Invoke())
			{
				yield return null;
			}
		}

		// Update is called once per frame
		void Update()
		{
			if (isDirty && tileFootprintObject.GetFootprint() != null)
			{
				List<FootprintTile> footParts = tileFootprintObject
					.GetFootprint()
					.GetAllTilesInFootprint();
				tileMesh.GenerateMeshFromTiles(footParts, transform);

				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;

				GetComponent<MeshRenderer>().material.color = HexMapUI.GetLevelColor(
					footParts[0].level
				);

				isDirty = false;
			}
		}

		void OnFootprintUpdated()
		{
			isDirty = true;
		}

		[EventListener]
		private void OnPlayingOutTurnStart(GameControllerFsm.Events.BeginPlayingOutTurnState @event)
		{
			GetComponent<MeshRenderer>().enabled = !hideWhenExecuting;
		}

		[EventListener]
		private void OnPlayingOutTurnEnd(GameControllerFsm.Events.EndPlayingOutTurnState @event)
		{
			GetComponent<MeshRenderer>().enabled = true;
		}
	}
}
