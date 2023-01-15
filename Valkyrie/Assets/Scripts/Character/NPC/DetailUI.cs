using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DetailUI : MonoBehaviour
{
    uint itemCount = 1;

    TextMeshProUGUI itemName;
    TextMeshProUGUI itemPrice;
    Image itemIcon;
    CanvasGroup canvas;
    ItemData itemData;
    Button buyButton;
    Button cancelButton;
    TMP_InputField inputField;

    public Action<ItemData, uint> OnBuy;

    public uint ItemCount
    {
        get => itemCount;
        set
        {
            if (itemCount != value)
            {
                itemCount = value;
                itemCount = (uint)Mathf.Max(1, itemCount);
                if (itemData != null)
                {
                    itemCount = (uint)Mathf.Min(itemCount, itemData.maxStackCount);
                }
                inputField.text = itemCount.ToString();
            }
        }
    }

    public void Open(ItemData data)
    {
        itemData = data;
        Refresh();
        canvas.alpha = 1; 
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }
    public void Close()
    {
        itemData = null;
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }
    public void Refresh()
    {
        if (itemData != null)
        {
            ItemCount = 1;
            itemName.text = itemData.itemName;
            itemPrice.text = itemData.value.ToString();
            itemIcon.sprite = itemData.itemIcon;
        }
    }
    private void Awake()
    {
        itemName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        itemIcon = transform.Find("Icon").GetComponent<Image>();
        itemPrice = transform.Find("Value").GetComponent<TextMeshProUGUI>();
        canvas = GetComponent<CanvasGroup>();

        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.onValueChanged.AddListener(OnInputChange);
        inputField.text = itemCount.ToString();

        buyButton = transform.Find("BuyButton").GetComponent<Button>();
        cancelButton = transform.Find("CancelButton").GetComponent<Button>();

        buyButton.onClick.AddListener(Buy);
        cancelButton.onClick.AddListener(Close);
    }

    private void OnInputChange(string input)
    {
        if (input.Length == 0)
        {
            ItemCount = 0; // ""인 경우 0으로 처리
        }
        else
        {
            ItemCount = uint.Parse(input); // uint 파싱해서 ItemSplitCount에 대입
        }
    }

    private void Buy()
    {
        OnBuy?.Invoke(itemData, itemCount);
        Close();
    }
}
