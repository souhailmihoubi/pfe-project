using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PlayersJoin : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI playerCountText;
    public TextMeshProUGUI roomId;
    public int x = 4;


    private void Start()
    { 

        if(PlayerPrefs.GetInt("friends") == 1)
        {
            roomId.text = "ROOM CODE :  " + PlayerPrefs.GetString("roomId");
        }
    }

     private void Update()
    {
        if (PhotonNetwork.InRoom)
        {

            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            playerCountText.text = playerCount + " / " + x;

            if (playerCount == 2)
            {
                PhotonNetwork.LoadLevel("nature");
            }
        }
    }
}