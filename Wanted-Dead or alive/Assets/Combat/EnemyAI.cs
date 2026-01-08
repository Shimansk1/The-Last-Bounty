using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 50f;
    public float attackRadius = 2.5f;
    public float exitAttackBuffer = 1.0f; // o kolik metrů musí hráč odskočit, aby AI přestala útočit
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
            // nech agent dojit dostatecne blizko (menší než attackRadius)
            agent.stoppingDistance = Mathf.Max(0.1f, attackRadius * 0.6f);
            agent.updateRotation = true;
            agent.updatePosition = true;
        }

        if (animator != null)
            animator.applyRootMotion = false; // root motion může kazit navmesh chování
    }

    private void Update()
    {
        if (player == null || agent == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Debug: uvidíš stav a vzdálenost
        //Debug.Log($"[ENEMY] {name} State={state} Dist={distance:F2} attackR={attackRadius} stopDist={agent.stoppingDistance:F2}");

        // Stavové přechody (hysteréze: buffer při opuštění útoku)
        switch (state)
        {
            case State.Idle:
                if (distance <= detectionRadius)
                    EnterChasing();
                break;

            case State.Chasing:
                // když hráč dojde do attack rady -> přepni na attack
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
                    // aktualizuj cíl při chase
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                    animator?.SetBool("IsRunning", true);
                    animator?.SetBool("IsFighting", false);
                }
                break;

            case State.Attacking:
                // pokud hráč zdrhne dál než attackRadius + buffer -> přepni zpátky do chase
                if (distance > attackRadius + exitAttackBuffer)
                {
                    EnterChasing();
                }
                else
                {
                    // zůstaň v attack stavu: otoč se k hráči a útoč dle cooldownu
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
        Debug.Log($"[ENEMY] {name} -> Idle");
    }

    private void EnterChasing()
    {
        state = State.Chasing;
        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator?.SetBool("IsRunning", true);
        animator?.SetBool("IsFighting", false);
        Debug.Log($"[ENEMY] {name} -> Chasing");
    }

    private void EnterAttacking()
    {
        state = State.Attacking;
        agent.isStopped = true;
        agent.ResetPath();
        animator?.SetBool("IsRunning", false);
        animator?.SetBool("IsFighting", true);
        Debug.Log($"[ENEMY] {name} -> Attacking");
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
        // debug na začátek
        Debug.Log($"[ENEMY] {name} TryAttack() lastDiff={(Time.time - lastAttackTime):F2} cooldown={attackCooldown}");

        if (Time.time - lastAttackTime < attackCooldown) return;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("[ENEMY] PlayerHealth not found on player!");
            return;
        }

        if (!playerHealth.isDead)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log($"[ENEMY] {name} DEALT {attackDamage} dmg to player. PlayerHP={playerHealth.currentHealth}");
            lastAttackTime = Time.time;
        }
    }
}
