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

    ControllerScript controllerScript;

    private void Start()
    {
        // Initialize the arrays with 3 elements each
        leftButtons = new Button[3];
        rightButtons = new Button[3];

        // Find the button components
        atkSpeed = GameObject.Find("atk speed").GetComponent<Button>();
        damage = GameObject.Find("damage").GetComponent<Button>();
        range = GameObject.Find("Range").GetComponent<Button>();
        health = GameObject.Find("healthh").GetComponent<Button>();
        speed = GameObject.Find("speed").GetComponent<Button>();
        xp = GameObject.Find("xp").GetComponent<Button>();

        // Add the button components to the arrays using the [] operator
        leftButtons[0] = xp;
        leftButtons[1] = health;
        leftButtons[2] = speed;

        rightButtons[0] = atkSpeed;
        rightButtons[1] = damage;
        rightButtons[2] = range;

        ShutDown();

        controllerScript = FindObjectOfType<ControllerScript>();

    }

    public void LevelUp()
    {


         Button leftButton = leftButtons[Random.Range(0, 2)];
         Button rightButton = rightButtons[Random.Range(0, 2)];

         Debug.Log(leftButton.name + " " + rightButton.name);

         leftButton.gameObject.SetActive(true);
         rightButton.gameObject.SetActive(true);
       

        leftButton.onClick.AddListener(() => { leftButton.gameObject.SetActive(false); rightButton.gameObject.SetActive(false); });
        rightButton.onClick.AddListener(() => { rightButton.gameObject.SetActive(false); leftButton.gameObject.SetActive(false); });
        
        StartCoroutine(DisableLevelUpCanvas());
    }

    public void OnAtkSpeedButtonClick()
    {
        // Handle the atk speed button click
    }

    public void OnDamageButtonClick()
    {
        // Handle the damage button click
    }

    public void OnRangeButtonClick()
    {
        // Handle the range button click
    }

    public void OnHealthButtonClick()
    {
        // Handle the health button click
    }

    public void OnSpeedButtonClick()
    {
        controllerScript.speed += 0.2f;
    }

    public void OnXPButtonClick()
    {
        // Handle the xp button click
    }



    private void ShutDown()
    {
        atkSpeed.gameObject.SetActive(false);
        speed.gameObject.SetActive(false);
        range.gameObject.SetActive(false);
        damage.gameObject.SetActive(false);
        xp.gameObject.SetActive(false);
        health.gameObject.SetActive(false);
    }


    private IEnumerator DisableLevelUpCanvas()
    {
        yield return new WaitForSeconds(5f);
    }

   

}

