using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collided : MonoBehaviour
{
    public float rangeScale;
    protected FixedJoystick joystick;

    private bool ready = false;

    private void Awake()
    {
        joystick = FindObjectOfType<FixedJoystick>();
    }

   
   

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            ready = true;  
        }
        else
        {
            ready = false;
        }
    }

    public bool EnemyCollided()
    {
        return ready;
    }

}
