using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquiptable
{
    ItemSlot EquipItemSlot { get; }
    void EquipWeapon(ItemSlot weaponSlot); 
    void UnEquipWeapon();
}
