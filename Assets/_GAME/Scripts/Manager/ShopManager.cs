using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Playgama;
using Playgama.Modules.Advertisement;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private UpgradeSelectManager upgradeSelectManager;

    public static Action onWatchAds;

    private string currentRewardType = "";
    private GameObject currentButton;

    private void Start()
    {
        Bridge.advertisement.rewardedStateChanged += OnRewardedStateChanged;
    }

    private void OnDestroy()
    {
        Bridge.advertisement.rewardedStateChanged -= OnRewardedStateChanged;
    }

    public void GoldPack()
    {
        ShowRewardedAd("GoldPack");
    }

    public void EnergyPack()
    {
        ShowRewardedAd("EnergyPack");
    }

    public void BigPack()
    {
        ShowRewardedAd("BigPack");
    }

    public void DamagePack()
    {
        ShowRewardedAd("DamagePack");
    }

    private void ShowRewardedAd(string rewardType)
    {
        if (Bridge.advertisement.rewardedState == RewardedState.Loading)
        {
            Debug.Log("Reklam yükleniyor, lütfen bekleyin...");
            return;
        }

        currentRewardType = rewardType;
        currentButton = EventSystem.current.currentSelectedGameObject;
        Bridge.advertisement.ShowRewarded();
    }

    private void OnRewardedStateChanged(RewardedState state)
    {
        Debug.Log($"Reklam durumu: {state}");

        if (state == RewardedState.Rewarded)
        {
            GrantReward(currentRewardType);

            onWatchAds?.Invoke();

            if (currentButton != null)
                currentButton.SetActive(false);

            currentRewardType = "";
            currentButton = null;
        }
    }

    private void GrantReward(string rewardType)
    {
        switch (rewardType)
        {
            case "GoldPack":
                DataManager.instance.AddGold(250);
                break;

            case "EnergyPack":
                DataManager.instance.AddEnergy(10);
                break;

            case "BigPack":
                DataManager.instance.AddEnergy(20);
                DataManager.instance.AddGold(500);
                break;

            case "DamagePack":
                upgradeSelectManager.HeroDamageItem(5);
                break;
        }

        Debug.Log($"{rewardType} ödülü verildi.");
    }
}
