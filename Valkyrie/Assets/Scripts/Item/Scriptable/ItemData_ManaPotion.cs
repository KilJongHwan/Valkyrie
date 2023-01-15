using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data_ManaPotion", menuName = "Scriptable Object/Item Data_ManaPotion", order = 3)]
public class ItemData_ManaPotion : ItemData, IUsable
{
    [Header("힐링 포션 데이터")]
    public float NanaPoint = 20.0f;
    public void Use(GameObject target = null)
    {
        IStatus mana = target.GetComponent<IStatus>();
        if (mana != null)
        {
            mana.MP += NanaPoint;
            Debug.Log($"{itemName}을 사용했습니다. HP가 {NanaPoint}만큼 회복됩니다. 현재 HP는 {mana.MP}입니다.");
        }
    }

}
