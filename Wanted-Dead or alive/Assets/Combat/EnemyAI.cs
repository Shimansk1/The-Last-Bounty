using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 50f;
    public float attackRadius = 2.5f;
    public float exitAttackBuffer = 1.0f;
    public float moveSpeed = 4.5f;

    public int attackDamage = 10;
    public float attackCooldown = 1.2f;

    private float lastAttackTime = -Mathf.Infinity;

    private NavMeshAgent agent;
    private Animator animator;

    private enum State { Idle, Chasing, Attacking }
    private State state = State.Idle;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.stoppingDistance = Mathf.Max(0.1f, attackRadius * 0.6f);
            agent.updateRotation = true;
            agent.updatePosition = true;
        }

        if (animator != null)
            animator.applyRootMotion = false;
    }

    private void Update()
    {
        if (player == null || agent == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        switch (state)
        {
            case State.Idle:
                if (distance <= detectionRadius)
                    EnterChasing();
                break;

            case State.Chasing:
                if (distance <= attackRadius)
                {
                    EnterAttacking();
                }
                else if (distance > detectionRadius)
                {
                    EnterIdle();
                }
                else
                {
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                    animator?.SetBool("IsRunning", true);
                    animator?.SetBool("IsFighting", false);
                }
                break;

            case State.Attacking:
                if (distance > attackRadius + exitAttackBuffer)
                {
                    EnterChasing();
                }
                else
                {
                    FacePlayer();
                    agent.isStopped = true;
                    animator?.SetBool("IsRunning", false);
                    animator?.SetBool("IsFighting", true);
                    TryAttack();
                }
                break;
        }
    }

    private void EnterIdle()
    {
        state = State.Idle;
        agent.ResetPath();
        agent.isStopped = true;
        animator?.SetBool("IsRunning", false);
        animator?.SetBool("IsFighting", false);
    }

    private void EnterChasing()
    {
        state = State.Chasing;
        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator?.SetBool("IsRunning", true);
        animator?.SetBool("IsFighting", false);
    }

    private void EnterAttacking()
    {
        state = State.Attacking;
        agent.isStopped = true;
        agent.ResetPath();
        animator?.SetBool("IsRunning", false);
        animator?.SetBool("IsFighting", true);
    }

    private void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion target = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 8f);
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null) return;

        if (!playerHealth.isDead)
        {
            playerHealth.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
    }
}