using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Data;
using UnityEngine.UI;
using System.Linq;

public class ScoreboardItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerKillsText;
    public GameObject crownImage;


    public GameObject redLine;
    public int playerRank;

    private Player player;

    public bool playerDied = false;

    public void Initialize(Player player)
    {
        this.player = player;
        
        playerNameText.text = PhotonNetwork.NickName;

        UpdateStats();
        
        UpdateCrownImage();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("kills"))
        {
            UpdateRanks();
            UpdateStats();
            UpdateCrownImage();
        }
    }

    private void UpdateRanks()
    {
        List<Player> players = PhotonNetwork.PlayerList.ToList();

        players.Sort((p1, p2) => {
            int p1Kills = p1.CustomProperties.ContainsKey("kills") ? (int)p1.CustomProperties["kills"] : 0;
            int p2Kills = p2.CustomProperties.ContainsKey("kills") ? (int)p2.CustomProperties["kills"] : 0;
            return p2Kills.CompareTo(p1Kills);
        });

        for (int i = 0; i < players.Count; i++)
        {
            Hashtable customProps = new Hashtable { { "rank", i + 1 } };

            players[i].SetCustomProperties(customProps);
        }
    }


    private void UpdateStats()
    {
        if (player.CustomProperties.TryGetValue("kills", out object kills))
        {
            playerKillsText.text = kills.ToString();

        }
    }

    private void UpdateCrownImage()
    {
        ScoreboardItem[] scoreboardItems = FindObjectsOfType<ScoreboardItem>();

        int playerRank = 0;

        bool success = player.CustomProperties.TryGetValue("rank", out object rank);

        if (success) { playerRank = (int)rank; }

        if (playerRank == 1)
        {
            crownImage.SetActive(true);
        }
        else
        {
            crownImage.SetActive(false);
        }
    }


    public int GetPlayerRank(Player player, List<Player> players)
    {
        players.Sort((p1, p2) => {
            int p1Kills = p1.CustomProperties.ContainsKey("kills") ? (int)p1.CustomProperties["kills"] : 0;
            int p2Kills = p2.CustomProperties.ContainsKey("kills") ? (int)p2.CustomProperties["kills"] : 0;

            return p2Kills.CompareTo(p1Kills);
        });

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == player)
            {
                return i + 1;
            }
        }

        return -1;
    }

}
