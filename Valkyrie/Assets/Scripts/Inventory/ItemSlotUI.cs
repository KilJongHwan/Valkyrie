using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IPointerMoveHandler, IPointerClickHandler
{
    uint id;

    protected ItemSlot itemSlot;


    InventoryUI invenUI;


    protected Image itemImage;

    protected TextMeshProUGUI countText;

    protected Image equipMark;
    

    public uint ID { get => id; }

    public ItemSlot ItemSlot { get => itemSlot; }

    protected virtual void Awake() 
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();  
        countText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        equipMark = transform.GetChild(2).GetComponent<Image>();
        equipMark.gameObject.SetActive(false);
    }

  
    public void Initialize(uint newID, ItemSlot targetSlot)
    {
        invenUI = GameManager.Inst.InvenUI; // 미리 찾아놓기

        id = newID;
        itemSlot = targetSlot;
        itemSlot.onSlotItemChange = Refresh; // ItemSlot에 아이템이 변경될 경우 실행될 델리게이트에 함수 등록        
    }

   
    public void Refresh()
    {
        if (itemSlot.SlotItemData != null)
        {
            itemImage.sprite = itemSlot.SlotItemData.itemIcon;  // 아이콘 이미지 설정하고
            itemImage.color = Color.white;  // 불투명하게 만들기
            countText.text = itemSlot.ItemCount.ToString();

            equipMark.gameObject.SetActive((itemSlot.SlotItemData is ItemData_Weapon) && itemSlot.ItemEquiped);

        }
        else
        {
            // 이 슬롯에 아이템이 없을 때
            itemImage.sprite = null;        // 아이콘 이미지 제거하고
            itemImage.color = Color.clear;  // 투명하게 만들기
            countText.text = "";
            equipMark.gameObject.SetActive(false);
        }
    }

    public void ClearEquipMark()
    {
        equipMark.gameObject.SetActive(false);
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        Vector2 mousePos = eventData.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!itemSlot.IsEmpty())
        {
            ItemSlot.UseSlotItem(GameManager.Inst.MainPlayer.gameObject);
            bool isEquiped = itemSlot.EquipSlotItem(GameManager.Inst.MainPlayer.gameObject);
            if (isEquiped)
            {
                invenUI.ClearAllEquipMark();
            }
            itemSlot.ItemEquiped = isEquiped;
        }
    }
}
