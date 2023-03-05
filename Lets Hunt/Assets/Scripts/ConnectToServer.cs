using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
  //  public Text buttonText;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnClickConnect()
    {
      //  buttonText.text = "Starting...";
        PhotonNetwork.OfflineMode = false; //true would fake an online connection
        PhotonNetwork.NickName = "Player Name";  //after login zidou
        //PhotonNetwork.AutomaticallySyncScene = true; //to call PhotonNetwork.loadLevel()

        PhotonNetwork.ConnectUsingSettings(); //automatic connection based on the config file PhotonServerSettings
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(cause.ToString());
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to The master!");
       // SceneManager.LoadScene("Lobby");
    }
}
