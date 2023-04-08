
using Photon.Pun;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public byte x = 4; //maximum number of players in a room

    public List<string> roomIds = new List<string>();   // a list to store generated room IDs

    public void CreateRoom()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string roomId = "";
        bool idExists = true;

        while (idExists)
        {
            roomId = "";
            for (int i = 0; i < 4; i++)
            {
                roomId += chars[Random.Range(0, chars.Length)];
            }
            idExists = roomIds.Contains(roomId);
        }

        roomIds.Add(roomId);

        var roomOptions = new Photon.Realtime.RoomOptions
        {
            MaxPlayers = x,
            IsOpen = false,
           /*CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() 
           {
            { "Map", SaveManager.instance.currentMap }
           },*/

            //CustomRoomPropertiesForLobby = new[] { "Map" }
        };

        PhotonNetwork.CreateRoom(roomId, roomOptions);

        PlayerPrefs.SetString("roomId", roomId);
        PlayerPrefs.SetInt("friends", 1);

        print("Room ID: " + roomId);
    }


    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text.ToUpper());
    }

    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        PhotonNetwork.CreateRoom("random_room", new Photon.Realtime.RoomOptions { MaxPlayers = x, IsOpen = true });

        PlayerPrefs.SetInt("friends", 0);
    }

    public override void OnConnectedToMaster()
    {
      //  PhotonNetwork.AutomaticallySyncScene = true;

    }

    public override void OnJoinedRoom()
    {

            int selectedMap = SaveManager.instance.currentMap;

            if(selectedMap == 0)
            {
                PhotonNetwork.LoadLevel("nature");

            }
            else  

            if (selectedMap == 1)
            {
                PhotonNetwork.LoadLevel("city");
            }
    }
}
