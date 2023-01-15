using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class LoadingSceneControler : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI loadingText;
    public string nextSceneName = "RPGgame";

    AsyncOperation async;

    PlayerInput inputs;

    WaitForSeconds waitSeconds;

    IEnumerator loadingTextCoroutine;
    IEnumerator loadSceneCoroutine;

    float loadRatio = 0.0f;
    bool loadCompleted = false;

    float sliderUpdateSpeed = 1.0f;

    private void Awake()
    {
        inputs = new PlayerInput();
    }
    private void OnEnable()
    {
        inputs.Loading.Enable();
        inputs.Loading.Press.performed += MousePress;
        inputs.Loading.Touch.performed += TouchPress;
    }

    private void OnDisable()
    {
        inputs.Loading.Touch.performed -= TouchPress;
        inputs.Loading.Press.performed -= MousePress;
        inputs.Loading.Disable();
    }

    void Start()
    {
        waitSeconds = new WaitForSeconds(0.2f);
        loadingTextCoroutine = LoadingTextProcess();
        StartCoroutine(loadingTextCoroutine);
        loadSceneCoroutine = LoadScene();
        StartCoroutine(loadSceneCoroutine);
    }
    private void Update()
    {
        //slider.value = Mathf.Lerp(slider.value, loadRatio, Time.deltaTime * sliderUpdateSpeed);

        // slider의 value가 아직 loadRatio보다 낮으면 빠르게 loadRatio까지 올리는 것이 목적
        if (slider.value < loadRatio)
        {
            // slider가 아직 loadRatio까지 도달하지 않았음.
            slider.value += Time.deltaTime * sliderUpdateSpeed; // slider 증가 시키기
        }
    }
    private void MousePress(InputAction.CallbackContext obj)
    {
        if (loadCompleted)
        {
            async.allowSceneActivation = true;
        }
    }
    private void TouchPress(InputAction.CallbackContext obj)
    {
        if (loadCompleted)
        {
            async.allowSceneActivation = true;
        }
    }
    IEnumerator LoadingTextProcess()
    {
        int point = 0;
        while (true)
        {
            string text = "Loading";
            for (int i = 0; i < point; i++)
            {
                text += ".";
            }
            loadingText.text = text;

            yield return waitSeconds;
            point++;
            point %= 6;
        }
    }
    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(nextSceneName); 
        async.allowSceneActivation = false;

        while (loadRatio < 1.0f)
        {
            loadRatio = async.progress + 0.1f;

            yield return null;
        }

        loadCompleted = true;           
        Debug.Log("Load Complete!");

        yield return new WaitForSeconds(1.0f);  
        StopCoroutine(loadingTextCoroutine);
        loadingText.text = "Loading Complete. \nPress Button.";
    }
}
