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
        invenUI = GameManager.Inst.InvenUI; // �̸� ã�Ƴ���

        id = newID;
        itemSlot = targetSlot;
        itemSlot.onSlotItemChange = Refresh; // ItemSlot�� �������� ����� ��� ����� ��������Ʈ�� �Լ� ���        
    }

   
    public void Refresh()
    {
        if (itemSlot.SlotItemData != null)
        {
            itemImage.sprite = itemSlot.SlotItemData.itemIcon;  // ������ �̹��� �����ϰ�
            itemImage.color = Color.white;  // �������ϰ� �����
            countText.text = itemSlot.ItemCount.ToString();

            equipMark.gameObject.SetActive((itemSlot.SlotItemData is ItemData_Weapon) && itemSlot.ItemEquiped);

        }
        else
        {
            // �� ���Կ� �������� ���� ��
            itemImage.sprite = null;        // ������ �̹��� �����ϰ�
            itemImage.color = Color.clear;  // �����ϰ� �����
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
