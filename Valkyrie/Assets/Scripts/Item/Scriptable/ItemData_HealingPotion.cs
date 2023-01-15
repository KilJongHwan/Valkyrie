using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data_HealingPotion", menuName = "Scriptable Object/Item Data_HealingPotion", order = 2)]
public class ItemData_HealingPotion : ItemData, IUsable
{
    [Header("���� ���� ������")]
    public float healPoint = 20.0f;
    public void Use(GameObject target = null)
    {
        IStatus health = target.GetComponent<IStatus>();
        if (health != null)
        {
            health.HP += healPoint;
            Debug.Log($"{itemName}�� ����߽��ϴ�. HP�� {healPoint}��ŭ ȸ���˴ϴ�. ���� HP�� {health.HP}�Դϴ�.");
        }
    }

}
