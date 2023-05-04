
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField joinInput;

    public byte x = 4;

    public List<string> roomIds = new List<string>();

    private int playersInRoom = 0;

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

        Hashtable customProps = new Hashtable() { { "PlayersInRoom", playersInRoom } };

        var roomOptions = new RoomOptions
        {
            MaxPlayers = x,
            IsVisible = false,
            CustomRoomProperties = customProps,
            CustomRoomPropertiesForLobby = new[] { "currentMap" }
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

        PlayerPrefs.SetInt("friends", 0);

        if (PhotonNetwork.IsConnectedAndReady)
        {
            int currentMap = SaveManager.instance.currentMap;
            Hashtable customProps = new Hashtable() { { "currentMap", currentMap } };
            PhotonNetwork.JoinRandomRoom(customProps, 0, MatchmakingMode.FillRoom, null, "", new string[0]);
        }
        else
        {
            Debug.LogWarning("Client is not connected to the master server yet. Wait for OnConnectedToMaster callback before joining a room.");
        }


    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PlayerPrefs.SetInt("friends", 0);

        int currentMap = SaveManager.instance.currentMap;

        Hashtable customProps = new Hashtable() { { "currentMap", currentMap } };

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = x, CustomRoomProperties = customProps, CustomRoomPropertiesForLobby = new[] { "currentMap" } });
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playersInRoom++;
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        playersInRoom--;
    }
    public override void OnJoinedRoom()
    {
         playersInRoom++;

         Hashtable customRoomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

         if (customRoomProperties.ContainsKey("PlayersInRoom"))
         {
            playersInRoom = (int)customRoomProperties["PlayersInRoom"];
         }

         if (playersInRoom == PhotonNetwork.CurrentRoom.MaxPlayers)
         {
             roomIds.Remove(PhotonNetwork.CurrentRoom.Name);
         }

         

        PhotonNetwork.LoadLevel("waiting");
    }

}
