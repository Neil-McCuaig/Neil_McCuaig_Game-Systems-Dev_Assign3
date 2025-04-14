using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Material[] material;
    public Renderer rend;

    public StateMachine stateMachine;
    public Transform player;
    //public Transform visionCone;

    public NavMeshAgent agent;

    //Could still do this and then have it so that either this would be used or the AIManager version would be used using an if statement.
    //public Vector3 lastKnownPlayerPos;

    public bool playerInCone;
    public bool canSeePlayer;

    public Transform[] patrolWaypoints;
    public int currentWaypointIndex;
    public float patrolSpeed = 5;
    public float visionAngle = 90f;
    public float detectionRange = 8f;
    public float rotationSpeed = 3f;
    public float hearingRange = 5f;
    public float hearingThreshhold = 10f;
    public float playerVolume = 15f;

    public bool isManaged = true;
    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];

        agent = GetComponent<NavMeshAgent>();
        stateMachine = new StateMachine();
        if (isManaged)
        {
            AIManager.Instance.RegisterAgent(this);
        }

        stateMachine.ChangeState(new StatePatrol(this));
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void ChangeState(State newState)
    {
        stateMachine.ChangeState(newState);
    }

    public bool CanSeePlayer()
    {

        return hasLineOfSight(player);

        //return Vector3.Distance
    }

    public bool CanHearPlayer(float noiseLevel)
    {
        if (player == null) return false;

        if (Vector3.Distance(transform.position, player.position) < hearingRange && noiseLevel > hearingThreshhold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetPlayerInVisionCone(bool isVisible)
    {
        playerInCone = isVisible;
    }

    public bool hasLineOfSight(Transform target)
    {
        /*if (!playerInCone) 
        {
            return false;
        }

        //Subtract their position from your position
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToTarget, out hit, detectionRange))
        {
            if (hit.transform == target) 
            { 
                return true;
            }
        }

        return false;*/

        //If your really close it will go for you.
        if(Vector3.Distance(transform.position, player.position) <= 0.5f)
        {
            AIManager.Instance.lastKnownPlayerPos = player.position;
            return true;
        }


        Vector3 diretionToTarget = (target.position - transform.position).normalized;

        float angleToPlayer = Vector3.Angle(transform.forward, diretionToTarget);

        if (angleToPlayer < visionAngle / 2f)
        {
            Debug.Log("playing in cone");

            RaycastHit hit;
            if (Physics.Raycast(transform.position, diretionToTarget, out hit, detectionRange))
            {
                if (hit.transform == target)
                {
                    AIManager.Instance.lastKnownPlayerPos = player.position;
                    return true; // Player is in line of sight, chase begins.
                }
            }
        }

        return false;
    }

    public void ChasePlayer()
    {

        /*Could be Vector3.position instead or something like that so that it goes for the player. Add a cooldown if it gets stuck on a wall.
        //transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * patrolSpeed);

        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Vector3 NewDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(NewDirection);
        }*/

        // || is OR for future reference

        //rend.sharedMaterial = material[2];

        if (player == null || agent == null)
        {
            return;
        }
        if (!agent.pathPending && agent.destination != player.position)
        {
            agent.SetDestination(player.position);
        }
    }

    public void Patrol()
    {
        //rend.sharedMaterial = material[0];

        if (patrolWaypoints.Length == 0)
        {
            return;
        }

        //Rotate ai toward the next waypoint
        Transform targetWaypoint = patrolWaypoints[currentWaypointIndex];

        /*Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
        Vector3 NewDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(NewDirection);
        }*/

        //transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, Time.deltaTime * patrolSpeed);
        agent.SetDestination(targetWaypoint.position);

        /*if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % patrolWaypoints.Length;
        }*/

        /*if (agent.remainingDistance < 0.2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % patrolWaypoints.Length;
        }*/



        if (agent.remainingDistance < 0.02f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % patrolWaypoints.Length;
            agent.SetDestination(patrolWaypoints[currentWaypointIndex].position);
        }

    }

    public void OnStateExit()
    {

    }
}
