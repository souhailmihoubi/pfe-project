using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System;
using Photon.Pun;
using PlayFab.AdminModels;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance { get; private set; }

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerNameInput;
    public TextMeshProUGUI newPwdInput;

    Auth auth;

    [Header("Leaderboard")]

    public GameObject playerRow;
    public Transform rowsParent;
    public TMP_InputField searchInputField;
    public Button searchButton;

    [Header("News")]
    public TextMeshProUGUI newsTitle;
    public TextMeshProUGUI newsBody;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {

        auth = GameObject.FindGameObjectWithTag("Authentification").GetComponent<Auth>();

        GetAppearance();

        Debug.Log(auth.playerName);

        SaveManager.instance.displayName = auth.playerName;

        PhotonNetwork.NickName = SaveManager.instance.displayName;

        SaveManager.instance.Save();

        playerName.text = PhotonNetwork.NickName;

        searchInputField.onValueChanged.AddListener(OnSearchInputValueChanged);

    }

    private void Update()
    {


        if (SaveManager.instance.changed)
        {
            SaveAppearance();
            SaveManager.instance.changed = false;
        }
    }


    //Player data
    public void GetAppearance()
     {
        PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest(), OnDataRecieved, OnError);
     }
     void OnDataRecieved(PlayFab.ClientModels.GetUserDataResult result)
     {
         if (result.Data != null & result.Data.ContainsKey("currentHunter") && result.Data.ContainsKey("matchPlayed") && result.Data.ContainsKey("ranked"))
         {
             SaveManager.instance.currentHunter = Int32.Parse(result.Data["currentHunter"].Value);
             SaveManager.instance.matchPlayed = Int32.Parse(result.Data["matchPlayed"].Value);
             SaveManager.instance.ranked = Int32.Parse(result.Data["ranked"].Value);

             bool[] huntersUnlocked = new bool[3];

             int index = 0;
             foreach (var entry in result.Data)
             {
                 if (entry.Key.StartsWith("HunterUnlocked_"))
                 {
                     bool value = bool.Parse(entry.Value.Value);
                     huntersUnlocked[index] = value;
                     index++;
                 }
             }

             SaveManager.instance.huntersUnlocked = huntersUnlocked;

             SaveManager.instance.Save();

             Debug.Log("Data Received!");
        }
         else
         {
             Debug.Log("Player data incomplete!");
         }
     }
     public void SaveAppearance()
     {
        bool[] huntersUnlocked = SaveManager.instance.huntersUnlocked;

         var dataDictionary = new Dictionary<string, string>
         {
             {"currentHunter", SaveManager.instance.currentHunter.ToString() },
             {"matchPlayed", SaveManager.instance.matchPlayed.ToString() },
             {"ranked", SaveManager.instance.ranked.ToString() },
         };

         for (int i = 0; i < huntersUnlocked.Length; i++)
         {
             string key = "HunterUnlocked_" + i.ToString();
             string value = huntersUnlocked[i].ToString();
             dataDictionary[key] = value;
         }

         var request = new PlayFab.ClientModels.UpdateUserDataRequest
         {
             Data = dataDictionary
         };

         PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
     }
    void OnDataSend(PlayFab.ClientModels.UpdateUserDataResult result)
    {
        Debug.Log("User data sent successfully!");

        SendLeaderboard(BalenceManager.instance.thundersBalance);

    }

    //Leaderboard
    public void SendLeaderboard(int thunders)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate()
                {
                    StatisticName = "Thunders Score",
                    Value = thunders,
                    Version = 1

                    
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Leaderboard sent successfully!");
    }
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Thunders Score",
            StartPosition = 0,
            MaxResultsCount = 100,
        };

        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    private List<PlayerLeaderboardEntry> leaderboardEntries = new List<PlayerLeaderboardEntry>();

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        leaderboardEntries.Clear();
       
        leaderboardEntries.AddRange(result.Leaderboard);

        FilterLeaderboard(searchInputField.text);
    }
    public void SearchLeaderboard()
    {
        string searchQuery = searchInputField.text;

        FilterLeaderboard(searchQuery);
    }

    private void FilterLeaderboard(string searchQuery)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        bool isSearchEmpty = string.IsNullOrEmpty(searchQuery);

        // Store the filtered leaderboard entries in a new list
        List<PlayerLeaderboardEntry> filteredEntries = new List<PlayerLeaderboardEntry>();

        foreach (var item in leaderboardEntries)
        {
            if (!isSearchEmpty && !item.DisplayName.ToLower().StartsWith(searchQuery.ToLower()))
                continue;

            filteredEntries.Add(item);
        }

        DisplayLeaderboard(filteredEntries);
    }

    private void OnSearchInputValueChanged(string searchQuery)
    {
        FilterLeaderboard(searchQuery);
    }

    private void DisplayLeaderboard(List<PlayerLeaderboardEntry> entries)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < entries.Count; i++)
        {
            var item = entries[i];
            
            //Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);

            GameObject newGo = Instantiate(playerRow, rowsParent);

            TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();
        }
    }

    public void SetPlayerName()
    {
        PhotonNetwork.NickName = playerNameInput.text;

        auth.UpdatePlayerName(playerNameInput.text);

        playerName.text = PhotonNetwork.NickName;

        SaveManager.instance.displayName = PhotonNetwork.NickName;

        SaveManager.instance.Save();

        playerNameInput.text = "";

    }

    public void OnClickChangePassword()
    {
        ChangePassword(newPwdInput.text, auth.authToken);
    }

    void ChangePassword(string newPassword, string token)
    {
        var request = new ResetPasswordRequest
        {
            Password = newPassword,
            Token = token
        };

        PlayFabAdminAPI.ResetPassword(request, result =>
        {
            Debug.Log("The player's password has been resetl");
        }, ResetFailureCallback);

    }

    void ResetFailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error : " + error.ErrorMessage);
        Debug.Log(error.GenerateErrorReport());
    }



    //Title Data
    /*   void GetTitleData()
       {
           PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnTitleDataRecieved, OnError);
       }

       void OnTitleDataRecieved(GetTitleDataResult result)
       {
           if(result.Data == null || result.Data.ContainsKey("Message") == false)
           {
               Debug.Log("No messages!");
               return;
           }     

           message.text = result.Data["Message"].ToString();
       }
       */


    //Title News
    public void GetTitleNew()
    {
        var request = new PlayFab.ClientModels.GetTitleNewsRequest();

        //request.Count = 1;

        PlayFabClientAPI.GetTitleNews(request, OnTitleNewsRecieved, OnError);
    }

    void OnTitleNewsRecieved(GetTitleNewsResult result)
    {
        newsTitle.text = result.News[result.News.Count - 1].Title;

        newsBody.text = result.News[result.News.Count - 1].Body;
    }
       


    //Currecry


}
