using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastData
{
    public SkillData skillData;

    private float currentCoolTime = 3.0f;
    private float currentDuration = 0.0f;

    public Action<float, float> onCoolTimeChange;
    public Action<float, float> onDurationTimeChange;
    public Action<bool> onDurationMode;

    public float CurrentCoolTime
    {
        get => currentCoolTime;
        set
        {
            currentCoolTime = value;
            onCoolTimeChange?.Invoke(currentCoolTime, skillData.skillCoolTime);
        }
    } 
    public float CurrentDuration
    {
        get => currentDuration;
        set
        {
            currentDuration = value;
            onDurationTimeChange?.Invoke(skillData.skillDuration - currentDuration, skillData.skillDuration);
        }
    }
    public bool IsCastingSkill { get => currentCoolTime <= 0.0f; }

    public CastData(SkillData skill, float startDelay = 0.0f)
    {
        this.skillData = skill;
        this.currentCoolTime = startDelay;
    }
    public void ResetCoolTime()
    {
        onDurationMode?.Invoke(false);
        CurrentCoolTime = skillData.skillCoolTime;
    }
    public void CastDuration()
    {
        GameManager.Inst.MainPlayer.lightningEffect.SetActive(true);
        CurrentDuration = 0.0f;
        onDurationMode?.Invoke(true);
    }
}
