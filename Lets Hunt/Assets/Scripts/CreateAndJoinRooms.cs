using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;

    /*public GameObject lobbyPanel;
    public GameObject roomPanel, envPanel,spawn;
    public Text roomName;*/


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
        PhotonNetwork.CreateRoom("random room name",new Photon.Realtime.RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Master : " + PhotonNetwork.IsMasterClient + "| Players in room : " + PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("waiting");

       /* lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        envPanel.SetActive(true);
        spawn.SetActive(true);
        roomName.text = PhotonNetwork.CurrentRoom.Name + " IS YOUR ROOM";*/
    }

    

}
