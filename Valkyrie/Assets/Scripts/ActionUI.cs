using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionUI : MonoBehaviour
{
    Transform action;
    public SkillWindow window;
    ActionSkillSlotUI[] actionSlotUIs;

    public ActionSkillSlotUI this[int index]
    {
        get => actionSlotUIs[index];
    }
    public int slotLength => actionSlotUIs.Length;

    const uint InvaildeID = uint.MaxValue;
    public uint startID = InvaildeID;

    private void Awake()
    {
        window = new SkillWindow();
        action = transform.Find("SkillSlots"); 
        actionSlotUIs = action.GetComponentsInChildren<ActionSkillSlotUI>();
    }
    private void Start()
    {
        InitailizAction(window);
    }
    public void InitailizAction(SkillWindow newWindow)
    {
        window = newWindow;   //즉시 할당
        // 크기가 같을 경우 슬롯UI들의 초기화만 진행
        
        for (int i = 0; i < slotLength; i++)
        {
            actionSlotUIs[i].Initialize((uint)i, window[i]);
        }

        RefreshAllSlots();  // 전체 슬롯UI 갱신
    }
    private void RefreshAllSlots()
    {
        foreach (var slotUI in actionSlotUIs)
        {
            slotUI.Refresh();
        }
    }
   
}
