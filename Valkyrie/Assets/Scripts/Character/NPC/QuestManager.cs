using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public int questID;
    public int questActionIndex;
    public int questPoint;

    float fadeTime = 0.05f;
    TextMeshProUGUI questClear;
    CanvasGroup canvas;

    public int QuestPoint
    {
        get => questPoint;
        set
        {
            questPoint = value;
            onRequirePoint?.Invoke(2000);
            onQuestClear(500.0f, 50000);
        }
    }

    Dictionary<int, QuestData> questList;

    public Action<float, int> onQuestClear;
    public Action<int> onRequirePoint;

    private void Awake()
    {
        questClear = FindObjectOfType<TextMeshProUGUI>();
        canvas = questClear.GetComponent<CanvasGroup>();
        questList = new Dictionary<int, QuestData>();
        GenerateData();
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
        StartCoroutine(QuestClearMotion());
    }
    IEnumerator QuestClearMotion()
    {
        if (GameManager.Inst.IsWarp)
        {
            questClear.text = "GameClear";
        }
        while (canvas.alpha < 1.0f)
        {
            canvas.alpha += fadeTime;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.0f);
        while (canvas.alpha > 0.0f)
        {
            canvas.alpha -= fadeTime;
            yield return new WaitForSeconds(0.1f);
        }
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
