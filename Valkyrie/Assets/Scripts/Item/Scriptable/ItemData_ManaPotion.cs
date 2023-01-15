using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data_ManaPotion", menuName = "Scriptable Object/Item Data_ManaPotion", order = 3)]
public class ItemData_ManaPotion : ItemData, IUsable
{
    [Header("���� ���� ������")]
    public float NanaPoint = 20.0f;
    public void Use(GameObject target = null)
    {
        IStatus mana = target.GetComponent<IStatus>();
        if (mana != null)
        {
            mana.MP += NanaPoint;
            Debug.Log($"{itemName}�� ����߽��ϴ�. HP�� {NanaPoint}��ŭ ȸ���˴ϴ�. ���� HP�� {mana.MP}�Դϴ�.");
        }
    }

}
