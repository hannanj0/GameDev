using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This script is for the dashing enemy, it is somewhat similar to the AIController script
/// </summary>
public class AIDasher : MonoBehaviour
{
    /// <summary>
    /// These are the public variables that will be accessible in the Unity Inspector
    /// </summary>
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float dashDistance = 15;
    public float dashCooldown = 5;
    public float dashSpeed = 20;
    public float speedWalk = 6;
    public float speedRun = 9;
    public float viewRadius = 15;
    public float viewAngle = 360;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public Transform[] waypoints;
    int m_CurrentWaypointIndex;
    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;
    float m_WaitTime;
    float m_TimeToRotate;
    float m_DashCooldownTimer;
    bool m_PlayerInRange;
    bool m_IsChasing;
    bool m_CaughtPlayer;

    /// <summary>
    /// Initialises the variables, setting up the Nav Agent and initial destination of first waypoint
    /// </summary>
    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsChasing = false;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;
        m_DashCooldownTimer = 0;
        m_CurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;

        if (waypoints.Length > 0)
        {
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
        else
        {
            Debug.LogWarning("No waypoints assigned to the AI controller.");
        }
    }
    /// <summary>
    /// This just checks whether the enemy will be in patrol mode or chase mode
    /// </summary>
    void FixedUpdate()
    {
        EnvironmentView();

        if (m_IsChasing)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    /// <summary>
    /// This method deduces what will happen when it is chasing the player, which would be to dash towards the player given the distance, and depending on cooldown as well, otherwise, it will move towards the player.
    /// </summary>
    private void Chasing()
    {
        
        playerLastPosition = Vector3.zero;

        if (!m_CaughtPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, m_PlayerPosition);
            float minDistanceToPlayer = 3f;

            Debug.Log("Distance to player: " + distanceToPlayer);
            Debug.Log("Dash cooldown timer: " + m_DashCooldownTimer);

            if (distanceToPlayer > minDistanceToPlayer && m_DashCooldownTimer <= 0)
            {
                Debug.Log("Dashing!");
                Dash();
                m_DashCooldownTimer = dashCooldown;
            }
            else
            {
                // Move towards the player at run speed
                Move(speedRun);
                navMeshAgent.SetDestination(m_PlayerPosition);
            }
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (!m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f) // if the player is too far, the enemy will go back to patrol mode
            {
                // Stop chasing and go back to patrol
                m_IsChasing = false;
                Move(speedWalk);
                m_DashCooldownTimer = 0;
                m_PlayerInRange = false;
                m_TimeToRotate = timeToRotate;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
        }
    }
    /// <summary>
    /// This calculates the dashing direction and destination, using Vector3.Lerp to move from its initial position to the player position
    /// </summary>
    private void Dash()
    {
        Vector3 dashDirection = (m_PlayerPosition - transform.position).normalized;
        
        
        // Dash directly to the player's position
        Vector3 dashDestination = m_PlayerPosition;

        // Use MoveTowards in FixedUpdate for more consistent movement
        transform.position = Vector3.Lerp(transform.position, dashDestination, dashSpeed * Time.fixedDeltaTime);
    }
    /// <summary>
    /// This is when the enemy is in patrol mode, it is self-explanatory, going from point A to B
    /// </summary>
    private void Patroling()
    {
        if (m_PlayerInRange)
        {
            m_IsChasing = true;
            m_WaitTime = 0; // Reset wait time when transitioning to chase
            return; // Don't execute the rest of the patrolling logic
        }

        if (m_TimeToRotate <= 0)
        {
            Move(speedWalk);
            
        }
        else
        {
            Stop();
            m_TimeToRotate -= Time.fixedDeltaTime;
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (m_WaitTime <= 0)
            {
                NextPoint();
                Move(speedWalk);
                m_WaitTime -= startWaitTime;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.fixedDeltaTime;
            }
        }
    }

    /// <summary>
    /// Used in the method above, movement of enemy
    /// </summary>
    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    /// <summary>
    /// Stopping between waypoints
    /// </summary>
    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    /// <summary>
    /// This ensures the next point in the array is used for the enemy to move to
    /// </summary>
    public void NextPoint()
    {
        if (waypoints.Length > 0)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
        else
        {
            Debug.LogWarning("No waypoints assigned to the AI controller.");
        }
    }

    /// <summary>
    /// This checks whether the player is in view of the enemy, if so, transition to chasing state, and if not, set the playerInRange to false
    /// </summary>
    void EnvironmentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_PlayerInRange = true;

                    // Transition to chasing state immediately
                    m_IsChasing = true;
                    m_WaitTime = 0;

                    m_PlayerPosition = player.transform.position;
                }
                else
                {
                    m_PlayerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                m_PlayerInRange = false;
            }
        }

        // Update dash cooldown timer
        m_DashCooldownTimer = Mathf.Max(0, m_DashCooldownTimer - Time.fixedDeltaTime);
    }
}
