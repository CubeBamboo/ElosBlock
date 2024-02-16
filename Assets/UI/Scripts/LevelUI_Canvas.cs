using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelUI_Canvas : MonoBehaviour
{
    private bool isGameEnd;
    
    public GameObject Panel_GamingEnd;

    public void OpenGamingEndPanel()
    {
        Panel_GamingEnd.SetActive(true);
        isGameEnd = true;
    }

    private void Update()
    {
        if (isGameEnd)
        {
            var keyboard = Keyboard.current;
            if (keyboard.anyKey.wasPressedThisFrame)
            {
                //SceneManager.UnloadSceneAsync(1).completed +=
                //    op =>
                //    {
                //        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
                //    };

                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            }
        }

    }
}
