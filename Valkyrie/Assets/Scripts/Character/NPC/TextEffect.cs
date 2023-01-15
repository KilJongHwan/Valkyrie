using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEffect : MonoBehaviour
{
    public int charPerSeconds;
    string targetMsg;
    TextMeshProUGUI msgText;
    int index;
    float interval;

    private void Awake()
    {
        msgText = GetComponent<TextMeshProUGUI>();
    }
    public void SetMsg(string msg)
    {
        targetMsg = msg;
        EffectStart();
    }
    void EffectStart()
    {
        msgText.text = "";
        index = 0;
        interval = 1.0f / charPerSeconds;

        Invoke("Effecting",interval);
    }

    void Effecting()
    {
        if (msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }
        msgText.text += targetMsg[index];
        index++;

        Invoke("Effecting", interval);
    }
    void EffectEnd()
    {

    }
}
