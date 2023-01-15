using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questID;
    public int questActionIndex;
    public int questPoint;

    public int QuestPoint
    {
        get => questPoint;
        set
        {
            questPoint = value;
            onRequirePoint?.Invoke(2000);
        }
    }

    Dictionary<int, QuestData> questList;

    public Action<float, int> onQuestClear;
    public Action<int> onRequireQuest;
    public Action<int> onRequirePoint;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
        onRequireQuest += (require) => require++;
        onQuestClear += QuestClear;
    }

    private void GenerateData()
    {
        questList.Add(10, new QuestData(new string[] { "First Quest" , "Talk Spirit"}, new int[] { 1000 , 2000}));
        questList.Add(20, new QuestData(new string[] { "Second Quest" , "Kill Dragon"}, new int[] { 0 }));
    }
    public void QuestClear(float exp, int gold)
    {
        GameManager.Inst.MainPlayer.Exp += exp;
        GameManager.Inst.MainPlayer.Gold += (uint)gold; 
    }
    public int GetQuestTalkIndex(int id)
    {
        return questID + questActionIndex;
    }
    public string CheckQuest(int id, int index = 0)
    {
        if(id == questList[questID].ID[questActionIndex])
            questActionIndex++;
        if (questActionIndex == questList[questID].ID.Length)
            NextQuest();
        return questList[questID].questText[index];
    }
    void NextQuest()
    {
        questID += 10;
        questActionIndex = 0;
    }
}
