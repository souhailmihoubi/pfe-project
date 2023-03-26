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

        if (other.gameObject.CompareTag("XP") && photonView.IsMine)
        {
            currentXP += 10;

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
}