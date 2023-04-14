using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    PhotonView photonView;

    public int kills;

    Hashtable hash;


    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        hash = new Hashtable();

    }

    public void GetKill()
    {
        photonView.RPC("RPC_GetKill", photonView.Owner);
    }

    [PunRPC]
    void RPC_GetKill()
    {
        kills++;

        hash["kills"] = kills;

        print(kills);

        photonView.Owner.SetCustomProperties(hash);
    }


    public static PlayerItem Find(Player player)
    {
        return FindObjectsOfType<PlayerItem>().SingleOrDefault(x => x.photonView.Owner == player);
    }


}
