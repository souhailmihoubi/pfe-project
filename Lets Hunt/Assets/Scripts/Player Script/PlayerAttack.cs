using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2.0f;
    public float attackDamage = 10.0f;
    public LayerMask enemyLayer;

    [SerializeField] private AnimatorController _animatorController;
    protected FixedJoystick joystick;
    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;


    PlayerMove playerMove;


    EnemyHealth enemyHealth;

    //------Rotation Variables------
    public float speed = 1f;
    private Coroutine LookCoroutine;

    public bool isAttacking = false;

    PhotonView view;
    PlayerItem playerItem;



    private void Start()
    {
        view = GetComponent<PhotonView>();
        playerItem = GetComponent<PlayerItem>();
    }
    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        joystick = FindObjectOfType<FixedJoystick>();
    }

    void Update()
    {
        // Check if enough time has elapsed since the last attack
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // Detect enemies within attack range
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

            float closestDistance = float.MaxValue;

            EnemyHealth closestEnemy = null;

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    if (playerMove.isMoving == false)
                    {
                        Vector3 direction = (hitCollider.transform.position - transform.position).normalized;

                        StartRotating();

                        float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                        if (distance < closestDistance)
                        {
                            closestEnemy = hitCollider.GetComponent<EnemyHealth>();
                            closestDistance = distance;
                        }
                    }

                }
            }

            // Attack the closest enemy
            if (closestEnemy != null)
            {
                closestEnemy.TakeDamage(attackDamage);
                _animatorController.PlayAttack();
                isAttacking = true;
                if(closestEnemy.currentHealth <= 0)
                {
                    _animatorController.StopAttack();

                    
                    playerItem.GetKill();
                }

           


                if (playerMove.isMoving == true)
                {
                    _animatorController.StopAttack();
                    isAttacking = false;
                }
            }

            // Reset attack timer
            lastAttackTime = Time.time;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw attack range gizmo in scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void StartRotating()
    {
        if (LookCoroutine != null)
        {
            StopCoroutine(LookCoroutine);
        }

        LookCoroutine = StartCoroutine(LookAt());
    }

    private IEnumerator LookAt()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        float closestDistance = float.MaxValue;
        Transform closestEnemy = null;


        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestEnemy = hitCollider.transform;
                    closestDistance = distance;
                }
            }
        }

        if (closestEnemy != null)
        {
            Vector3 direction = closestEnemy.position - transform.position;
            direction.y = 0f; // zero out the Y-component to only rotate on the Y-axis
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            float time = 0;

            while (time < 1)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

                time += Time.deltaTime * speed;

                yield return null;
            }
        }
    }

    public void IncreaseDamage(float amount)
    {
        attackDamage += amount;

        if (view.IsMine)
        {
            view.RPC("UpdateDamage", RpcTarget.AllBuffered, attackDamage);
        }
    }

    [PunRPC]
    private void UpdateDamage(float newDamage)
    {
        attackDamage = newDamage;
    }

    public void IncreaseAtkSpeed()
    {
        attackCooldown = attackCooldown * 0.75f;
        if (view.IsMine)
        {
            view.RPC("UpdateAtkSpeed", RpcTarget.AllBuffered, attackCooldown);
        }
    }

    [PunRPC]
    private void UpdateAtkSpeed(float newSpeed)
    {
        attackCooldown = newSpeed;
    }


   
    
}