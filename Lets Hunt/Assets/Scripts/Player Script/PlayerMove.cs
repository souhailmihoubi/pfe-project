using DitzeGames.MobileJoystick;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    private FixedJoystick _joystick;

    [SerializeField] private AnimatorController _animatorController;

    [SerializeField] private float _moveSpeed;

    PhotonView view;

    public bool isMoving = false;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void Awake()
    {
        _joystick = FindObjectOfType<FixedJoystick>();
    }

    public void IncreaseSpeed(float amount)
    {
        _moveSpeed += amount;
        if (view.IsMine)
        {
            view.RPC("UpdateMoveSpeed", RpcTarget.AllBuffered, _moveSpeed);
        }
    }

    [PunRPC]
    private void UpdateMoveSpeed(float newSpeed)
    {
        _moveSpeed = newSpeed;
    }

    void FixedUpdate()
    {
        if (view.IsMine)
        {
            _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveSpeed);

            if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(_joystick.Horizontal, 0, _joystick.Vertical));
                _animatorController.PlayRun();

                isMoving = true;
            }
            else
            {
                _animatorController.StopRun();
                isMoving = false;
            }
        }
    }
}
