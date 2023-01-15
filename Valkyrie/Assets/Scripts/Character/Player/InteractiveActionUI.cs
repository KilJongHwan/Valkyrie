using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractiveActionUI : MonoBehaviour, IPointerClickHandler
{
    public enum ActionType
    {
        Attack,
        Interaction,
        Consume
    }
    public ActionType action = ActionType.Attack;
    public Sprite[] actionImages;


    Image image;
    Animator anim;
    Player player;
    SpiritUI spiritUI;
    Enemy enemy;

    private void Awake()
    {
        enemy = FindObjectOfType<Enemy>();
        spiritUI = FindObjectOfType<SpiritUI>();
        anim = FindObjectOfType<Player>().GetComponent<Animator>();
        image = GetComponent<Image>();
    }
    private void Start()
    {
        player = GameManager.Inst.MainPlayer;
        player.onChangeActionState += ChangeState;
    }

    private void ChangeState()
    {
        switch (action)
        {
            case ActionType.Attack:
                image.sprite = actionImages[0];
                break;
            case ActionType.Interaction:
                image.sprite = actionImages[1];
                break;
            case ActionType.Consume:
                image.sprite = actionImages[2];
                break;
            default:
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (action)
        {
            case ActionType.Attack:
                if (player.EquipItemSlot != null)
                {
                    if (!GameManager.Inst.IsTargeting)
                    {
                        player.Targeting();
                    }
                    else
                    {
                        anim.SetFloat("ComboState", Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1.0f));
                        anim.ResetTrigger("Attack");
                        anim.SetTrigger("Attack");
                    }
                }
                GameManager.Inst.MainPlayer.Level++;
                GameManager.Inst.MainPlayer.Gold += 5000;
                GameManager.Inst.Quest.QuestPoint++;
                break;
            case ActionType.Interaction:

                if (!GameManager.Inst.IsTargeting)
                {
                    if (!GameManager.Inst.ShopUI.canvas.blocksRaycasts)
                    {
                        player.Targeting();
                        if(GameManager.Inst.IsTargeting)
                        spiritUI.SpiritUIOnOff();
                    }
                }
                break;
            case ActionType.Consume:
                player.ItemConsume();
                break;
            default:
                break;
        }

    }

}
