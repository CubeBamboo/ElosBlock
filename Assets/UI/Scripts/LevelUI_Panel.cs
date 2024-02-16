using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUI_Panel : MonoBehaviour
{
    public TextMeshProUGUI Text_Score, Text_Time;

    public void ChangeScoreText(int val)
    {
        Text_Score.text = "Score: " + val.ToString();
    }

    public void ChangeTimeText(float val)
    {
        var intVal = (int)val;
        Text_Time.text = "Time: " + (intVal%3600/60).ToString("00") + ":" + (intVal%60).ToString("00");
    }
}
