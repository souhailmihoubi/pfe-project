using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class leaveRoom : MonoBehaviourPunCallbacks
{

    public void OnClickLeaveRoom()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickLeaveRoom0()
    {

        FindObjectOfType<ConnectToServer>().LeaveRoom();

    }





}
