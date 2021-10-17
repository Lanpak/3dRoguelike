
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Animator anim;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public GameObject deathEffect;

    public float health;

    public bool isDead = false;
    public bool isDisabled = true;


    public bool isAttacking;
    public bool isChasing;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange, shootForce;
    public bool playerInSightRange, playerInAttackRange;

    public Transform shootPoint;

    private void Awake()
    {
        Debug.Log("ok");
        player = GameObject.Find("Player(Clone)").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!isDisabled)
        {
            if (!isDead)
            {
                
                HandleAi();
                HandleAnimation();
            }
        }
    }
    private void HandleAnimation()
    {

        if (isChasing)
        {
            anim.SetBool("isChasing", true);
        }
        else
        {
            anim.SetBool("isChasing", false);

        }
        if (isAttacking)
        {
            anim.SetBool("isFiring", true);
        }
        else
        {
            anim.SetBool("isFiring", false);

        }
    }

    private void HandleAi()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
            isChasing = true;
            isAttacking = false;
        }


        if ((playerInSightRange && !playerInAttackRange) || !HandleLineOfSight())
        {
            ChasePlayer();
            isChasing = true;
            isAttacking = false;
        }
        if (playerInAttackRange && playerInSightRange)
        {
            if (HandleLineOfSight())
            {
                AttackPlayer();
                isChasing = false;
                isAttacking = true;
            }
            
            
        }
    }

    private bool HandleLineOfSight()
    {
        bool isLos = false;

        RaycastHit hit;
        var rayDirection = player.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
            if (hit.transform == player)
            {
                isLos = true;
                Debug.Log("enemy can see the player!");
            }
            else
            {
                isLos = false;
                Debug.Log("there is something obstructing the view");
            }
        }
        return isLos;
    }



    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        if (!isDead)
        {
            isDead = true;
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            
            agent.SetDestination(transform.position);
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
