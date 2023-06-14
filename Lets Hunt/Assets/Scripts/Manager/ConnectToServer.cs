using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using TMPro;
using UnityEngine.Networking;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private Image barFill;
    [SerializeField] private TextMeshProUGUI loadingText;
    public GameObject panel;

    private const string PlayFabUrl = "https://AF633.playfabapi.com";

    private bool canConnect = false;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        StartCoroutine(CheckInternetConnection());
    }

    private IEnumerator CheckInternetConnection()
    {
        UnityWebRequest request = new UnityWebRequest(PlayFabUrl);

        request.timeout = 5;

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Device is not connected to the internet.");
            
            if(panel != null)
            {
                panel.SetActive(true);
            }
            canConnect = false;
        }
        else
        {
            Debug.Log("Device is connected to the internet.");

            if(panel != null)
            {
                panel.SetActive(false);
            }

            canConnect = true;

            StartConnection();
        }
    }

    public void RetryConnection()
    {
        StartCoroutine(CheckInternetConnection());
    }

    private void StartConnection()
    {
        if (canConnect)
        {
            PhotonNetwork.ConnectUsingSettings();

            StartCoroutine(LoadSceneAsync());
        }
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Authentification");

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress);

            barFill.fillAmount = progressValue;

            loadingText.text = "Loading " + Mathf.RoundToInt(progressValue * 100) + "%";

            yield return new WaitForSeconds(1f);
        }
    }
    public void ExitApplication()
    {
        Application.Quit();
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
            StartCoroutine(CheckInternetConnection());
        }
    }
}
