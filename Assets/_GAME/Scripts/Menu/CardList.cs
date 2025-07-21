using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CardList : MonoBehaviour
{
    [Header("Hero Panel")]
    [SerializeField] private MenuHeroCardSO[] heroes;
    [SerializeField] private GameObject heroCardPrefab;
    [SerializeField] private Transform heroTransform;
    [SerializeField] private GameObject heroCardDetailsPrefabs;
    [SerializeField] private Transform heroDetailsTransform;

    public static Action onCardUpgrade;

    private void Start()
    {
        StartCoroutine(LoadAllHeroesThenSetupUI());
    }

    private IEnumerator LoadAllHeroesThenSetupUI()
    {
        int loadedCount = 0;

        for (int i = 0; i < heroes.Length; i++)
        {
            int index = i;
            heroes[i].LoadData(() =>
            {
                loadedCount++;
            });
        }

        // Tüm kartlar yüklenene kadar bekle
        while (loadedCount < heroes.Length)
        {
            yield return null;
        }

        Debug.Log("Tüm kahraman verileri yüklendi!");
        HeroPanelUpdate();
    }

    public void HeroPanelUpdate()
    {
        // Önce varsa eski kartlarý temizle
        foreach (Transform child in heroTransform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < heroes.Length; i++)
        {
            GameObject cardObj = Instantiate(heroCardPrefab, heroTransform);
            MenuHeroListCard heroScript = cardObj.GetComponent<MenuHeroListCard>();

            heroScript.Config(
                heroes[i].cardName,
                heroes[i].heroIcon,
                heroes[i].cardType.ToString());

            heroScript.cardIndex = i;

            Button cardButton = heroScript.detailsButton;
            int capturedIndex = i;
            cardButton.onClick.AddListener(() => CardDetailsPanel(capturedIndex));
        }
    }

    public void CardDetailsPanel(int index)
    {
        // Önce eski detay panel temizle
        foreach (Transform child in heroDetailsTransform)
        {
            Destroy(child.gameObject);
        }

        GameObject cardDetails = Instantiate(heroCardDetailsPrefabs, heroDetailsTransform);
        MenuHeroDetails cardScript = cardDetails.GetComponent<MenuHeroDetails>();

        DOTween.Kill(cardDetails.transform);
        cardDetails.transform.localScale = Vector3.zero;
        cardDetails.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);

        var hero = heroes[index];

        cardScript.Config(
            hero.cardName,
            hero.heroIcon,
            hero.cardType.ToString(),
            hero.cardDescription,
            hero.GetCurrentDamage(),
            hero.GetCurrentHealth(),
            hero.range,
            hero.cooldown,
            hero.moveSpeed,
            hero.GetUpgradeCost(),
            hero.specialStatName,
            hero.specialStat
        );

        Button upgradeButton = cardScript.GetUpgradeButton();
        upgradeButton.onClick.RemoveAllListeners(); // Önemli: Önce listener temizle
        upgradeButton.onClick.AddListener(() =>
        {
            hero.UpgradeHero();

            // UI güncelle (detay ve liste)
            CardDetailsPanel(index);
            HeroPanelUpdate();

            onCardUpgrade?.Invoke();
        });
    }
}
