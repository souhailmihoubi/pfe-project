using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PlayersJoin : MonoBehaviour
{
    public TextMeshProUGUI playerCountText;

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
            playerCountText.text = playerCount + " / 4";

            if(playerCount == 2)
            {
                PhotonNetwork.LoadLevel("GamePlay");
            }
        }
    }
}
