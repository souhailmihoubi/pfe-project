using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGamePanel : MonoBehaviour
{
    public Canvas canvas;

    public TextMeshProUGUI rank;
    public TextMeshProUGUI coins;
    public TextMeshProUGUI kills;
    public TextMeshProUGUI thunders;

    public GameObject adsPanel;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        //canvas.rootCanvas.enabled = false;
    }



    
}
