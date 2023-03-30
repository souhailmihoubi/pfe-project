using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager instance;

    // The player's coin balance
    public int coinBalance;

    // The key used to store and retrieve the coin balance in PlayerPrefs
    private string coinBalanceKey = "CoinBalance";

    public TextMeshProUGUI coinBalanceText; // The text displaying the player's coin balance


    // Called when the game starts
    private void Start()
    {

        FindText();
        DontDestroyOnLoad(gameObject);


        // Set the GameManager instance
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Load the player's coin balance from PlayerPrefs
        coinBalance = PlayerPrefs.GetInt(coinBalanceKey, 0);

        // Set the coin balance text in the main menu
        UpdateCoinBalanceText(coinBalance);
    }


    public void Initialize()
    {

        Start();
    }

    void FindText()
    {
        GameObject textObject = GameObject.FindGameObjectWithTag("CoinText");

        if (textObject != null)
        {
            coinBalanceText = textObject.GetComponentInChildren<TextMeshProUGUI>();
        }

    }

    // Called when the game ends
    private void OnDestroy()
    {
        // Save the player's coin balance to PlayerPrefs
        PlayerPrefs.SetInt(coinBalanceKey, coinBalance);
        PlayerPrefs.Save();
    }

    // Add coins to the player's balance
    public void AddCoins(int amount)
    {
       coinBalance += amount;
       UpdateCoinBalanceText(coinBalance);
    }

    public void UpdateCoinBalanceText(int coinBalance)
    {
        coinBalanceText.text = coinBalance.ToString();
    }
}
