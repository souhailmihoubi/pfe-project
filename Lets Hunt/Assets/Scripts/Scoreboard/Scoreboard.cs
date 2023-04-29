using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject playerScoreboard;
    [SerializeField] TextMeshProUGUI deadPlayerName;
    GameObject playerDies;

    public int playersInRoom = 4;

    public bool dead = false;


    Dictionary<Player, ScoreboardItem> scoreboardItems = new Dictionary<Player, ScoreboardItem>();


    private void Start()
    {
        playerDies = GameObject.FindGameObjectWithTag("whenPlayerDies");
        playerDies.SetActive(false);

        foreach(Player player in PhotonNetwork.PlayerList)
        {
            AddScoreoardItem(player);
        }

    }

    void AddScoreoardItem(Player player)
    {
        ScoreboardItem item = Instantiate(playerScoreboard,container).GetComponent<ScoreboardItem>();
        item.Initialize(player);
        scoreboardItems[player] = item;
    }



    void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreoardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //RemoveScoreboardItem(otherPlayer);      
    }
    
    public void PlayerDies(Player player)
    {
        dead = true;

        scoreboardItems[player].redLine.SetActive(true);

        scoreboardItems[player].crownImage.SetActive(false);

        playersInRoom = PhotonNetwork.CurrentRoom.PlayerCount;

        playersInRoom--;

        player.SetCustomProperties(new Hashtable { { "rank", playersInRoom } });

        PlayerPrefs.SetInt("playersInRoom", playersInRoom);

        playerDies.SetActive(true);

        deadPlayerName.text = player.NickName;

        StartCoroutine(ClosePanel());

    }

    IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(3f);
        playerDies.SetActive(false);
    }
    
    public ScoreboardItem GetScoreboardItem(Player player)
    {
        if (scoreboardItems.ContainsKey(player))
        {
            return scoreboardItems[player];
        }
        else
        {
            Debug.LogWarningFormat("Player {0} not found in scoreboardItems dictionary", player.NickName);
            return null;
        }
    }
}


