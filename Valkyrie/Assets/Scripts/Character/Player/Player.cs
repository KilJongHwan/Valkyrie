using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IStatus, IEquiptable
{
    public float hp = 100.0f;
    float maxHP = 100.0f;
    public float mp = 100.0f;
    float maxMP = 100.0f;
    public float attackPos = 10.0f;
    public float defensePos = 10.0f;

    public uint level = 1;
    public float currentExp = 0.0f;
    float maxExp = 100.0f;

    public bool isSkillReady = false;
    bool isDead = false;

    public float consumeRange = 10.0f;
    public float interactRange = 5.0f;
    public float criticalRate = 0.3f;

    public GameObject emitPosition;
    public GameObject levelUpEffect = null;
    public GameObject lightningEffect = null;

    uint gold = 0;

    public Spirit spirit;
    GameObject weapon;

    public GameObject targetEffect;
    Transform target = null;

    public Transform Target => target;
    InteractiveActionUI interactUI;

    ItemSlot equipItemSlot;
    public ItemSlot EquipItemSlot
    {
        get => equipItemSlot;
        set
        {
            equipItemSlot = value;
        }
    }

    public Inventory inven;
    public CastData[] castDatas;

    public SkillWindow window;
    BoxCollider weaponCollider;
    Animator anim;
    TargetHP_Bar targetHP;
    TPSCharactorControler controler;
    RedHollowControl hollow;

    public float HP 
    { 
        get => hp;
        set
        {
            if (hp != value)
            {
                hp = Mathf.Clamp(value, 0, maxHP);
                onHPchange?.Invoke();
            }
        }
    }

    public float MaxHP
    {
        get => maxHP;
        set
        {
            maxHP = value;
        }
    }

    public float MP
    {
        get => mp;
        set
        {
            if (mp != value)
            {
                mp = Mathf.Clamp(value, 0, maxMP);
                onMPchange?.Invoke();
            }
        }
    }

    public float MaxMP
    {
        get => maxMP;
        set
        {
            maxMP = value;
        }
    }

    public float AttackPos => attackPos;
    public float DefensePos => defensePos;

    public uint Level
    {
        get => level;
        set
        {
            if (level != value)
            {
                level = value;
                onLevelChange?.Invoke();
                switch (level)
                {
                    case 5:
                        window.AddSkill(SkillCode.Hollow);
                        break;
                    case 10:
                        window.AddSkill(SkillCode.Ice);
                        break;
                    case 15:
                        window.AddSkill(SkillCode.Thunder);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public float Exp
    {
        get => currentExp;
        set
        {
            if (currentExp != value)
            {
                currentExp = value;
                if(currentExp <= maxExp)
                    currentExp = Mathf.Clamp(currentExp, 0, MaxExp);
                if (currentExp >= MaxExp)
                {
                    float amount = MaxExp - currentExp;
                    if (amount < 0)
                    {
                        currentExp = Mathf.Abs(amount);
                        while (currentExp >= maxExp)
                        {
                            Level++;
                            currentExp -= maxExp;
                        }
                    }
                    else
                    {
                        currentExp = 0;
                        Level++;
                    }
                    MaxExp += 10.0f;
                    onEXPchange?.Invoke();
                }
            }
        }
    }

    public float MaxExp
    {
        get => maxExp;
        set
        {
            maxExp = value;
        }
    }
    public uint Gold
    {
        get => gold;
        set
        {
            if (gold != value)
            {
                gold = value;
                onGoldChange?.Invoke(gold);
            }
        }
    }
    public System.Action onHPchange { get; set; }
    public System.Action onMPchange { get; set; }
    public System.Action onEXPchange;
    public System.Action onLevelChange;
    public System.Action<uint> onGoldChange;
    public System.Action onChangeActionState;
    private void Awake()
    {
        controler = GetComponentInParent<TPSCharactorControler>();
        weapon = GetComponentInChildren<FindWeapon>().gameObject;
        interactUI = FindObjectOfType<InteractiveActionUI>();
        targetHP = FindObjectOfType<TargetHP_Bar>();
        anim = GetComponent<Animator>();
        lightningEffect = transform.GetChild(4).gameObject;
        hollow = GetComponentInChildren<RedHollowControl>();
        onLevelChange += LevelUp;
        
        window = new SkillWindow();
        inven = new Inventory();
    }
    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        castDatas = new CastData[gameManager.SkillData.Length];
        for (int i = 0; i < gameManager.SkillData.Length; i++)
        {
            castDatas[i] = new CastData(gameManager.SkillData.skillDatas[i]);
        }
        GameManager.Inst.WindowUI.InitailizeWindow(window);
        ActionUI actionUI = FindObjectOfType<ActionUI>();
        castDatas[0].onCoolTimeChange += actionUI[0].RefreshUI;
        castDatas[0].onCoolTimeChange += actionUI[1].RefreshUI;
        castDatas[0].onCoolTimeChange += actionUI[2].RefreshUI;
        GameManager.Inst.InvenUI.InitializeInventory(inven);
        if (targetEffect == null)
        {
            targetEffect = GameObject.Find("targetEffect");
            targetEffect.SetActive(false);
        }
    }
    private void Update()
    {
        if (!isDead)
        {
            foreach (var data in castDatas)
            {
                if (lightningEffect.activeSelf)
                {
                    data.CurrentDuration += Time.deltaTime;
                }
                else
                {
                    data.CurrentCoolTime -= Time.deltaTime;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            interactUI.action = InteractiveActionUI.ActionType.Interaction;
            spirit = other.GetComponent<Spirit>();
            onChangeActionState?.Invoke();
        }
        if (other.gameObject.CompareTag("Item"))
        {
            interactUI.action = InteractiveActionUI.ActionType.Consume;
            onChangeActionState?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            interactUI.action = InteractiveActionUI.ActionType.Attack;
            spirit = null;
            onChangeActionState?.Invoke();
            target = null;
            if (targetHP.canvas.blocksRaycasts)
            {
                targetHP.TargetHPToggle();
                InteractionOff();
                GameManager.Inst.IsTargeting = false;
            }
        }
        if (other.gameObject.CompareTag("Monster"))
        {
            if (targetHP.canvas.blocksRaycasts)
            {
                targetHP.TargetHPToggle();
                InteractionOff();
                target = null;
                GameManager.Inst.IsTargeting = false;
            }
        }
        if (other.gameObject.CompareTag("Item"))
        {
            interactUI.action = InteractiveActionUI.ActionType.Attack;
            onChangeActionState?.Invoke();
        }
    }
    public void Dead()
    {

    }
    private void LevelUp()
    {
        maxHP += 10.0f;
        maxMP += 10.0f;
        attackPos += Random.Range(1, 5);
        defensePos += Random.Range(1, 5);
        hp = maxHP;
        mp = maxMP;
        GameObject obj = Instantiate(levelUpEffect, transform);
        Destroy(obj, 3.0f);
    }

    public void AttackTarget(IStatus target)
    {
        if (target != null)
        {
            float damage = attackPos;
            if (EquipItemSlot != null && EquipItemSlot.ItemEquiped)
            {
                ItemData_Weapon weapon = EquipItemSlot.SlotItemData as ItemData_Weapon;
                damage += weapon.attackPower;
            }

            if (Random.Range(0.0f, 1.0f) < criticalRate)
            {
                damage *= 2.0f;
            }
            target.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - defensePos;
        if (finalDamage < 1.0f)
        {
            finalDamage = 1.0f;
        }
        HP -= finalDamage;
        if (HP > 0.0f)
        {
            anim.SetTrigger("Hit");
        }
        else
        {

        }
    }
    public void WeaponColliderOn()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
        }
    }
    public void WeaponColliderOff()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }

    public void ItemConsume()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, consumeRange, LayerMask.GetMask("Item"));
        foreach (var col in cols)
        {
            Item item = GetComponent<Item>();

            IConsumable consumable = item.data as IConsumable;
            if (consumable != null)
            {
                consumable.Consume(this);
                Destroy(col.gameObject);
            }
            else
            {
                if (inven.AddItem(item.data))
                {
                    Destroy(col.gameObject);
                }
            }
        }
    }
    public void Targeting()
    {
        if (target == null)
        {
            Interaction();
        }
        else
        {
            if (!Interaction())
            { 
                InteractionOff();
            }
        }
    }
    bool Interaction()
    {
        bool result = false;
        Collider[] cols = Physics.OverlapSphere(transform.position, interactRange, LayerMask.GetMask("NPC", "Monster"));
        if (cols.Length > 0)
        {
            Collider near = null;
            float nearestDistance = float.MaxValue;
            foreach (var col in cols)
            {
                float distanceSqr = (col.transform.position - transform.position).sqrMagnitude;
                if (distanceSqr < nearestDistance)
                {
                    nearestDistance = distanceSqr;
                    near = col;
                }
            }
            if (target?.gameObject != near.gameObject)
            {
                target = near.transform;

                Vector3 dir = target.transform.position - transform.position;
                dir.y = 0.0f;
                if (dir.sqrMagnitude < interactRange * interactRange)
                {
                    targetEffect.transform.position = target.position;
                    targetEffect.transform.parent = target;
                    targetEffect.SetActive(true);
                    targetHP.target = target.GetComponent<IStatus>();
                    targetHP.nameText.text = target.name;
                    targetHP.TargetHPToggle();
                    controler.onLoockup?.Invoke();

                    GameManager.Inst.IsTargeting = true;
                    result = true;
                }
            }
        }
        return result;
    }
    void InteractionOff()
    {
        target = null;
        targetEffect.transform.parent = null;
        targetEffect.SetActive(false);
    }
    public void EquipWeapon(ItemSlot weaponSlot)
    {
        weapon.SetActive(true);
        GameObject obj = Instantiate(weaponSlot.SlotItemData.prefab, weapon.transform);
        obj.transform.localPosition = new(0, 0, 0);
        weaponCollider = obj.GetComponent<BoxCollider>();
        equipItemSlot = weaponSlot;
        weaponSlot.ItemEquiped = true;
    }

    public void UnEquipWeapon()
    {
        equipItemSlot.ItemEquiped = false;
        equipItemSlot = null;
        weaponCollider = null;
        Transform weaponChild = weapon.transform.GetChild(0);
        weaponChild.parent = null;
        Destroy(weaponChild.gameObject);
    }

    public void Casting(SkillCode code)
    {
        if (castDatas[(int)code].IsCastingSkill)
        {
            switch (code)
            {
                case SkillCode.Hollow:
                    hollow.gameObject.transform.position = emitPosition.transform.position;
                    StartCoroutine(SkillHollow());
                    GameManager.Inst.MainPlayer.isSkillReady = true;
                    castDatas[(int)code].ResetCoolTime();
                    break;
                case SkillCode.Ice:
                    SkillFactory.MakeSkill(code, emitPosition.transform.position);
                    GameManager.Inst.MainPlayer.isSkillReady = true;
                    castDatas[(int)code].ResetCoolTime();
                    break;
                case SkillCode.Thunder:
                    castDatas[(int)code].CastDuration();
                    GameManager.Inst.MainPlayer.isSkillReady = true;
                    break;
                default:
                    break;
            }
        }
    }
    IEnumerator SkillHollow()
    {
        hollow.Play_Charging();
        yield return new WaitForSeconds(3.0f);
        hollow.Finish_Charging();
        yield return new WaitForSeconds(1.0f);
        hollow.Burst_Beam();
        yield return new WaitForSeconds(3.0f);
        hollow.Dead();
    }
}
