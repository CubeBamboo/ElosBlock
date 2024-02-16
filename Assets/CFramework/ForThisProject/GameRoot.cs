using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.enabled = true;

        //SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(1);
    }
}