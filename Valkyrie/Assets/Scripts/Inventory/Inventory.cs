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
        slots = new ItemSlot[size];     // 입력받은 갯수로 슬롯만들기
        for (int i = 0; i < size; i++)
        {
            slots[i] = new ItemSlot();
        }
        tempSlot = new ItemSlot();      // 임시 용도로 사용하는 슬롯은 따로 생성
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
        //Debug.Log($"인벤토리에 {data.itemName}을 추가합니다");

        ItemSlot target = FindSameItem(data);   // 같은 종류의 아이템이 인벤토리에 있는지 찾기
        if (target != null)
        {
            // 같은 종류의 아이템이 있으니 1만 증가시킨다.
            target.IncreaseSlotItem();
            result = true;
            //Debug.Log($"{data.itemName}을 하나 증가시킵니다.");
        }
        else
        {
            // 같은 종류의 아이템이 없다.
            ItemSlot empty = FindEmptySlot();    // 적절한 빈 슬롯 찾기
            if (empty != null)
            {
                empty.AssignSlotItem(data);      // 아이템 할당
                result = true;
                //Debug.Log($"아이템 슬롯에 {data.itemName}을 할당합니다.");
            }
            else
            {
                // 모든 슬롯에 아이템이 들어있다.(인벤토리가 가득찼다.)
                //Debug.Log($"실패 : 인벤토리가 가득찼습니다.");
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

        //Debug.Log($"인벤토리의 {index} 슬롯에  {data.itemName}을 추가합니다");
        ItemSlot slot = slots[index];   // index번째의 슬롯 가져오기

        if (slot.IsEmpty())              // 찾은 슬롯이 비었는지 확인
        {
            slot.AssignSlotItem(data);  // 비어있으면 아이템 추가
            result = true;
            //Debug.Log($"추가에 성공했습니다.");
        }
        else
        {
            if (slot.SlotItemData == data)  // 같은 종류의 아이템인가?
            {
                if (slot.IncreaseSlotItem() == 0)  // 들어갈 자리가 있는가?
                {
                    result = true;
                    //Debug.Log($"아이템 갯수 증가에 성공했습니다.");
                }
                else
                {
                    //Debug.Log($"실패 : 슬롯이 가득 찼습니다.");
                }
            }
            else
            {
                //Debug.Log($"실패 : {index} 슬롯에는 다른 아이템이 들어있습니다.");
            }
        }

        return result;
    }

    public bool RemoveItem(uint slotIndex, uint decreaseCount = 1)
    {
        bool result = false;

        //Debug.Log($"인벤토리에서 {slotIndex} 슬롯의 아이템을 {decreaseCount}개 비웁니다.");
        if (IsValidSlotIndex(slotIndex))        // slotIndex가 적절한 범위인지 확인
        {
            ItemSlot slot = slots[slotIndex];
            slot.DecreaseSlotItem(decreaseCount);
            //Debug.Log($"삭제에 성공했습니다.");
            result = true;
        }
        else
        {
            //Debug.Log($"실패 : 잘못된 인덱스입니다.");
        }

        return result;
    }

    public bool ClearItem(uint slotIndex)
    {
        bool result = false;

        //Debug.Log($"인벤토리에서 {slotIndex} 슬롯을 비웁니다.");
        if (IsValidSlotIndex(slotIndex))        // slotIndex가 적절한 범위인지 확인
        {
            ItemSlot slot = slots[slotIndex];
            //Debug.Log($"{slot.SlotItemData.itemName}을 삭제합니다.");
            slot.ClearSlotItem();               // 적절한 슬롯이면 삭제 처리
            //Debug.Log($"삭제에 성공했습니다.");
            result = true;
        }
        else
        {
            //Debug.Log($"실패 : 잘못된 인덱스입니다.");
        }

        return result;
    }

    /// <summary>
    /// 모든 아이템 슬롯을 비우는 함수
    /// </summary>
    public void ClearInventory()
    {
        Debug.Log("인벤토리 클리어");
        foreach (var slot in slots)
        {
            slot.ClearSlotItem();   // 전체 슬롯들을 돌면서 하나씩 삭제
        }
    }

    public void MoveItem(uint from, uint to)
    {
       
        // from은 밸리드한 슬롯 인덱스고 슬롯이 비어있지 않다. 그리고 to는 밸리드한 슬롯 인덱스다.
        if (IsValidAndNotEmptySlot(from) && IsValidSlotIndex(to))
        {
            ItemSlot fromSlot = null;
            ItemSlot toSlot = null;

            // 인덱스로 슬롯 찾기
            if (from == TempSlotID)
            {
                fromSlot = TempSlot;    // temp슬롯은 별도로 인덱스 확인
            }
            else
            {
                fromSlot = slots[from]; // 다른 슬롯은 인덱스값 그대로 활용
            }
            if (to == TempSlotID)
            {
                toSlot = TempSlot;      // temp슬롯은 별도로 인덱스 확인
            }
            else
            {
                toSlot = slots[to];     // 다른 슬롯은 인덱스값 그대로 활용
            }

            // 두 슬롯에 들어있는 아이템 확인
            if (fromSlot.SlotItemData == toSlot.SlotItemData)
            {
                // 같은 종류의 아이템이다. => to에 최대한 채우고 넘치면 temp에 그대로 남긴다.
                uint overCount = toSlot.IncreaseSlotItem(fromSlot.ItemCount);
                fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);
            }
            else
            {
                // 다른 종류의 아이템이다. => 아이템과 아이템 갯수를 서로 스왑한다.
                ItemData tempItemData = toSlot.SlotItemData;    // 임시 저장
                uint tempItemCount = toSlot.ItemCount;
                toSlot.AssignSlotItem(fromSlot.SlotItemData, fromSlot.ItemCount);   // to에다 from의 정보 넣기
                fromSlot.AssignSlotItem(tempItemData, tempItemCount);               // from에다가 임시로 저장한 to의 정보 넣기                                
            }
            (toSlot.ItemEquiped, fromSlot.ItemEquiped) = (fromSlot.ItemEquiped, toSlot.ItemEquiped);
        }
    }

    public void TempRemoveItem(uint from, uint count = 1, bool equiped = false)
    {
        if (IsValidAndNotEmptySlot(from))  // from이 절절한 슬롯이면
        {
            ItemSlot slot = slots[from];
            tempSlot.AssignSlotItem(slot.SlotItemData, count);  // temp 슬롯에 지정된 갯수의 아이템 할당
            slot.DecreaseSlotItem(count);   // from 슬롯에서 해당 갯수만큼 감소            
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

        foreach (var slot in slots)  // slots를 전부 순회하면서
        {
            if (slot.IsEmpty())     // 빈 슬롯인지 확인
            {
                result = slot;      // 빈 슬롯이면 foreach break하고 리턴
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
            // 같은 종류의 아이템이 있고 슬롯에 아이템이 들어갈 여유가 있음
            if (slots[i].SlotItemData == itemData && slots[i].ItemCount < slots[i].SlotItemData.maxStackCount)
            {
                slot = slots[i];
                break;      // 찾으면 break로 종료
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
            testSlot = slots[index];    // index가 tempSlot이 아니면 인덱스로 찾기
        }
        else
        {
            testSlot = TempSlot;    // index가 tempSlot인 경우 TempSlot 저장
        }

        return (IsValidSlotIndex(index) && !testSlot.IsEmpty());
    }

    public void PrintInventory()
    {
      

        string printText = "[";
        for (int i = 0; i < SlotCount - 1; i++)         // 슬롯이 전체6개일 경우 0~4까지만 일단 추가(5개추가)
        {
            if (slots[i].SlotItemData != null)
            {
                printText += $"{slots[i].SlotItemData.itemName}({slots[i].ItemCount})";
            }
            else
            {
                printText += "(빈칸)";
            }
            printText += ",";
        }
        ItemSlot slot = slots[SlotCount - 1];   // 마지막 슬롯만 따로 처리
        if (!slot.IsEmpty())
        {
            printText += $"{slot.SlotItemData.itemName}({slot.ItemCount})]";
        }
        else
        {
            printText += "(빈칸)]";
        }

        //string.Join(',', 문자열 배열);
        Debug.Log(printText);
    }
}
