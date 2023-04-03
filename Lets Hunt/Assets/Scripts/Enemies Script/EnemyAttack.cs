using Photon.Pun.Demo.PunBasics;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float damageAmount = 5f;

    private Transform playerTarget;
    private Animator anim;
    private bool finishedAttack = true;
    [SerializeField] private float damageDistance = 2f;


    private PlayerHealth playerHealth;

    void Awake()
    {
        playerTarget = GameObject.FindWithTag("Player").transform;

        anim = GetComponent<Animator>();

        playerHealth = playerTarget.GetComponent<PlayerHealth>();
    }

    void Update()
    {

        if (finishedAttack)
        {
            if (playerTarget)
            {
                DealDamage(CheckIfAttacking());
            }
        }
        else
        {
            if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                finishedAttack = true;
            }
        }
    }

    bool CheckIfAttacking()
    {
        bool isAttacking = false;

        if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
            {
                isAttacking = true;
                finishedAttack = false;
            }
        }

        return isAttacking;
    }

    void DealDamage(bool isAttacking)
    {
        if (isAttacking)
        {
            if (Vector3.Distance(transform.position, playerTarget.position) <= damageDistance)
            {
                playerHealth.TakeDamage(damageAmount);

                anim.SetBool("Attack", true);

                print("enemy attacking");

            }
        }
    }

   /* void OnDrawGizmosSelected()
    {
        // Draw attack range gizmo in scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageDistance);
    }*/

    //----------------

}



