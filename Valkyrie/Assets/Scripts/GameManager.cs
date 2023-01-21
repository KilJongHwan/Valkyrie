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
    ShopUI shopUI;
    CharacterData saveData;

    bool isSkillSet = false;
    bool isControlCam = false;
    bool isTargeting = false;
    bool isWarp = false;

    ItemDataManager itemData;
    static GameManager instence = null;

    public Action onSkillset;

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
    public ShopUI ShopUI
    {
        get => shopUI;
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
        skillData = GetComponent<SkillDataManager>();
        itemData = GetComponent<ItemDataManager>();
        talkData = GetComponent<CommunicationManager>();
        quest = GetComponent<QuestManager>();
    }
    public void SaveData(Player sPlayer)
    {
        saveData = new CharacterData
        {
            Level = sPlayer.Level,
            Gold = sPlayer.Gold,
            MaxHP = sPlayer.MaxHP,
            MaxMP = sPlayer.MaxMP,
            Exp = sPlayer.Exp,
            MaxExp = sPlayer.MaxExp,
            Inven = sPlayer.inven,
            InvenSaveUI = InvenUI,
            EquipItemSlot = sPlayer.EquipItemSlot,
            SkillWindow = sPlayer.window,
            SkillWindowSave = WindowUI
        };
    }
    public void LoadData(Player sPlayer)
    {
        sPlayer.Level = saveData.Level;
        sPlayer.Gold = saveData.Gold;
        sPlayer.MaxHP = saveData.MaxHP;
        sPlayer.MaxMP = saveData.MaxMP;
        sPlayer.Exp = saveData.Exp;
        sPlayer.MaxExp = saveData.MaxExp;
        sPlayer.inven = saveData.Inven;
        invenUI = saveData.InvenSaveUI;
        sPlayer.EquipItemSlot = saveData.EquipItemSlot;
        sPlayer.window = saveData.SkillWindow;
        windowUI = saveData.SkillWindowSave;
        sPlayer.onGoldChange?.Invoke(sPlayer.Gold);
    }
}