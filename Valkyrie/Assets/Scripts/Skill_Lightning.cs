using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Lightning : MonoBehaviour
{
    public float lightningDamage = 5.0f;
    GameObject start;
    GameObject end;
    Player player;
    private void Awake()
    {
        start = transform.GetChild(0).gameObject;
        end = transform.GetChild(1).gameObject;
    }
    private void Start()
    {
        player = GameManager.Inst.MainPlayer;
    }
    private void Update()
    {
        start.transform.position = player.emitPosition.transform.position;
        if (player.Target != null)
            end.transform.position = player.Target.position;
        else
            end.transform.position = start.transform.forward;
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
            IStatus enemy = other.GetComponent<IStatus>();
            enemy.TakeDamage(lightningDamage);
        }
    }
}