using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFactory
{
    static int skillCount = 0;
    public static GameObject MakeSkill(SkillCode code)
    {
        GameObject obj = new();
        Skill skill = obj.AddComponent<Skill>();

        skill.data = GameManager.Inst.SkillData[code];
        string[] skillName = skill.data.name.Split("_");
        obj.name = $"{skillName[1]}_{skillCount}";
        obj.layer = LayerMask.NameToLayer("Skill");
        //SphereCollider col = obj.AddComponent<SphereCollider>();
        //col.radius = 0.5f;
        //col.isTrigger = true;
        skillCount++;

        return obj;
    }
    public static GameObject MakeSkill(SkillCode code, Vector3 pos)
    {
        GameObject obj = MakeSkill(code);

        obj.transform.position = pos;
        obj.transform.forward = GameManager.Inst.MainPlayer.transform.forward;
        return obj;
    }
}
