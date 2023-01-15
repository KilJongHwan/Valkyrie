using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWindow
{
    SkillSlot[] skillSlots = null;
    SkillSlot tempSlot = null;

    public const int Default_Skill_Size = 3;
    public const uint TempSlotID = 99999;

    public int SlotSize => skillSlots.Length;

    public SkillSlot TempSlot => tempSlot;


    public SkillSlot this[int index] => skillSlots [index];
    

    public SkillWindow(int size = Default_Skill_Size)
    {
        skillSlots = new SkillSlot[size];
        for (int i = 0; i < size; i++)
        {
            skillSlots[i] = new SkillSlot();
        }
        tempSlot = new SkillSlot();
    }
    public bool AddSkill(SkillCode code)
    {
        return AddSkill(GameManager.Inst.SkillData[code]);
    }
    public bool AddSkill(SkillData data)
    {
        bool result = false;
        Debug.Log($"Skill {data.skillName} Add");

        SkillSlot target = FindSameSkill(data);
        if (target != null)
        {
            ///target.AssignSlotSkill(data, target.CastCost);
            target.IncreaseSlotSkill();
            result = true;
            Debug.Log($"{data}를 하나 증가 시킵니다.");
        }
        else
        {
            SkillSlot empty = FindEmptySlot();
            if (empty != null)
             {
                empty.AssignSlotSkill(data);
                result = true;
                Debug.Log($"추가에 성공했습니다.");
            }
            else
            {
                Debug.Log($"실패.");
            }
        }

        return result;
    }
   
    public bool AddSkill(uint id, uint index)
    {
        return AddSkill(GameManager.Inst.SkillData[id], index);
    }
    public bool AddSkill(SkillCode code, uint index)
    {
        return AddSkill(GameManager.Inst.SkillData[code], index);
    }
    public bool AddSkill(SkillData data, uint index)
    {
        bool result = false;
        SkillSlot slot = skillSlots[index];

        if (slot.IsEmpty())
        {
            slot.AssignSlotSkill(data);
            result = true;
        }
        else
        {
            if (slot.SkillData == data)
            {
                if (slot.IncreaseSlotSkill() == 0)
                {
                    result = true;
                    Debug.Log($"스킬추가");
                }
            }
        }

        return result;
    }
    public void MoveSkill(uint from , uint to)
    {
        if (IsValidAndNotEmptySlot(from) && IsValidSlotIndex(to))
        {
            SkillSlot fromSlot = null;
            SkillSlot toSlot = null;
            if (from == TempSlotID)
            {
                fromSlot = TempSlot;
            }
            else
            {
                fromSlot = skillSlots[from]; 
            }
            if (to == TempSlotID)
            {
                toSlot = TempSlot;
            }
            else
            {
                toSlot = skillSlots[from];
            }
            if (fromSlot.SkillData != toSlot.SkillData)
            {
                SkillData tempSlotData = toSlot.SkillData;
                uint tempCost = toSlot.CastCost;
                toSlot.AssignSlotSkill(fromSlot.SkillData, fromSlot.CastCost);
                fromSlot.AssignSlotSkill(tempSlotData, tempCost);
            }
        }
    }
    public bool ClearSkill(uint slotIndex)
    {
        bool result = false;

        if (IsValidSlotIndex(slotIndex))     
        {
            SkillSlot slot = skillSlots[slotIndex];
            slot.ClearSkillSlot();               
            result = true;
        }
        else
        {
            Debug.Log($"실패 : 잘못된 인덱스입니다.");
        }

        return result;
    }
        public void PrintInventory()
    {
        string printText = "[";
        for (int i = 0; i < SlotSize; i++)
        {
            if (skillSlots[i].SkillData != null)
            {
                printText += $" {skillSlots[i].SkillData.skillName}({skillSlots[i].CastCost})";
            }
            else
            {
                printText += "(빈칸)";
            }
            printText += ",";
        }
        SkillSlot slot = skillSlots[SlotSize - 1];   // 마지막 슬롯만 따로 처리
        if (!slot.IsEmpty())
        {
            printText += $"{slot.SkillData.skillName}({slot.CastCost})]";
        }
        else
        {
            printText += "]";
        }
        Debug.Log(printText);
    }
    private SkillSlot FindEmptySlot()
    {
        SkillSlot result = null;

        foreach (var slot in skillSlots)  // slots를 전부 순회하면서
        {
            if (slot.IsEmpty())     // 빈 슬롯인지 확인
            {
                result = slot;      // 빈 슬롯이면 foreach break하고 리턴
                break;
            }
        }

        return result;
    }
  
    private SkillSlot FindSameSkill(SkillData skillData)
    {
        SkillSlot slot = null;
        for (int i = 0; i < SlotSize; i++)
        {
            // 같은 종류의 아이템이 있고 슬롯에 아이템이 들어갈 여유가 있음
            if (skillSlots[i].SkillData == skillData && skillSlots[i].CastCost < skillSlots[i].SkillData.maxStackCount)
            {
                slot = skillSlots[i];
                break; 
            }
        }
        return slot;
    }
    private bool IsValidSlotIndex(uint index) => (index < SlotSize) || (index == TempSlotID);
    private bool IsValidAndNotEmptySlot(uint index)
    {
        SkillSlot testSlot = null;
        if (index != TempSlotID)
        {
            testSlot = skillSlots[index];
        }
        else
        {
            testSlot = TempSlot; 
        }

        return (IsValidSlotIndex(index) && !testSlot.IsEmpty());
    }
}
