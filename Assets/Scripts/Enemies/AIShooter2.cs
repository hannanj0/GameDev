using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIShooter2 : MonoBehaviour
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
    private Animation_Test animationScript;
    int m_CurrentWaypointIndex;
    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;
    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;
    EnemyShoot enemyShooter;
    

    private bool isPatrolling = true;

    public float rotationSpeed = 5f;

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
        enemyShooter = GetComponent<EnemyShoot>();
        animationScript = GetComponent<Animation_Test>();
    }

    void Update()
    {
        if (!m_CaughtPlayer)
        {
            EnvironmentView();

            if (!m_IsPatrol)
            {
                ChasingPlayer();
                if (m_PlayerInRange)
                {
                    enemyShooter.ShootAtPlayer();
                    animationScript.AttackAni(); // Play Attack animation during shooting
                }
            }
            else if (isPatrolling)
            {
                Patroling();
            }
        }

        // Play appropriate animation based on movement
        if (navMeshAgent.velocity.magnitude > 0.1f && !m_CaughtPlayer)
        {
            animationScript.RunAni(); // Play Run animation
        }
        else
        {
            animationScript.IdleAni(); // Play Idle animation
        }
    }

    private void ChasingPlayer()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if (!m_CaughtPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, m_PlayerPosition);

            // Adjust the target position to the height of the player's transform.
            // This assumes the player's transform is at the base of the player.
            Vector3 targetPosition = new Vector3(m_PlayerPosition.x, m_PlayerPosition.y + 1.5f, m_PlayerPosition.z); // Adjust the y value to the height you want to target.

            if (distanceToPlayer > 3f && distanceToPlayer < viewRadius)
            {
                Move(speedRun);
                navMeshAgent.SetDestination(targetPosition);
                transform.LookAt(targetPosition);
            }
            else if (distanceToPlayer <= 3f)
            {
                // Attack the player if within attack range.
                // You may want to call a method to handle attacking here.
                StopMovement();
                transform.LookAt(targetPosition);
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
                    StopMovement();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }

        if (Vector3.Distance(transform.position, m_PlayerPosition) < 3f)
        {
            animationScript.AttackAni();
        }
    }

    void LookAtPlayer(Vector3 player)
    {
        // Get the player's upper body position.
        Vector3 targetPosition = new Vector3(player.x, player.y + 1.5f, player.z); // Adjust 1.5f to the desired height
        navMeshAgent.SetDestination(player);
        transform.LookAt(targetPosition);

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
                StopMovement();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }



    private void Patroling()
    {
        if (m_PlayerNear)
        {
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookAtPlayer(playerLastPosition);
            }
            else
            {
                StopMovement();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;

            // Check if the NavMeshAgent has arrived at the destination
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (m_WaitTime <= 0)
                {
                // Move to the next waypoint
                NextPoint();
                Move(speedWalk);
                m_WaitTime -= startWaitTime;
                }
                else
                {
                StopMovement();
                m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void StopMovement()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    public void NextPoint()
    {
        if (waypoints.Length > 0)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            Debug.Log("Setting destination to waypoint: " + m_CurrentWaypointIndex);
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
        else
        {
            Debug.LogWarning("No waypoints assigned to the AI controller.");
        }
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
        isPatrolling = false;
        animationScript.DeathAni();
    }

    
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
