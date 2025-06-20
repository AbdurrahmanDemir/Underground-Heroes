using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class Hook : MonoBehaviour
{
    public Transform hookedTransform;
    [SerializeField] private HookManager hookManager;

    private Camera mainCamera;
    private Collider2D coll;

    private int length;
    private int strength;
    private int heroCount;

    private bool canMove = true;

    public List<HookedHero> hookedHero;

    private Tweener cameraTween;

    public static Action onThrowStarting;
    public static Action onThrowEnding;

    public static int throwCount = 0;
    [SerializeField] private TextMeshProUGUI throwPriceText;



    private IEnumerator Start()
    {
        yield return new WaitUntil(() => Camera.main != null
                                         && Camera.main.pixelWidth > 0
                                         && Camera.main.pixelHeight > 0
                                         && Camera.main.rect.width > 0);

        mainCamera = Camera.main;

        coll = GetComponent<Collider2D>();
        hookedHero = new List<HookedHero>();
        throwPriceText.text = (throwCount * 10).ToString();

        TowerController.onGameLose += ResetThrowCount;
        EnemyTowerController.onGameWin += ResetThrowCount;
    }


    private void OnDestroy()
    {
        TowerController.onGameLose -= ResetThrowCount;
        EnemyTowerController.onGameWin -= ResetThrowCount;
    }

    private void Update()
    {
        if (mainCamera == null || !canMove) return;

        

        if (Input.GetMouseButton(0))
        {
            Vector3 camPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            camPos.z = 0;
            Vector3 hookPosition = transform.position;
            hookPosition.x = camPos.x;
            transform.position = hookPosition;
        }
    }

    void ResetThrowCount()
    {
        throwCount = 0;
    }

    public void StartThrow()
    {
        if (hookManager.TryPurchaseToken(throwCount * 10))
        {
            throwCount++;
            throwPriceText.text = (throwCount * 10).ToString();
            length = HookManager.instance.hookLength - 20;
            strength = HookManager.instance.hookStrength;
            heroCount = 0;
            float time = (-length) * 0.1f;

            cameraTween = mainCamera.transform.DOMoveY(length, 2 * time * 0.25f, false).OnUpdate(() =>
            {
                if (mainCamera.transform.position.y <= -10)
                {
                    transform.SetParent(mainCamera.transform);
                }
            }).OnComplete(() =>
            {
                coll.enabled = true;
                cameraTween = mainCamera.transform.DOMoveY(0, time * 6, false).OnUpdate(() =>
                {
                    if (mainCamera.transform.position.y >= -25f)
                        StopThrow();
                });
            });

            coll.enabled = false;
            canMove = true;
            hookedHero.Clear();

            onThrowStarting?.Invoke();
        }
    }

    public void StopThrow()
    {
        canMove = false;
        cameraTween?.Kill(false);
        cameraTween = mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(() =>
        {
            if (mainCamera.transform.position.y >= -11)
            {
                transform.SetParent(null);
            }
        }).OnComplete(() =>
        {
            transform.position = Vector2.down * 10;
            coll.enabled = true;
            int num = 0;
            for (int i = 0; i < hookedHero.Count; i++)
            {
                hookedHero[i].transform.SetParent(null);
                hookedHero[i].ResetHero();
                num += hookedHero[i].Type.price;
            }

            hookManager.AddToken(num);
            Debug.Log(num);
            onThrowEnding?.Invoke();
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HookedHero") && heroCount != strength)
        {
            heroCount++;
            HookedHero component = collision.GetComponent<HookedHero>();
            component.Hooked();
            hookedHero.Add(component);
            collision.transform.SetParent(transform);
            collision.transform.position = hookedTransform.position;
            collision.transform.rotation = hookedTransform.rotation;

            collision.transform.DOShakeRotation(5, Vector3.forward * 45, 10, 90, false)
                .SetLoops(1, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    collision.transform.rotation = Quaternion.identity;
                });

            if (heroCount == strength)
                StopThrow();
        }
    }
}
