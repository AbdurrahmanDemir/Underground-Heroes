using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using Playgama;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header(" Data ")]
    [SerializeField] private int gold;
    [SerializeField] private int xp;
    [SerializeField] private int energy;

    [Header(" Text ")]
    [SerializeField] private TextMeshProUGUI[] GoldText;
    [SerializeField] private TextMeshProUGUI[] XpText;
    [SerializeField] private TextMeshProUGUI[] EnergyText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadData();
    }

    #region Purchasing

    public bool TryPurchaseGold(int price)
    {
        if (price <= gold)
        {
            gold -= price;
            SaveData();
            UpdateGoldText();
            return true;
        }
        else
        {
            PopUpController.instance.OpenPopUp("NOT ENOUGH GOLD");
        }
        return false;
    }

    public bool TryPurchaseEnergy(int price)
    {
        if (price <= energy)
        {
            energy -= price;
            SaveData();
            UpdateEnergyText();
            return true;
        }
        else
        {
            PopUpController.instance.OpenPopUp("NOT ENOUGH ENERGY");
        }
        return false;
    }

    #endregion

    #region Add Methods

    public void AddGold(int value)
    {
        gold += value;
        UpdateGoldText();
        SaveData();
    }

    public void AddXP(int value)
    {
        xp += value;
        UpdateXPText();
        SaveData();
    }

    public void AddEnergy(int value)
    {
        energy += value;
        UpdateEnergyText();
        SaveData();
    }

    #endregion

    #region UI Updates

    private void UpdateGoldText()
    {
        for (int i = 0; i < GoldText.Length; i++)
        {
            GoldText[i].text = gold.ToString();
        }
    }

    private void UpdateXPText()
    {
        for (int i = 0; i < XpText.Length; i++)
        {
            XpText[i].text = xp.ToString();
        }
    }

    private void UpdateEnergyText()
    {
        for (int i = 0; i < EnergyText.Length; i++)
        {
            EnergyText[i].text = energy.ToString();
        }
    }

    #endregion

    #region Save & Load

    private void SaveData()
    {
        var keys = new List<string>() { "Gold", "XP", "Energy" };
        var values = new List<object>() { gold, xp, energy };

        Bridge.storage.Set(keys, values, (success) =>
        {
            Debug.Log($"Data saved to storage. Success: {success}");
        });
    }

    private void LoadData()
    {
        var keys = new List<string>() { "Gold", "XP", "Energy" };
        Bridge.storage.Get(keys, (success, data) =>
        {
            if (success)
            {
                // Gold
                if (!string.IsNullOrEmpty(data[0]))
                {
                    int.TryParse(data[0], out gold);
                }
                else
                {
                    AddGold(200); // Ýlk giriþ bonusu
                }

                // XP
                if (!string.IsNullOrEmpty(data[1]))
                {
                    int.TryParse(data[1], out xp);
                }

                // Energy
                if (!string.IsNullOrEmpty(data[2]))
                {
                    int.TryParse(data[2], out energy);
                }
                else
                {
                    AddEnergy(5); // Ýlk giriþ bonusu
                }

                Debug.Log($"GOLD: {gold}, XP: {xp}, ENERGY: {energy}");

                SaveData();
                UpdateGoldText();
                UpdateXPText();
                UpdateEnergyText();
            }
            else
            {
                Debug.LogWarning("Data loading failed from storage.");

                // Yeni oyuncu ise default verileri ekle
                AddGold(200);
                AddEnergy(5);
                SaveData();
                UpdateGoldText();
                UpdateXPText();
                UpdateEnergyText();
            }
        });
    }

    #endregion
}
