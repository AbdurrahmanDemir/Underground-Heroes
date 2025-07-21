using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Playgama;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;   

    [SerializeField] private GameManager gameManager;   

    [Header("Elements")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject gameLosePanel;
    [SerializeField] private GameObject menuBar;
    [Header("Settings")]
    [SerializeField] private GameObject throwStartingButton;
    [SerializeField] private GameObject upgradeHookPanel;
    [Header("Level")]
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Transform heroParent;
    [SerializeField] private TowerController towerController;
    [Header("Game Win/Lose Panel Settings")]
    [SerializeField] private TextMeshProUGUI winArenaText;
    [SerializeField] private TextMeshProUGUI winGoldText;
    [SerializeField] private TextMeshProUGUI winBonusGoldText;
    [SerializeField] private TextMeshProUGUI winEnemyCountText;
    [SerializeField] private TextMeshProUGUI loseArenaText;
    [SerializeField] private TextMeshProUGUI loseGoldText;
    [SerializeField] private TextMeshProUGUI loseBonusGoldText;
    [SerializeField] private TextMeshProUGUI loseEnemyCountText;

    public static Action onNewArena;

    [SerializeField] private AudioSource mainTheme;
    [SerializeField] private AudioSource battleTheme;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Hook.onThrowStarting += StartingThrow;
        Hook.onThrowEnding += EndingThrow;

        TowerController.onGameLose += GameLosePanel;
        EnemyTowerController.onGameWin += GameWinPanel;
    }
    private void OnDestroy()
    {
        Hook.onThrowStarting -= StartingThrow;
        Hook.onThrowEnding -= EndingThrow;

        TowerController.onGameLose -= GameLosePanel;
        EnemyTowerController.onGameWin -= GameWinPanel;


    }

    private void Start()
    {
        GameUIStageChanged(UIGameStage.Menu);
        Bridge.platform.SendMessage(Playgama.Modules.Platform.PlatformMessage.GameReady);


    }
    void StartingThrow()
    {
        throwStartingButton.SetActive(false);
        upgradeHookPanel.SetActive(false);
    }

    void EndingThrow()
    {
        upgradeHookPanel.SetActive(true);
        throwStartingButton.SetActive(true);
    }

    public void StartWave()
    {
        upgradeHookPanel.SetActive(false);
        throwStartingButton.SetActive(true);
    }
    public void PlayButton()
    {
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            Bridge.platform.SendMessage(Playgama.Modules.Platform.PlatformMessage.GameplayStarted);


            mainTheme.Stop();
            battleTheme.Play();

            GameUIStageChanged(UIGameStage.Game);
            int waveIndex = PlayerPrefs.GetInt("WaveIndex", 0);
            GameObject arena = null;

            switch (waveIndex)
            {
                case 0:
                case 1:
                    arena = gameManager.GetArenaTileset(0);
                    break;
                case 2:
                case 3:
                    arena = gameManager.GetArenaTileset(1);
                    break;
                case 4:
                case 5:
                    arena = gameManager.GetArenaTileset(2);
                    break;
                case 6:
                case 7:
                    arena = gameManager.GetArenaTileset(3);
                    break;
                case 8:
                case 9:
                    arena = gameManager.GetArenaTileset(4);
                    break;
                case 10:
                    arena = gameManager.GetArenaTileset(0);
                    break;
                case 11:
                    arena = gameManager.GetArenaTileset(1);
                    break;
                case 12:
                    arena = gameManager.GetArenaTileset(2);
                    break;
                default:
                    arena = gameManager.GetArenaTileset(0);
                    break;
            }

            if (arena != null)
                arena.SetActive(true);

            WaveManager.instance.StartWaves(waveIndex);
        }
        else
        {
            if (DataManager.instance.TryPurchaseEnergy(1))
            {
                Bridge.platform.SendMessage(Playgama.Modules.Platform.PlatformMessage.GameplayStarted);

                mainTheme.Stop();
                    battleTheme.Play();
                    GameUIStageChanged(UIGameStage.Game);
                    int waveIndex = PlayerPrefs.GetInt("WaveIndex", 0);
                    GameObject arena = null;

                    switch (waveIndex)
                    {
                        case 0:
                        case 1:
                            arena = gameManager.GetArenaTileset(0);
                            break;
                        case 2:
                        case 3:
                            arena = gameManager.GetArenaTileset(1);
                            break;
                        case 4:
                        case 5:
                            arena = gameManager.GetArenaTileset(2);
                            break;
                        case 6:
                        case 7:
                            arena = gameManager.GetArenaTileset(3);
                            break;
                        case 8:
                        case 9:
                            arena = gameManager.GetArenaTileset(4);
                            break;
                        case 10:
                            arena = gameManager.GetArenaTileset(0);
                            break;
                        case 11:
                            arena = gameManager.GetArenaTileset(1);
                            break;
                        case 12:
                            arena = gameManager.GetArenaTileset(2);
                            break;
                        default:
                            arena = gameManager.GetArenaTileset(0);
                            break;
                    }

                    if (arena != null)
                        arena.SetActive(true);

                    WaveManager.instance.StartWaves(waveIndex);
                
            }
            
        }
        

            

    }

    public void GameLosePanel()
    {
        GameUIStageChanged(UIGameStage.GameLose);

        gameLosePanel.transform.localScale = Vector3.zero;
        gameLosePanel.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);

        int waveIndex = PlayerPrefs.GetInt("WaveIndex", 0);
        loseArenaText.text = (waveIndex - 1).ToString();

        int enemyCount = GameManager.enemyCount;
        int rewardedGold = enemyCount * 5;

        loseEnemyCountText.text = "";
        loseBonusGoldText.text = "";
        loseGoldText.text = "";

        DOTween.To(() => 0, x => loseEnemyCountText.text = "Number of enemies killed: " + x.ToString(), enemyCount, 1f);
        DOTween.To(() => 0, x => loseBonusGoldText.text = x.ToString(), rewardedGold, 1f).SetDelay(0.5f);
        DOTween.To(() => 0, x => loseGoldText.text = x.ToString(), 0, 0.5f).SetDelay(1f);

        Bridge.platform.SendMessage(Playgama.Modules.Platform.PlatformMessage.GameplayStopped);
            DataManager.instance.AddGold(rewardedGold);
    }

    public void GameLoseButton()
    {
        Bridge.advertisement.ShowInterstitial();


        GameUIStageChanged(UIGameStage.Menu);

            towerController.ResetTower();
            gameManager.PowerUpReset();
            SceneManager.LoadScene(0);

    }

    public void GameWinPanel()
    {
        GameUIStageChanged(UIGameStage.GameWin);
        onNewArena?.Invoke();
        gameWinPanel.transform.localScale = Vector3.zero;
        gameWinPanel.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack); 

        int waveIndex = PlayerPrefs.GetInt("WaveIndex", 0);
        winArenaText.text = waveIndex.ToString();

        int enemyCount = GameManager.enemyCount;
        int rewardedGold = enemyCount * 5;
        int baseGold = gameManager.arenaWinReward[waveIndex];
        int totalGold = rewardedGold + baseGold;

        winEnemyCountText.text = "";
        winBonusGoldText.text = "";
        winGoldText.text = "";

        DOTween.To(() => 0, x => winEnemyCountText.text = "Number of enemies killed: " + x.ToString(), enemyCount, 1f);
        DOTween.To(() => 0, x => winBonusGoldText.text = x.ToString(), rewardedGold, 1f).SetDelay(0.5f);
        DOTween.To(() => 0, x => winGoldText.text = x.ToString(), baseGold, 1f).SetDelay(1f);

        Bridge.platform.SendMessage(Playgama.Modules.Platform.PlatformMessage.GameplayStopped);


        DataManager.instance.AddGold(totalGold);
    }

    public void GameWinButton()
    {
        Bridge.advertisement.ShowInterstitial();


        towerController.ResetTower();
            gameManager.PowerUpReset();
            SceneManager.LoadScene(0);

    }
    public void GameUIStageChanged(UIGameStage stage)
    {
        switch (stage)
        {
            case UIGameStage.Menu:
                menuPanel.SetActive(true);
                gamePanel.SetActive(false);
                gameWinPanel.SetActive(false);
                gameLosePanel.SetActive(false);
                menuBar.SetActive(true);
                break;
            case UIGameStage.Game:
                menuPanel.SetActive(false);
                gamePanel.SetActive(true);
                gameWinPanel.SetActive(false);
                gameLosePanel.SetActive(false);
                menuBar.SetActive(false);

                break;
            case UIGameStage.GameWin:
                menuPanel.SetActive(false);
                gamePanel.SetActive(false);
                gameWinPanel.SetActive(true);
                gameLosePanel.SetActive(false);


                break;
            case UIGameStage.GameLose:
                menuPanel.SetActive(false);
                gamePanel.SetActive(false);
                gameWinPanel.SetActive(false);
                gameLosePanel.SetActive(true);
                break;

            default:
                break;
        }

    }
    public void TogglePanel(GameObject panel)
    {
        if (panel.activeSelf)
        {
            panel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => panel.SetActive(false));
        }
        else
        {
            panel.SetActive(true);
            panel.transform.localScale = Vector3.zero;
            panel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
    }


}
public enum UIGameStage
{
    Menu,
    Game,
    GameWin,
    GameLose
}
