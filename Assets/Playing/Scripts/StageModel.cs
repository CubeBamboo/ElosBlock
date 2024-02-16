using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class StageModel : MonoBehaviour
{
    public GameObject[] ElosBlockPrefab;
    public Vector2Int initBlockPos;
    private float mLevelTimer;
    public float LevelTimer
    {
        get => mLevelTimer;
        set
        {
            mLevelTimer = value;
            OnLevelTimerChanged?.Invoke(mLevelTimer);
        }
    }

    private int mScore;
    public int Score
    {
        get => mScore;
        set
        {
            mScore = value;
            OnScoreChanged?.Invoke(mScore);
        }
    }

    public int scoreStep;
    public UnityEvent<int> OnScoreChanged;
    public UnityEvent<float> OnLevelTimerChanged;

    public int FailLine; //level fail if block higher than the line

    public GameObject GetRandomTetrisBlock()
    {
        return ElosBlockPrefab[Random.Range(0, ElosBlockPrefab.Length)];
        //return ElosBlockPrefab[4];
    }
}
