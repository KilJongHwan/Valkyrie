using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardObject : MonoBehaviour
{
    // 적을 List나 배열에 넣고 
    // 전방에 있는지 가까운 거리에 있는지를 판단해서 그중에 적캐릭터하나를 선택해서 Attack이 들어가도록 
    // 하면 될꺼 같습니다.
    [SerializeField] private GameObject[] _EnemyList;    // 적 캐릭터 배열

    [SerializeField] private GameObject _MainCharacter;    // 주인공 캐릭터
    [SerializeField] private GameObject _Enemy;    // 적 캐릭터.

    private float _ShortDistance;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("angle = " + ToVectorAngle(_MainCharacter.transform, _Enemy.transform));
    }


    // 전방에 있는 경우
    // 대략 0 ~ 0.8 사이에 있으면 앞쪽에 있는 것이고 
    // 거리는 적당히
    // 전방 각도안에 들어있고 내가 정한 거리에 있으면 
    // 타켓 오브젝트로 설정하면 될거 같습니다.

    /// <summary>
    /// 두 캐릭터 사이의 각도를 구한다.
    /// -1 ~ 1 사이의 값이다.
    /// 0도면 1, 180도면 -1
    /// </summary>
    /// <param name="tr1"></param>
    /// <param name="tr2"></param>
    /// <returns></returns>
    float ToVectorAngle(Transform tr1, Transform tr2)
    {
        Vector3 enemyVec = tr2.position - tr1.position;
        float angle = Vector3.Dot(tr1.forward, enemyVec.normalized);
        return angle;
    }

    /// <summary>
    /// 두 점 사이의 거리를 구한다.
    /// </summary>
    /// <param name="tr1"></param>
    /// <param name="tr2"></param>
    /// <returns></returns>

    float ToDistance(Transform tr1, Transform tr2)
    {
        return Vector3.Distance(tr1.position, tr2.position);
    }
    private GameObject SearchTarget()
    {
        float _Angle = ToVectorAngle(_MainCharacter.transform, _Enemy.transform);
        float _Distance = ToDistance(_MainCharacter.transform, _Enemy.transform);
        _EnemyList = GameObject.FindGameObjectsWithTag("Monster");
        
        _ShortDistance = Vector3.Distance(gameObject.transform.position, _EnemyList[0].transform.position);
        _Enemy = _EnemyList[0];

        GameObject target = null;   // target오브젝트 저장
        foreach (GameObject Found in _EnemyList)
        {
            if (_Distance < _ShortDistance)
            {
                _Enemy = Found;
                _ShortDistance = _Distance;
            }
        }

        for (int i = 0; i < _EnemyList.Length; i++)
        {
            if (_EnemyList.Length != 0)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction * 10);

                foreach (RaycastHit rch in hits)
                {
                    if (rch.collider != null && rch.transform.tag == "Monster")
                    {
                        target = rch.collider.gameObject;
                        break;
                    }
                }
            }
        }

        return target;

    }

    // Update is called once per frame
    void Update()
    {
        SearchTarget();
        Debug.Log("angle = " + ToVectorAngle(_MainCharacter.transform, _Enemy.transform));
        Debug.Log("distance = " + ToDistance(_MainCharacter.transform, _Enemy.transform));
    }
}