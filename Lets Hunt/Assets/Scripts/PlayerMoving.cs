using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMoving : MonoBehaviour
{
    public Joystick joystick;
    bool controlIsActive = true;
    public float speed;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }
    void Update()
    {

            transform.position += new Vector3(joystick.Horizontal * speed * Time.deltaTime, 0, joystick.Vertical * speed * Time.deltaTime);


    }
}
