using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class Passage : MonoBehaviour
{
    public UpgradeSelectManager upgradeSelectManager;
    [Header("Settings")]
    [SerializeField] private PassageType passageType;
    public string passageName;
    public int amount;
    public SpriteRenderer passageRenderer;
    public Color passageColor;
    public Sprite passageSprite;

    bool apply = false;
    bool throwEnding = true;
    [Header("UI")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;
    public Image icon;
    private void Start()
    {
        nameText.text = passageName.ToString();
        amountText.text = amount.ToString();
        passageRenderer.color = passageColor;
        icon.sprite = passageSprite;
    }
    private void Awake()
    {
        Hook.onThrowEnding += Boost;
        Hook.onThrowStarting += Boost;
    }
    private void OnDestroy()
    {
        Hook.onThrowEnding -= Boost;
        Hook.onThrowStarting -= Boost;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        apply = true;
    }
    void Boost()
    {
        if (throwEnding)
            throwEnding = false;
        else
            throwEnding = true;

        if (apply && throwEnding)
        {
            ApplyBoost();
        }
    }
    
    protected abstract void ApplyBoost();

}
public enum PassageType
{
    TowerHealth = 0,
    HookLenght = 1,
    HookStrenght = 2,
    EpicHeroCount = 3,
    LegendaryHeroCount = 4,
    DamageUpgrade = 5,
    HealthUpgrade = 6,
    TokenAdd = 7
}