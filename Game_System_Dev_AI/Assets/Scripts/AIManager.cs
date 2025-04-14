using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager Instance { get; private set; }

    //Player spotted
    //List of agents that want to make use of shared alerts


    //Last known player position.


    //Registered agents for the alarm
    //Needs System.Collections.Generic to work
    //Options for both when initialized or added later.
    public List<AIController> registeredAgents = new List<AIController>();

    public Vector3 lastKnownPlayerPos;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void RegisterAgent(AIController ai)
    {
        if (ai.isManaged)
        {
            registeredAgents.Add(ai);
        }

    }

    //If it dies
    public void UnregisterAgent(AIController ai)
    {
        if (ai.isManaged)
        {
            registeredAgents.Remove(ai);
        }

    }

    public void AlertPlayerSpotted()
    {
        //player spotted = true
        //update player position

        //notify all managed agents
        foreach (var ai in registeredAgents)
        {
            ai.ChangeState(new StateSearchForPlayer(ai));
        }
    }
}
