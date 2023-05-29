using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;
using TMPro;

public class PlayerItem : MonoBehaviour
{

    PhotonView photonView;

    public int kills;

    private int coinsCollected;

    Hashtable hash;

    private TextMeshProUGUI killsCount;



    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        hash = new Hashtable();

        killsCount = GameObject.FindGameObjectWithTag("playerKills").GetComponent<TextMeshProUGUI>();


    }

    public void GetKill()
    {
        kills++;

        if (photonView.IsMine)
        {
            killsCount.text = kills.ToString();
        }

        photonView.RPC("RPC_GetKill", photonView.Owner);
    }

    [PunRPC]
    void RPC_GetKill()
    {
       
        hash["kills"] = kills;

        photonView.Owner.SetCustomProperties(hash);
    }


    public static PlayerItem Find(Player player)
    {

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObject in playerObjects)
        {
            PhotonView photonView = playerObject.GetComponent<PhotonView>();

            if (photonView.Owner.ActorNumber == player.ActorNumber)
            {
                return playerObject.GetComponent<PlayerItem>();
            }
        }

        return null;
    }

    


}
