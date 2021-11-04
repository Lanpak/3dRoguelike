using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    [SerializeField] private float spreadX;
    [SerializeField] private float spreadY;
    [SerializeField] private float spreadZ;

    public AudioSource source;
    public AudioClip hitClip;

    private RoomLogic roomLogic;
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

    private int startHealth;
    public SkinnedMeshRenderer renderer;

    public Material[] mats;

    private void Awake()
    {
        startHealth = (int)health;
        roomLogic = transform.parent.parent.GetChild(1).gameObject.GetComponent<RoomLogic>();
        player = GameObject.Find("Player(Clone)").transform;
        agent = GetComponent<NavMeshAgent>();

        transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
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


    private void HandleHitSFX()
    {
        source.clip = hitClip;
        source.Play();
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
            }
            else
            {
                isLos = false;
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

    private Vector3 HandleSpread()
    {
        Vector3 targetPos = shootPoint.position + shootPoint.forward;
        targetPos = new Vector3
            (
            targetPos.x + Random.Range(-spreadX, spreadX),
            targetPos.y + Random.Range(-spreadY, spreadY),
            targetPos.z + Random.Range(-spreadZ, spreadZ)
            );
        Vector3 direction = targetPos - shootPoint.position;
        return direction.normalized;
    }

    private void HandleMatHit()
    {
        for (int i = 0; i < mats.Length; i++)
        {
            if (health <=startHealth - (i * (startHealth / mats.Length)))
            {
                renderer.material = mats[i];
            }
        }






        //mat.SetColor("_EmissionColor", newCol);
        //mat.color = newCol;
    }


    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            GameObject shot = Instantiate(projectile, shootPoint.position, Quaternion.identity);
            Rigidbody rb = shot.GetComponent<Rigidbody>();
            rb.AddForce(HandleSpread() * shootForce, ForceMode.Impulse);
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
        if (roomLogic.triggered)
        {
            HandleHitSFX();
            health -= damage;
            Debug.Log(health);
            HandleMatHit();
            if (health <= 0)
            {
                DestroyEnemy();
            }
        }
        

        //if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        if (!isDead)
        {
            isDead = true;
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            
            agent.SetDestination(transform.position);
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            
            roomLogic.EnemyDied();
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
