using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public QuestText[] qTexts;
    private void Awake()
    {
        qTexts = GetComponentsInChildren<QuestText>();
    }
    private void Start()
    {
        foreach (var text in qTexts)
        {
            text.gameObject.SetActive(false);
        }
    }
}
