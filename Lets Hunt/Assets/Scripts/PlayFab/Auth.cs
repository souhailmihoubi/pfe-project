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

    public string playerName;

    private void Start()
    {
        DontDestroyOnLoad(this);

        rememberMeToggle.onValueChanged.AddListener(OnRememberMeToggleChanged);

        if (PlayerPrefs.HasKey(RememberMeKey))
        {
            bool rememberMeValue = PlayerPrefs.GetInt(RememberMeKey) == 1;
            rememberMeToggle.isOn = rememberMeValue;

            if (rememberMeValue)
                ResumeSession();
        }

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

        PlayerPrefs.SetString("playerName", playerName);

        PlayerPrefs.SetInt("RememberMe", rememberMeToggle.isOn ? 1 : 0);

        PlayerPrefs.SetString("SessionTicket", result.SessionTicket);

        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void SaveInitialAppearance()
    {
        var dataDictionary = new Dictionary<string, string>
         {
             {"Coins", "50" },
             {"Gems", "0" },
             {"Thunders", "10" },
             {"currentHunter", "1" },
             {"matchPlayed", "0" },
             {"ranked", "0" },
             {"HunterUnlocked_0","false" },
             {"HunterUnlocked_1","true" },
             {"HunterUnlocked_2","false" }
         };

        var request = new UpdateUserDataRequest
        {
            Data = dataDictionary
        };

        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }
    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Initial data sent successfully!");
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
            RequireBothUsernameAndEmail = false,
            TitleId = "AF633"
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {

        SaveInitialAppearance();

        SceneManager.LoadSceneAsync("MainMenu");
    }

    private void OnVerificationEmailSent(SendAccountRecoveryEmailResult result)
    {
        Debug.Log("Verification email sent successfully.");
    }

    private void OnVerificationEmailFailure(PlayFabError error)
    {
        Debug.Log("Sending verification email failed: " + error.ErrorMessage);
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
        if (PlayerPrefs.HasKey("playerName") && PlayerPrefs.HasKey("SessionTicket"))
        {
            playerName = PlayerPrefs.GetString("playerName");

            string sessionTicket = PlayerPrefs.GetString("SessionTicket");

            //AuthenticateWithSessionTicket(sessionTicket);

        }
    }

    /* public void AuthenticateWithSessionTicket(string sessionTicket)
     {
         var request = new AuthenticateSessionTicketRequest
         {
             SessionTicket = sessionTicket,
         };

         PlayFabClientAPI.AuthenticateSessionTicket(request, AuthenticateSessionTicketSuccess, OnError);
     }

     private void AuthenticateSessionTicketSuccess(AuthenticateSessionTicketResult result)
     {
         Debug.Log("Authentication successful!");
         
         SceneManager.LoadSceneAsync("MainMenu");
     }*/



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


    public bool IsLoggedIn()
    {
        return !string.IsNullOrEmpty(playerName);
    }

}
