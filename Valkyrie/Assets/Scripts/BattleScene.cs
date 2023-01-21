using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    private void Start()
    {
        GameManager.Inst.LoadData(GameManager.Inst.MainPlayer);
    }
}
