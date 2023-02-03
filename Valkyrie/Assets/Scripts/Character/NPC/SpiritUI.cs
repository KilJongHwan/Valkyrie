using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpiritUI : MonoBehaviour, IPointerClickHandler
{
    int index = 0;
    int talkIndex;

    CommunicationManager talk;
    QuestManager quest;
    QuestUI questWindow;
    CanvasGroup canvas;
    TextEffect talkText;
    ShopUI shop;

    bool isTalk;

    Action<int> onStartTalk;

    private void Awake()
    {
        questWindow = FindObjectOfType<QuestUI>();
        canvas = GetComponent<CanvasGroup>();
        talkText = GetComponentInChildren<TextEffect>();
    }
    private void Start()
    {
        talk = GameManager.Inst.TalkData;
        quest = GameManager.Inst.Quest;
        shop = GameManager.Inst.ShopUI;
        onStartTalk += Talk;
        quest.onRequirePoint += QuestNotice;
    }
    public void SpiritUIOnOff()
    {
        if (canvas.blocksRaycasts)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
    void Open()
    {
        canvas.alpha = 1;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
        onStartTalk?.Invoke(GameManager.Inst.MainPlayer.spirit.ID);
    }

    void Close()
    {
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }
    void Talk(int id)
    {
        int questTalkIndex = quest.GetQuestTalkIndex(id);
        string talkData = talk.GetCommunication(id + questTalkIndex ,talkIndex);
        if (talkData == null)
        {
            isTalk = false;
            talkIndex = 0;
            Close();
            Debug.Log(quest.CheckQuest(id));
            QuestNotice(id);
            if (id == 1000)
            {
                shop.OnOff();
            }
            return;
        }
        talkText.SetMsg(talkData);
        isTalk = true;
        talkIndex++;
    }

    public void QuestNotice(int id)
    {
        switch (quest.questID)
        {
            case 10:
                if (index == 0)
                {
                    questWindow.qTexts[index].Upload(quest.CheckQuest(id, 0), quest.CheckQuest(id, 1));
                    questWindow.qTexts[index].gameObject.SetActive(true);
                }
                break;
            case 20:
                if (index == 0)
                {
                    quest.onQuestClear?.Invoke(550.0f, 5000);
                    questWindow.qTexts[index].gameObject.SetActive(false);
                }
                index = 1;
                questWindow.qTexts[index].Upload(quest.CheckQuest(id, 0), quest.CheckQuest(id, 1) + $" {quest.QuestPoint} / 1");
                break;
            default:
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Talk(GameManager.Inst.MainPlayer.spirit.ID);
    }
}