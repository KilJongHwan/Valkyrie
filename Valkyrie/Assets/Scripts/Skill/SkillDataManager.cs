using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataManager : MonoBehaviour
{
    public SkillData[] skillDatas;

    public SkillData this[uint i]
    {
        get => skillDatas[i];
    }
    public SkillData this[SkillCode code]
    {
        get => skillDatas[(int)code];
    }
    public int Length
    {
        get => skillDatas.Length;
    }

}
