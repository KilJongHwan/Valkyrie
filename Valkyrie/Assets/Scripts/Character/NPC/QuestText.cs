using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestText : MonoBehaviour
{
    public TextMeshProUGUI tittle;
    public TextMeshProUGUI detail;

    private void Awake()
    {
        tittle = GetComponent<TextMeshProUGUI>();
        detail = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void Upload(string content, string details)
    {
        tittle.text = content;
        detail.text = details;
    }
}
