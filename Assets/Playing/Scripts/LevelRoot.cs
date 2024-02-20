using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelRoot : MonoBehaviour
{
    private float timer;
    private float Timer
    {
        get => timer;
        set
        {
            timer = value;
            OnTimerValueChanged?.Invoke(Mathf.Clamp01(timer / timeBuffer));
        }
    }
    private float timeBuffer = 2.0f;

    public UnityEvent<float> OnTimerValueChanged;

    private void Update()
    {
        if (Keyboard.current.escapeKey.ReadValue() >= 0.9f)
        {
            Timer += Time.deltaTime;
            if (Timer > timeBuffer) Escape();
        }
        else
            Timer = 0;


    }

    private void Escape()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
