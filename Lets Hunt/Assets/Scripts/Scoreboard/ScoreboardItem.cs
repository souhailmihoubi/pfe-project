using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Data;

public class ScoreboardItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI killsText;

    [SerializeField] TextMeshProUGUI playerKills;

    Player player;

    private void Start()
    {
        playerKills = GameObject.FindGameObjectWithTag("playerKills").GetComponentInParent<TextMeshProUGUI>();
    }
    public void Initialize(Player player)
    {
        playerName.text = player.NickName;

        this.player = player;

        UpdateStats();
    }

    void UpdateStats()
    {
        if(player.CustomProperties.TryGetValue("kills",out object kills))
        {
            killsText.text = kills.ToString();

            playerKills.text = kills.ToString();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer,Hashtable changedProps)
    {
        if(targetPlayer == player)
        {
            if (changedProps.ContainsKey("kills"))
            {
                UpdateStats();
            }
        }
    }
}
