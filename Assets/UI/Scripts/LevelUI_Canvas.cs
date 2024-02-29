using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelUI_Canvas : MonoBehaviour
{
    private bool isGameEnd;
    
    public GameObject Panel_GamingEnd;

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void OpenGamingEndPanel()
    {
        Panel_GamingEnd.SetActive(true);
    }
}
