using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PlayersJoin : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI playerCountText;
    public int x = 4; // update x to maximum number of players allowed in a room

    private void Update()
    {
        // Check if we are connected to a room
        if (PhotonNetwork.InRoom)
        {
            // Get the current room
            Room currentRoom = PhotonNetwork.CurrentRoom;

            // Get the number of players in the current room
            int playerCount = currentRoom.PlayerCount;

            // Display the player count in the text object
            playerCountText.text = playerCount + " / " + x;

            if (playerCount == x)
            {
                PhotonNetwork.LoadLevel("map1");
            }
        }
    }
}