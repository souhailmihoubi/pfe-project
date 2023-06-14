using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttack : MonoBehaviour
{
    public float attackRange = 2.0f;
    public float attackDamage = 10.0f;
    public LayerMask enemyLayer;

    [SerializeField] private AnimatorController _animatorController;
    protected FixedJoystick joystick;
    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    PlayerMove playerMove;

    public bool isAttacking = false;

    PhotonView view;
    PlayerItem playerItem;

    [Header("Arrow Attack Variables")]
    public bool performArrowAttack = true;
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    EnemyHealth closestEnemy;

    private Coroutine lookCoroutine;

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

    private void Update()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

            float closestDistance = float.MaxValue;
            closestEnemy = null;

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    if (!playerMove.isMoving)
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

            // Attack the closest enemy if it exists
            if (closestEnemy != null && Vector3.Distance(transform.position, closestEnemy.transform.position) <= attackRange)
            {
                if (performArrowAttack)
                {

                   StartCoroutine(RangedAttackInterval());
                }
            }

            // Reset attack timer
            lastAttackTime = Time.time;
        }
    }

    private IEnumerator RangedAttackInterval()
    {
        performArrowAttack = false;

        if (closestEnemy != null && !closestEnemy.enemyDead)
        {
            _animatorController.PlayAttack();

            yield return new WaitForSeconds(attackCooldown / ((100 + attackCooldown) * 0.01f));
        }

        _animatorController.StopAttack();

        performArrowAttack = true;
    }

    public void RangedAttack()
    {
        if (closestEnemy != null)
        {
            SpawnRangedProj("Enemy", closestEnemy);
        }

        performArrowAttack = true;
    }

    private void SpawnRangedProj(string typeOfEnemy, EnemyHealth targetedEnemyObj)
    {
        if (typeOfEnemy == "Enemy" && targetedEnemyObj != null)
        {
            ArrowLauncher arrowLauncher = arrowPrefab.GetComponent<ArrowLauncher>();

            arrowLauncher.damage = attackDamage;
            arrowLauncher.targetType = typeOfEnemy;
            arrowLauncher.target = targetedEnemyObj;
            arrowLauncher.targetSet = true;

            arrowLauncher.shooter = playerItem;

            Instantiate(arrowPrefab, arrowSpawnPoint.transform.position, Quaternion.identity);
        }
    }

    public void StartRotating()
    {
        if (lookCoroutine != null)
        {
            StopCoroutine(lookCoroutine);
        }

        lookCoroutine = StartCoroutine(Look());
    }

    private IEnumerator Look()
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
            transform.LookAt(transform.position + direction);
        }

        yield return null;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw attack range gizmo in scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
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
        attackCooldown *= 0.75f;
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
