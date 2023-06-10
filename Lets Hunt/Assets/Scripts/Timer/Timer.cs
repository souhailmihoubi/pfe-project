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
    private FixedJoystick joyStick;
    public int duration = 60;
    private float startTime;

    public GameObject timeOver;

    [SerializeField] AudioSource endGameAudioSource;

    [Header("Countdown start")]

    private int startTimer = 3;
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] AudioSource startGameAudioSource;
    [SerializeField] AudioSource bgGameAudioSource;



    PhotonView photonView;


    private void Start()
    {
        joyStick = movement.GetComponent<FixedJoystick>();

        joyStick.enabled = false;
        
        Destroy(GameObject.FindWithTag("PlayFabManger"));

        photonView = GetComponent<PhotonView>();

        StartCoroutine(CountdownStart());
        

    }

    private IEnumerator CountdownStart()
    {
        startGameAudioSource.Play();

        while (startTimer>0)
        {
            countdownText.text = startTimer.ToString();

            yield return new WaitForSeconds(1f);

            startTimer--;
        }

        countdownText.text = "Let's Hunt!";

        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);

        bgGameAudioSource.Play();

        joyStick.enabled = true;


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
        bgGameAudioSource.Stop();
        endGameAudioSource.Play();

        joyStick.enabled=false;
        lvl.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
        score.gameObject.SetActive(false);

        SaveManager.instance.matchPlayed++;
        SaveManager.instance.Save();

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

        timeOver.gameObject.SetActive(false);

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerObject = player.TagObject as GameObject;

            if (playerObject != null)
            {
                PlayerScore playerScore = playerObject.GetComponent<PlayerScore>();

                if (playerScore != null)
                {
                    playerScore.SetScore();                  

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
