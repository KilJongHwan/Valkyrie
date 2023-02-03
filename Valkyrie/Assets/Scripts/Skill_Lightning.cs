using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Lightning : MonoBehaviour
{
    public float lightningDamage = 5.0f;
    GameObject start;
    GameObject end;
    Player player;
    SphereCollider col;

    List<IStatus> targetList;
    private void Awake()
    {
        start = transform.GetChild(0).gameObject;
        end = transform.GetChild(1).gameObject;
        col = GetComponent<SphereCollider>();
    }
    private void Start()
    {
        player = GameManager.Inst.MainPlayer;
        targetList = new List<IStatus>();
    }
    private void Update()
    {
        start.transform.position = player.emitPosition.transform.position;
        if (player.Target != null)
            end.transform.position = player.Target.position;
        else
            end.transform.position = player.transform.forward;
        col.gameObject.transform.position = end.transform.position;
        if (targetList != null)
        {
            foreach (var list in targetList)
            {
                list.TakeDamage(lightningDamage);
            }
        }
       
        if (DurationEnd)
        {
            player.lightningEffect.SetActive(false);
            player.castDatas[2].ResetCoolTime();
            Debug.Log("Skill - end");
        }
    }
    bool DurationEnd => player.castDatas[2].CurrentDuration >= player.castDatas[2].skillData.skillDuration;
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == false)
        {
            IStatus enemy = other.gameObject.GetComponent<IStatus>();
            if (enemy != null)
            {
                targetList.Add(enemy);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger == false)
        {
            targetList.Remove(other.gameObject.GetComponent<IStatus>());
        }
    }
}