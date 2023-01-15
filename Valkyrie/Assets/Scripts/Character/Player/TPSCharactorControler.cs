using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCharactorControler : MonoBehaviour
{
    [SerializeField] private Transform _charactorBody;
    [SerializeField] public Transform _cameraArm;

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
        Ray dir = new(_charactorBody.transform.position, _charactorBody.forward);

        if (GameManager.Inst.MainPlayer.Target != null)
        {
            Vector3 look = GameManager.Inst.MainPlayer.Target.position - _charactorBody.transform.position;
            look.y = 0.0f;
            _charactorBody.transform.rotation = Quaternion.Lerp(_charactorBody.transform.rotation, Quaternion.LookRotation(look), turnSpeed * Time.deltaTime);

            //while (!Physics.Raycast(dir, out RaycastHit hit, 100.0f, LayerMask.GetMask("NPC", "Monster")))
            //{
            //    _charactorBody.transform.rotation = Quaternion.Slerp(_charactorBody.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            //    if (GameManager.Inst.MainPlayer.Target == null)
            //        break;
            //    yield return new WaitForSeconds(0.1f);
            //}
            yield return new WaitForSeconds(5.0f);
        }
    }
    public void Move(Vector2 inputDirection)
    {
        Vector2 _moveInput = inputDirection;
        bool _isMove = _moveInput.magnitude != 0;
        //animator.SetBool("ismove", _isMove);
        anim.SetFloat("moveSpeed", _moveInput.magnitude);
        if (_isMove)
        {
            Vector3 _lookForward = new Vector3(_cameraArm.forward.x, 0f, _cameraArm.forward.z).normalized;
            Vector3 _lookRight = new Vector3(_cameraArm.right.x, 0f, _cameraArm.right.z).normalized;
            Vector3 _moveDir = _lookForward * _moveInput.y + _lookRight * _moveInput.x;

            if (GameManager.Inst.IsTargeting)
            {
                _charactorBody.transform.rotation = Quaternion.Slerp(_charactorBody.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
            else
            {
                _charactorBody.forward = _moveDir;
            }

            transform.position += _moveDir * Time.deltaTime * speed;
        }
    }
    public void LookAround(Vector2 inputDirection)
    {
        //new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
        Vector2 _camDelta = inputDirection;
        Vector3 _camAngle = _cameraArm.rotation.eulerAngles;

        float x = _camAngle.x - _camDelta.y;
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);   // 0으로 제한하지 않는 이유는 0이 될때 수평면으로 내려가지않는것을 방지
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        _cameraArm.rotation = Quaternion.Euler(x, _camAngle.y - _camDelta.x, _camAngle.z);
    }
    
}
