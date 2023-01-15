using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    public float activeRange = 20.0f;
    public int ID = 0;

    Inventory shopInven;
    
    private void Awake()
    {
        shopInven = new Inventory();
    }
    private void Start()
    {
        GameManager.Inst.ShopUI.InitailizeShop(shopInven);
        shopInven.AddItem(ItemCode.HealingPotion);
        shopInven.AddItem(ItemCode.ManaPotion);
        shopInven.AddItem(ItemCode.Sword01);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GameManager.Inst.ShopUI.canvas.blocksRaycasts)
            {
                GameManager.Inst.ShopUI.OnOff();
            }
        }
    }
}
