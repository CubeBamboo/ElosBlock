using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuRoot : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Quit();
        }

        if (!Keyboard.current.escapeKey.wasPressedThisFrame && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Addressables.LoadAssetAsync<GameObject>("TransAnim Particle").Completed += handle =>
            {
                var instance = Object.Instantiate(handle.Result);
                instance.gameObject.SetPosition(Vector3.zero)
                        .transform.SetParent(SceneTransition.Instance.transform, false);
                
                SceneTransition.Instance.DoTransition(halfTimeCoroutine: LoadScene());
            };
        }
    }

    private IEnumerator LoadScene()
    {
        yield return SceneManager.LoadSceneAsync("Level_Normal", LoadSceneMode.Additive);
        yield return SceneManager.UnloadSceneAsync("MainMenu");
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
