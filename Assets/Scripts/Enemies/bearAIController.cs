using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The AIController script is used to determine how the enemies react in the environment in accordance to the player, and whether or not to chase them or not, depending on if they can see them, otherwise they will continue to patrol their own particular area
/// </summary>
public class bearAIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;
    public float viewRadius = 15;
    public float viewAngle = 360;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] waypoints;
    private int m_CurrentWaypointIndex;

    private Vector3 playerLastPosition = Vector3.zero;
    private Vector3 m_PlayerPosition;

    private float m_WaitTime;
    private float m_TimeToRotate;
    private bool m_PlayerInRange;
    private bool m_PlayerNear;
    private bool m_IsPatrol;
    private bool m_CaughtPlayer;

    private Animator animator; // Reference to the Animator component

    private float attackCooldown = 2f; // Cooldown time between attacks
    private float timeSinceLastAttack = 0f; // Time since last attack
    private MobEnemyState mobEnemyState; // Reference to the MobEnemyState component


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
        mobEnemyState = GetComponent<MobEnemyState>(); // Make sure to attach MobEnemyState script to the bear GameObject

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing from this GameObject", this);
        }

        if (mobEnemyState == null)
        {
            Debug.LogError("MobEnemyState component missing from this GameObject", this);
        }

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        if (waypoints.Length > 0)
        {
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
        else
        {
            Debug.LogError("No waypoints assigned in the waypoints array", this);
        }
    }

    void Update()
    {
        if (animator != null)
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

            // Handle attack cooldown
            if (timeSinceLastAttack < attackCooldown)
            {
                timeSinceLastAttack += Time.deltaTime;
            }

            if (m_PlayerInRange && timeSinceLastAttack >= attackCooldown)
            {
                AttackPlayer();
                timeSinceLastAttack = 0f; // Reset the attack cooldown
            }
        }
    }

    private void AttackPlayer()
    {
        float attackRange = 2.0f; // Example attack range
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null && Vector3.Distance(transform.position, playerObject.transform.position) <= attackRange)
        {
            PlayerState playerState = playerObject.GetComponent<PlayerState>();
            if (playerState != null)
            {
                playerState.TakeDamage(mobEnemyState.AttackDamage());
                animator.SetTrigger("Attack"); // Make sure there is a trigger named "Attack" in your Animator Controller
            }
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

    void Move(float speed)
    {
        animator.SetBool("Idle", false);
        animator.SetBool("WalkForward", true);
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void Stop()
    {
        animator.SetBool("WalkForward", false);
        animator.SetBool("Idle", true);
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    public void NextPoint()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
        else
        {
            Debug.LogWarning("Waypoints array is null or empty", this);
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
