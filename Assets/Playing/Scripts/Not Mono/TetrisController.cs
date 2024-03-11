using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Framework;

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
        
        private CustomGrid currentGrid;
        public CustomGrid CurrentGrid
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
        private GameObject ghostBlocks; //tips for drop point

        public UnityEvent OnTouchGround;
        public UnityEvent OnMoveFail;

        public TetrisController(GameObject gameObject)
        {
            //init property
            this.gameObject = gameObject;
            Debug.Assert(this.gameObject != null);
            OnTouchGround = new UnityEvent();
            OnMoveFail = new UnityEvent();
            autoDropTimer = 0f;

            blocks = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                blocks[i] = transform.GetChild(i);
            }

            //init ghost block
            ghostBlocks = gameObject.Instantiate().SetParent(transform).SetName("Ghost Blocks");
            var sprites = ghostBlocks.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in sprites)
            {
                var co = sprite.color; co.a = 0.3f;
                sprite.color = co;
            }
            
            OnTouchGround.AddListener(() => GameObject.Destroy(ghostBlocks));
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

        private void UpdateGhostBlocks()
        {
            //update transform
            ghostBlocks.transform.rotation = transform.rotation; ghostBlocks.transform.localScale = transform.localScale;
            //update position
            int minDist = CurrentGrid.height;
            foreach (var item in blocks)
            {
                var pos = CurrentGrid.WorldToCell(item.position);
                minDist = Mathf.Min(minDist, pos.y);
                minDist = Mathf.Min(minDist, pos.y - CurrentGrid.GetMaximumRowHaveContent(pos.x, pos.y) - 1);
            }
            ghostBlocks.transform.position = transform.position + new Vector3(0, -minDist * CurrentGrid.cellSize.y, 0);
        }

        private void DoCommand(System.Action OnDoCommand, System.Func<bool> OnInvalidCheck=null)
        {
            //检查合法性
            if (OnInvalidCheck != null && !OnInvalidCheck())
            {
                OnMoveFail?.Invoke();
                return;
            }
            
            //行动
            OnDoCommand?.Invoke();
            if (isOnground)
                return;
            
            //更新落点提示
            UpdateGhostBlocks();
        }

        public void MoveDown()
        {
            //if下面有方块，不执行移动。触发落地
            DoCommand(() =>
            {
                transform.position += new Vector3(0, -CurrentGrid.cellSize.y, 0);
            }, () =>
            {
                foreach (var block in blocks)
                {
                    var nextCellPos = CurrentGrid.WorldToCell(block.position) + Vector3Int.down;
                    if (CurrentGrid.IsOutOfBound(nextCellPos) || CurrentGrid.IsHaveBlock(nextCellPos))
                    {
                        TouchGround();
                        return false;
                    }
                }
                return true;
            });
        }

        public void MoveLeft()
        {
            //if左边有方块，不执行动作
            DoCommand(() =>
            {
                transform.position += new Vector3(-CurrentGrid.cellSize.x, 0, 0);
            }, () =>
            {
                foreach (var block in blocks)
                {
                    var nextCellPos = CurrentGrid.WorldToCell(block.position) + Vector3Int.left;
                    if (CurrentGrid.IsOutOfBound(nextCellPos) || CurrentGrid.IsHaveBlock(nextCellPos)) return false;
                }
                return true;
            });
        }
        
        public void MoveRight()
        {
            DoCommand(() =>
            {
                transform.position += new Vector3(CurrentGrid.cellSize.x, 0, 0);
            }, () =>
            {
                foreach (var block in blocks)
                {
                    var nextCellPos = CurrentGrid.WorldToCell(block.position) + Vector3Int.right;
                    if (CurrentGrid.IsOutOfBound(nextCellPos) || CurrentGrid.IsHaveBlock(nextCellPos)) return false;
                }
                return true;
            });
        }

        public void Rotate()
        {
            DoCommand(() =>
            {
                transform.Rotate(0f, 0f, 90f);
            }, () =>
            {
                bool res = true;
                transform.Rotate(0f, 0f, 90f);
                foreach (var block in blocks)
                {
                    var nextCellPos = CurrentGrid.WorldToCell(block.position);
                    if (CurrentGrid.IsOutOfBound(nextCellPos) || CurrentGrid.IsHaveBlock(nextCellPos))
                    {
                        res = false;
                        break;
                    }
                }
                transform.Rotate(0f, 0f, -90f);
                return res;
            });
        }

        public void DropToGround()
        {
            DoCommand(() =>
            {
                //minDist -> 要下落的距离
                int minDist = CurrentGrid.height;
                foreach (var item in blocks)
                {
                    var pos = CurrentGrid.WorldToCell(item.position);
                    minDist = Mathf.Min(minDist, pos.y);
                    minDist = Mathf.Min(minDist, pos.y - CurrentGrid.GetMaximumRowHaveContent(pos.x, pos.y) - 1);
                }
                transform.position += new Vector3(0, -minDist * CurrentGrid.cellSize.y, 0);
                TouchGround();
            });
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