using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillWindowUI : MonoBehaviour
{
    public GameObject slotPrefab;
    SkillWindow window;
    Transform parent;

    SkillSlotUI[] slotUIs;

    CanvasGroup canvasGroup;
    Button onOffButton;


    public System.Action onWindowOpen;
    public System.Action onWindowClose;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        parent = transform.Find("SkillWindowSlots");

        onOffButton = GameObject.Find("SkillWindowOnOffButton").GetComponent<Button>();
        onOffButton.onClick.AddListener(OnOffswitch);
    }
    public void InitailizeWindow(SkillWindow newWindow)
    {
        window = newWindow;   //즉시 할당
        if (SkillWindow.Default_Skill_Size != newWindow.SlotSize)    // 기본 사이즈와 다르면 기본 슬롯UI 삭제
        {
            // 기존 슬롯UI 전부 삭제
            SkillSlotUI[] slots = GetComponentsInChildren<SkillSlotUI>();
            foreach (var slot in slots)
            {
                Destroy(slot.gameObject);
            }

            // 새로 만들기
            slotUIs = new SkillSlotUI[window.SlotSize];
            for (int i = 0; i < window.SlotSize; i++)
            {
                GameObject obj = Instantiate(slotPrefab, parent);
                obj.name = $"{slotPrefab.name}_{i}";            // 이름 지어주고
                slotUIs[i] = obj.GetComponent<SkillSlotUI>();
                slotUIs[i].Initialize((uint)i, window[i]);       // 각 슬롯UI들도 초기화
            }
        }
        else
        {
            // 크기가 같을 경우 슬롯UI들의 초기화만 진행
            slotUIs = parent.GetComponentsInChildren<SkillSlotUI>();
            for (int i = 0; i < window.SlotSize; i++)
            {
                slotUIs[i].Initialize((uint)i, window[i]);
            }
        }



        RefreshAllSlots();  // 전체 슬롯UI 갱신
    }
   
    public void RefreshAllSlots()
    {
        foreach (var slotUI in slotUIs)
        {
            slotUI.Refresh();
        }
    }
    void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        onWindowOpen?.Invoke();
    }
    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        onWindowClose?.Invoke();
    }
    void OnOffswitch()
    {

        if (canvasGroup.blocksRaycasts) 
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
