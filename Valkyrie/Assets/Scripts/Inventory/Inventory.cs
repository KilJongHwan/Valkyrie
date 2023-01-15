using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    ItemSlot[] slots = null;

    ItemSlot tempSlot = null;

    public const int Default_Inventory_Size = 8;

    public const uint TempSlotID = 99999;  

    public int SlotCount => slots.Length;

    public ItemSlot TempSlot => tempSlot;

    public ItemSlot this[int index] => slots[index];


    public Inventory(int size = Default_Inventory_Size)
    {
        slots = new ItemSlot[size];     // �Է¹��� ������ ���Ը����
        for (int i = 0; i < size; i++)
        {
            slots[i] = new ItemSlot();
        }
        tempSlot = new ItemSlot();      // �ӽ� �뵵�� ����ϴ� ������ ���� ����
    }

    public bool AddItem(uint id)
    {
        return AddItem(GameManager.Inst.ItemData[id]);
    }

    public bool AddItem(ItemCode code)
    {
        return AddItem(GameManager.Inst.ItemData[code]);
    }

    public bool AddItem(ItemData data)
    {
        bool result = false;
        //Debug.Log($"�κ��丮�� {data.itemName}�� �߰��մϴ�");

        ItemSlot target = FindSameItem(data);   // ���� ������ �������� �κ��丮�� �ִ��� ã��
        if (target != null)
        {
            // ���� ������ �������� ������ 1�� ������Ų��.
            target.IncreaseSlotItem();
            result = true;
            //Debug.Log($"{data.itemName}�� �ϳ� ������ŵ�ϴ�.");
        }
        else
        {
            // ���� ������ �������� ����.
            ItemSlot empty = FindEmptySlot();    // ������ �� ���� ã��
            if (empty != null)
            {
                empty.AssignSlotItem(data);      // ������ �Ҵ�
                result = true;
                //Debug.Log($"������ ���Կ� {data.itemName}�� �Ҵ��մϴ�.");
            }
            else
            {
                // ��� ���Կ� �������� ����ִ�.(�κ��丮�� ����á��.)
                //Debug.Log($"���� : �κ��丮�� ����á���ϴ�.");
            }
        }

        return result;
    }

    public bool AddItem(uint id, uint index)
    {
        return AddItem(GameManager.Inst.ItemData[id], index);
    }

    public bool AddItem(ItemCode code, uint index)
    {
        return AddItem(GameManager.Inst.ItemData[code], index);
    }

    public bool AddItem(ItemData data, uint index)
    {
        bool result = false;

        //Debug.Log($"�κ��丮�� {index} ���Կ�  {data.itemName}�� �߰��մϴ�");
        ItemSlot slot = slots[index];   // index��°�� ���� ��������

        if (slot.IsEmpty())              // ã�� ������ ������� Ȯ��
        {
            slot.AssignSlotItem(data);  // ��������� ������ �߰�
            result = true;
            //Debug.Log($"�߰��� �����߽��ϴ�.");
        }
        else
        {
            if (slot.SlotItemData == data)  // ���� ������ �������ΰ�?
            {
                if (slot.IncreaseSlotItem() == 0)  // �� �ڸ��� �ִ°�?
                {
                    result = true;
                    //Debug.Log($"������ ���� ������ �����߽��ϴ�.");
                }
                else
                {
                    //Debug.Log($"���� : ������ ���� á���ϴ�.");
                }
            }
            else
            {
                //Debug.Log($"���� : {index} ���Կ��� �ٸ� �������� ����ֽ��ϴ�.");
            }
        }

        return result;
    }

    public bool RemoveItem(uint slotIndex, uint decreaseCount = 1)
    {
        bool result = false;

        //Debug.Log($"�κ��丮���� {slotIndex} ������ �������� {decreaseCount}�� ���ϴ�.");
        if (IsValidSlotIndex(slotIndex))        // slotIndex�� ������ �������� Ȯ��
        {
            ItemSlot slot = slots[slotIndex];
            slot.DecreaseSlotItem(decreaseCount);
            //Debug.Log($"������ �����߽��ϴ�.");
            result = true;
        }
        else
        {
            //Debug.Log($"���� : �߸��� �ε����Դϴ�.");
        }

        return result;
    }

    public bool ClearItem(uint slotIndex)
    {
        bool result = false;

        //Debug.Log($"�κ��丮���� {slotIndex} ������ ���ϴ�.");
        if (IsValidSlotIndex(slotIndex))        // slotIndex�� ������ �������� Ȯ��
        {
            ItemSlot slot = slots[slotIndex];
            //Debug.Log($"{slot.SlotItemData.itemName}�� �����մϴ�.");
            slot.ClearSlotItem();               // ������ �����̸� ���� ó��
            //Debug.Log($"������ �����߽��ϴ�.");
            result = true;
        }
        else
        {
            //Debug.Log($"���� : �߸��� �ε����Դϴ�.");
        }

        return result;
    }

    /// <summary>
    /// ��� ������ ������ ���� �Լ�
    /// </summary>
    public void ClearInventory()
    {
        Debug.Log("�κ��丮 Ŭ����");
        foreach (var slot in slots)
        {
            slot.ClearSlotItem();   // ��ü ���Ե��� ���鼭 �ϳ��� ����
        }
    }

    public void MoveItem(uint from, uint to)
    {
       
        // from�� �븮���� ���� �ε����� ������ ������� �ʴ�. �׸��� to�� �븮���� ���� �ε�����.
        if (IsValidAndNotEmptySlot(from) && IsValidSlotIndex(to))
        {
            ItemSlot fromSlot = null;
            ItemSlot toSlot = null;

            // �ε����� ���� ã��
            if (from == TempSlotID)
            {
                fromSlot = TempSlot;    // temp������ ������ �ε��� Ȯ��
            }
            else
            {
                fromSlot = slots[from]; // �ٸ� ������ �ε����� �״�� Ȱ��
            }
            if (to == TempSlotID)
            {
                toSlot = TempSlot;      // temp������ ������ �ε��� Ȯ��
            }
            else
            {
                toSlot = slots[to];     // �ٸ� ������ �ε����� �״�� Ȱ��
            }

            // �� ���Կ� ����ִ� ������ Ȯ��
            if (fromSlot.SlotItemData == toSlot.SlotItemData)
            {
                // ���� ������ �������̴�. => to�� �ִ��� ä��� ��ġ�� temp�� �״�� �����.
                uint overCount = toSlot.IncreaseSlotItem(fromSlot.ItemCount);
                fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);
            }
            else
            {
                // �ٸ� ������ �������̴�. => �����۰� ������ ������ ���� �����Ѵ�.
                ItemData tempItemData = toSlot.SlotItemData;    // �ӽ� ����
                uint tempItemCount = toSlot.ItemCount;
                toSlot.AssignSlotItem(fromSlot.SlotItemData, fromSlot.ItemCount);   // to���� from�� ���� �ֱ�
                fromSlot.AssignSlotItem(tempItemData, tempItemCount);               // from���ٰ� �ӽ÷� ������ to�� ���� �ֱ�                                
            }
            (toSlot.ItemEquiped, fromSlot.ItemEquiped) = (fromSlot.ItemEquiped, toSlot.ItemEquiped);
        }
    }

    public void TempRemoveItem(uint from, uint count = 1, bool equiped = false)
    {
        if (IsValidAndNotEmptySlot(from))  // from�� ������ �����̸�
        {
            ItemSlot slot = slots[from];
            tempSlot.AssignSlotItem(slot.SlotItemData, count);  // temp ���Կ� ������ ������ ������ �Ҵ�
            slot.DecreaseSlotItem(count);   // from ���Կ��� �ش� ������ŭ ����            
            tempSlot.ItemEquiped = equiped;
        }
    }

    public void BuySlotItem(ItemData data)
    {
        AddItem(data);
    }
    private ItemSlot FindEmptySlot()
    {
        ItemSlot result = null;

        foreach (var slot in slots)  // slots�� ���� ��ȸ�ϸ鼭
        {
            if (slot.IsEmpty())     // �� �������� Ȯ��
            {
                result = slot;      // �� �����̸� foreach break�ϰ� ����
                break;
            }
        }

        return result;
    }

    private ItemSlot FindSameItem(ItemData itemData)
    {
        ItemSlot slot = null;
        for (int i = 0; i < SlotCount; i++)
        {
            // ���� ������ �������� �ְ� ���Կ� �������� �� ������ ����
            if (slots[i].SlotItemData == itemData && slots[i].ItemCount < slots[i].SlotItemData.maxStackCount)
            {
                slot = slots[i];
                break;      // ã���� break�� ����
            }
        }
        return slot;
    }

    private bool IsValidSlotIndex(uint index) => (index < SlotCount) || (index == TempSlotID);
    

    private bool IsValidAndNotEmptySlot(uint index)
    {
        ItemSlot testSlot = null;
        if (index != TempSlotID)
        {
            testSlot = slots[index];    // index�� tempSlot�� �ƴϸ� �ε����� ã��
        }
        else
        {
            testSlot = TempSlot;    // index�� tempSlot�� ��� TempSlot ����
        }

        return (IsValidSlotIndex(index) && !testSlot.IsEmpty());
    }

    public void PrintInventory()
    {
      

        string printText = "[";
        for (int i = 0; i < SlotCount - 1; i++)         // ������ ��ü6���� ��� 0~4������ �ϴ� �߰�(5���߰�)
        {
            if (slots[i].SlotItemData != null)
            {
                printText += $"{slots[i].SlotItemData.itemName}({slots[i].ItemCount})";
            }
            else
            {
                printText += "(��ĭ)";
            }
            printText += ",";
        }
        ItemSlot slot = slots[SlotCount - 1];   // ������ ���Ը� ���� ó��
        if (!slot.IsEmpty())
        {
            printText += $"{slot.SlotItemData.itemName}({slot.ItemCount})]";
        }
        else
        {
            printText += "(��ĭ)]";
        }

        //string.Join(',', ���ڿ� �迭);
        Debug.Log(printText);
    }
}
