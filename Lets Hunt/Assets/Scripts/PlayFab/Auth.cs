using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using PlayFab.ServerModels;
using System;

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

        if(RememberMe && !string.IsNullOrEmpty(RememberMeId))
        {
            var request = new LoginWithCustomIDRequest
            {
                TitleId = PlayFabSettings.TitleId,
                CustomId = RememberMeId,
                CreateAccount = true,

                InfoRequestParameters = new PlayFab.ClientModels.GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
            };

            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
        }
        else
        {
            PlayerPrefs.SetString("PlayFabIdPassGuid", "");
        }

    }

    public bool RememberMe
    {
        get
        {
            return PlayerPrefs.GetInt(RememberMeKey, 0) == 0 ? false : true;
        }
        set
        {
            PlayerPrefs.SetInt(RememberMeKey, value ? 1 : 0);
        }
    }

    private string RememberMeId
    {
        get
        {
            return PlayerPrefs.GetString("PlayFabIdPassGuid","");
        }
        set
        {
            var guid = string.IsNullOrEmpty(value) ? Guid.NewGuid().ToString() : value;

            PlayerPrefs.SetString("PlayFabIdPassGuid", guid);
        }
    }


    // Login
    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailLoginInput.text,

            Password = pwdLoginInput.text,

            InfoRequestParameters = new PlayFab.ClientModels.GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        playerName = result.InfoResultPayload.PlayerProfile.DisplayName;

        //PlayerPrefs.SetString("playerName", playerName);

        //PlayerPrefs.SetInt(RememberMeKey, rememberMeToggle.isOn ? 1 : 0);

        PlayerPrefs.SetString("PlayFabId", result.InfoResultPayload.PlayerProfile.PlayerId);

        if (RememberMe)
        {
            RememberMeId = Guid.NewGuid().ToString();

            PlayFabClientAPI.LinkCustomID(new LinkCustomIDRequest
            {
                CustomId = RememberMeId,
              
            },null,null);
        }

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

        var request = new PlayFab.ClientModels.UpdateUserDataRequest
        {
            Data = dataDictionary
        };

        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }
    void OnDataSend(PlayFab.ClientModels.UpdateUserDataResult result)
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


    private void OnRememberMeToggleChanged(bool value)
    {
        RememberMe = value;

        if (!value)
        {
            PlayerPrefs.SetString("PlayFabIdPassGuid", "");
        }
    }

    public void AuthenticateWithSessionTicket(string sessionTicket)
    {
        var request = new AuthenticateSessionTicketRequest
        {
            SessionTicket = sessionTicket,
        };

        PlayFabServerAPI.AuthenticateSessionTicket(request, AuthenticateSessionTicketSuccess, OnError);
    }

    private void AuthenticateSessionTicketSuccess(AuthenticateSessionTicketResult result)
    {

        Debug.Log("Authentication successful!");

        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void CheckSessionTicketValidity(string sessionTicket)
    {
        var request = new PlayFab.ClientModels.GetPlayerCombinedInfoRequest
        {
            InfoRequestParameters = new PlayFab.ClientModels.GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            },
            AuthenticationContext = new PlayFabAuthenticationContext
            {
                PlayFabId = PlayerPrefs.GetString("PlayFabId"),
                ClientSessionTicket = sessionTicket
            }
        };

        PlayFabClientAPI.GetPlayerCombinedInfo(request, OnGetPlayerCombinedInfoSuccess, OnError);
    }

    private void OnGetPlayerCombinedInfoSuccess(PlayFab.ClientModels.GetPlayerCombinedInfoResult result)
    {
        Debug.Log("Session ticket is valid!");
        AuthenticateWithSessionTicket(PlayerPrefs.GetString("SessionTicket"));

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
        Debug.Log("Display name updated successfully: " + result. DisplayName);

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
