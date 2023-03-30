using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI coinBalanceText; // The text displaying the player's coin balance
    public static MainMenu instance;

    // Called when the main menu starts
    private void Start()
    {
        // Set the MainMenu instance
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Load the player's coin balance from PlayerPrefs
        GameManager.instance.coinBalance = PlayerPrefs.GetInt("coinBalanceKey", 0);

        // Update the coin balance text
        UpdateCoinBalanceText(GameManager.instance.coinBalance);
    }

    // Called when the main menu is destroyed
    public void OnDestroy()
    {
        // Save the player's coin balance to PlayerPrefs
        PlayerPrefs.SetInt("coinBalanceKey", GameManager.instance.coinBalance);
        PlayerPrefs.Save();
    }

    // Updates the coin balance text
    public void UpdateCoinBalanceText(int coinBalance)
    {
        coinBalanceText.text = coinBalance.ToString();
    }
}
