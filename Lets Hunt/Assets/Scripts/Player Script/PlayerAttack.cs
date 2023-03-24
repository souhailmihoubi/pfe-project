using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //------Rotation Variables------
    public float speed = 1f;
    private Coroutine LookCoroutine;

    public bool isAttacking = false;

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
                        // Rotate player towards closest enemy
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
}


/*using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 2.0f;

    [SerializeField] private float attackDamage = 10.0f;

    public LayerMask enemyLayer;

    public float attackSpeed = 2f;

    private float lastAttackTime = 0f;

    [SerializeField] private AnimatorController _animatorController;

    private bool canAttack = true;

    private PlayerMove playerMove;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();

    }
    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        
    }

    void Update()
    {
        // Detect enemies within attack range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        float closestDistance = float.MaxValue;
        EnemyHealth closestEnemy = null;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                if (!playerMove.isMoving)
                {
                    Debug.Log("Attack");
                }
            }
        }
    }



}*/