using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class SetupXPBar : MonoBehaviourPunCallbacks
{
    public int maxXP = 100;
    public int XPvalue = 30;

    [SerializeField] private int currentXP = 0;
    [SerializeField] private int currentLevel = 1;

    private TextMeshProUGUI levelText;
    private Image xpFill;

    public LevelUpScript levelUpScript;

    [SerializeField]
    private float updateSpeedSeconds = 0.5f;


    private void Start()
    {
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("XP"))
        {
            Destroy(other.gameObject);
        }
        switch (currentLevel)
        {
            case 1:
                maxXP = 100; 
                break;
                
            case 2:
                maxXP = 150;
                break;
                
            case 3:
                maxXP = 250;
                break;
            case 4:
                maxXP = 350;
                break;

            default:
                maxXP = 450;
                break;
        }

        if (other.gameObject.CompareTag("XP") && photonView.IsMine)
        {
            currentXP += XPvalue;
            
            if (currentXP >= maxXP)
            {
                currentXP = currentXP - maxXP;
                currentLevel++;

                levelUpScript.LevelUp();
            }

            UpdateUI();
            
        }
    }

    private void UpdateUI()
    {
        if (levelText == null)
        {
            levelText = GameObject.Find("level count").GetComponent<TextMeshProUGUI>();
        }

        if (xpFill == null)
        {
            xpFill = GameObject.Find("Xp Bar fill").GetComponent<Image>();
        }

        if (levelText != null)
        {
            levelText.text = $"Level: {currentLevel}";
        }

        if (xpFill != null)
        {

            StartCoroutine(ChangeToPct((float)currentXP / (float)maxXP));
        }
    }


    //----------FOR SMOOTH ANIMATION-----------

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = xpFill.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            xpFill.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        xpFill.fillAmount = pct;
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentXP);
            stream.SendNext(currentLevel);
        }
        else
        {
            currentXP = (int)stream.ReceiveNext();
            currentLevel = (int)stream.ReceiveNext();
            UpdateUI();
        }
    }

    public void IncreaseValue(int value)
    {
        XPvalue += value;

        if(photonView.IsMine)
        {
            photonView.RPC("UpdateXPvalue", RpcTarget.AllBuffered, XPvalue);

        }
    }
    [PunRPC]
    private void UpdateXPvalue(int value)
    {
        XPvalue = value;
    }
}