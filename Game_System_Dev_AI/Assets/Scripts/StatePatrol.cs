using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatrol : State
{ 
    //Constructor
    public StatePatrol(AIController ai) : base(ai) { }

   
    public override void Enter()
    {
        Debug.Log("Entering Patrol State");

        ai.rend.sharedMaterial = ai.material[0];
    }

    public override void Update()
    {
        if (ai.CanSeePlayer()) 
        {
            Debug.Log("can see player");
            ai.ChangeState(new StateChase(ai));
        }
        else if (ai.CanHearPlayer(ai.playerVolume) && !ai.CanSeePlayer())
        {
            ai.ChangeState(new StateSearchForPlayer(ai));
        }
        else
        {
            ai.Patrol();
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }
}
