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
    [SerializeField] private float attack_Distance = 2f;
    [SerializeField] private float alert_attack_Distance = 8f;
    [SerializeField] private float follow_Distance = 15f;

    private float enemyToPlayerDistance;

    [HideInInspector]
    public EnemyState enemy_CurrentState = EnemyState.IDLE;
    private EnemyState enemy_LastState = EnemyState.IDLE;

    private Transform playerTarget;
    private Vector3 initialPostion;

    private float move_Speed = 3f;
    private float walk_Speed = 2f;

    private CharacterController charController;
    private Vector3 whereToMove = Vector3.zero;

    //attack variables
    private float currentAttackTime;
    [SerializeField] private float waitAttackTime = 1f;

    private Animator anim;
    private bool finished_Animation = true;
    private bool finished_Movement = true;

    //Navigation variables
    private NavMeshAgent navAgent;
    private Vector3 whereToNavigate;

    //Health Script
    private EnemyHealth enemyHealth;

    void Awake()
    {
        playerTarget = GameObject.FindWithTag("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
        charController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        initialPostion = transform.position;
        whereToNavigate = transform.position;

        enemyHealth = GetComponent<EnemyHealth>();
    }


    void Update()
    {
        //IF Health is <= 0 we will set the state of death
        if (enemyHealth.currentHealth <= 0)
        {
            enemy_CurrentState = EnemyState.DEATH;
        }

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
            anim.SetBool("Die", true);

            charController.enabled = false;

            navAgent.enabled = false;

            if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Die")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {

                Destroy(this.gameObject, 1f);

            }
        }
    }

    EnemyState SetEnemyState(EnemyState curState, EnemyState lastState, float enemyToPlayerDis)
    {

        enemyToPlayerDis = Vector3.Distance(transform.position, playerTarget.position);

        float initiatDistance = Vector3.Distance(initialPostion, transform.position);

        if (initiatDistance > follow_Distance)
        {
            lastState = curState;
            curState = EnemyState.GOBACK;

        }
        else if (enemyToPlayerDis <= attack_Distance)
        {
            lastState = curState;
            curState = EnemyState.ATTACK;
        }
        else if (enemyToPlayerDis >= alert_attack_Distance && lastState == EnemyState.PAUSE || lastState == EnemyState.ATTACK)
        {
            lastState = curState;
            curState = EnemyState.PAUSE;
        }
        else if (enemyToPlayerDis <= alert_attack_Distance && enemyToPlayerDis > attack_Distance)
        {
            if (curState != EnemyState.GOBACK || lastState == EnemyState.WALK)
            {
                lastState = curState;
                curState = EnemyState.PAUSE;
            }
        }
        else if (enemyToPlayerDis > alert_attack_Distance && lastState != EnemyState.GOBACK && lastState != EnemyState.PAUSE)
        {
            lastState = curState;
            curState = EnemyState.WALK;
        }

        return curState;
    }

    void GetStateControl(EnemyState curState)
    {
        if (curState == EnemyState.RUN || curState == EnemyState.PAUSE)
        {
            if (curState != EnemyState.ATTACK)
            {
                Vector3 targetPosition = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);

                if (Vector3.Distance(transform.position, targetPosition) >= 2.1f)
                {
                    anim.SetBool("Run", true); 
                    anim.SetBool("Walk", false);

                    navAgent.SetDestination(targetPosition);
                }
            }
        }
        else if (curState == EnemyState.ATTACK)
        {
            anim.SetBool("Run", false);
            whereToMove.Set(0f, 0f, 0f);

            navAgent.SetDestination(transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerTarget.position - transform.position), 5f * Time.deltaTime);

            if (currentAttackTime >= waitAttackTime)
            {
                
                anim.SetBool("Attack", true);

                finished_Animation = false;

                currentAttackTime = 0f;
            }
            else
            {
                anim.SetBool("Attack", false);
                currentAttackTime += Time.deltaTime;
            }
        }
        else if (curState == EnemyState.GOBACK)
        {
            anim.SetBool("Run", true);

            Vector3 targetPosition = new Vector3(initialPostion.x, transform.position.y, initialPostion.z);

            navAgent.SetDestination(targetPosition);

            if (Vector3.Distance(targetPosition, initialPostion) <= 3.5f)
            {
                enemy_LastState = curState;
                curState = EnemyState.WALK;
            }
        }
        else if (curState == EnemyState.WALK)
        {
            anim.SetBool("Run", false);
            anim.SetBool("Walk", true);

            if (Vector3.Distance(transform.position, whereToNavigate) <= 2f)
            {
                whereToNavigate.x = Random.Range(initialPostion.x - 5f, initialPostion.x + 5f);
                whereToNavigate.z = Random.Range(initialPostion.z - 5f, initialPostion.z + 5f);
            }
            else
            {
                navAgent.SetDestination(whereToNavigate);
            }
        }
        else
        {
            anim.SetBool("Run", false);
            anim.SetBool("Walk", false);
            whereToMove.Set(0f, 0f, 0f);
            navAgent.isStopped = true;
        }

        //        charController.Move(whereToMove);

    }

   /*void OnDrawGizmosSelected()
    {
        // Draw attack range gizmo in scene view
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attack_Distance);
    }*/
}

