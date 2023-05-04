using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Linq;
using System.ComponentModel;

public class Timer : MonoBehaviourPunCallbacks
{
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;

    public GameObject movement, timer, lvl,score;
    public int duration = 60;
    private float startTime;

    public GameObject timeOver;


    PhotonView photonView;


    private void Start()
    {


        photonView = GetComponent<PhotonView>();
        
        if (PhotonNetwork.IsMasterClient)
        {
            startTime = (float)PhotonNetwork.Time;

            photonView.RPC("SyncStartTime", RpcTarget.OthersBuffered, startTime);

            StartCoroutine(UpdateTimer());

        }
    }

    private IEnumerator UpdateTimer()
    {
        while (true)
        {
            float timePassed = (float)PhotonNetwork.Time - startTime;

            int remainingDuration = Mathf.Max(0, duration - (int)timePassed);

            uiText.text = $"{remainingDuration / 60:00}:{remainingDuration % 60:00}";
            uiFill.fillAmount = Mathf.InverseLerp(0, duration, remainingDuration);

            if (remainingDuration <= 0)
            {
                break;
            }
            
            yield return null;
        }

        OnEnd();
    }

    private void OnEnd()
    {
        movement.gameObject.SetActive(false);
        lvl.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
        score.gameObject.SetActive(false);

        StartCoroutine(ShowResult());   
    }

    IEnumerator ShowResult()
    {
        timeOver.gameObject.SetActive(true);

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerObject = player.TagObject as GameObject;

            if (playerObject != null)
            {
                PlayerScore playerScore = playerObject.GetComponent<PlayerScore>();

                playerScore.WinLosePanel();
            }
        }

        yield return new WaitForSeconds(3f);

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerObject = player.TagObject as GameObject;

            if (playerObject != null)
            {
                PlayerScore playerScore = playerObject.GetComponent<PlayerScore>();

                if (playerScore != null)
                {
                    playerScore.SetScore();

                    SaveManager.instance.matchPlayed += 1;
                    SaveManager.instance.Save();

                }
            }
        }

        Time.timeScale = 0f;

    }

    [PunRPC]
    private void SyncStartTime(float startTime)
    {
        this.startTime = startTime;
        StartCoroutine(UpdateTimer());

    }
}
