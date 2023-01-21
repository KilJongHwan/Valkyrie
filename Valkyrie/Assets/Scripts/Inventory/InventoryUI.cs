using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour ,IDragHandler
{

    public GameObject slotPrefab;   // 초기화시 새로 생성해야할 경우 사용

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
        inven = newInven;   //즉시 할당
        if (Inventory.Default_Inventory_Size != newInven.SlotCount)    // 기본 사이즈와 다르면 기본 슬롯UI 삭제
        {
            // 기존 슬롯UI 전부 삭제
            ItemSlotUI[] slots = GetComponentsInChildren<ItemSlotUI>();
            foreach (var slot in slots)
            {
                Destroy(slot.gameObject);
            }

            // 새로 만들기
            slotUIs = new ItemSlotUI[inven.SlotCount];
            for (int i = 0; i < inven.SlotCount; i++)
            {
                GameObject obj = Instantiate(slotPrefab, slotParent);
                obj.name = $"{slotPrefab.name}_{i}";            // 이름 지어주고
                slotUIs[i] = obj.GetComponent<ItemSlotUI>();
                slotUIs[i].Initialize((uint)i, inven[i]);       // 각 슬롯UI들도 초기화
            }
        }
        else
        {
            // 크기가 같을 경우 슬롯UI들의 초기화만 진행
            slotUIs = slotParent.GetComponentsInChildren<ItemSlotUI>();
            for (int i = 0; i < inven.SlotCount; i++)
            {
                slotUIs[i].Initialize((uint)i, inven[i]);
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

 
    private void RefreshMoney(uint money)
    {
        goldText.text = $"{money:N0}";
    }

    public void InventoryOnOffSwitch()
    {
        if (canvasGroup.blocksRaycasts)  // 캔버스 그룹의 blocksRaycasts를 기준으로 처리
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
        inven.TempRemoveItem(slotID, count);    // slotID에서 count만큼 덜어내서 TempSlot에 옮기기
        
    }

  
    public void OnDrag(PointerEventData eventData)
    {

    }
}
