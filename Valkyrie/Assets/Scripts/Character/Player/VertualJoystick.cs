using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class VertualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]  private RectTransform lever;
    private RectTransform rectTransform;
    [SerializeField, Range(10,150)]
    private float leverRange;
    private Vector2 inputDrection;
    private bool isInput;
    [SerializeField] private TPSCharactorControler controller;
    public enum JoystickType { Move, Rotate}
    [SerializeField] public JoystickType _JoystickType;
    [SerializeField] float _sensitivity = 1f;

    Player player;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        player = GameManager.Inst.MainPlayer;
    }
    void Update()
    {
        if (isInput)
        {
            InputControlVector();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
        isInput = true;
    }
    // 오브젝트를 클릭해서 드래그 하는 도중에 들어오는 함수
    // 하지만 클릭을 유지한 상태로 마우스를 멈추면 이벤트가 들어오지 않음
    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
        switch (_JoystickType)
        {
            case JoystickType.Move:
                controller.Move(Vector2.zero);
                if (player.Target != null)
                {
                    controller.targetRotation = Quaternion.LookRotation(player.Target.position - player.transform.position);
                }
                break;
            case JoystickType.Rotate:
                StartCoroutine(Control());
                break;
        }
    }
    IEnumerator Control()
    {
        yield return new WaitForSeconds(3.0f);
        controller._cameraArm.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        GameManager.Inst.IsControlCam = false;
    }
    private void ControlJoystickLever(PointerEventData eventData)
    {
        var inputPos = eventData.position - rectTransform.anchoredPosition;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        inputDrection = inputVector / leverRange;   // 해상도 기준이라 캐릭터로 쓰기엔 값이 너무 큼
    }

    private void InputControlVector()
    {
        // 캐릭터에게 입력 vector 전달
        switch (_JoystickType)
        {
            case JoystickType.Move:
                controller.Move(inputDrection * _sensitivity); 
                if (player.Target != null)
                {
                    controller.targetRotation = Quaternion.LookRotation(player.Target.position - player.transform.position);
                }
                break;
            case JoystickType.Rotate:
                controller.LookAround(inputDrection * _sensitivity);
                GameManager.Inst.IsControlCam = true;
                break;
        }
        //_PlayerAnimater.Play()

    }
   
}
