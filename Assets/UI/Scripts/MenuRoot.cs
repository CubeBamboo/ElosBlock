using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuRoot : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Quit();
        }

        if (!Keyboard.current.escapeKey.wasPressedThisFrame && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Level_Normal");
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        Debug.Log("Quit");
#else
        Application.Quit();
#endif

    }
}
