using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot
{
    SkillData skillData;

    uint castCost = 0;

    public SkillData SkillData
    {
        get => skillData;
        private set
        {
            if (skillData != value)
            {
                skillData = value;
                onSlotChange?.Invoke();
            }
        }
    }

    public uint CastCost
    {
        get => castCost;
        private set
        {
            castCost = value;
            onSlotChange?.Invoke();
        }
    }
    public System.Action onSlotSkillCast;
    public System.Action onSlotChange;
    public SkillSlot() { }
    public SkillSlot(SkillData data, uint cost)
    {
        skillData = data;
        castCost = cost;
    }
    public SkillSlot(SkillSlot other)
    {
        skillData = other.SkillData;
        castCost = other.CastCost;
    }

    public uint IncreaseSlotSkill(uint count = 1)
    {
        uint newCount = CastCost + count;
        int overCount = (int)newCount - (int)skillData.maxStackCount;    // ��ģ ���� ���
        if (overCount > 0)
        {
            // ���ƴ�.
            CastCost = skillData.maxStackCount;
        }
        else
        {
            // ����� �߰� �����ϴ�.
            CastCost = newCount;
            overCount = 0;
        }
        return (uint)overCount; // ��ģ ���� �����ֱ�
    }
    public void AssignSlotSkill(SkillData skill, uint cost = 1)
    {
        CastCost = cost;
        SkillData = skill;
    }
    public void ClearSkillSlot()
    {
        SkillData = null;
        castCost = 0;
    }
    public void CastSpell(GameObject target = null)
    {
        ICastable castable = SkillData as ICastable;
        if (castable != null)
        {
            castable.Cast(target);
        }
    }
    public bool IsEmpty()
    {
        return skillData == null;
    }
}
