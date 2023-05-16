using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Auth : MonoBehaviour
{
    public TextMeshProUGUI message;

    [Header("Login")]
    public TextMeshProUGUI emailLoginInput;
    public TextMeshProUGUI pwdLoginInput;

    [Header("Register")]
    public TextMeshProUGUI nameInput;
    public TextMeshProUGUI emailRegisterInput;
    public TextMeshProUGUI pwdRegisterInput;

    [Header("Reset Password")]
    public TextMeshProUGUI emailResetInput;

    public string playerName = null;


    private void Start()
    {
        DontDestroyOnLoad(this);
    }


    // Login

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailLoginInput.text,
            Password = pwdLoginInput.text,

            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {

        playerName = result.InfoResultPayload.PlayerProfile.DisplayName;

        Debug.Log(playerName + " logged in!");

        PlayerPrefs.SetString("playerName", playerName);

        SceneManager.LoadSceneAsync("MainMenu");


    }


    //Register

    public void RgisterButton()
    {
        if (pwdRegisterInput.text.Length < 6)
        {
            message.text = "Password too short!";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = nameInput.text,
            Email = emailRegisterInput.text,
            Password = pwdRegisterInput.text,
            RequireBothUsernameAndEmail = false,

        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        message.text = "Registred and logged in!";
    }

    // Reset password

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailResetInput.text,
            TitleId = "AF633",
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        message.text = "Password reset! mail sent";
    }

    void OnError(PlayFabError error)
    {
        message.text = error.ErrorMessage;
        Debug.Log("Error while logging in !");
        Debug.Log(error.GenerateErrorReport());
    }

    public void UpdatePlayerName(string newDisplayName)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newDisplayName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateDisplayNameSuccess, OnUpdateDisplayNameFailure);
    }

    private void OnUpdateDisplayNameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        // Display name updated successfully
        Debug.Log("Display name updated successfully: " + result.DisplayName);

        this.playerName = result.DisplayName;
    }

    private void OnUpdateDisplayNameFailure(PlayFabError error)
    {
        // Display name update failed
        Debug.LogError("Update display name failed: " + error.ErrorMessage);
    }


}
