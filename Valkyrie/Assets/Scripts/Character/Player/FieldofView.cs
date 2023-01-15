using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldofView : MonoBehaviour
{
    public float viewArea;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    // 타겟 사이에 다른 오브젝트 존재 시 그 오브젝트를 투과하여 뒤의 오브젝트를 확인 가능
    public List<GameObject> visibleTargets = new List<GameObject>();
    public GameObject targeting;
    public float shortDis;

    public GameObject targetingUI;
    // Start is called before the first frame update
    void Start()
    {
        // 플래이 시 FindTargetsDelay 코루틴을 실행 0,2초 간격으로
        StartCoroutine("FindTargetsDelay", 0.2f);
    }
    IEnumerator FindTargetsDelay(float _delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(_delay);
        }
    }
    void FindTargets()
    {
        visibleTargets.Clear();
        Collider[] targetInSightArea = Physics.OverlapSphere(transform.position, viewArea, targetMask);
        for (int i = 0; i < targetInSightArea.Length; i++)
        {
            Transform target = targetInSightArea[i].transform;   // 타겟 위치
            Vector3 dirTarget = (target.position - transform.position).normalized;
            // vector3타입의 타겟의 방향 변수 선언 = 타겟의 방향벡터, 타겟의 position - 이 게임오브젝트의 position) normalized = 벡터 크기 정규화 = 단위벡터화
            if (Vector3.Angle(transform.forward, dirTarget)< viewAngle / 2) // 전방 벡터와 타겟방향의 벡터의 크기가 시야각의 1/2이면 = 시야각 안에 타겟 존재
            {
                float desTarget = Vector3.Distance(transform.position, target.position);    // 타겟과의 거리 계산
                if (!Physics.Raycast(transform.position,dirTarget,desTarget,obstacleMask))  // 
                {
                    visibleTargets.Add(target.transform.gameObject);
                    Debug.DrawRay(transform.position, dirTarget * 10f, Color.red, 5f);
                    print("raycast hit");
                }
            }
        }
        if (visibleTargets.Count != 0) // 범위 안에 있는 게임 오브젝트 리스트 존재허면 거리 계산 시작
        {
            if (targetingUI != null)    // 시야 범위 안에 있고,이전 targetingUI가 존재 한다면 UI를 끔
            {
                targetingUI.SetActive(false);
            }
            targeting = visibleTargets[0];
            //첫번째를 지정, 첫번째는 타겟팅 대상, 실행했을 때 리스트에 값이 없으면 ArgumentOutOfRangeException 에러가 나옴
            //visibleTargets[0] != null visibleTargets[0]의 값이 비어있지 않으면 실행하려고 했으나 계속 에러 발생.
            //리스트 개수를 통해 해결함 visibleTargets.Count != 0 
            shortDis = Vector3.Distance(transform.position, visibleTargets[0].transform.position); //visiableTargets 리스트의 첫번째와의 거리를 기준으로 잡기

            // 리스트중에서 가장 가까운 거리의 게임 오브젝트 찾기
            foreach (GameObject found in visibleTargets)
            {
                float _distance = Vector3.Distance(transform.position, found.transform.position);
                if (_distance < shortDis)
                {
                    targeting = found;
                    shortDis = _distance;
                }
            }

            Debug.Log(targeting.name);  // 가장 가까운 거리의 게임 오브젝트 찾기

            // 가장 가까운 거리의 게임 오브젝트의 자식 오브젝트 Canvas에 접근
            targetingUI = targeting.transform.Find("Monster").transform.gameObject;

            //Canvas 활성화 = targetingUI활성화
            targetingUI.SetActive(true);

        }
        //범위 안에 게임 오브젝트가 없고, targetingUI가 비어있지 않으면 이전 UI 비활성화
        else if (visibleTargets.Count == 0 && targetingUI != null)
        {
            targetingUI.SetActive(false);
        }
        
    }
}
