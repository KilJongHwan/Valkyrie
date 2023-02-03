using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    Player player;
    private void Start()
    {
        player = GameManager.Inst.MainPlayer;
        GameManager.Inst.LoadData(player);
        player.HP = player.MaxHP;
        player.MP = player.MaxMP;
        player.Exp += 1500.0f;
    } 
}
