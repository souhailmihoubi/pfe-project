using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Coin : MonoBehaviourPunCallbacks
{
    public int value = 10;
    private bool isCollected = false;
    public int coinsCollected;

    public TextMeshProUGUI coins;

    private void Start()
    {
      //  coins = GameObject.FindGameObjectWithTag("coinsCollected").GetComponentInParent<TextMeshProUGUI>();
    }

    [PunRPC]
    private void CollectCoin()
    {
        isCollected = true;

        //hnee naamlou l animation w sound effect
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollected && other.CompareTag("Player") && PhotonNetwork.IsConnected)
        {
            photonView.RPC("CollectCoin", RpcTarget.AllBuffered);

            SaveManager.instance.coins += value;

            SaveManager.instance.Save();

            PhotonNetwork.Destroy(gameObject);
        }
    }

}
