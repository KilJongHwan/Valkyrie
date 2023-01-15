using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_MPBar : MonoBehaviour
{
    IStatus target;
    Slider slider;
    private void Start()
    {
        target = GameManager.Inst.MainPlayer.GetComponent<IStatus>();
        slider = GetComponentInChildren<Slider>();
        target.onMPchange += SetMP_Value;
    }

    private void SetMP_Value()
    {
        if (target != null)
        {
            float ratio = target.MP / target.MaxMP;
            slider.value = ratio;
        }
    }
}
