using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    public void LogoutButton()
    {
        PlayFabClientAPI.ForgetAllCredentials();

        Destroy(GameObject.FindWithTag("PlayFabManger"));
        Destroy(GameObject.FindWithTag("Authentification"));

        PlayerPrefs.DeleteKey("playerName");
        PlayerPrefs.DeleteKey("RememberMe");
        PlayerPrefs.DeleteKey("SessionTicket");

        SceneManager.LoadScene("Authentification");
    }
}
