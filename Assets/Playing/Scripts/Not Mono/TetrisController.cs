using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElosBlock.Playing
{
    public class TetrisController
    {
        public GameObject gameObject;
        public Transform transform => gameObject.transform;
        
        private bool isOnground=false;
        //方块每帧要做状态更新
        //方块->(下落状态, 着地状态)
        private StageController mStage;
        
        private GridBehavior currentGrid;
        public GridBehavior CurrentGrid
        {
            get
            {
                if(!currentGrid)
                {
                    currentGrid = GameManager.Instance.Register.grid;
                }

                return currentGrid;
            }
        }

        private float autoDropInterval = 1.0f;
        private float autoDropTimer;

        private Transform[] blocks;

        public UnityEvent OnTouchGround;
        public UnityEvent OnMoveFail;

        public TetrisController(GameObject gameObject)
        {
            this.gameObject = gameObject;
            Debug.Assert(this.gameObject != null);
            autoDropTimer = 0f;

            blocks = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                blocks[i] = transform.GetChild(i);
            }
        }

        public void OnFixedUpdate()
        {
            if (isOnground) return; //just for test
            
            autoDropTimer -= Time.fixedDeltaTime;
            if(autoDropTimer <= 0)
            {
                autoDropTimer = autoDropInterval;
                MoveDown();
            }
        }

        public void MoveDown()
        {
            //if下面有方块，不执行移动。触发落地

            foreach (var block in blocks)
            {
                var nextCellPos = CurrentGrid.WorldToCell(block.position) + Vector3Int.down;
                if (CurrentGrid.IsOutOfBound(nextCellPos) || CurrentGrid.IsHaveBlock(nextCellPos))
                {
                    TouchGround();
                    return;
                }
            }

            //正常下落
            //transform.DOMoveY(transform.position.y - CurrentGrid.cellSize.y, 0.5f);
            transform.position += new Vector3(0, -CurrentGrid.cellSize.y, 0);
        }

        public void MoveLeft()
        {
            //if左边有方块，不执行动作。
            foreach (var block in blocks)
            {
                var nextCellPos = CurrentGrid.WorldToCell(block.position) + Vector3Int.left;
                if (CurrentGrid.IsOutOfBound(nextCellPos) || CurrentGrid.IsHaveBlock(nextCellPos)) return;
            }

            //transform.DOMoveX(transform.position.x - CurrentGrid.cellSize.x, 0.5f);
            transform.position += new Vector3(-CurrentGrid.cellSize.x, 0, 0);
        }
        public void MoveRight()
        {
            //if右边有方块，不执行动作。
            foreach (var block in blocks)
            {
                var nextCellPos = CurrentGrid.WorldToCell(block.position) + Vector3Int.right;
                if (CurrentGrid.IsOutOfBound(nextCellPos) || CurrentGrid.IsHaveBlock(nextCellPos)) return;
            }

            //transform.DOMoveX(transform.position.x + CurrentGrid.cellSize.x, 0.5f);
            transform.position += new Vector3(CurrentGrid.cellSize.x, 0, 0);
        }

        public void Rotate()
        {
            transform.Rotate(0f, 0f, 90f);

            foreach (var block in blocks)
            {
                var nextCellPos = CurrentGrid.WorldToCell(block.position);
                if (CurrentGrid.IsOutOfBound(nextCellPos) || CurrentGrid.IsHaveBlock(nextCellPos))
                {
                    transform.Rotate(0f, 0f, -90f);
                    return;
                }
            }

            //bool fail = false;

            //transform.Rotate(0f, 0f, 90f);
            //foreach (var block in blocks)
            //{
            //    var nextCellPos = CurrentGrid.WorldToCell(block.position);
            //    if (CurrentGrid.IsOutOfBound(nextCellPos) || CurrentGrid.IsHaveBlock(nextCellPos))
            //    {
            //        fail = true;
            //        //return;
            //    }
            //}
            //transform.Rotate(0f, 0f, -90f);

            //if (!fail)
            //{
            //    transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 0, 90f), 0.5f, RotateMode.FastBeyond360);
            //}
        }

        public void DropToGround()
        {
            //minDist -> 要下落的距离
            int minDist = CurrentGrid.height;
            foreach (var item in blocks)
            {
                var pos = CurrentGrid.WorldToCell(item.position);
                minDist = Mathf.Min(pos.y, minDist);
                for (int i = pos.y - 1; i >= 0; i--)
                {
                    if (CurrentGrid.GetContent(new Vector3Int(pos.x, i)))
                    {
                        //Debug.Log($"当前格子{item.name}距离地面: {pos.y - i}");
                        minDist = Mathf.Min(pos.y - i - 1, minDist); //找到了
                        break;
                    }
                }
            }

            //Debug.Log("dropDist" + minDist);
            transform.position += new Vector3(0, -minDist * CurrentGrid.cellSize.y, 0);
            TouchGround();
        }

        //on tetris touch ground
        private void TouchGround()
        {
            //update grid block info
            foreach (var block in blocks)
            {
                CurrentGrid.AddContent(CurrentGrid.WorldToCell(block.position), block.gameObject);
            }

            //touch ground event
            OnTouchGround?.Invoke();
            isOnground = true;
            gameObject = null;
        }

    }
}