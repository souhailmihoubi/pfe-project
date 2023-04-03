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

        if (PlayerPrefs.GetInt("friends") == 1)
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
                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Map"))
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        int currentMap = SaveManager.instance.currentMap;
                        int selectedMap = (int)PhotonNetwork.CurrentRoom.CustomProperties["Map"];

                        if (currentMap == selectedMap)
                        {
                            if (currentMap == 0)
                            {
                                PhotonNetwork.LoadLevel("nature");
                            }
                            else if (currentMap == 1)
                            {
                                PhotonNetwork.LoadLevel("city");
                            }
                        }
                    }
                } else
                {
                    if (SaveManager.instance.currentMap == 0)
                    {
                        PhotonNetwork.LoadLevel("nature");
                    }
                    else if (SaveManager.instance.currentMap == 1)
                    {
                        PhotonNetwork.LoadLevel("city");
                    }
                }
            }
        }
    }

}
