using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data_Coin", menuName = "Scriptable Object/Item Data_Coin", order = 5)]
public class ItemData_Coin : ItemData, IConsumable
{
    public void Consume(Player player)
    {
        player.Gold += value;
    }
}
