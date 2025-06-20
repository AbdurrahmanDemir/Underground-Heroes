using Playgama;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TMPro;


public class ChestManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject rewardPopUp;
    [SerializeField] private GameObject rewardPopUpShop;
    [SerializeField] private GameObject rewardContainerPrefab;
    [SerializeField] private Transform rewardContainersParent;
    [SerializeField] private Transform rewardContainersParentShop;
    [SerializeField] private Sprite rewardGoldIcon;
    [SerializeField] private Sprite rewardEnergyIcon;
    [Header("CardboardBox Settings")]
    [SerializeField] private int[] cardboardGoldPossibilities;
    [SerializeField] private int[] cardboardEnergyPossibilities;
    [Header("GoldBox Settings")]
    [SerializeField] private int[] goldBoxGoldPossibilities;
    [SerializeField] private int[] goldBoxEnergyPossibilities;
    [Header("Legendary Settings")]
    [SerializeField] private int[] legendaryBoxGoldPossibilities;
    [SerializeField] private int[] legendaryBoxEnergyPossibilities;




    public void CardboardBox()
    {
        Bridge.advertisement.ShowRewarded();

        rewardContainersParent.Clear();

            int randomType = Random.Range(0, 2);
            int randomGoldNumber = Random.Range(0, cardboardGoldPossibilities.Length);
            int randomEnergyNumber = Random.Range(0, cardboardEnergyPossibilities.Length);
            UIManager.instance.TogglePanel(rewardPopUp);

            switch (randomType)
            {
                case 0:
                    GameObject containerInstance = Instantiate(rewardContainerPrefab, rewardContainersParent);
                    Image targetImage = containerInstance.transform.GetChild(0).GetComponent<Image>();
                    targetImage.sprite = rewardGoldIcon;
                    containerInstance.GetComponentInChildren<TextMeshProUGUI>().text = cardboardGoldPossibilities[randomGoldNumber].ToString();

                    DataManager.instance.AddGold(cardboardGoldPossibilities[randomGoldNumber]);
                    break;
                case 1:
                    GameObject containerInstance2 = Instantiate(rewardContainerPrefab, rewardContainersParent);
                    Image targetImage2 = containerInstance2.transform.GetChild(0).GetComponent<Image>();
                    targetImage2.sprite = rewardEnergyIcon;

                    containerInstance2.GetComponentInChildren<Image>().sprite = rewardEnergyIcon;
                    containerInstance2.GetComponentInChildren<TextMeshProUGUI>().text = cardboardEnergyPossibilities[randomEnergyNumber].ToString();

                    DataManager.instance.AddEnergy(cardboardEnergyPossibilities[randomEnergyNumber]);
                    break;
            }

            GameObject button = EventSystem.current.currentSelectedGameObject;
            button.SetActive(false);
       

    }
    public void CardboardBoxBuy()
    {
        if (DataManager.instance.TryPurchaseGold(100))
        {
            rewardContainersParent.Clear();

            int randomType = Random.Range(0, 2);
            int randomGoldNumber = Random.Range(0, cardboardGoldPossibilities.Length);
            int randomEnergyNumber = Random.Range(0, cardboardEnergyPossibilities.Length);
            UIManager.instance.TogglePanel(rewardPopUp);

            switch (randomType)
            {
                case 0:
                    GameObject containerInstance = Instantiate(rewardContainerPrefab, rewardContainersParent);
                    Image targetImage = containerInstance.transform.GetChild(0).GetComponent<Image>();
                    targetImage.sprite = rewardGoldIcon;
                    containerInstance.GetComponentInChildren<TextMeshProUGUI>().text = cardboardGoldPossibilities[randomGoldNumber].ToString();

                    DataManager.instance.AddGold(cardboardGoldPossibilities[randomGoldNumber]);
                    break;
                case 1:
                    GameObject containerInstance2 = Instantiate(rewardContainerPrefab, rewardContainersParent);
                    Image targetImage2 = containerInstance2.transform.GetChild(0).GetComponent<Image>();
                    targetImage2.sprite = rewardEnergyIcon;

                    containerInstance2.GetComponentInChildren<Image>().sprite = rewardEnergyIcon;
                    containerInstance2.GetComponentInChildren<TextMeshProUGUI>().text = cardboardEnergyPossibilities[randomEnergyNumber].ToString();

                    DataManager.instance.AddEnergy(cardboardEnergyPossibilities[randomEnergyNumber]);
                    break;
            }

            GameObject button = EventSystem.current.currentSelectedGameObject;
            button.SetActive(false);
        }




    }


    public void GoldBox()
    {
        Bridge.advertisement.ShowRewarded();

        rewardContainersParent.Clear();

            int randomType = Random.Range(0, 2);
            int randomGoldNumber = Random.Range(0, goldBoxGoldPossibilities.Length);
            int randomEnergyNumber = Random.Range(0, goldBoxEnergyPossibilities.Length);
            UIManager.instance.TogglePanel(rewardPopUp);

            switch (randomType)
            {
                case 0:
                    GameObject containerInstance = Instantiate(rewardContainerPrefab, rewardContainersParent);

                    Image targetImage = containerInstance.transform.GetChild(0).GetComponent<Image>();
                    targetImage.sprite = rewardGoldIcon;
                    containerInstance.GetComponentInChildren<TextMeshProUGUI>().text = goldBoxGoldPossibilities[randomGoldNumber].ToString();

                    DataManager.instance.AddGold(goldBoxGoldPossibilities[randomGoldNumber]);
                    break;
                case 1:
                    GameObject containerInstance2 = Instantiate(rewardContainerPrefab, rewardContainersParent);
                    Image targetImage2 = containerInstance2.transform.GetChild(0).GetComponent<Image>();
                    targetImage2.sprite = rewardEnergyIcon;
                    containerInstance2.GetComponentInChildren<TextMeshProUGUI>().text = goldBoxEnergyPossibilities[randomEnergyNumber].ToString();


                    DataManager.instance.AddEnergy(goldBoxEnergyPossibilities[randomEnergyNumber]);
                    break;
            }

            GameObject button = EventSystem.current.currentSelectedGameObject;
            button.SetActive(false);


    }
    public void GoldBoxBuy()
    {
        if (DataManager.instance.TryPurchaseGold(250))
        {
            rewardContainersParent.Clear();

            int randomType = Random.Range(0, 2);
            int randomGoldNumber = Random.Range(0, goldBoxGoldPossibilities.Length);
            int randomEnergyNumber = Random.Range(0, goldBoxEnergyPossibilities.Length);
            UIManager.instance.TogglePanel(rewardPopUp);

            switch (randomType)
            {
                case 0:
                    GameObject containerInstance = Instantiate(rewardContainerPrefab, rewardContainersParent);

                    Image targetImage = containerInstance.transform.GetChild(0).GetComponent<Image>();
                    targetImage.sprite = rewardGoldIcon;
                    containerInstance.GetComponentInChildren<TextMeshProUGUI>().text = goldBoxGoldPossibilities[randomGoldNumber].ToString();

                    DataManager.instance.AddGold(goldBoxGoldPossibilities[randomGoldNumber]);
                    break;
                case 1:
                    GameObject containerInstance2 = Instantiate(rewardContainerPrefab, rewardContainersParent);
                    Image targetImage2 = containerInstance2.transform.GetChild(0).GetComponent<Image>();
                    targetImage2.sprite = rewardEnergyIcon;
                    containerInstance2.GetComponentInChildren<TextMeshProUGUI>().text = goldBoxEnergyPossibilities[randomEnergyNumber].ToString();


                    DataManager.instance.AddEnergy(goldBoxEnergyPossibilities[randomEnergyNumber]);
                    break;
            }

            GameObject button = EventSystem.current.currentSelectedGameObject;
            button.SetActive(false);
        }





    }

    public void LegendaryBox()
    {
        Bridge.advertisement.ShowRewarded();

        rewardContainersParentShop.Clear();

            int randomType = Random.Range(0, 2);
            int randomGoldNumber = Random.Range(0, legendaryBoxGoldPossibilities.Length);
            int randomEnergyNumber = Random.Range(0, legendaryBoxEnergyPossibilities.Length);
            UIManager.instance.TogglePanel(rewardPopUpShop);

            switch (randomType)
            {
                case 0:
                    GameObject containerInstance = Instantiate(rewardContainerPrefab, rewardContainersParentShop);


                    Image targetImage = containerInstance.transform.GetChild(0).GetComponent<Image>();
                    targetImage.sprite = rewardGoldIcon;
                    containerInstance.GetComponentInChildren<TextMeshProUGUI>().text = legendaryBoxGoldPossibilities[randomGoldNumber].ToString();

                    DataManager.instance.AddGold(legendaryBoxGoldPossibilities[randomGoldNumber]);
                    break;
                case 1:
                    GameObject containerInstance2 = Instantiate(rewardContainerPrefab, rewardContainersParentShop);
                    Image targetImage2 = containerInstance2.transform.GetChild(0).GetComponent<Image>();
                    targetImage2.sprite = rewardEnergyIcon;
                    containerInstance2.GetComponentInChildren<TextMeshProUGUI>().text = legendaryBoxEnergyPossibilities[randomEnergyNumber].ToString();


                    DataManager.instance.AddEnergy(legendaryBoxEnergyPossibilities[randomEnergyNumber]);
                    break;
            }

            GameObject button = EventSystem.current.currentSelectedGameObject;
            button.SetActive(false);


    }

    //public void CardboardBoxPass()
    //{
    //    rewardContainersParent.Clear();

    //    int randomType = Random.Range(0, 2);
    //    int randomGoldNumber = Random.Range(0, cardboardGoldPossibilities.Length);
    //    int randomEnergyNumber = Random.Range(0, cardboardEnergyPossibilities.Length);
    //    MenuManager.instance.TogglePanel(rewardPopUp);

    //    switch (randomType)
    //    {
    //        case 0:
    //            UIRewardContainer containerInstance = Instantiate(rewardContainerPrefab, rewardContainersParent);
    //            containerInstance.Configure(rewardGoldIcon, cardboardGoldPossibilities[randomGoldNumber].ToString());
    //            DataManager.instance.AddGold(cardboardGoldPossibilities[randomGoldNumber]);
    //            break;
    //        case 1:
    //            UIRewardContainer containerInstance2 = Instantiate(rewardContainerPrefab, rewardContainersParent);
    //            containerInstance2.Configure(rewardEnergyIcon, cardboardEnergyPossibilities[randomEnergyNumber].ToString());
    //            DataManager.instance.AddEnergy(cardboardEnergyPossibilities[randomEnergyNumber]);
    //            break;
    //    }

    //}
    //public void GoldBoxPass()
    //{
    //    rewardContainersParent.Clear();

    //    int randomType = Random.Range(0, 2);
    //    int randomGoldNumber = Random.Range(0, goldBoxGoldPossibilities.Length);
    //    int randomEnergyNumber = Random.Range(0, goldBoxEnergyPossibilities.Length);
    //    MenuManager.instance.TogglePanel(rewardPopUp);

    //    switch (randomType)
    //    {
    //        case 0:
    //            UIRewardContainer containerInstance = Instantiate(rewardContainerPrefab, rewardContainersParent);
    //            containerInstance.Configure(rewardGoldIcon, goldBoxGoldPossibilities[randomGoldNumber].ToString());
    //            DataManager.instance.AddGold(goldBoxGoldPossibilities[randomGoldNumber]);
    //            break;
    //        case 1:
    //            UIRewardContainer containerInstance2 = Instantiate(rewardContainerPrefab, rewardContainersParent);
    //            containerInstance2.Configure(rewardEnergyIcon, goldBoxEnergyPossibilities[randomEnergyNumber].ToString());
    //            DataManager.instance.AddEnergy(goldBoxEnergyPossibilities[randomEnergyNumber]);
    //            break;
    //    }



    //}


}
