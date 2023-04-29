using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{

    public GameObject endGameCanvas;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    public void SetScore()
    {
        Vector3 pos = new Vector3(113.5f, 236.45f, -4.5f);

        EndGamePanel endGamePanel = Instantiate(endGameCanvas, pos, Quaternion.identity).GetComponent<EndGamePanel>();

        int playerRank = 0;

        bool success = view.Owner.CustomProperties.TryGetValue("rank", out object rank);

        if (success) { playerRank = (int)rank; }

        switch (playerRank)
        {
            case 1:
                endGamePanel.rank.text = "Rank : 1";
                endGamePanel.thunders.text = " +10 ";
                SaveManager.instance.thunders += 10;
                SaveManager.instance.Save();
                break;
            case 2:
                endGamePanel.rank.text = "Rank : 2";
                endGamePanel.thunders.text = " +7 ";
                SaveManager.instance.thunders += 7;
                SaveManager.instance.Save();
                break;
            case 3:
                endGamePanel.rank.text = "Rank : 3";
                endGamePanel.thunders.text = " +4 ";
                SaveManager.instance.thunders += 4;
                SaveManager.instance.Save();
                break;
            case 4:
                endGamePanel.rank.text = "Rank : 4";
                endGamePanel.thunders.text = " -5 ";
                SaveManager.instance.thunders -= 5;
                SaveManager.instance.Save();
                break;
        }

        int playerKills = 0;
        int playerCoinsCollected = 0;

        success = view.Owner.CustomProperties.TryGetValue("kills", out object kills);

        if (success) playerKills = (int)kills;

        success = view.Owner.CustomProperties.TryGetValue("coinsCollected", out object coinsCollected);

        if (success) playerCoinsCollected = (int)coinsCollected;

        endGamePanel.kills.text = playerKills.ToString();

        endGamePanel.coins.text = "+" + playerCoinsCollected.ToString();

    }

    public void SetScoreDeadPlayer()
    {
        Vector3 pos = new Vector3(113.5f, 236.45f, -4.5f);

        EndGamePanel endGamePanel = Instantiate(endGameCanvas, pos, Quaternion.identity).GetComponent<EndGamePanel>();

        //---- Rank ----

        int playerRank = 0;

        bool success = view.Owner.CustomProperties.TryGetValue("rank", out object rank);

        if (success) 
        { 
            playerRank = (int)rank; 
        }

        endGamePanel.rank.text = playerRank.ToString();

        endGamePanel.thunders.text = " -5 ";

        SaveManager.instance.thunders -= 5;

        SaveManager.instance.Save();

        //---- Kills ----

        int playerKills = 0;

        success = view.Owner.CustomProperties.TryGetValue("kills", out object kills);

        if (success) 
        {
            playerKills = (int)kills;
        }

        endGamePanel.kills.text = playerKills.ToString();

        //---- Coins Collected ----

        int playerCoinsCollected = 0;

        success = view.Owner.CustomProperties.TryGetValue("coinsCollected", out object coinsCollected);

        if (success) 
        {
            playerCoinsCollected = (int)coinsCollected;
        }

        endGamePanel.coins.text = "+" + playerCoinsCollected.ToString();

    }
}
