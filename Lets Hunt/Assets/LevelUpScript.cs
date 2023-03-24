using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpScript : MonoBehaviour
{
 
    private Button atkSpeed, damage, range, health, speed, xp;

    Button[] leftButtons, rightButtons;

    PhotonView photonView;




    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        leftButtons = new Button[3];
        rightButtons = new Button[3];


        atkSpeed = GameObject.Find("atk speed").GetComponent<Button>();
        damage = GameObject.Find("damage").GetComponent<Button>();
        range = GameObject.Find("Range").GetComponent<Button>();
        health = GameObject.Find("healthh").GetComponent<Button>();
        speed = GameObject.Find("speed").GetComponent<Button>();
        xp = GameObject.Find("xp").GetComponent<Button>();

        
        leftButtons[0] = xp;
        leftButtons[1] = health;
        leftButtons[2] = speed;

        rightButtons[0] = atkSpeed;
        rightButtons[1] = damage;
        rightButtons[2] = range;

        ShutDown();
        ButtonOnClick();


    }

    public void LevelUp()
    {


        Button leftButton = leftButtons[1]; //leftButtons[Random.Range(0, 2)];
        Button rightButton = rightButtons[2];             //rightButtons[Random.Range(0, 2)];

         Debug.Log(leftButton.name + " " + rightButton.name);

         leftButton.gameObject.SetActive(true);
         rightButton.gameObject.SetActive(true);
       

        leftButton.onClick.AddListener(() => { leftButton.gameObject.SetActive(false); rightButton.gameObject.SetActive(false); });
        rightButton.onClick.AddListener(() => { rightButton.gameObject.SetActive(false); leftButton.gameObject.SetActive(false); });
        
    }

    public void OnAtkSpeedButtonClick()
    {
        // Handle the atk speed button click
    }

    public void OnDamageButtonClick()
    {
        // change player damage
    }

     public void OnRangeButtonClick()
     {
       PlayerRange playerRange = GetComponent<PlayerRange>();

        playerRange.ChangeRange(100, 100);
        playerRange.UpdateUI();

       PlayerAttack pa = GetComponent<PlayerAttack>();

        pa.attackRange += 1f;

        photonView.RPC("UpdateRange", RpcTarget.OthersBuffered, playerRange.currentRange);


    }

   
   

    public void OnHealthButtonClick()
    {
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        playerHealth.maxHealth += 20f;
        playerHealth.currentHealth += 20f;
        playerHealth.UpdateUI();

        photonView.RPC("UpdateHealth", RpcTarget.OthersBuffered, playerHealth.currentHealth);
    }


    public void OnSpeedButtonClick()
    {
        ControllerScript controllerScript = GetComponent<ControllerScript>();
        controllerScript.speed += 0.3f;
        Debug.Log("speed : " + controllerScript.speed);
    }

    public void OnXPButtonClick()
    {
        // Handle the xp button click
    }



    private void ShutDown()
    {
        foreach (Button button in leftButtons)
        {
            button.gameObject.SetActive(false);
        }
        foreach (Button button in rightButtons)
        {
            button.gameObject.SetActive(false);
        }

    }



    void ButtonOnClick()
    {
        atkSpeed.onClick.AddListener(OnAtkSpeedButtonClick);
        speed.onClick.AddListener(OnSpeedButtonClick);
        range.onClick.AddListener(OnRangeButtonClick);
        damage.onClick.AddListener(OnDamageButtonClick);
        xp.onClick.AddListener(OnXPButtonClick);
        health.onClick.AddListener(OnHealthButtonClick);
    }




}

