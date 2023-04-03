using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAnotherWay : MonoBehaviour
{
    private Animator anim;
    private bool canAttack = true;
    private PlayerMove playerMove;

    public float attackRange = 2.0f;
    public float attackDamage = 10.0f;
    public LayerMask enemyLayer;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }
    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }

        Attacking();
    }

    void Attacking()
    {
        if (!playerMove.isMoving  && canAttack)
        {
            anim.SetBool("Attack", true);
        }
        else
        {
            anim.SetBool("Attack", false);
        }
    }
}
