using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("������ ������")]
    public uint id = 0;
    public string itemName = "������";
    public Sprite itemIcon;
    public GameObject prefab;
    public uint value;
    public uint maxStackCount = 1;
}
