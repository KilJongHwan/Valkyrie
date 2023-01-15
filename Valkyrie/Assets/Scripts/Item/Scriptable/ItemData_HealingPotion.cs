using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data_HealingPotion", menuName = "Scriptable Object/Item Data_HealingPotion", order = 2)]
public class ItemData_HealingPotion : ItemData, IUsable
{
    [Header("힐링 포션 데이터")]
    public float healPoint = 20.0f;
    public void Use(GameObject target = null)
    {
        IStatus health = target.GetComponent<IStatus>();
        if (health != null)
        {
            health.HP += healPoint;
            Debug.Log($"{itemName}을 사용했습니다. HP가 {healPoint}만큼 회복됩니다. 현재 HP는 {health.HP}입니다.");
        }
    }

}
