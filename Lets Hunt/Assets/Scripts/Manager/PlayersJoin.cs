using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;

public class PlayersJoin : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI playerCountText;

    public TextMeshProUGUI roomId;

    public int maxPlayers;

    private bool sceneLoaded = false;

    private void Start()
    {
        if (PlayerPrefs.GetInt("friends") == 1)
        {
            roomId.text = "ROOM CODE :  " + PlayerPrefs.GetString("roomId");

            if (PhotonNetwork.IsMasterClient)
            {
                int currentMap = SaveManager.instance.currentMap;

                PlayerPrefs.SetInt("masterMap", currentMap);

                Hashtable customProps = new Hashtable() { { "currentMap", currentMap } };

                PhotonNetwork.CurrentRoom.SetCustomProperties(customProps);

            }
        }
    }

    private void Update()
    {

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        playerCountText.text = playerCount + " / " + maxPlayers;

        if (playerCount == maxPlayers && !sceneLoaded)
        {
            sceneLoaded = true;

            LoadLevel();
        }
    }

    private void LoadLevel()
    {

        int currentMap = (int)PhotonNetwork.CurrentRoom.CustomProperties["currentMap"];

        if (currentMap == 0)
        {
            PhotonNetwork.LoadLevel("nature");
        }
        else if (currentMap == 1)
        {
            PhotonNetwork.LoadLevel("city");
        }

        PlayerPrefs.DeleteKey("friends");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if ( PlayerPrefs.GetInt("friends") == 1 )
        {
            int currentMap = PlayerPrefs.GetInt("masterMap");

            SaveManager.instance.currentMap = currentMap;

            SaveManager.instance.Save();

            Hashtable customProps = new Hashtable() { { "currentMap", currentMap } };

            PhotonNetwork.CurrentRoom.SetCustomProperties(customProps);
        }
    }


}
