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
    [SerializeField] private GameObject crownImage;
    [SerializeField] private GameObject redLine;
    public int playerRank;

    private Player player;
    private int maxKills;

    public void Initialize(Player player)
    {
        this.player = player;
        
        playerNameText.text = player.NickName;

        UpdateStats();
        
        UpdateCrownImage();
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == player && changedProps.ContainsKey("kills"))
        {
            UpdateStats();
            
            UpdateCrownImage();

            int rank = GetRank(PhotonNetwork.PlayerList.ToList());

            playerRank = rank;

            Debug.Log($"{player} : rank : {rank}");
        }
    }

    public int GetRank(List<Player> players)
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

    int rank = GetRank(PhotonNetwork.PlayerList.ToList());

        if (PhotonNetwork.IsConnectedAndReady)
        {
            player.SetCustomProperties(new Hashtable { { "rank", rank } });
        }

        if (rank == 1)
    {
        crownImage.SetActive(true);
    }
    else
    {
        crownImage.SetActive(false);
    }
}


    public void PlayerOut()
    {

        redLine.SetActive(true);

        player.SetCustomProperties(new Hashtable { { "rank", PhotonNetwork.PlayerList.Length } });

        Debug.Log(GetRank(PhotonNetwork.PlayerList.ToList()));
    }

}
