using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Framework;

namespace ElosBlock
{
    public class StageLevelManager : GridBehavior
    {
        private PlayerInput mPlayerInput;

        private bool isLevelEnd;
        public bool IsLevelEnd
        {
            get => isLevelEnd;
            set
            {
                isLevelEnd = value;
                if (isLevelEnd)
                {
                    StartCoroutine(SetLevelEnd());
                }
            }
        }

        private StageModel mStageModel;
        private StageController mStageController;

        public UnityEvent OnLevelReset, OnLevelEnd;

        public GameObject levelResetParticlePrefab;

        private void Awake()
        {
            mPlayerInput = GetComponent<PlayerInput>();
            mStageModel = GetComponent<StageModel>();
            mStageController = GetComponent<StageController>();
        }

        private void Start()
        {
            //init event
            OnLevelEnd.AddListenerWithCustomUnRegister(() =>
            {
                mPlayerInput.enabled = false;
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        public void StartLevel()
        {
            IsLevelEnd = false;
            mPlayerInput.enabled = true;
            mStageModel.Score = 0;
            mStageModel.LevelTimer = 0;
            mStageController.RandomCreateTetrisBlock();
        }

        public void RestartLevel()
        {
            CurrentGrid.ClearAllContent();
            StartLevel();
        }

        private IEnumerator SetLevelEnd()
        {
            //level end callback
            OnLevelEnd?.Invoke();

            yield return new WaitForSeconds(0.6f);

            var initPos = transform.position;
            transform.DOShakePosition(0.4f, 3f, 20).OnComplete(() => { transform.position = initPos; });
            CurrentGrid.ClearAllContent();
            levelResetParticlePrefab.Instantiate().SetPosition(transform.position);
            OnLevelReset?.Invoke();

            //wait 1s and restart
            yield return new WaitForSeconds(1.2f);
            RestartLevel();
        }
    }
    
}