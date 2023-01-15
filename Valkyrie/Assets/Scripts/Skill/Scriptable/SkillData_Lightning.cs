using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill_Linghtning Data", menuName = "ScriptableObject/Skill_Lightning Data", order = 4)]
public class SkillData_Lightning : SkillData, ICastable
{
    public void Cast(GameObject target = null)
    {
        IStatus status = target.GetComponent<IStatus>();
        if (status  != null)
        {
            status.MP -= cost;
            Debug.Log($"Cast {skillName}. Use MP : {cost}  Current MP {status.MP}");
        }
    }
  
}
