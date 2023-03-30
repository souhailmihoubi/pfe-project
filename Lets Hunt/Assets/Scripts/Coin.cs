using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coin : MonoBehaviourPunCallbacks
{
    public int value = 1; // The value of the coin
    private bool isCollected = false; // Whether the coin has been collected
    public  int coinsCollected=0; 
    private void OnTriggerEnter(Collider other)
    {
        // Check if the coin has not been collected and the player has collided with it
        if (!isCollected && other.CompareTag("Player"))
        {
            // Set the coin as collected
            isCollected = true;

            // Add the coin value to the player's balance
            GameManager.instance.AddCoins(value);

            // Destroy the coin object
            PhotonNetwork.Destroy(gameObject);

        }
    }
}
