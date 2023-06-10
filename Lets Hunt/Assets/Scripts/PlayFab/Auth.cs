using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.ServerModels;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using PlayFab.AdminModels;
using PlayFab.AuthenticationModels;
using ExecuteCloudScriptResult = PlayFab.ClientModels.ExecuteCloudScriptResult;
using SendAccountRecoveryEmailRequest = PlayFab.ClientModels.SendAccountRecoveryEmailRequest;
using SendAccountRecoveryEmailResult = PlayFab.ClientModels.SendAccountRecoveryEmailResult;

public class Auth : MonoBehaviour
{
    public TextMeshProUGUI message;
    public TextMeshProUGUI mailMessage;

    public GameObject loadingPanel;

    public GameObject mailVerifPanel;

    public string authToken;


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
    public string playFabID;
    private PlayFab.ClientModels.EmailVerificationStatus? accountStatus;

    private void Start()
    {
        DontDestroyOnLoad(this);

        rememberMeToggle.onValueChanged.AddListener(OnRememberMeToggleChanged);

        if (RememberMe && !string.IsNullOrEmpty(RememberMeId))
        {
            loadingPanel.SetActive(true);

            var request = new LoginWithCustomIDRequest
            {
                TitleId = PlayFabSettings.TitleId,
                CustomId = RememberMeId,
                CreateAccount = false,

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
            return PlayerPrefs.GetString("PlayFabIdPassGuid", "");
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

        loadingPanel.SetActive(true);

        CheckMailConfirmed(isConfirmed =>
        {
            if (isConfirmed)
            {
                
                playerName = result.InfoResultPayload.PlayerProfile.DisplayName;

                authToken = result.EntityToken.EntityToken;

                PlayerPrefs.SetString("PlayFabId", result.InfoResultPayload.PlayerProfile.PlayerId);

                if (RememberMe)
                {
                    RememberMeId = Guid.NewGuid().ToString();

                    PlayFabClientAPI.LinkCustomID(new LinkCustomIDRequest
                    {
                        CustomId = RememberMeId,

                    }, null, null);
                }


                SceneManager.LoadSceneAsync("MainMenu");
            }
            else
            {
                loadingPanel.SetActive(false);

                mailVerifPanel.SetActive(true);
            }

        });


    }

    public void SaveInitialAppearance()
    {
        var dataDictionary = new Dictionary<string, string>
         {
             {"currentHunter", "1" },
             {"matchPlayed", "0" },
             {"ranked", "0" },
             {"owned", "1" },
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
        mailVerifPanel.SetActive(true);

        playFabID = result.PlayFabId;

        playerName = nameInput.text;



        var emailAddress = emailRegisterInput.text;

        AddOrUpdateContactEmail(emailAddress);
    }

    private void LoadScene()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    void AddOrUpdateContactEmail(string emailAddress)
    {
        var request = new AddOrUpdateContactEmailRequest
        {
            EmailAddress = emailAddress
        };
        PlayFabClientAPI.AddOrUpdateContactEmail(request, result =>
        {
            Debug.Log("The player's account has been updated with a contact email");

        }, FailureCallback);
    }

    void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    // Reset password
    public void ResetPasswordButton()
    {
        var tokenRequest = new GetEntityTokenRequest();
        
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailResetInput.text,
            EmailTemplateId = "BD6EBC6AE010AB5",
            TitleId = "AF633",
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        message.text = "Mail sent! Check your Email!";

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

    // Update display name
    public void UpdatePlayerName(string newDisplayName)
    {
        var request = new PlayFab.ClientModels.UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newDisplayName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateDisplayNameSuccess, OnUpdateDisplayNameFailure);
    }

    private void OnUpdateDisplayNameSuccess(PlayFab.ClientModels.UpdateUserTitleDisplayNameResult result)
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


    private void GetPlayerData(Action<PlayFab.ClientModels.GetPlayerProfileResult> callback)
    {
        var request = new PlayFab.ClientModels.GetPlayerProfileRequest
        {
            PlayFabId = playFabID,
        };

        var constraints = new PlayFab.ClientModels.PlayerProfileViewConstraints
        {
            ShowContactEmailAddresses = true,
        };

        request.ProfileConstraints = constraints;

        PlayFabClientAPI.GetPlayerProfile(request, result => callback?.Invoke(result), OnError);
    }


    void OnPlayerDataResult(PlayFab.ClientModels.GetPlayerProfileResult result)
    {
        var myList = result.PlayerProfile.ContactEmailAddresses;
        accountStatus = myList[0].VerificationStatus;
        Debug.Log("Verification status: " + myList[0].VerificationStatus);
    }

    void CheckMailConfirmed(Action<bool> callback)
    {
        GetPlayerData(result =>
        {
            var myList = result.PlayerProfile.ContactEmailAddresses;
            accountStatus = myList[0].VerificationStatus;
            Debug.Log("Verification status: " + myList[0].VerificationStatus);

            bool isConfirmed = accountStatus == PlayFab.ClientModels.EmailVerificationStatus.Confirmed;
            callback?.Invoke(isConfirmed);
        });
    }



    public void CheckifVerified()
    {

        CheckMailConfirmed(isConfirmed =>
        {
            if (isConfirmed)
            {
                loadingPanel.SetActive(true);

                SaveInitialAppearance();


                Invoke("LoadScene", 3f);
            }
            else
            {
                mailMessage.text = "Email still not verified!";
            }

        });

    }




}
