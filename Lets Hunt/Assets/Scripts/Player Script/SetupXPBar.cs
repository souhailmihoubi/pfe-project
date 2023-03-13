using System.Collections;
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

}
