using UnityEngine;
using Playgama;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Hero", menuName = "Create Hero")]
public class HeroSO : ScriptableObject
{
    [Header("Hero")]
    public string heroName;
    public Sprite heroImage;

    [Header("Hero Settings")]
    public string attackType;
    public bool isAreaOfEffect;
    public int maxHealth;
    public int damage;
    public float range;
    public float moveSpeed;
    public float cooldown;

    private int cachedHealth;
    private int cachedDamage;
    private bool isLoaded = false;

    public void LoadData(System.Action onLoaded = null)
    {
        var keys = new List<string>() { $"{heroName}_Health", $"{heroName}_Damage" };

        Bridge.storage.Get(keys, (success, data) =>
        {
            if (success)
            {
                if (!string.IsNullOrEmpty(data[0]))
                {
                    int.TryParse(data[0], out cachedHealth);
                }
                else
                {
                    cachedHealth = maxHealth;
                }

                if (!string.IsNullOrEmpty(data[1]))
                {
                    int.TryParse(data[1], out cachedDamage);
                }
                else
                {
                    cachedDamage = damage;
                }
            }
            else
            {
                cachedHealth = maxHealth;
                cachedDamage = damage;
            }

            isLoaded = true;
            Debug.Log($"{heroName} data loaded: Health = {cachedHealth}, Damage = {cachedDamage}");

            onLoaded?.Invoke();
        });
    }

    public int GetCurrentHealth()
    {
        if (!isLoaded) return maxHealth;
        return cachedHealth;
    }

    public int GetCurrentDamage()
    {
        if (!isLoaded) return damage;
        return cachedDamage;
    }
}
