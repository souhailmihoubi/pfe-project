using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using TMPro;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private Image barFill;
    [SerializeField] private TextMeshProUGUI loadingText;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        OnClickConnect();

        StartCoroutine(LoadSceneAsync());
    }

    public void OnClickConnect()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            // There is internet

            PhotonNetwork.OfflineMode = false;
            PhotonNetwork.ConnectUsingSettings();

        }
        else
        {
            Debug.Log("Network Error");
        }
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Authentification");

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress);

            barFill.fillAmount = progressValue;

            loadingText.text = "Loading" + Mathf.RoundToInt(progressValue * 100) + "%";

            yield return new WaitForSeconds(1f);
        }
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(cause.ToString());
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to The master!");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("player left the room");
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            OnClickConnect();
        }
    }
}
