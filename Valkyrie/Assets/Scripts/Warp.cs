using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    WarpUI warpUI;
    private void Awake()
    {
        warpUI = FindObjectOfType<WarpUI>();
    }
    private void OnTriggerEnter(Collider other)
    {
        warpUI.gameObject.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        warpUI.gameObject.SetActive(false);
    }
}
