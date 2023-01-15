using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    ItemData slotItemData;

    // ������ ����(int)
    uint itemCount = 0;

    // ������ ��񿩺�
    bool itemEquiped = false;

    public ItemData SlotItemData
    {
        get => slotItemData;
        private set
        {
            if (slotItemData != value)
            {
                slotItemData = value;
                onSlotItemChange?.Invoke();  // ������ �Ͼ�� ��������Ʈ ����(�ַ� ȭ�� ���ſ�)
            }
        }
    }

    public uint ItemCount
    {
        get => itemCount;
        private set
        {
            itemCount = value;
            onSlotItemChange?.Invoke();  // ������ �Ͼ�� ��������Ʈ ����(�ַ� ȭ�� ���ſ�)
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
        int overCount = (int)newCount - (int)SlotItemData.maxStackCount;    // ��ģ ���� ���
        if (overCount > 0)
        {
            // ���ƴ�.
            ItemCount = SlotItemData.maxStackCount;
        }
        else
        {
            // ����� �߰� �����ϴ�.
            ItemCount = newCount;
            overCount = 0;
        }
        return (uint)overCount; // ��ģ ���� �����ֱ�
    }

    public void DecreaseSlotItem(uint count = 1)
    {
        int newCount = (int)ItemCount - (int)count;
        if (newCount < 1)   // ���������� ������ 0�̵Ǹ� ���� ����
        {
            // �� ����.
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
            // �������� ��밡���ϸ�
            usable.Use(target); // ������ ����ϰ�
            DecreaseSlotItem(); // ���� �ϳ� ����
        }
    }

    public bool EquipSlotItem(GameObject target = null)
    {
        bool result = false;
        IEquipTarget equipItem = SlotItemData as IEquipTarget;  // �� ������ �������� ��� ������ ���������� Ȯ��
        if (equipItem != null)
        {
            // �������� ��񰡴��ϴ�.

            ItemData_Weapon weaponData = SlotItemData as ItemData_Weapon;   // ������ ������ ���� ����
            IEquiptable equipTarget = target.GetComponent<IEquiptable>(); // �������� ����� ����� �������� ����� �� �ִ��� Ȯ��
            if (equipTarget != null)
            {
                // ����� Ư�� ������ �������� ����ϰ� �ִ�. �׸��� �������� ���Ǿ� �ִ�.
                if (equipTarget.EquipItemSlot != null)    // ���⸦ ����ϰ� �մ��� Ȯ��
                {
                    // ���⸦ ����ϰ� �ִ�.

                    if (equipTarget.EquipItemSlot != this)      // ����ϰ� �ִ� �������� ������ Ŭ���ߴ��� Ȯ��
                    {
                        // �ٸ� ������ ����ϰ� �ִ�.
                        equipTarget.UnEquipWeapon();            // �ϴ� ���⸦ ���´�.
                        equipTarget.EquipWeapon(this);    // �ٸ� ���⸦ ����Ѵ�.
                        result = true;
                    }
                    else
                    {
                        equipTarget.UnEquipWeapon();            // ���� ���⸦ ����� ��Ȳ�̸� ���⸸ �Ѵ�.
                    }
                }
                else
                {
                    // ���⸦ ����ϰ� ���� �ʴ�. => �׳� ���
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
