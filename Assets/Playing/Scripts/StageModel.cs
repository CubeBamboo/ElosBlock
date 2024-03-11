using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace ElosBlock
{
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

        private StageLevelManager levelManager;

        private void Start()
        {
            levelManager = GetComponent<StageLevelManager>();
        }

        public GameObject GetRandomTetrisBlock()
        {
            //return ElosBlockPrefab[Random.Range(0, ElosBlockPrefab.Length)];
            return ElosBlockPrefab[4];
        }

        private void FixedUpdate()
        {
            if (!levelManager.IsLevelEnd) LevelTimer += Time.fixedDeltaTime;
        }
    }
}