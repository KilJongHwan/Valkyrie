using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    public ItemData[] itemDatas;

    public ItemData this[uint i]
    {
        get => itemDatas[i];
    }
    public ItemData this[ItemCode code]
    {
        get => itemDatas[(int)code];
    }
    public int Length
    {
        get => itemDatas.Length;
    }
}