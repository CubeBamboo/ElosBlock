using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ElosBlock
{
    public class GameRoot : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            AudioManager.Instance.enabled = true;
            GameManager.Instance.enabled = true;

            //SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
            //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}