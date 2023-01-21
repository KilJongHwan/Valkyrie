using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour ,IDragHandler
{

    public GameObject slotPrefab;   // �ʱ�ȭ�� ���� �����ؾ��� ��� ���

    Player player;

    Inventory inven;

    Transform slotParent;

    ItemSlotUI[] slotUIs;

    CanvasGroup canvasGroup;

    Button openCloseButton;

    const uint InvalideID = uint.MaxValue;

    uint dragStartID = InvalideID;


    TextMeshProUGUI goldText;

    public Action OnInventoryOpen;
    public Action OnInventoryClose;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        goldText = transform.Find("Gold").Find("GoldText").GetComponent<TextMeshProUGUI>();
        slotParent = transform.Find("ItemSlots");

        openCloseButton = GameObject.Find("ItemInventoryOnOffButton").GetComponent<Button>();
        openCloseButton.onClick.AddListener(InventoryOnOffSwitch);
    }

    private void Start()
    {
        player = GameManager.Inst.MainPlayer;   
        player.onGoldChange += RefreshMoney;   
        RefreshMoney(player.Gold); 
    }
 
    public void InitializeInventory(Inventory newInven)
    {
        inven = newInven;   //��� �Ҵ�
        if (Inventory.Default_Inventory_Size != newInven.SlotCount)    // �⺻ ������� �ٸ��� �⺻ ����UI ����
        {
            // ���� ����UI ���� ����
            ItemSlotUI[] slots = GetComponentsInChildren<ItemSlotUI>();
            foreach (var slot in slots)
            {
                Destroy(slot.gameObject);
            }

            // ���� �����
            slotUIs = new ItemSlotUI[inven.SlotCount];
            for (int i = 0; i < inven.SlotCount; i++)
            {
                GameObject obj = Instantiate(slotPrefab, slotParent);
                obj.name = $"{slotPrefab.name}_{i}";            // �̸� �����ְ�
                slotUIs[i] = obj.GetComponent<ItemSlotUI>();
                slotUIs[i].Initialize((uint)i, inven[i]);       // �� ����UI�鵵 �ʱ�ȭ
            }
        }
        else
        {
            // ũ�Ⱑ ���� ��� ����UI���� �ʱ�ȭ�� ����
            slotUIs = slotParent.GetComponentsInChildren<ItemSlotUI>();
            for (int i = 0; i < inven.SlotCount; i++)
            {
                slotUIs[i].Initialize((uint)i, inven[i]);
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

 
    private void RefreshMoney(uint money)
    {
        goldText.text = $"{money:N0}";
    }

    public void InventoryOnOffSwitch()
    {
        if (canvasGroup.blocksRaycasts)  // ĵ���� �׷��� blocksRaycasts�� �������� ó��
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
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        OnInventoryOpen?.Invoke();
    }

    void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        OnInventoryClose?.Invoke();
    }

    public void ClearAllEquipMark()
    {
        foreach (var slot in slotUIs)
        {
            slot.ClearEquipMark();
        }
    }

    private void OnSpliteOK(uint slotID, uint count)
    {
        inven.TempRemoveItem(slotID, count);    // slotID���� count��ŭ ����� TempSlot�� �ű��
        
    }

  
    public void OnDrag(PointerEventData eventData)
    {

    }
}
