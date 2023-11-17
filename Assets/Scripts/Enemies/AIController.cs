using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The AIController script is used to determine how the enemies react in the environment in accordance to the player, and whether or not to chase them or not, depending on if they can see them, otherwise they will continue to patrol their own particular area
/// </summary>
public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent; // This is a reference to the NavMeshAgent for AI nagivation
    public float startWaitTime = 4; // Time waiting before starting the patrolling
    public float timeToRotate = 2; // Time it takes to rotate when in patrol
    public float speedWalk = 6; // Walking speed of AI
    public float speedRun = 9; // Chasing speed of AI

    public float viewRadius = 15; // This radius is used for detecting the player
    public float viewAngle = 360; // This is the angle used for detecting the player
    public LayerMask playerMask; // The layer mask for detecting what is a "player" (the user)
    public LayerMask obstacleMask; // The layer mask for detecting what is an "obstacle" (the trees)
    public float meshResolution = 1f; // Used for FOV calculations
    public int edgeIterations = 4; // Used to detect edges in FOV mesh
    public float edgeDistance = 0.5f; // Used to detect edges in FOV mesh

    public Transform[] waypoints; // Array of waypoints for patrols
    int m_CurrentWaypointIndex; // Tracks the current waypoint during patrol

    Vector3 playerLastPosition = Vector3.zero; // This stores the players last known position
    Vector3 m_PlayerPosition; // This is the current player position

    float m_WaitTime; // The time to wait before moving
    float m_TimeToRotate; // The time to rotate
    bool m_PlayerInRange; // Determines whether the player is in range or not
    bool m_PlayerNear; // Determines if the player is near or not
    bool m_IsPatrol; // Determines when enemy is in patrol mode or not
    bool m_CaughtPlayer; // If player has been caught or not

    /// <summary>
    /// This initialises the variables and starts the patrol mode for enemy, the current waypoint index and sets the enemy speed and destination
    /// </summary>
    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_CurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    /// <summary>
    /// This checks the environment and decides whether to chase the player or continue with patrolling
    /// </summary>
    void Update()
    {
        EnvironmentView();

        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    /// <summary>
    /// When chasing, it calculates the distance between the enemy and the player, setting the movement to running speed and stops when the player is close. Also, if the player gains distance, there will be a point where the enemy will return back to patrolling
    /// </summary>
    private void Chasing()
{
    m_PlayerNear = false;
    playerLastPosition = Vector3.zero;

    if (!m_CaughtPlayer)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, m_PlayerPosition);
        float minDistanceToPlayer = 3f;

        if (distanceToPlayer > minDistanceToPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition);
        }
        else
        {
            Stop();
        }
    }

    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
    {
        if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
        {
            m_IsPatrol = true;
            m_PlayerNear = false;
            Move(speedWalk);
            m_TimeToRotate = timeToRotate;
            m_WaitTime = startWaitTime;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
        else
        {
            if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }
}

    /// <summary>
    /// When in patrol mode, the enemy will move, wait and rotate, and move to the next waypoint
    /// </summary>
    private void Patroling()
    {
        if (m_PlayerNear)
        {
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
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
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    /// <summary>
    /// This is so that the enemy moves at the particular speed required
    /// </summary>
    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    /// <summary>
    /// Stops the enemy moving
    /// </summary>
    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    /// <summary>
    /// Moves to the next point, with a debug log warning if necessary
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
    /// Handles when the enemy catches the player
    /// </summary>
    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    /// <summary>
    /// When the enemy is looking for the player, within the patrolling period
    /// </summary>
    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// This is to detect whether the player is in the viewing range of the enemy, and if it is, then the enemy will switch to chasing mode in order to pursue the player
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
                    m_IsPatrol = false;
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
            if (m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }
        }
    }
}
