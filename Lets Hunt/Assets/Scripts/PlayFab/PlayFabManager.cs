using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System;
using Photon.Pun;

public class PlayFabManager : MonoBehaviour
{
    public GameObject playerRow;
    public Transform rowsParent;
    public TextMeshProUGUI message;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerNameInput;

    Auth auth;

    public GameObject loadingPanel;

    private void Start()
    {
        DontDestroyOnLoad(this);

        auth = GameObject.FindGameObjectWithTag("Authentification").GetComponent<Auth>();

        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        SaveManager.instance.displayName = PlayerPrefs.GetString("playerName");

        PhotonNetwork.NickName = SaveManager.instance.displayName;

        Debug.Log(PhotonNetwork.NickName);

        playerName.text = PhotonNetwork.NickName;

        SaveManager.instance.Save();

        GetAppearance();

        SaveAppearance();

        SendLeaderboard(SaveManager.instance.thunders);

        yield return new WaitForSeconds(3f);

        loadingPanel.SetActive(false);
    }

    //Player data
    public void GetAppearance()
     {
         PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnError);
     }
     void OnDataRecieved(GetUserDataResult result)
     {

         if (result.Data != null && result.Data.ContainsKey("Coins") && result.Data.ContainsKey("Gems") && result.Data.ContainsKey("Thunders") && result.Data.ContainsKey("currentHunter") && result.Data.ContainsKey("matchPlayed") && result.Data.ContainsKey("ranked"))
         {
             SaveManager.instance.coins = Int32.Parse(result.Data["Coins"].Value);
             SaveManager.instance.gems = Int32.Parse(result.Data["Gems"].Value);
             SaveManager.instance.thunders = Int32.Parse(result.Data["Thunders"].Value);
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
             {"Coins", SaveManager.instance.coins.ToString() },
             {"Gems", SaveManager.instance.gems.ToString() },
             {"Thunders", SaveManager.instance.thunders.ToString() },
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

         var request = new UpdateUserDataRequest
         {
             Data = dataDictionary
         };

         PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
     }
     void OnDataSend(UpdateUserDataResult result)
     {
         Debug.Log("User data sent successfully!");
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
                    Value = thunders
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
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }
    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach(Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);

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

    void OnError(PlayFabError error)
    {
        message.text = error.ErrorMessage;
        Debug.Log("Error while logging in !");
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

}
