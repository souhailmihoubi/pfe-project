using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Timer : MonoBehaviourPunCallbacks
{
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;

    public int duration = 60;
    private float startTime;

    private void Start()
    {
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
        // End Time , if want Do something
        Debug.Log("End");
    }

    [PunRPC]
    private void SyncStartTime(float startTime)
    {
        this.startTime = startTime;
        StartCoroutine(UpdateTimer());
    }
}
