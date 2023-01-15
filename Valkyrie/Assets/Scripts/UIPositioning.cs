using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPositioning : MonoBehaviour
{
    public float moveQuantity = 1.0f;

    float timeElapsed = 0;
    Vector3 startPos;
    float half;
    private void Start()
    {
        startPos = transform.position;
        half = moveQuantity * 0.5f;
    }
    private void Update()
    {
        timeElapsed += Time.deltaTime * 2.0f;
        transform.position = startPos + half * new Vector3(0, (1 - Mathf.Cos(timeElapsed)));
    }
}
