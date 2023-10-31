
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public Transform eyePosition;

    public EnemyGun gun;
    public float missArea = 5;

    [SerializeField] private Animator _animator;

    public bool PlayerInSightRange
    {
        get
        {
            if (Physics.CheckSphere(transform.position + transform.forward * sightRange, sightRange, whatIsPlayer))
            {
                RaycastHit _hit;
                if (Physics.Raycast(eyePosition.position, player.transform.position - (eyePosition.position), out _hit,
                    sightRange))
                    return (_hit.transform.CompareTag("Player"));
            }
            return false;
        }
    }
    
    public bool PlayerInAttackRange
    {
        get
        {
            if (Physics.CheckSphere(transform.position + transform.forward * attackRange, attackRange, whatIsPlayer))
            {
                RaycastHit _hit;
                if (Physics.Raycast(eyePosition.position, player.transform.position - (eyePosition.position), out _hit,
                    attackRange))
                    return (_hit.transform.CompareTag("Player"));
            }
            return false;
        }
    }

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        //playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!PlayerInSightRange && !PlayerInAttackRange) Patroling();
        if (PlayerInSightRange && !PlayerInAttackRange) ChasePlayer();
        if (PlayerInAttackRange && PlayerInSightRange) AttackPlayer();
        
        _animator.SetFloat("Speed_f", agent.velocity.magnitude);
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
        _animator.SetBool("Shoot_b", alreadyAttacked);

        if (!alreadyAttacked)
        {
            if (GameManager.Instance.CurrentState != GameManager.GameStates.STARTED)
                return;
            var aim = player.position + new Vector3(Random.Range(-missArea, missArea), Random.Range(-missArea, missArea)
                      ,Random.Range(-missArea, missArea));
            gun.Shoot(aim);
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward * sightRange, sightRange);
    }
}