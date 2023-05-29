using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class BalenceManager : MonoBehaviour
{
    public static BalenceManager instance;

    public int coinBalance;
    public int gemsBalance;
    public int thundersBalance;


    public TextMeshProUGUI coinBalanceText;
    public TextMeshProUGUI gemsBalanceText;
    public TextMeshProUGUI thundersBalanceText;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        GetVirtualCurrencies();
    }

    private void Update()
    {
        UpdateText(coinBalance, gemsBalance, thundersBalance);
    }

    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
    }

    void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        coinBalance = result.VirtualCurrency["CN"];

        gemsBalance = result.VirtualCurrency["GM"];

        thundersBalance = result.VirtualCurrency["TH"];

        Debug.Log("Currecry balence recieved!");

    }

    public void BuyHunters(int amount)
    {
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = amount,
        };

        PlayFabClientAPI.SubtractUserVirtualCurrency(request, OnSubtractCoinsSuccess, OnError);
    }

    void OnSubtractCoinsSuccess(ModifyUserVirtualCurrencyResult result)
    {
        GetVirtualCurrencies();
        // unlock hunter 
    }

    public void AddCoins(int amount)
    {
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = amount,
        };

        PlayFabClientAPI.AddUserVirtualCurrency(request, OnAddCoinsSuccess, OnError);

    }

    void OnAddCoinsSuccess(ModifyUserVirtualCurrencyResult result)
    {
        GetVirtualCurrencies();
    }

    public void LoseThunders(int amount)
    {
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "TH",
            Amount = amount,
        };

        PlayFabClientAPI.SubtractUserVirtualCurrency(request, OnThundersUpdate, OnError);
    }

    public void AddThunder(int amount)
    {
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = "TH",
            Amount = amount,
        };

        PlayFabClientAPI.AddUserVirtualCurrency(request, OnThundersUpdate, OnError);

    }

    void OnThundersUpdate(ModifyUserVirtualCurrencyResult result)
    {
        GetVirtualCurrencies();
        //update leaderboard
    }




    public void UpdateText(int coinBalance, int gemsBalance, int thundersBalance)
    {
       
        coinBalanceText.text = coinBalance.ToString();
        
        gemsBalanceText.text = gemsBalance.ToString();

        thundersBalanceText.text = thundersBalance.ToString();

    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error : " + error.ErrorMessage);
        Debug.Log(error.GenerateErrorReport());
    }
}
