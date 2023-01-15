using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_ExpBar : MonoBehaviour
{
    Player target;
    Slider slider;
    private void Start()
    {
        target = GameManager.Inst.MainPlayer;
        slider = GetComponentInChildren<Slider>();
        target.onEXPchange += SetExp_Value;
    }

    private void SetExp_Value()
    {
        if (target != null)
        {
            float ratio = target.Exp / target.MaxExp;
            slider.value = ratio;
        }
    }
}
