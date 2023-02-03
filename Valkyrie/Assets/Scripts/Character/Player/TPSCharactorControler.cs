using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCharactorControler : MonoBehaviour
{
    [SerializeField] private Transform charactorBody;
    [SerializeField] public Transform cameraArm;

    public float speed = 3.0f;
    public float turnSpeed = 10.0f;

    public Quaternion targetRotation = Quaternion.identity;

    Animator anim;

    public System.Action onLoockup;

    private void Start()
    {
        anim = GameManager.Inst.MainPlayer.GetComponent<Animator>();
        onLoockup += () =>
        {
            StartCoroutine(Lookup());
        };
    }
    IEnumerator Lookup()
    {
        Ray ray = new(charactorBody.transform.position, charactorBody.forward);

        if (GameManager.Inst.MainPlayer.Target != null)
        {
            
                Vector3 look = GameManager.Inst.MainPlayer.Target.position - charactorBody.transform.position;
                look.y = 0.0f;
                charactorBody.transform.rotation = Quaternion.Lerp(charactorBody.transform.rotation, Quaternion.LookRotation(look), turnSpeed * Time.deltaTime);

                yield return new WaitForSeconds(3.0f);
        }
    }
    public void Move(Vector2 inputDirection)
    {
        Vector2 moveInput = inputDirection;
        bool isMove = moveInput.magnitude != 0;
        //animator.SetBool("ismove", _isMove);
        anim.SetFloat("moveSpeed", moveInput.magnitude);
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            if (GameManager.Inst.IsTargeting)
            {
                Quaternion lookRotation = Quaternion.Slerp(charactorBody.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                charactorBody.transform.rotation = lookRotation;
            }
            else
            {
                charactorBody.forward = moveDir;
            }

            transform.position += moveDir * Time.deltaTime * speed;
        }
    }
    public void LookAround(Vector2 inputDirection)
    {
        //new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
        Vector2 camDelta = inputDirection;
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        float x = camAngle.x - camDelta.y;
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);   // 0으로 제한하지 않는 이유는 0이 될때 수평면으로 내려가지않는것을 방지
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        cameraArm.rotation = Quaternion.Euler(x, camAngle.y - camDelta.x, camAngle.z);
    }
    
}
