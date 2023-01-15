using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour, IPointerClickHandler
{
    uint id;

    ShopUI shopUI;

    protected ItemSlot shopItemSlot;

    protected Image itemImage;

    public ItemSlot ShopItemSlot => shopItemSlot;
    public uint ID => id;

    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
    }
    public void Initialize(uint newID, ItemSlot targetSlot)
    {
        shopUI = GameManager.Inst.ShopUI;

        id = newID;
        shopItemSlot = targetSlot;
        shopItemSlot.onSlotItemChange = Refresh;
    }
    public void Refresh()
    {
        if (shopItemSlot.SlotItemData != null)
        {
            itemImage.sprite = shopItemSlot.SlotItemData.itemIcon;
            itemImage.color = Color.white; 
        }
        else
        {
            itemImage.sprite = null;      
            itemImage.color = Color.clear; 
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (shopItemSlot.SlotItemData != null)
        {
            shopUI.Detail.Open(shopItemSlot.SlotItemData);
        }
    }
}
