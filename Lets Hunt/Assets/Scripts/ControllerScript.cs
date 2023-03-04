using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody),typeof(CapsuleCollider))]
public class ControllerScript : MonoBehaviour
{
    //Input 
    protected FixedJoystick joystick;

    protected Rigidbody _rigidbody;
    
    [SerializeField] private AnimatorController _animatorController;


    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;

    private Vector3 moveVector;







    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        joystick = FindObjectOfType<FixedJoystick>();
    }

    private void Update()
    {

        Move();
    }

    private void Move()
    {
        
        if (view.IsMine)
        {
            
          
            moveVector = Vector3.zero;
            moveVector.x = joystick.Horizontal * speed * Time.deltaTime;
            moveVector.z = joystick.Vertical * speed * Time.deltaTime;

            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                Vector3 direction = Vector3.RotateTowards(transform.forward, moveVector, rotateSpeed * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(direction);
                _animatorController.PlayRun();
            }
            else if (joystick.Horizontal == 0 && joystick.Vertical == 0)
            {
                _animatorController.PlayIdle();
            }

            //Show Range Indicator
           
        }


        _rigidbody.MovePosition(_rigidbody.position + moveVector);
    }

   
}
