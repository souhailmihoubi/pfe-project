using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public byte x = 4; // initialize x to maximum number of players allowed in a room

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        //no room available

        PhotonNetwork.CreateRoom("random room name", new Photon.Realtime.RoomOptions { MaxPlayers = x, IsOpen = true });
    }

    public override void OnJoinedRoom()
    {
        //Debug.Log("Master : " + PhotonNetwork.IsMasterClient + "| Players in room : " + x);

        PhotonNetwork.LoadLevel("map1");
    }
}
