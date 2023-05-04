using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Stats : MonoBehaviour
{

    public TextMeshProUGUI total; 
    public TextMeshProUGUI ranked; 
    public TextMeshProUGUI owned; 
    public TextMeshProUGUI mostplayed; 

    void Start()
    { 
        total.text = SaveManager.instance.matchPlayed.ToString();
        owned.text = SaveManager.instance.owned.ToString();
        mostplayed.text = SaveManager.instance.MostPlayed();
        ranked.text = SaveManager.instance.ranked.ToString();

    }

}
