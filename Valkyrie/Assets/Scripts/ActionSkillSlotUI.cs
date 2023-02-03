using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class ActionSkillSlotUI : SkillSlotUI
{
    public uint index = 0;

    public Action<SkillSlotUI> onSkillSet;

    protected Image coolTimeImage;
    protected TextMeshProUGUI coolTimeText;

    Player player;
    protected override void Awake()
    {
        skillImage = transform.GetChild(0).GetComponent<Image>();
        coolTimeImage = transform.GetChild(1).GetComponent<Image>();
        coolTimeText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        slotArrows = FindObjectsOfType<UIPositioning>();
        actionUI = FindObjectOfType<ActionUI>();
    }
    private void Start()
    {
        player = GameManager.Inst.MainPlayer;
    }
    public override void Refresh()
    {
        if (skillSlot.SkillData != null)
        {
            skillImage.sprite = skillSlot.SkillData.skillIcon;
            skillImage.color = Color.white;
        }
        else
        {
            skillImage.sprite = null;
            skillImage.color = Color.clear;
        }
    }
    public virtual void RefreshUI(float current, float max)
    {
        if (!SkillSlot.IsEmpty())
        {
            if (current < 0)
            {
                current = 0;
            }
            coolTimeText.text = $"{current:f1}";
            coolTimeImage.fillAmount = current / max;
        }
        else
            coolTimeText.text = $"";
    }
    public void SetDurationMode(bool duration)
    {
        if (duration)
        {
            coolTimeImage.color = Color.cyan;
        }
        else
        {
            coolTimeImage.color = Color.black;
        }
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Inst.IsSkillSet)
        {
            if (slotUI != null)
            {
                uint id = slotUI.SkillSlot.SkillData.id;
                if (id >= 2)
                {
                    player.castDatas[id].onCoolTimeChange -= actionUI[(int)index].RefreshUI;
                    player.castDatas[id].onDurationTimeChange -= actionUI[(int)index].RefreshUI;
                    player.castDatas[id].onDurationMode -= actionUI[(int)index].SetDurationMode;
                    player.castDatas[id].onCoolTimeChange += actionUI[(int)index].RefreshUI;
                    player.castDatas[id].onDurationTimeChange += actionUI[(int)index].RefreshUI;
                    player.castDatas[id].onDurationMode += actionUI[(int)index].SetDurationMode;
                }
                else
                {
                    player.castDatas[id].onCoolTimeChange -= actionUI[(int)index].RefreshUI;
                    player.castDatas[id].onCoolTimeChange += actionUI[(int)index].RefreshUI;
                }
                if (!actionUI.window.AddSkill(slotUI.SkillSlot.SkillData, index))
                {
                    actionUI.window.ClearSkill(index);
                    actionUI.window.AddSkill(slotUI.SkillSlot.SkillData, index);
                }
                SetArrow(false);
                GameManager.Inst.IsSkillSet = false;
            }
        }
        else
        {
            if (!SkillSlot.IsEmpty() && !player.lightningEffect.activeSelf)
            {
                player.Casting((SkillCode)slotUI.SkillSlot.SkillData.id);
                if (player.isSkillReady)
                {
                    SkillSlot.CastSpell(player.gameObject);
                    player.isSkillReady = false;
                }
            }
        }
    }
}
