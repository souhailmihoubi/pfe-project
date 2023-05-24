using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class leaveRoom : MonoBehaviourPunCallbacks
{

    public void OnClickLeaveRoom()
    {
        SceneManager.LoadScene("MainMenu");

        PlayFabManager.instance.GetAppearance();
        PlayFabManager.instance.SaveAppearance();

        PhotonNetwork.LeaveRoom();

    }
}
