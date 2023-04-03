using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coin : MonoBehaviourPunCallbacks
{
    public int value = 10;
    private bool isCollected = false;
    public  int coinsCollected = 0; 
    private void OnTriggerEnter(Collider other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            isCollected = true;

            SaveManager.instance.coins += value;
            SaveManager.instance.Save();

            PhotonNetwork.Destroy(gameObject);

        }
    }
}
