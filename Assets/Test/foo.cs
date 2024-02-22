using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class foo : MonoBehaviour
{
    public SimpleEvent _evt;

    private void Update()
    {
        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            _evt.Invoke();
        }
    }
}
