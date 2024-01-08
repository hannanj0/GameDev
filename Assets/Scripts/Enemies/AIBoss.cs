using UnityEngine;
using UnityEngine.AI;

public class AIBoss : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float viewRadius = 75;
    public float viewAngle = 360;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float speedRun = 9; 
    public float attackRange = 10f;


    private Animator animator;
    private Vector3 m_PlayerPosition;
    private bool m_PlayerInRange;
    private bool m_CaughtPlayer;
    public AudioSource dragonAttack;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = true; 
        

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing from this GameObject", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_CaughtPlayer)
        {
            EnvironmentView();
            if (m_PlayerInRange)
            {
                Chasing();
            }
            else
            {
                Idle();
            }
        }
    }

    private void Chasing()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, m_PlayerPosition);

        if (distanceToPlayer <= viewRadius)
        {
            // Only move towards the player if they are outside of attack range
            if (distanceToPlayer > attackRange)
            {
                Move(speedRun);
                navMeshAgent.SetDestination(m_PlayerPosition);
                animator.SetBool("Walk", true);
                animator.SetBool("Attack", false);
            }
            else
            {
                // Stop and attack if within attack range
                navMeshAgent.isStopped = true;
                animator.SetBool("Walk", false);
                animator.SetBool("Attack", true);
                dragonAttack.Play();
            }
        }
        else
        {
            Idle();
        }
    }

    private void Idle()
    {
        navMeshAgent.isStopped = true;
        if (animator != null)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", false);
        }
    }


    private void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    /// <summary>
    /// This method deduces what will happen when it is chasing the player, which would be to dash towards the player given the distance, and depending on cooldown as well, otherwise, it will move towards the player.    
    /// </summary>
    void EnvironmentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        m_PlayerInRange = false;
        foreach (var playerCollider in playerInRange)
        {
            Transform player = playerCollider.transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_PlayerInRange = true;
                    m_PlayerPosition = player.transform.position;
                }
            }
        }
    }

    public void OnDeath()
    {
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }
}
