using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotater : MonoBehaviour
{
    float volPlaneSpeed = 1.0f;
    float turnSpeed = 360.0f;

    float timeElapesd = 0.0f;
    Vector3 startPosition;
    float moveHalf;

    private void Start()
    {
        startPosition = transform.position;
        moveHalf = volPlaneSpeed * 0.5f;
    }

    private void Update()
    {
        timeElapesd +=  Time.deltaTime;
        transform.position =  startPosition + moveHalf * new Vector3(0, 1-Mathf.Cos(timeElapesd), 0);
        transform.Rotate(turnSpeed * Time.deltaTime * Vector3.up, Space.World);
    }
}
