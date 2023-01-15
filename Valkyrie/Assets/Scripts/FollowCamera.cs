using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float speed = 3.0f;

    Vector3 offset = Vector3.zero;
    Action onTurn;
    void Start()
    {
        if (target == null)
        {
            target = FindObjectOfType<Player>().transform;
        }
        offset = transform.position - target.transform.position;

        onTurn += TurnCamera;
    }
    private void LateUpdate()
    {
        if (target.transform.eulerAngles.y > (90.0f - 0.001f) && target.transform.eulerAngles.y < (270.0f - 0.001f))
        {
            transform.position = Vector3.Lerp(transform.position, target.position - (offset + offset) + new Vector3(0.0f,6.0f,-3.0f), speed * Time.deltaTime);
            if(!GameManager.Inst.IsControlCam)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(30.0f, 180.0f, 0.0f), speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
            if (!GameManager.Inst.IsControlCam)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(30.0f, 0.0f, 0.0f), speed * Time.deltaTime);
        }
    }
    void TurnCamera()
    {
    }
}
