using TMPro;
using UnityEngine;

public class BalenceManager : MonoBehaviour
{
    public static BalenceManager instance { get; private set; }

    public int coinBalance;
    public int gemsBalance;
    public int thundersBalance;


    public TextMeshProUGUI coinBalanceText;
    public TextMeshProUGUI gemsBalanceText;
    public TextMeshProUGUI thundersBalanceText;


    private void Update()
    {
        coinBalance = SaveManager.instance.coins;
        gemsBalance = SaveManager.instance.gems;
        thundersBalance = SaveManager.instance.thunders;

        UpdateText(coinBalance, gemsBalance, thundersBalance);
    }


    public void UpdateText(int coinBalance, int gemsBalance, int thundersBalance)
    {
        if(coinBalance < 0)
        {
            coinBalanceText.text = "0";
        }
        else
        {
            coinBalanceText.text = coinBalance.ToString();
        }
        if(gemsBalance < 0)
        {
            gemsBalanceText.text = "0";
        }
        else
        {
            gemsBalanceText.text = gemsBalance.ToString();
        }
        if (thundersBalance < 0)
        {
            thundersBalanceText.text = "0";
        }
        else
        {
            thundersBalanceText.text = thundersBalance.ToString();
        }


    }
}
