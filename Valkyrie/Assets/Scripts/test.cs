using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class test : MonoBehaviour
{
    void Start()
    {
        SkillWindow window = new SkillWindow();
        SkillWindowUI skillWindowUI = FindObjectOfType<SkillWindowUI>();
        skillWindowUI.InitailizeWindow(window);

        window.AddSkill(SkillCode.Hollow);

        window.PrintInventory();


        Inventory inven = new Inventory();
        InventoryUI invenUI = FindObjectOfType<InventoryUI>();
        invenUI.InitializeInventory(inven);

        inven.AddItem(ItemCode.HealingPotion);
        inven.PrintInventory();
    }
}
