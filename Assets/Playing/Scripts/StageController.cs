using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Framework;

namespace ElosBlock
{
    public class StageController : GridBehavior
    {
        //component
        private StageLevelManager levelManager;
        private StageModel mStageModel;

        public UnityEvent OnTouchGround;
        public UnityEvent OnLineClear;

        //TODO: place it to another place
        public GameObject blockClearFXPrefab;

        private Playing.TetrisController activeBlock;
        public Playing.TetrisController ActiveBlock => activeBlock;

        public Transform blockParent;

        private void Awake()
        {
            levelManager = GetComponent<StageLevelManager>();
            mStageModel = GetComponent<StageModel>();
        }

        private void OnEnable()
        {
            GameManager.Instance.Register.stageController = this;
        }

        private void Start()
        {
            OnTouchGround.AddListenerWithCustomUnRegister(() =>
            {
                CheckLineClear();
                CheckLevelFail();
                RandomCreateTetrisBlock();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            //init game
            levelManager.StartLevel();
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if ((EditorApplication.isPlaying))
            {
                Gizmos.color = Color.red;
                var start = CurrentGrid.CellToWorld(new Vector3Int(0, mStageModel.FailLine));
                var end = CurrentGrid.CellToWorld(new Vector3Int(CurrentGrid.width - 1, mStageModel.FailLine));
                Gizmos.DrawLine(start, end);
            }
#endif
        }

        private void FixedUpdate()
        {
            activeBlock?.OnFixedUpdate();
        }

        public void RandomCreateTetrisBlock()
        {
            if (levelManager.IsLevelEnd) return;

            var pos = new Vector3Int(mStageModel.initBlockPos.x, mStageModel.initBlockPos.y, 0);
            var go =
                mStageModel.GetRandomTetrisBlock()
                .Instantiate()
                .SetPosition(CurrentGrid.CellToWorld(pos))
                .SetParent(blockParent);

            activeBlock = new Playing.TetrisController(go);
            activeBlock.OnTouchGround.AddListener(() => OnTouchGround?.Invoke());
        }

        public void CheckLineClear()
        {
            var blocks = CurrentGrid.GetContentContainer();
            bool[] toClearArray = new bool[blocks.GetLength(1)];
            bool needToClear = false;

            //check every line
            for (int j = 0; j < blocks.GetLength(1); j++)
            {
                toClearArray[j] = true;
                for (int i = 0; i < blocks.GetLength(0); i++)
                    if (!blocks[i, j]) { toClearArray[j] = false; break; }

                if (toClearArray[j]) needToClear = true;
            }

            ////////////////////////
            if (!needToClear) return;

            OnLineClear?.Invoke();
            int[] dropCntArray = new int[blocks.GetLength(1)];

            //clear
            for (int j = 0; j < blocks.GetLength(1); j++)
            {
                if (toClearArray[j])
                {
                    //logic1 - remove block of line ${j} and show particle effects
                    for (int i = 0; i < blocks.GetLength(0); i++)
                    {
                        CurrentGrid.RemoveContent(new Vector3Int(i, j, 0), true);
                        blockClearFXPrefab.Instantiate().SetPosition(CurrentGrid.CellToWorld(new Vector3Int(i, j, 0)));
                    }
                    //logic2 - add score
                    mStageModel.Score += mStageModel.scoreStep;
                }
            }

            //logic3 - drop the block (from bottom to top)
            for (int j = 0; j < blocks.GetLength(1); j++)
            {
                if (toClearArray[j])
                {
                    for (int index = j + 1; index < dropCntArray.Length; index++)
                        dropCntArray[index]++;
                    dropCntArray[j] = 0;
                }
            }
            for (int j = 0; j < blocks.GetLength(1); j++)
            {
                if (dropCntArray[j] <= 0) continue;
                for (int i = 0; i < blocks.GetLength(0); i++)
                    if (CurrentGrid.GetContent(new Vector3Int(i, j)))
                        CurrentGrid.MoveContent(new Vector3Int(i, j), new Vector3Int(i, j - dropCntArray[j]));
            }
        }

        public void CheckLevelFail()
        {
            //方块是否比该位置更高
            var blocks = CurrentGrid.GetContentContainer();
            var failLine = mStageModel.FailLine;

            for (int i = blocks.GetLength(1) - 1; i >= 0; i--)
            {
                if (i < failLine) break; //end check
                if (levelManager.IsLevelEnd) break;

                for (int j = 0; j < blocks.GetLength(0); j++)
                {
                    if (blocks[j, i])
                        { levelManager.IsLevelEnd = true; break; }
                }
            }

            if (levelManager.IsLevelEnd) Debug.Log("LevelEnd");
        }
    }
}