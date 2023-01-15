using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetHP_Bar : MonoBehaviour
{
    public IStatus target;
    public Slider slider;

    public TextMeshProUGUI nameText;
    public CanvasGroup canvas;

    private void Awake()
    {
        target = FindObjectOfType<Enemy>();
    }
    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        nameText = GetComponentInChildren<TextMeshProUGUI>();
        canvas = GetComponent<CanvasGroup>();
        target.onHPchange += SetHP_Value;
        TargetHPToggle();
    }
    public void TargetHPToggle()
    {
        if (canvas.blocksRaycasts)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
    void Open()
    {
        canvas.alpha = 1;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }
    void Close()
    {
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
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
