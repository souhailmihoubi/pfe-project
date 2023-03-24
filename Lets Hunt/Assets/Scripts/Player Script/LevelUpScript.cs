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


        GameObject[] buttons = GameObject.FindGameObjectsWithTag("LevelUp");
        foreach (GameObject button in buttons)
        {
            Button buttonComponent = button.GetComponent<Button>();
            if (buttonComponent != null)
            {
                // Set the Button component for each button game object
                switch (button.name)
                {
                    case "atk speed":
                        atkSpeed = buttonComponent;
                        break;
                    case "damage":
                        damage = buttonComponent;
                        break;
                    case "Range":
                        range = buttonComponent;
                        break;
                    case "healthh":
                        health = buttonComponent;
                        break;
                    case "speed":
                        speed = buttonComponent;
                        break;
                    case "xp":
                        xp = buttonComponent;
                        break;
                }
            }
        }


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

        
        Button leftButton = leftButtons[2]; //leftButtons[Random.Range(0, 2)];
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
        PlayerMove playerMove = GetComponent<PlayerMove>();
        if (playerMove != null)
        {
            playerMove.IncreaseSpeed(1.5f);
        }
    }

    public void OnXPButtonClick()
    {
        // Handle the xp button click
    }



    private void ShutDown()
    {
        if (photonView.IsMine)
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
    }



    void ButtonOnClick()
    {
        if (atkSpeed != null)
            atkSpeed.onClick.AddListener(OnAtkSpeedButtonClick);
        if (speed != null)
            speed.onClick.AddListener(OnSpeedButtonClick);
        if (range != null)
            range.onClick.AddListener(OnRangeButtonClick);
        if (damage != null)
            damage.onClick.AddListener(OnDamageButtonClick);
        if (xp != null)
            xp.onClick.AddListener(OnXPButtonClick);
        if (health != null)
            health.onClick.AddListener(OnHealthButtonClick);
    }





}

