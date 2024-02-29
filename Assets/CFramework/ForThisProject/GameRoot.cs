using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

namespace Framework
{
    public class GameRoot : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            AudioManager.Instance.enabled = true;
            ElosBlock.GameManager.Instance.enabled = true;
            SceneTransition.Instance.enabled = true;

            StartCoroutine(InitScene());
        }

        private IEnumerator InitScene()
        {
            yield return SceneManager.LoadSceneAsync("Persistent", LoadSceneMode.Additive);
            yield return SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
            
            SceneManager.UnloadSceneAsync("Launch");
        }
    }
}