using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Timer : MonoBehaviour
{
    

    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;

    public int Duration;

    private int remainingDuration;

    private bool Pause;

    private void Start()
    {
        Being(Duration);
    }

    private void Being(int Second)
    {
        remainingDuration = Second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while(remainingDuration >= 0)
        {   
                uiText.text = $"{remainingDuration / 60:00}:{remainingDuration % 60:00}";
                uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
                remainingDuration--;
                if(remainingDuration <= Duration * 0.25f)
                {
                    uiFill.color = Color.red;
                    uiText.color = Color.red;
                }
                yield return new WaitForSeconds(1f);
        }
        OnEnd();
    }

    private void OnEnd()
    {
        //End Time , if want Do something
        print("End");
    }
}
