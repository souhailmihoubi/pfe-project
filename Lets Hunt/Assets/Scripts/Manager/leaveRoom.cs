using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class leaveRoom : MonoBehaviourPunCallbacks
{

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);

        SceneManager.LoadScene("MainMenu");

        PlayFabManager.instance.GetAppearance();
        PlayFabManager.instance.SaveAppearance();
    }

    private IEnumerator LeaveRoomCoroutine()
    {
        PhotonNetwork.Disconnect();

        bool disconnected = false;
        while (!disconnected)
        {
            yield return null;

            if (!PhotonNetwork.IsConnected)
            {
                disconnected = true;
            }
        }

        
    }


}
