using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Data;
using UnityEngine.UI;


public class ScoreboardItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerKillsText;
    [SerializeField] private GameObject crownImage;
    public int rankedFirst;

    private Player player;
    private int maxKills;

    public void Initialize(Player player)
    {
        this.player = player;
        
        playerNameText.text = player.NickName;

        UpdateStats();
        
        UpdateCrownImage();
    }

    private void Update()
    {
        UpdateCrownImage();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == player && changedProps.ContainsKey("kills"))
        {
            UpdateStats();
            
            UpdateCrownImage();
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

        int maxKills = -1;

        ScoreboardItem maxKillsPlayer = null;

        foreach (ScoreboardItem scoreboardItem in scoreboardItems)
        {
            if (scoreboardItem.player.CustomProperties.TryGetValue("kills", out object kills))
            {
                int playerKills = (int)kills;

                if (playerKills > maxKills)
                {
                    maxKills = playerKills;

                    maxKillsPlayer = scoreboardItem;
                }
            }
        }

        if (maxKillsPlayer != null && maxKillsPlayer.player == player)
        {
            crownImage.SetActive(true);

            foreach (ScoreboardItem scoreboardItem in scoreboardItems)
            {
                if (scoreboardItem != maxKillsPlayer)
                {
                    scoreboardItem.crownImage.SetActive(false);
                }
            }
        }
        else
        {
            crownImage.SetActive(false);
        }
    }
}
