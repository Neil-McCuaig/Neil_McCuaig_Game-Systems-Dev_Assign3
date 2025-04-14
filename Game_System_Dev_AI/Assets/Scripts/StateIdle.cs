using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : State
{
    //Constructor
    public StateIdle(AIController ai) : base(ai) { }

    public override void Enter()
    {
        Debug.Log("Entering Idle State");

        ai.rend.sharedMaterial = ai.material[0];
    }

    public override void Update()
    {
        if (ai.CanSeePlayer())
        {
            ai.ChangeState(new StateChase(ai));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}
