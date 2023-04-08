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
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI killsText;
    [SerializeField] TextMeshProUGUI playerKills;
    [SerializeField] Image crownImage;

    Player player;
    int maxKills;

    private void Start()
    {
        playerKills = GameObject.FindGameObjectWithTag("playerKills").GetComponentInParent<TextMeshProUGUI>();
        crownImage.gameObject.SetActive(false);
    }

    public void Initialize(Player player)
    {
        playerName.text = player.NickName;
        this.player = player;
        UpdateStats();
    }

    private void Update()
    {
        if (PhotonNetwork.LocalPlayer == null) return;

        object killsObj;
        if (player.CustomProperties.TryGetValue("kills", out killsObj))
        {
            int playerKills = (int)killsObj;

            if (playerKills > maxKills)
            {
                maxKills = playerKills;
                UpdateCrownImage();
            }
            else if (playerKills < maxKills && player.Equals(PhotonNetwork.LocalPlayer))
            {
                crownImage.gameObject.SetActive(false);
            }
        }
    }

    void UpdateStats()
    {
        if (player.CustomProperties.TryGetValue("kills", out object kills))
        {
            killsText.text = kills.ToString();
            playerKills.text = kills.ToString();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == player && changedProps.ContainsKey("kills"))
        {
            UpdateStats();
            UpdateCrownImage();
        }
    }

    void UpdateCrownImage()
    {
        if (PhotonNetwork.LocalPlayer == null) return;

        object killsObj;
        if (player.CustomProperties.TryGetValue("kills", out killsObj))
        {
            int playerKills = (int)killsObj;

            if (player.Equals(PhotonNetwork.LocalPlayer) && playerKills == maxKills)
            {
                crownImage.gameObject.SetActive(true);
            }
            else
            {
                crownImage.gameObject.SetActive(false);
            }
        }
    }
}
