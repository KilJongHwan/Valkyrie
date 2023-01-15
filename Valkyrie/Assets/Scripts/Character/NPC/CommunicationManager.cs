using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationManager : MonoBehaviour
{
    Dictionary<int, string[]> talkDatas;
    private void Awake()
    {
        talkDatas = new Dictionary<int, string[]>();
        GenerateData();
    }

    private void GenerateData()
    {
        talkDatas.Add(1000, new string[] { "Hello", "my friend" });

        talkDatas.Add(2000, new string[] { "The name's Macree" });

        talkDatas.Add(10 + 1000, new string[] { "So you can do that", "I'll show you quest" });

        talkDatas.Add(11 + 2000, new string[] { "How are you?"});

        talkDatas.Add(20 + 2000, new string[] { "My Quest is here", "And More?" });

    }
    public string GetCommunication(int id,  int index)
    {
        if (!talkDatas.ContainsKey(id))
        {
            if(!talkDatas.ContainsKey(id - id % 10))
               return GetCommunication(id - id % 100, index);
            else
               return GetCommunication(id - id % 10, index);
        }
        if (index == talkDatas[id].Length)
            return null;
        else
            return talkDatas[id][index];
    }
}