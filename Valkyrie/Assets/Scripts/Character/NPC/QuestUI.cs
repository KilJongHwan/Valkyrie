using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public QuestText[] qTexts;
    private void Awake()
    {
        qTexts = GetComponentsInChildren<QuestText>();
    }
    private void Start()
    {
        foreach (var text in qTexts)
        {
            text.gameObject.SetActive(false);
        }
        if (GameManager.Inst.IsWarp)
        {
            qTexts[1].Upload(GameManager.Inst.Quest.CheckQuest(2000, 0), GameManager.Inst.Quest.CheckQuest(2000, 1) + $" {GameManager.Inst.Quest.QuestPoint} / 1");
            qTexts[1].gameObject.SetActive(true);
        }
    }
}
