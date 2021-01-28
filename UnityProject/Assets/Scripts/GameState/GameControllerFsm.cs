using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;
using MeEngine.Events;
public partial class GameControllerFsm : MeFsm
{
    public static class Events{
        public struct NewTurnEvent : IEvent {}
    }

    // Start is called before the first frame update
    protected override void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
