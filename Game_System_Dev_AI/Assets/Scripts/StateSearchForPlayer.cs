using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSearchForPlayer : State
{
    private float searchTimer = 5f;
    private float currentSearchTime = 0f;
    private bool reachedSearchPosition = false;
    //Constructor
    public StateSearchForPlayer(AIController ai) : base(ai) { }

    public override void Enter()
    {
        ai.rend.sharedMaterial = ai.material[1];

        Debug.Log("Entering search State");
        ai.agent.SetDestination(AIManager.Instance.lastKnownPlayerPos);
        //ai.agent.SetDestination(ai.lastKnownPlayerPos);
        reachedSearchPosition = false;
        currentSearchTime = 0;

    }

    public override void Update()
    {
        //check awareness

        if (ai.CanSeePlayer())
        {
            ai.ChangeState(new StateChase(ai));
            return;
        }

        //Travel to last known player position before searching / counting timer
        if (!reachedSearchPosition)
        {
            if (ai.agent.remainingDistance <= ai.agent.stoppingDistance)
            {
                reachedSearchPosition = true;
            }
        }

        //Spins around to look for player once they have reached last known player position.
        ai.transform.Rotate(Vector3.up * 60f * Time.deltaTime);
        Debug.Log("I should be spinning");
        //Go back to patrol after 5 seconds or so
        currentSearchTime += Time.deltaTime;
        if (currentSearchTime >= searchTimer) 
        { 
            ai.ChangeState(new StatePatrol(ai));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Search State");
    }
}
