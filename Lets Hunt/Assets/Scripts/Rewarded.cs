using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class Rewarded : MonoBehaviour
{
    private RewardedAd rewardedAd;
    private string adUnitId = "ca-app-pub-3940256099942544/5224354917";
    public float timer;
    public Text Ttimer;
    public Button Breward;

    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) => { });
        timer = 5f;
        RequestRewardVideo();
    }

    void Update()
    {
        //Ttimer.text = ("reward in " + Mathf.Round(timer) + " sec");

        /*if (timer <= 0f)
        {
            Breward.interactable = true;
        }
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            Breward.interactable = false;
        }*/
    }

    public void showrewarded()
    {
        RequestRewardVideo();
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
            timer = 5f;
        }
    }

    public void RequestRewardVideo()
    {
        this.rewardedAd = new RewardedAd(adUnitId);
        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);


    }


    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {  
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        Debug.Log("You got your reward");
    }






}
