using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class foo1 : MonoBehaviour
{
    public foo _foo;
    private void Start()
    {
        _foo._evt.Register(() =>
        {
            Debug.Log("111");
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        _foo._evt.UnRegister(() =>
        {
            Debug.Log("111");
        });
    }
}