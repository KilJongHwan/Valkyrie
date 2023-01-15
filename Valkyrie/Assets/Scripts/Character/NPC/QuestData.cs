using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    public string[] questText;
    public int[] ID;

    public QuestData(string[] text, int[] id)
    {
        questText = text;
        ID = id;
    }
}
