using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_LevelEscTimer : MonoBehaviour
{
    [SerializeField]
    private Image bar;

    /// <param name="val">val should be Clamp01.</param>
    public void SetValue(float val)
    {
        if (Mathf.Abs(val - bar.fillAmount) < 0.001f) return;
        
        if(val <= 0.01f) bar.enabled = false;
        else bar.enabled = true;

        val = Mathf.Clamp01(val);

        //anim tween
        bar.fillAmount = val;
    }
}
