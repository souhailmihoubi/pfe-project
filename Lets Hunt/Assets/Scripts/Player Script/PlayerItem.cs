using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;




public class PlayerItem : MonoBehaviour
{
    PhotonView photonView;

    public int kills;


    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void GetKill()
    {
        photonView.RPC(nameof(RPC_GetKill), photonView.Owner);
    }

    [PunRPC]
    void RPC_GetKill()
    {
        kills++;



        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);

        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public static PlayerItem Find(Player player)
    {
        return FindObjectsOfType<PlayerItem>().SingleOrDefault(x => x.photonView.Owner == player);
    }
}
