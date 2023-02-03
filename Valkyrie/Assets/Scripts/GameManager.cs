using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    Player player;
    SkillWindowUI windowUI;
    SkillDataManager skillData;
    InventoryUI invenUI;
    CommunicationManager talkData;
    QuestManager quest;
    QuestUI questUI;
    ShopUI shopUI;
    SpiritUI spiritUI;
    CharacterData saveData;

    bool isSkillSet = false;
    bool isControlCam = false;
    bool isTargeting = false;
    bool isWarp = false;

    ItemDataManager itemData;
    static GameManager instence = null;

    public static Action onSkillset;

    public static GameManager Inst
    {
        get => instence;
    }
    public Player MainPlayer
    {
        get => player;
    }
    public SkillDataManager SkillData
    {
        get => skillData;
    }
    public ItemDataManager ItemData
    {
        get => itemData;
    }
    public InventoryUI InvenUI
    {
        get => invenUI;
    }
    public SkillWindowUI WindowUI
    {
        get => windowUI;
    }
    public CommunicationManager TalkData
    {
        get => talkData;
    }
    public QuestManager Quest
    {
        get => quest;
    }
    public QuestUI QuestUI
    {
        get => questUI;
    }
    public ShopUI ShopUI
    {
        get => shopUI;
    }
    public SpiritUI SpiritUI
    {
        get => spiritUI;
    }
    public bool IsSkillSet
    {
        get => isSkillSet;
        set
        {
            isSkillSet = value;
        }
    }
    public bool IsControlCam
    {
        get => isControlCam;
        set
        {
            isControlCam = value;
        }
    }
    public bool IsTargeting
    {
        get => isTargeting;
        set
        {
            isTargeting = value;
        }
    }
    public bool IsWarp
    {
        get => isWarp;
        set
        {
            isWarp = value;
        }
    }
    private void Awake()
    {
        if (instence == null)
        {
            instence = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            if (instence != this)
            {
                Destroy(this.gameObject);
            }
        }
    }
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Initialize();
    }

    private void Initialize()
    {
        player = FindObjectOfType<Player>();
        windowUI = FindObjectOfType<SkillWindowUI>();
        invenUI = FindObjectOfType<InventoryUI>();
        shopUI = FindObjectOfType<ShopUI>();
        spiritUI = FindObjectOfType<SpiritUI>();
        skillData = GetComponent<SkillDataManager>();
        itemData = GetComponent<ItemDataManager>();
        talkData = GetComponent<CommunicationManager>();
        quest = GetComponent<QuestManager>();
    }
    public void SaveData(Player sPlayer)
    {
        saveData = new CharacterData();
        saveData.Level = sPlayer.Level;
        saveData.Gold = sPlayer.Gold;
        saveData.MaxHP = sPlayer.MaxHP;
        saveData.MaxMP = sPlayer.MaxMP;
        saveData.Exp = sPlayer.Exp;
        saveData.MaxExp = sPlayer.MaxExp;
        saveData.Inven = new Inventory();
        saveData.Inven = sPlayer.inven;
        saveData.InvenSaveUI = sPlayer.invenUI;
        saveData.SkillWindow = new SkillWindow();
        saveData.SkillWindow = sPlayer.window;
        saveData.SkillWindowSave = sPlayer.windowUI;
    }
    public bool LoadData(Player sPlayer)
    {
        bool result = false;
        if (saveData != null)
        {
            sPlayer.Level = saveData.Level;
            sPlayer.Gold = saveData.Gold;
            sPlayer.MaxHP = saveData.MaxHP;
            sPlayer.MaxMP = saveData.MaxMP;
            sPlayer.MaxExp = saveData.MaxExp;
            sPlayer.Exp = saveData.Exp;
            sPlayer.inven = saveData.Inven;
            sPlayer.invenUI = saveData.InvenSaveUI;
            sPlayer.window = saveData.SkillWindow;
            sPlayer.windowUI = saveData.SkillWindowSave;
            sPlayer.onGoldChange?.Invoke(sPlayer.Gold);
            invenUI.InitializeInventory(sPlayer.inven);
            windowUI.InitailizeWindow(sPlayer.window);
            result = true;
        }
        return result;
    }
}