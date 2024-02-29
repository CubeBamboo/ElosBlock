using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

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
        //ExposedReference<foo> _foo = new ExposedReference<foo>();
        //_foo.Resolve(new PlayableGraph().GetResolver());
    }
}