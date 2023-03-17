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

    public TextMeshProUGUI levelText;
    public Image xpFill;

    public LevelUpScript levelUpScript;


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
            xpFill.fillAmount = (float)currentXP / (float)maxXP;
        }
    }

   /* [PunRPC]
    private void UpdateXP(int newXP, int newLevel)
    {
        currentXP = newXP;
        currentLevel = newLevel;
        UpdateUI();
    }*/

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




/*using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SetupXPBar : MonoBehaviour
{
    PhotonView view;

    public Image foregroundImage;
    public TextMeshProUGUI LvlCount;
    public float updateSpeedSeconds = 0.5f;

    private float points = 10f;

    public float maxPoints = 100f;
    private float currentPoints = 0f;

    bool collided = false;

    private void Start()
    {
        view = GetComponent<PhotonView>();

    }
    private void Awake()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        if(view.IsMine)
        {
            if (other.gameObject.CompareTag("XP"))
            {
                collided = true; // Remove "bool" keyword to set class-level variable
                Debug.Log(collided);
                Destroy(other.gameObject);
                //currentPoints += 10f;
                currentPoints = Mathf.Clamp(currentPoints, 0f, maxPoints);
                Debug.Log(currentPoints);
            }
        }
       
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        foregroundImage.fillAmount = pct;
    }

    private void Update()
    {
        if(view.IsMine)
        {
            if (collided == true)
            {
                currentPoints += points;
                currentPoints = Mathf.Clamp(currentPoints, 0f, maxPoints);
                float pct = currentPoints / maxPoints;
                StartCoroutine(ChangeToPct(pct));

                if (currentPoints >= maxPoints)
                {
                    int lvl = int.Parse(LvlCount.text);
                    lvl++;
                    LvlCount.SetText(lvl.ToString());
                    currentPoints = 0f;
                    foregroundImage.fillAmount = 0f;
                    Debug.Log("returned");
                }

                collided = false;
            }
        }
       
    }

}*/
