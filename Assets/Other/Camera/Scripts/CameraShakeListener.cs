using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeListener : MonoBehaviour
{
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    public void ShakeTrigger()
    {
        transform.DOShakePosition(0.15f, 0.2f).OnComplete(() =>
        {
            transform.position = startPos;
        });
    }

}
