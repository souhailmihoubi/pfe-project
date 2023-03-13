using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float startingHealth;
    public float currentHealth;

    void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(currentHealth.ToString());
        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
            //Play death animation
        }
    }
}
