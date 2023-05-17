using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{

    public GameObject endGameCanvas,victory,defeat;

    PhotonView view;

    GameObject victoryPanel,defeatPanel;

    Vector3 pos2, pos;

    PlayerHealth playerHealth;

    public int playerRank = 0;

    PlayFabManager playFabManager;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        view = GetComponent<PhotonView>();

        pos = new Vector3(113.5f, 236.45f, -4.5f);
        pos2 = new Vector3(36.5f, 99.95f, -4.5f);

       playFabManager = GameObject.FindGameObjectWithTag("PlayFabManger").GetComponent<PlayFabManager>();

    }

    public void SetScore()
    {

        EndGamePanel endGamePanel = Instantiate(endGameCanvas, pos, Quaternion.identity).GetComponent<EndGamePanel>();

        bool success = view.Owner.CustomProperties.TryGetValue("rank", out object rank);

        if (success) 
        { 
            playerRank = (int) rank; 
        }

        switch (playerRank)
        {
            case 1:
                endGamePanel.rank.text = "Rank : 1";
                endGamePanel.thunders.text = " +10 ";
                SaveManager.instance.thunders += 10;
                SaveManager.instance.ranked += 1;
                break;
            case 2:
                endGamePanel.rank.text = "Rank : 2";
                endGamePanel.thunders.text = " +7 ";
                SaveManager.instance.thunders += 7;
                break;
            case 3:
                endGamePanel.rank.text = "Rank : 3";
                endGamePanel.thunders.text = " +4 ";
                SaveManager.instance.thunders += 4;
                break;
            case 4:
                endGamePanel.rank.text = "Rank : 4";
                endGamePanel.thunders.text = " -5 ";
                SaveManager.instance.thunders -= 5;
                break;
        }

        int playerKills = 0;
        int playerCoinsCollected = 0;

        success = view.Owner.CustomProperties.TryGetValue("kills", out object kills);

        if (success) playerKills = (int)kills;

        success = view.Owner.CustomProperties.TryGetValue("coinsCollected", out object coinsCollected);

        if (success) playerCoinsCollected = (int)coinsCollected;

        PlayerPrefs.SetInt("CollectedCoins", playerCoinsCollected);

        endGamePanel.kills.text = playerKills.ToString();

        endGamePanel.coins.text = "+" + playerCoinsCollected.ToString();

        endGamePanel.adsPanel.gameObject.SetActive(true);

        SaveManager.instance.Save();

        playFabManager.SendLeaderboard(SaveManager.instance.thunders);

        PhotonNetwork.Destroy(gameObject);


    }

    public void SetScoreDeadPlayer()
    {
        Vector3 pos = new Vector3(113.5f, 236.45f, -4.5f);

        EndGamePanel endGamePanel = Instantiate(endGameCanvas, pos, Quaternion.identity).GetComponent<EndGamePanel>();

        endGamePanel.rank.text = "DEFEAT!";

        endGamePanel.thunders.text = " -5 ";

        SaveManager.instance.thunders -= 5;

        SaveManager.instance.Save();

        playFabManager.SendLeaderboard(SaveManager.instance.thunders);


        //---- Kills ----

        int playerKills = 0;

        bool success = view.Owner.CustomProperties.TryGetValue("kills", out object kills);

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

    public void WinLosePanel()
    {
        bool success = view.Owner.CustomProperties.TryGetValue("rank", out object rank);

        if (success)
        {
            playerRank = (int)rank;
        }
        if(playerRank == 1 && !playerHealth.playerDead)
        {
            victoryPanel = Instantiate(victory, pos2, Quaternion.identity);

            StartCoroutine(ClosePanel(victoryPanel));
        }
        else
        {
            defeatPanel = Instantiate(defeat, pos2, Quaternion.identity);

            StartCoroutine(ClosePanel(defeatPanel));
        }

    }

    IEnumerator ClosePanel(GameObject panel)
    {
        yield return new WaitForSeconds(3f);

        Destroy(panel.gameObject);
    }
}
