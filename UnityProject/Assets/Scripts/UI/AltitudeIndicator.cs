using System.Collections;
using System.Collections.Generic;
using MeEngine.Events;
using UnityEngine;

namespace HotJupiter
{
	public class AltitudeIndicator : MonoBehaviour
	{
		//private const bool hideWhenExecuting = true;

		private const float xzScale = 0.05f;
		private const float visibleDiffThreshold = 0.1f; //Hide the indicator of the attached object is this close to the "plane"

		public Transform attachedObject;
		public GameObject heightCylinder;
		public GameObject baseSprite;

		private Vector3 centerPoint = Vector3.zero; //Assume Planet is at Vector3.zero for now

		private Vector3? lastPosition;
		private int? lastUIMapAltitude;

		/*void Start() {
		    GameControllerFsm.eventPublisher.SubscribeAll(this);
		}*/

		// Update is called once per frame
		void Update()
		{
			float objectRelativeAltitude =
				attachedObject.position.y - HexMapUI.currentUIMapAltitude;

			if (objectRelativeAltitude > visibleDiffThreshold)
			{
				if (
					(!lastPosition.HasValue || lastPosition.Value != attachedObject.position)
					|| (
						!lastUIMapAltitude.HasValue
						|| lastUIMapAltitude.Value != HexMapUI.currentUIMapLevel
					)
				)
				{
					lastPosition = attachedObject.position;
					lastUIMapAltitude = HexMapUI.currentUIMapLevel;

					this.gameObject.SetActive(true);

					//normal
					Vector3 normal = (attachedObject.position - centerPoint).normalized;
					Vector3 gridIntercectPoint =
						centerPoint
						+ (
							normal
							* HexMapHelper.GetRadialOffsetFromLevel(HexMapUI.currentUIMapLevel)
						);

					//Set the cylinder position to midway between object and grid
					heightCylinder.transform.position =
						(attachedObject.position + gridIntercectPoint) / 2f;
					heightCylinder.transform.localScale = new Vector3(
						xzScale,
						xzScale,
						Vector3.Distance(attachedObject.position, gridIntercectPoint)
					);
					heightCylinder.transform.rotation = Quaternion.LookRotation(normal);

					//Set the little target sprite to grid height
					baseSprite.transform.position = gridIntercectPoint;
					baseSprite.transform.rotation = Quaternion.LookRotation(normal);
					baseSprite.GetComponent<SpriteRenderer>().color = HexMapUI.GetLevelColor(
						HexMapUI.currentUIMapLevel
					);
				}
			}
			else
			{
				this.gameObject.SetActive(false);
			}
		}

		/*[EventListener]
		private void OnPlayingOutTurnStart(GameControllerFsm.Events.BeginPlayingOutTurnState @event){
		    heightCylinder.GetComponentInChildren<MeshRenderer>().enabled = !hideWhenExecuting;
		    baseSprite.GetComponent<SpriteRenderer>().enabled = !hideWhenExecuting;
		}
    
		[EventListener]
		private void OnPlayingOutTurnEnd(GameControllerFsm.Events.EndPlayingOutTurnState @event){
		    heightCylinder.GetComponentInChildren<MeshRenderer>().enabled = true;
		    baseSprite.GetComponent<SpriteRenderer>().enabled = true;
		}*/
	}
}
