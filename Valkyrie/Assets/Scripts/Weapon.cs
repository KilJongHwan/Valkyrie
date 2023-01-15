using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject hitEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == false)
        {
            IStatus target = other.GetComponent<Enemy>();
            if (target != null)
            {
                GameManager.Inst.MainPlayer.AttackTarget(target);

                Vector3 hitPoint = transform.position + transform.up;
                Vector3 effectPoint = other.ClosestPoint(hitPoint);
                Instantiate(hitEffect, effectPoint, Quaternion.identity);
                Destroy(hitEffect, 2.0f);
            }
        }
    }
}
