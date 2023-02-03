using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHollowControl : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float hue = 0;
    public float skillDamage = 50.0f;

    Animator animator;

    List<IStatus> attackTarget;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        attackTarget = new List<IStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).GetComponent<HueControl>().hue = hue;
        if (attackTarget != null)
        {
            foreach (var attack in attackTarget)
            {
                attack.TakeDamage(skillDamage);
            }
        }
    }

    public void Play_Charging() {
        animator.Play("Red Hollow - Charging");
    }

    public void Finish_Charging() {
        animator.Play("Red Hollow - Charged");
    }

    public void Burst_Beam() {
        animator.Play("Red Hollow - Burst");
	//if(Camera.main.transform.GetComponent<CameraShake>() != null){
	//	Camera.main.transform.GetComponent<CameraShake>().Shake(0.5f, 1f);
	//}
    }

    public void Dead()
    {
        animator.Play("Red Hollow - Dead");
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger == false)
        {
            IStatus target = other.gameObject.GetComponent<IStatus>();
            if (target != null)
            {
                attackTarget.Add(target);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger == false)
        {
            attackTarget.Remove(other.gameObject.GetComponent<IStatus>());
        }
    }
}
