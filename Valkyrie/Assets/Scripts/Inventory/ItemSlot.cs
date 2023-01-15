using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    ItemData slotItemData;

    // 아이템 갯수(int)
    uint itemCount = 0;

    // 아이템 장비여부
    bool itemEquiped = false;

    public ItemData SlotItemData
    {
        get => slotItemData;
        private set
        {
            if (slotItemData != value)
            {
                slotItemData = value;
                onSlotItemChange?.Invoke();  // 변경이 일어나면 델리게이트 실행(주로 화면 갱신용)
            }
        }
    }

    public uint ItemCount
    {
        get => itemCount;
        private set
        {
            itemCount = value;
            onSlotItemChange?.Invoke();  // 변경이 일어나면 델리게이트 실행(주로 화면 갱신용)
        }
    }

    public bool ItemEquiped
    {
        get => itemEquiped;
        set
        {
            itemEquiped = value;
            onSlotItemChange?.Invoke();
        }
    }

    public System.Action onSlotItemChange;

    public ItemSlot() { }
    public ItemSlot(ItemData data, uint count)
    {
        slotItemData = data;
        itemCount = count;
    }
    public ItemSlot(ItemSlot other)
    {
        slotItemData = other.SlotItemData;
        itemCount = other.ItemCount;
    }

    public void AssignSlotItem(ItemData itemData, uint count = 1)
    {
        ItemCount = count;
        SlotItemData = itemData;
    }

    public uint IncreaseSlotItem(uint count = 1)
    {
        uint newCount = ItemCount + count;
        int overCount = (int)newCount - (int)SlotItemData.maxStackCount;    // 넘친 갯수 계산
        if (overCount > 0)
        {
            // 넘쳤다.
            ItemCount = SlotItemData.maxStackCount;
        }
        else
        {
            // 충분히 추가 가능하다.
            ItemCount = newCount;
            overCount = 0;
        }
        return (uint)overCount; // 넘친 갯수 돌려주기
    }

    public void DecreaseSlotItem(uint count = 1)
    {
        int newCount = (int)ItemCount - (int)count;
        if (newCount < 1)   // 최종적으로 갯수가 0이되면 완전 비우기
        {
            // 다 뺀다.
            ClearSlotItem();
        }
        else
        {
            ItemCount = (uint)newCount;
        }
    }

    public void ClearSlotItem()
    {
        SlotItemData = null;
        ItemCount = 0;
        ItemEquiped = false;
    }

    public void UseSlotItem(GameObject target = null)
    {
        IUsable usable = SlotItemData as IUsable;  
        if (usable != null)
        {
            // 아이템이 사용가능하면
            usable.Use(target); // 아이템 사용하고
            DecreaseSlotItem(); // 갯수 하나 감소
        }
    }

    public bool EquipSlotItem(GameObject target = null)
    {
        bool result = false;
        IEquipTarget equipItem = SlotItemData as IEquipTarget;  // 이 슬롯의 아이템이 장비 가능한 아이템인지 확인
        if (equipItem != null)
        {
            // 아이템은 장비가능하다.

            ItemData_Weapon weaponData = SlotItemData as ItemData_Weapon;   // 아이템 데이터 따로 보관
            IEquiptable equipTarget = target.GetComponent<IEquiptable>(); // 아이템을 장비할 대상이 아이템을 장비할 수 있는지 확인
            if (equipTarget != null)
            {
                // 대상은 특정 슬롯의 아이템을 장비하고 있다. 그리고 아이템이 장비되어 있다.
                if (equipTarget.EquipItemSlot != null)    // 무기를 장비하고 잇는지 확인
                {
                    // 무기를 장비하고 있다.

                    if (equipTarget.EquipItemSlot != this)      // 장비하고 있는 아이템의 슬롯을 클릭했는지 확인
                    {
                        // 다른 슬롯을 장비하고 있다.
                        equipTarget.UnEquipWeapon();            // 일단 무기를 벗는다.
                        equipTarget.EquipWeapon(this);    // 다른 무기를 장비한다.
                        result = true;
                    }
                    else
                    {
                        equipTarget.UnEquipWeapon();            // 같은 무기를 장비한 상황이면 벗기만 한다.
                    }
                }
                else
                {
                    // 무기를 장비하고 있지 않다. => 그냥 장비
                    equipTarget.EquipWeapon(this);
                    result = true;
                }
            }
        }
        return result;
    }

    public bool IsEmpty()
    {
        return slotItemData == null;
    }
}
