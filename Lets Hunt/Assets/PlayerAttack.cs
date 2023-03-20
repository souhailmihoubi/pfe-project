using Photon.Pun;
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



}