using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupHealthBar : MonoBehaviour
{
    public Image foregroundImage;
    [SerializeField]
    private float updateSpeedSeconds = 0.5f;
    public TextMeshProUGUI healthTxt;

    private void Awake()
    {
        //foregroundImage = GetComponentInChildren<Image>();
        GetComponentInParent<PlayerHealth>().OnHealthPctChange += HandleHealthChanged;
    }


    private void HandleHealthChanged(float pct)
    {
        //foregroundImage.fillAmount = pct;  // this works too
        StartCoroutine(ChangeToPct(pct));  // add coroutine to add some smooth animation 
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

    private void LateUpdate()
    {
        //transform.LookAt(Camera.main.transform);
      //  transform.Rotate(0, 0, 0);
    }
}
