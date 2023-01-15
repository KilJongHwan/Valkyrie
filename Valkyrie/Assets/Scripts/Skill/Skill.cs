using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public SkillData data;
    private void Start()
    {
        Instantiate(data.prefab, transform.position, transform.rotation, transform);
    }
}
