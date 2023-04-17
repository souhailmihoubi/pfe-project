using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGamePanel : MonoBehaviour
{
    public Canvas canvas;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.rootCanvas.enabled = false;
    }

    
}
