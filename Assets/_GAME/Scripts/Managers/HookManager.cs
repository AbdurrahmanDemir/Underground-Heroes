using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;
using Playgama;

public class HookManager : MonoBehaviour
{
    public static HookManager instance;

    [Header("Settings")]
    public int hookLength;
    public int hookStrength;
    public int offlineEarnings;
    [SerializeField] private int lengthCost;
    [SerializeField] private int strengthCost;
    [SerializeField] private int towerUpgradeCost;
    [SerializeField] private int totalGain;


    [Header("UI Settings")]
    public Button lengthButton;
    public Button strengthButton;
    public Button offlineButton;

    public TextMeshProUGUI gameScreenMoney;
    public TextMeshProUGUI lengthCostText;
    public TextMeshProUGUI lengthValueText;
    public TextMeshProUGUI menuLengthValueText;
    public TextMeshProUGUI strengthCostText;
    public TextMeshProUGUI strengthValueText;
    public TextMeshProUGUI offlineCostText;
    public TextMeshProUGUI offlineValueText;
    public TextMeshProUGUI endScreenMoney;
    public TextMeshProUGUI returnScreenMoney;

    public GameObject startThrowButton;
    public Sprite startThrowButtonBack;
    public Sprite startThrowButtonFront;


    [Header(" Data ")]
    public int token;
    [SerializeField] private TextMeshProUGUI tokenText;


    public TextMeshProUGUI cooldownText;
    public float cooldownDuration = 60f;


    public int[] costs = new int[]
  {
        120,
        151,
        197,
        250,
        324,
        414,
        537,
        687,
        892,
        1145,
        1484,
        1911,
        2479,
        3196,
        4148,
        5359,
        6954,
        9000,
        11687
  };

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();

        UpgradeSelectManager.hookLenghtItem += PowerUpLength;
        UpgradeSelectManager.hookStranghtItem+= PowerUpStrength;
        UpgradeSelectManager.tokenAddItem += AddToken;

        TowerController.onGameLose += ResetToken;
        EnemyTowerController.onGameWin += ResetToken;
    }
    private void OnDestroy()
    {
        UpgradeSelectManager.hookLenghtItem = PowerUpLength;
        UpgradeSelectManager.hookStranghtItem -= PowerUpStrength;
        UpgradeSelectManager.tokenAddItem -= AddToken;

        TowerController.onGameLose -= ResetToken;
        EnemyTowerController.onGameWin -= ResetToken;
    }
    void Start()
    {
        //CheckIdles();
        UpdateTexts();
        AddToken(20);
    }
    private void Update()
    {
        if (token >= (Hook.throwCount * 10))
        {
            startThrowButton.GetComponent<Animator>().Play("Idle");
            startThrowButton.GetComponent<Image>().sprite = startThrowButtonFront;

        }
        else
        {
            startThrowButton.GetComponent<Animator>().StopPlayback();
            startThrowButton.GetComponent<Image>().sprite = startThrowButtonBack;
        }
    }
    void LoadData()
    {
        hookLength = -PlayerPrefs.GetInt("Length", 30);
        hookStrength = PlayerPrefs.GetInt("Strength", 3);
        offlineEarnings = PlayerPrefs.GetInt("Offline", 3);
        lengthCost = costs[-hookLength / 10 - 3];
        strengthCost = costs[hookStrength - 3];
        towerUpgradeCost = costs[offlineEarnings - 3];
    }

    void ResetToken()
    {
        token = 20;
        UpdateTokenText();

    }
    public bool TryPurchaseToken(int price)
    {
        if (price <= token)
        {
            token -= price;
            UpdateTokenText();
            return true;
        }
        else
        {
            PopUpController.instance.OpenPopUp("NOT ENOUGH TOKEN");
        }
        return false;
    }

    public void AddToken(int value)
    {
        token += value;
        UpdateTokenText();
    }

    public void Buy30Token()
    {
        Bridge.advertisement.ShowRewarded();


        AddToken(30);
            StartCoroutine(CooldownCoroutine());
            Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            button.interactable = false;

    }
    IEnumerator CooldownCoroutine()
    {
        float remainingTime = cooldownDuration;

        while (remainingTime > 0)
        {
            cooldownText.text = "" + Mathf.CeilToInt(remainingTime) + " s";
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        cooldownText.text = "AD";
        Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        button.interactable = true;
        Debug.Log("Buton tekrar aktif!");
    }
    private void UpdateTokenText()
    {
        tokenText.text = token.ToString();
    }


    public void BuyLength()
    {
        if(hookLength == 100)
            PopUpController.instance.OpenPopUp("LENGTH MAX SIZE");

        if (TryPurchaseToken(costs[-hookLength / 10 - 3]))
        {
            hookLength -= 10;
            lengthCost = costs[-hookLength / 10 - 3];
            //PlayerPrefs.SetInt("Length", -hookLength);
            UpdateTexts();
        }

    }
    public void UpgradeLenght()
    {
        if (hookLength == 100)
            PopUpController.instance.OpenPopUp("LENGTH MAX SIZE");

            hookLength -= 1;
            PlayerPrefs.SetInt("Length", -hookLength);
            UpdateTexts();
       
    }

    public void BuyStrength()
    {
        if (TryPurchaseToken(costs[hookStrength - 3]))
        {
            hookStrength++;
            strengthCost = costs[hookStrength - 3];
            //PlayerPrefs.SetInt("Strength", hookStrength);
            UpdateTexts();
        }

    }

    public void BuyOfflineEarnings()
    {
            offlineEarnings++;
            strengthCost = costs[offlineEarnings - 3];
            //PlayerPrefs.SetInt("Offline", offlineEarnings);
            UpdateTexts();
    

    }

    public void PowerUpLength()
    {
        if (hookLength == 100)
            PopUpController.instance.OpenPopUp("LENGTH MAX SIZE");

        if (TryPurchaseToken(0))
        {
            hookLength -= 10;
            lengthCost = costs[-hookLength / 10 - 3];
            //PlayerPrefs.SetInt("Length", -hookLength);
            UpdateTexts();
        }
    }
    public void PowerUpStrength()
    {
        if (TryPurchaseToken(0))
        {
            hookStrength++;
            strengthCost = costs[hookStrength - 3];
            //PlayerPrefs.SetInt("Strength", hookStrength);
            UpdateTexts();
        }
    }


    public void CollectMoney()
    {
        AddToken(totalGain);
    }

    public void CollectDoubleMoney()
    {
        AddToken(totalGain * 2);

    }
    public void UpdateTexts()
    {
        lengthCostText.text = lengthCost.ToString();
        lengthValueText.text = -hookLength + "m <color=green>+10 m</color>";
        menuLengthValueText.text = -hookLength + "m <color=green>+1 m</color>";
        strengthCostText.text = strengthCost.ToString();
        strengthValueText.text = hookStrength + " heroes. <color=green>+1</color>";
        offlineCostText.text = towerUpgradeCost.ToString();
        //offlineValueText.text = "$" + offlineEarnings + "/min <color=green>+5</color>";
    }


    //public void CheckIdles()
    //{        
    //    if (token < lengthCost)
    //        lengthButton.interactable = false;
    //    else
    //        lengthButton.interactable = true;

    //    if (token < strengthCost)
    //        strengthButton.interactable = false;
    //    else
    //        strengthButton.interactable = true;

    //    if (token < offlineEarningsCost)
    //        offlineButton.interactable = false;
    //    else
    //        offlineButton.interactable = true;
    //}
}
