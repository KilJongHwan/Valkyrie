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
        window = newWindow;   //��� �Ҵ�
        if (SkillWindow.Default_Skill_Size != newWindow.SlotSize)    // �⺻ ������� �ٸ��� �⺻ ����UI ����
        {
            // ���� ����UI ���� ����
            SkillSlotUI[] slots = GetComponentsInChildren<SkillSlotUI>();
            foreach (var slot in slots)
            {
                Destroy(slot.gameObject);
            }

            // ���� �����
            slotUIs = new SkillSlotUI[window.SlotSize];
            for (int i = 0; i < window.SlotSize; i++)
            {
                GameObject obj = Instantiate(slotPrefab, parent);
                obj.name = $"{slotPrefab.name}_{i}";            // �̸� �����ְ�
                slotUIs[i] = obj.GetComponent<SkillSlotUI>();
                slotUIs[i].Initialize((uint)i, window[i]);       // �� ����UI�鵵 �ʱ�ȭ
            }
        }
        else
        {
            // ũ�Ⱑ ���� ��� ����UI���� �ʱ�ȭ�� ����
            slotUIs = parent.GetComponentsInChildren<SkillSlotUI>();
            for (int i = 0; i < window.SlotSize; i++)
            {
                slotUIs[i].Initialize((uint)i, window[i]);
            }
        }



        RefreshAllSlots();  // ��ü ����UI ����
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
