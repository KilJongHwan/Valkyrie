using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill_Fire Data", menuName = "ScriptableObject/Skill_Fire Data", order = 2)]
public class SkillData_Fire : SkillData, ICastable
{
    public GameObject hitPrefab;
    public void Cast(GameObject target = null)
    {
        IStatus status = target.GetComponent<IStatus>();
        if (status  != null)
        {
            status.MP -= cost;
            Debug.Log($"Cast {skillName}. Use MP : {cost}  Current MP {status.MP}");
        }
    }
    public void Hit(Vector3 pos, Vector3 normal)
    {
        Instantiate(hitPrefab, pos, Quaternion.LookRotation(normal));
    }
}
