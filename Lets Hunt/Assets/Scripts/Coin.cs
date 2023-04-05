using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Coin : MonoBehaviourPunCallbacks
{
    public int value = 10;
    private bool isCollected = false;
    public  int coinsCollected;

    public TextMeshProUGUI coins;

    private void Start()
    {
        coins = GameObject.FindGameObjectWithTag("coinsCollected").GetComponentInParent<TextMeshProUGUI>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            isCollected = true;

            SaveManager.instance.coins += value;
            SaveManager.instance.Save();

            coinsCollected = int.Parse(coins.text) + value ;

            coins.text = coinsCollected.ToString();

            PhotonNetwork.Destroy(gameObject);

        }
    }
}
