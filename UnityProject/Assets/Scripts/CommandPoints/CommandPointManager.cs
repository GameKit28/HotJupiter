using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.Events;
public class CommandPointManager : MonoBehaviour
{
    public GameObject commandPointPrefab;
    public GameObject playerShipPiece;

    void Awake(){
        EventManager.SubscribeAll(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [EventListener]
    void OnStartNewTurn(GameControllerFsm.Events.NewTurnEvent @event)
    {

        //Standard Positions

        //Forward Facing (current speed)

        //Forward Facing (speed up)

        //Forward Facing (slow down)

        //Turn Left

        //Turn Right

        //Climb Altitude

        //Descend Altitude
    }
}
