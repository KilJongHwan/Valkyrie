using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HPBar : MonoBehaviour
{
    IStatus target;
    Slider slider;
    private void Start()
    {
        target = GameManager.Inst.MainPlayer.GetComponent<IStatus>();
        slider = GetComponentInChildren<Slider>();
        target.onHPchange += SetHP_Value;
    }

    private void SetHP_Value()
    {
        if (target != null)
        {
            float ratio = target.HP / target.MaxHP;
            slider.value = ratio;
        }
    }
}
