using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory
{
    static int itemCount = 0;   // 이때까지 생성된 총 아이템 개수. (각 아이템별 고유 아이디 용도로 사용)

    public static GameObject MakeItem(ItemCode code)
    {
        GameObject obj = new GameObject();              // 빈 오브젝트 만들고 ((0,0,0)에 생성됨)
        Item item = obj.AddComponent<Item>();           // Item 컴포넌트 추가

        item.data = GameManager.Inst.ItemData[code];    // ItemData 설정
        string[] itemName = item.data.name.Split("_");  // 내가 생성하는 종류에 맞게 이름 변경
        obj.name = $"{itemName[1]}_{itemCount}";        // 고유 아이디도 추가
        obj.layer = LayerMask.NameToLayer("Item");      // 레이어 설정
        obj.tag = "Item";
        SphereCollider col = obj.AddComponent<SphereCollider>();    // 코드로 컬라이더 추가
        col.radius = 2.0f;
        col.isTrigger = true;
        itemCount++;    // 생성할 때마다 값을 증가시켜서 중복이 없도록 처리

        GameObject indicator = Resources.Load<GameObject>("MinimapItemIndicator_Item"); // 리소시스 폴더에서 동적으로 로딩
        if (indicator != null)  // 로딩이 안되었을 때를 대비해서 추가
        {
            // 쿼드를 눕히기 위해 90도 회전
            GameObject.Instantiate(indicator,
                obj.transform.position, obj.transform.rotation * Quaternion.Euler(90, 0, 0), obj.transform);
        }

        return obj;     // 생성완료된 것 리턴
    }

    public static GameObject MakeItem(ItemCode code, Vector3 position, bool randomNoise = false)
    {
        GameObject obj = MakeItem(code);
        if (randomNoise)
        {
            Vector2 noise = Random.insideUnitCircle * 0.5f;
            position.x += noise.x;
            position.z += noise.y;
        }
        obj.transform.position = position;

        return obj;
    }
    public static void MakeItems(ItemCode code, Vector3 position, uint count)
    {
        for (int i = 0; i < count; i++)
        {
            MakeItem(code, position, true);
        }
    }

    public static GameObject MakeItem(uint id)
    {
        return MakeItem((ItemCode)id);
    }

    public static GameObject MakeItem(uint id, Vector3 position, bool randomNoise = false)
    {
        return MakeItem((ItemCode)id, position, randomNoise);
    }

    public static void MakeItems(uint id, Vector3 position, uint count)
    {
        MakeItems((ItemCode)id, position, count);
    }
}
