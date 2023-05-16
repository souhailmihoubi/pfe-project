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
    public Toggle rememberMeToggle;

    [Header("Register")]
    public TextMeshProUGUI nameInput;
    public TextMeshProUGUI emailRegisterInput;
    public TextMeshProUGUI pwdRegisterInput;

    [Header("Reset Password")]
    public TextMeshProUGUI emailResetInput;

    private const string RememberMeKey = "RememberMe";

    public string playerName = null;

    private void Start()
    {
        DontDestroyOnLoad(this);

        // Check if remember me is enabled
        if (PlayerPrefs.HasKey(RememberMeKey))
        {
            bool rememberMeValue = PlayerPrefs.GetInt(RememberMeKey) == 1;
            rememberMeToggle.isOn = rememberMeValue;

            if (rememberMeValue)
                ResumeSession();
        }

        // Add event listener to Remember Me toggle
        rememberMeToggle.onValueChanged.AddListener(OnRememberMeToggleChanged);
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

        PlayerPrefs.SetInt("RememberMe", rememberMeToggle.isOn ? 1 : 0);

        SceneManager.LoadSceneAsync("MainMenu");
    }

    // Register
    public void RegisterButton()
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
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        message.text = "Registered and logged in!";
        SceneManager.LoadSceneAsync("MainMenu");
    }

    // Reset password
    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailResetInput.text,
            TitleId = "AF633"
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        message.text = "Password reset! Mail sent";
    }

    void OnError(PlayFabError error)
    {
        message.text = error.ErrorMessage;
        Debug.Log("Error while logging in!");
        Debug.Log(error.GenerateErrorReport());
    }

    // Resume session
    public void ResumeSession()
    {
        if (PlayerPrefs.HasKey("playerName"))
        {
            playerName = PlayerPrefs.GetString("playerName");

            Debug.Log("Resuming session for player: " + playerName);

            // Continue with the logged-in flow
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }

    private void OnRememberMeToggleChanged(bool value)
    {
        int rememberMeValue = value ? 1 : 0;

        PlayerPrefs.SetInt(RememberMeKey, rememberMeValue);
    }

    // Update display name

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
        Debug.Log("Display name updated successfully: " + result.DisplayName);

        this.playerName = result.DisplayName;
    }

    private void OnUpdateDisplayNameFailure(PlayFabError error)
    {
        Debug.LogError("Update display name failed: " + error.ErrorMessage);
    }


}
