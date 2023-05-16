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

        PlayerPrefs.DeleteKey("playerName");
        PlayerPrefs.DeleteKey("RememberMe");

        SceneManager.LoadScene("Authentification");
    }
}
