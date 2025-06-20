using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Quest[] quests;
    private Dictionary<int, Quest> uncompletedQuestDictionnary = new Dictionary<int, Quest>();

    [Header("Elements")]
    [SerializeField] private QuestContainer QuestContainerPrefab;
    [SerializeField] private Transform questContainerParent;



    private void Awake()
    {
        QuestContainer.onRewardClaimed += QuestRewardClaimedCallback;
    }

    private void OnDestroy()
    {
        QuestContainer.onRewardClaimed -= QuestRewardClaimedCallback;
    }

    private void Start()
    {
        CreatQuestContainers();
    }
    private void QuestRewardClaimedCallback(int questIndex)
    {
        SetQuestComplete(questIndex);

        int reward = quests[questIndex].reward;
        DataManager.instance.AddGold(reward);
        UpdateQuest();


    }

    private void UpdateQuest()
    {

        foreach (Transform child in questContainerParent)
        {
            Destroy(child.gameObject);
        }

        CreatQuestContainers(); 
    }
    public void CreatQuestContainers() 
    {
        StoreUncompletedMissions();

        foreach (KeyValuePair<int, Quest> questData in uncompletedQuestDictionnary)
        {
            CreatQuestContainer(questData);
        }
    }

    private void StoreUncompletedMissions() 
    {
        uncompletedQuestDictionnary.Clear(); 

        for (int i = 0; i < quests.Length; i++)
        {
            if (IsQuestComplete(i))
                continue;

            Quest quest = quests[i];
            quest.progress = GetQuestProgress(new KeyValuePair<int, Quest>(i, quest));

            uncompletedQuestDictionnary.Add(i, quest);

            if (uncompletedQuestDictionnary.Count >= 3)
                break;
        }
    }

    public void CreatQuestContainer(KeyValuePair<int, Quest> questData) 
    {
        QuestContainer QuestContainerInstance = Instantiate(QuestContainerPrefab, questContainerParent);


        string title = GetQuestTitle(questData.Value);
        string rewardString = questData.Value.reward.ToString();
        float progress = GetQuestProgress(questData);

        QuestContainerInstance.Configure(title, rewardString, progress, questData.Key);


        Debug.Log("KEY" + QuestContainerInstance.GetKey());
    }


    private string GetQuestTitle(Quest quest) 
    {
        switch (quest.Type)
        {
            case QuestType.Kill:
                return "Kill " + quest.target.ToString() + " enemies";

            case QuestType.ArenaLevel:
                return "Complete " + quest.target.ToString() + " arena";

            case QuestType.AdsWatch:
                return "Watch " + quest.target.ToString() + " ads";

            case QuestType.TowerUpgrade:
                return "Upgrade tower " + quest.target.ToString() + " time";
            case QuestType.UpgradeCard:
                return "Upgrade heroes " + quest.target.ToString() + " time";

            default:
                return "Blank";
        }
    }

    public void UpdateQuestProgress(int questIndex, float newProgress)
    {
        Debug.Log("New Progress : " + newProgress);

        SaveQuestProgress(questIndex, newProgress);

        Quest quest = quests[questIndex];
        quest.progress = newProgress;
        quests[questIndex] = quest;

        uncompletedQuestDictionnary[questIndex] = quest;

        if (questContainerParent != null)
        {
            for (int i = 0; i < questContainerParent.childCount; i++)
            {
                QuestContainer questContainer = questContainerParent.GetChild(i).GetComponent<QuestContainer>();

                if (questContainer.GetKey() != questIndex)
                    continue;


                questContainer.UpdateProgress(newProgress);
            }
        }

    }

    public Dictionary<int, Quest> GetCurrentQuest() 
    {
        return uncompletedQuestDictionnary;
    }
    private float GetQuestProgress(KeyValuePair<int, Quest> questData)
    {
        return PlayerPrefs.GetFloat("QuestProgress" + questData.Key);
    }

    private void SaveQuestProgress(int key, float progress) 
    {
        PlayerPrefs.SetFloat("QuestProgress" + key, progress);

    }

    private void SetQuestComplete(int questIndex) 
    {
        PlayerPrefs.SetInt("Quest" + questIndex, 1);
    }

    private bool IsQuestComplete(int questIndex) 
    {
        return PlayerPrefs.GetInt("Quest" + questIndex) == 1;

    }
}
public enum QuestType { Kill, ArenaLevel, TowerUpgrade, AdsWatch, UpgradeCard } 

[System.Serializable]
public struct Quest
{
    public QuestType Type;
    public int target;
    public int reward;
    public float progress;
}