using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    Player player;

    Inventory shopInven;

    Transform parentSlot;

    ShopSlotUI[] slotUIs;

    public CanvasGroup canvas;

    DetailUI detail;
    public DetailUI Detail => detail;

    const uint InvalideID = uint.MaxValue;

    uint dragStartID = InvalideID;


    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        detail = GetComponentInChildren<DetailUI>();
        parentSlot = transform.Find("ShopSlots");
    }
    private void Start()
    {
        player = GameManager.Inst.MainPlayer;
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
    public void OnOff()
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
    public void InitailizeShop(Inventory newInven)
    {
        shopInven = newInven;
        slotUIs = parentSlot.GetComponentsInChildren<ShopSlotUI>();
        for (int i = 0; i < shopInven.SlotCount; i++)
        {
            slotUIs[i].Initialize((uint)i, shopInven[i]);
        }
        detail.OnBuy += BuyItem;
        RefreshAllSlots();
    }

    private void BuyItem(ItemData data, uint count)
    {
        if (player.Gold >= (data.value * count))
        {
            if (count == 1)
            {
                player.inven.BuySlotItem(data);
                player.Gold -= data.value;
            }
            else
            {
                count = count / 2;
                while (count > 0)
                {
                    player.inven.BuySlotItem(data);
                    player.Gold -= data.value;
                    count--;
                    if (player.Gold - data.value < 0)
                        break;
                }
            }
        }
        else
        {
            Debug.Log("Not Enough Gold!");
        }
    }

    void RefreshAllSlots()
    {
        foreach (var slotUI in slotUIs)
        {
            slotUI.Refresh();
        }
    }
}
