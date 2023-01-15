using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour, IPointerClickHandler
{
    uint id;

    public SkillSlotUI slotUI;

    protected SkillSlot skillSlot;

    SkillWindowUI skillWindowUI;

    protected Image skillImage;

    protected TextMeshProUGUI costText;

    protected UIPositioning[] slotArrows;

    protected ActionUI actionUI;

    ActionSkillSlotUI[] actions;

    public uint ID { get => id; }

    public SkillSlot SkillSlot { get => skillSlot; }
  

    protected virtual void Awake()
    {
        skillImage = transform.GetChild(0).GetComponent<Image>();
        costText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        slotArrows = FindObjectsOfType<UIPositioning>();
        actionUI = FindObjectOfType<ActionUI>();
        actions = FindObjectsOfType<ActionSkillSlotUI>();
        Array.Sort(actions, (a, b) =>
        {
            return a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex());
        });
        foreach (var act in actions)
        {
            act.onSkillSet += (UI) => act.slotUI = UI;
        }
    }


    public void Initialize(uint newID, SkillSlot targetSlot)
    {
        skillWindowUI = GameManager.Inst.WindowUI;
        if (slotArrows[0].isActiveAndEnabled)
        {
            SetArrow(false);
        }

        id = newID;
        skillSlot = targetSlot;
        skillSlot.onSlotChange = Refresh; 
    }


    public virtual void Refresh()
    {
        if (skillSlot.SkillData != null)
        {
            skillImage.sprite = skillSlot.SkillData.skillIcon;  
            skillImage.color = Color.white; 
            costText.text = skillSlot.CastCost.ToString();
        }
        else
        {
            skillImage.sprite = null;
            skillImage.color = Color.clear;
            costText.text = "";
        }
    }

    protected void SetArrow(bool active)
    {
        for (int i = 0; i < slotArrows.Length; i++)
        {
            slotArrows[i].gameObject.SetActive(active);
        }
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!SkillSlot.IsEmpty())
            {
                GameObject sendObject = eventData.pointerCurrentRaycast.gameObject;
                SkillSlotUI slotUI = sendObject.GetComponent<SkillSlotUI>();
                actions[(int)slotUI.skillSlot.SkillData.id].onSkillSet?.Invoke(slotUI);
                if (slotUI != null)
                {
                    SetArrow(true);
                    GameManager.Inst.IsSkillSet = true; 
                }
            }
        }
    }
}
