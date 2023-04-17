using Photon.Pun;
using UnityEngine.SceneManagement;

public class leaveRoom : MonoBehaviourPunCallbacks
{

    public void OnClickLeaveRoom()
    {
        SceneManager.LoadScene("MainMenu");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        //
    }
}
