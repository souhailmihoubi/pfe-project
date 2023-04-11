using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    IDLE,
    WALK,
    RUN,
    PAUSE,
    GOBACK,
    ATTACK,
    DEATH
}

public class EnemyControl : MonoBehaviour
{
    [SerializeField] private float damageAmount = 5f;
    [SerializeField] private float attack_Distance = 2f;
    [SerializeField] private float alert_attack_Distance = 8f;
    [SerializeField] private float follow_Distance = 15f;

    private float enemyToPlayerDistance;

    [HideInInspector]
    public EnemyState enemy_CurrentState = EnemyState.IDLE;
    private EnemyState enemy_LastState = EnemyState.IDLE;

    private Transform playerTarget;

    private Vector3 initialPostion;

    [SerializeField] private float move_Speed = 2f;
    [SerializeField] private float walk_Speed = 1f;


    private Vector3 whereToMove = Vector3.zero;

    //attack variables
    private float currentAttackTime;

    [SerializeField] private float waitAttackTime = 1f;

    private Animator anim;
    private bool finished_Animation = true;
    private bool finished_Movement = true;

    PlayerHealth playerHealth;

    //Navigation variables
    private NavMeshAgent navAgent;
    private Vector3 whereToNavigate;

    //Health Script
    private EnemyHealth enemyHealth;

    void Awake()
    {
        playerTarget = GameObject.FindWithTag("Player").transform;

        playerHealth = playerTarget.GetComponent<PlayerHealth>();

        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        initialPostion = transform.position;
        whereToNavigate = transform.position;

        enemyHealth = GetComponent<EnemyHealth>();
    }


    void Update()
    {
       
        if (enemy_CurrentState != EnemyState.DEATH)
        {
            enemy_CurrentState = SetEnemyState(enemy_CurrentState, enemy_LastState, enemyToPlayerDistance);

            if (finished_Movement)
            {
                GetStateControl(enemy_CurrentState);
            }
            else
            {
                if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    finished_Movement = true;
                }
                else if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    anim.SetBool("Attack", false);
                }
            }
        }
        else
        {
            navAgent.enabled = false;
        }
    }

    EnemyState SetEnemyState(EnemyState curState, EnemyState lastState, float enemyToPlayerDis)
    {
        enemyToPlayerDis = Vector3.Distance(transform.position, playerTarget.position);

        if (enemyToPlayerDis <= alert_attack_Distance)
        {
            lastState = curState;
            curState = EnemyState.ATTACK;
        }
        else if (enemyToPlayerDis <= follow_Distance)
        {
            lastState = curState;
            curState = EnemyState.IDLE;
        }

        return curState;
    }


    void GetStateControl(EnemyState curState)
    {
        if (curState == EnemyState.ATTACK)
        {
            anim.SetBool("Run", false);
            whereToMove.Set(0f, 0f, 0f);
            navAgent.SetDestination(playerTarget.position); // set destination to player's position
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerTarget.position - transform.position), 5f * Time.deltaTime);
            if (currentAttackTime >= waitAttackTime)
            {
                anim.SetBool("Attack", true);
                playerHealth.TakeDamage(damageAmount);
                finished_Animation = false;
                currentAttackTime = 0f;
            }
            else
            {
                anim.SetBool("Attack", false);
                currentAttackTime += Time.deltaTime;
            }
        }
        else if (curState == EnemyState.IDLE)
        {
            anim.SetBool("Run", false);
            anim.SetBool("Walk", false);
            anim.SetBool("Attack", false);
            navAgent.SetDestination(transform.position);
            transform.LookAt(playerTarget.position);
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, alert_attack_Distance);
    }


}

