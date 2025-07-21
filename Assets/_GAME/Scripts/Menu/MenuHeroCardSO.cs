using UnityEngine;
using Playgama;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MenuHero", menuName = "HeroCard")]
public class MenuHeroCardSO : ScriptableObject
{
    [Header("Hero")]
    public string cardName;
    public Sprite heroIcon;
    public CardType cardType;
    public int baseUpgradeCost = 10;
    public float range;
    public int damage;
    public int health;
    public float cooldown;
    public float moveSpeed;
    public float undergroundRange;

    [Header("Other Stat")]
    public string specialStatName;
    public float specialStat;
    [TextArea] public string cardDescription;

    private int cachedHealth;
    private int cachedDamage;
    private int cachedUpgradeCost;

    private bool isLoaded = false;

    public void LoadData(System.Action onLoaded = null)
    {
        var keys = new List<string>()
        {
            $"{cardName}_Health",
            $"{cardName}_Damage",
            $"{cardName}_UpgradeCost"
        };

        Bridge.storage.Get(keys, (success, data) =>
        {
            if (success)
            {
                cachedHealth = ParseOrDefault(data[0], health);
                cachedDamage = ParseOrDefault(data[1], damage);
                cachedUpgradeCost = ParseOrDefault(data[2], baseUpgradeCost);
            }
            else
            {
                cachedHealth = health;
                cachedDamage = damage;
                cachedUpgradeCost = baseUpgradeCost;
            }

            isLoaded = true;
            Debug.Log($"[HeroCard] {cardName} loaded: HP={cachedHealth}, DMG={cachedDamage}, Cost={cachedUpgradeCost}");
            onLoaded?.Invoke();
        });
    }

    private int ParseOrDefault(string str, int defaultVal)
    {
        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out int result))
            return result;
        else
            return defaultVal;
    }

    public int GetCurrentHealth()
    {
        if (!isLoaded) Debug.LogWarning($"{cardName} health is not loaded yet!");
        return isLoaded ? cachedHealth : health;
    }

    public int GetCurrentDamage()
    {
        if (!isLoaded) Debug.LogWarning($"{cardName} damage is not loaded yet!");
        return isLoaded ? cachedDamage : damage;
    }

    public int GetUpgradeCost()
    {
        if (!isLoaded) Debug.LogWarning($"{cardName} upgrade cost is not loaded yet!");
        return isLoaded ? cachedUpgradeCost : baseUpgradeCost;
    }

    public void UpgradeHero()
    {
        if (!isLoaded)
        {
            Debug.LogWarning("Data not loaded, cannot upgrade!");
            return;
        }

        if (DataManager.instance.TryPurchaseGold(cachedUpgradeCost))
        {
            cachedDamage += 5;
            cachedUpgradeCost *= 2;
            cachedHealth += 50;

            var keys = new List<string>()
            {
                $"{cardName}_Damage",
                $"{cardName}_Health",
                $"{cardName}_UpgradeCost"
            };

            var values = new List<object>()
            {
                cachedDamage,
                cachedHealth,
                cachedUpgradeCost

            };

            Bridge.storage.Set(keys, values, (success) =>
            {
                Debug.Log($"[HeroCard] Upgraded: HP={cachedHealth}, DMG={cachedDamage}, Cost={cachedUpgradeCost}. Success: {success}");
            });
        }
    }
}

public enum CardType
{
    Cammon = 0,
    Rare = 1,
    Epic = 2,
    Legendary = 3
}
