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


    // Start is called before the first frame update
    void Start()
    { 
        total.text = SaveManager.instance.matchPlayed.ToString();
        owned.text = SaveManager.instance.owned.ToString();
        //mostplayed.text = SaveManager.instance.mostPlayed.ToString();
        //ranked.text = SaveManager.instance.ranked.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
