using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChase : State
{
    //Constructor
    public StateChase(AIController ai) : base(ai) { }

    public override void Enter()
    {
        //Debug.Log("Entering Chase State");

        ai.rend.sharedMaterial = ai.material[2];
    }

    public override void Update()
    {
        ai.ChasePlayer();
        if (!ai.CanSeePlayer())
        {
            ai.ChangeState(new StateSearchForPlayer(ai));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Chase State");
    }
}
