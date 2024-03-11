using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] private float shakeDuration = 0.15f;
    [SerializeField] private float shakeStrength = 0.15f;
    //[SerializeField] private Vector3 shakeStrength = new Vector3(0, -0.15f, 0);

    private void Start()
    {
        startPos = transform.position;
    }

    public void ShakeTrigger()
    {
        transform.DOKill();

        transform.DOShakePosition(shakeDuration, shakeStrength, randomness: 0, randomnessMode: ShakeRandomnessMode.Harmonic).OnComplete(() =>
        {
            transform.position = startPos;
        });
    }

    public void ShakeTrigger(Vector3 dir)
    {
        transform.DOKill();

        transform.DOShakePosition(shakeDuration, shakeStrength * dir, randomness: 0, randomnessMode: ShakeRandomnessMode.Harmonic).OnComplete(() =>
        {
            transform.position = startPos;
        });
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
