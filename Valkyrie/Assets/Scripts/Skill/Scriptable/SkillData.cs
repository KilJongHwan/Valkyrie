using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Data",menuName = "ScriptableObject/Skill Data",order = 1)]
public class SkillData : ScriptableObject
{
    [Header("�Ϲ� ��ų������")]
    public uint id = 0;
    public string skillName = "��ų";
    public Sprite skillIcon;
    public GameObject prefab;
    public float skillCoolTime;
    public float skillDuration;
    public float cost = 10.0f;
    public uint maxStackCount = 99999;
}
