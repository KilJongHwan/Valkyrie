using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data_Weapon", menuName = "Scriptable Object/Item Data_Weapon", order = 4)]
public class ItemData_Weapon : ItemData, IEquipTarget
{
    [Header("무기 데이터")]
    public float attackPower = 10.0f;
    public float attackSpeed = 3.0f;
}
