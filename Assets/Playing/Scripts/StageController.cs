using ElosBlock.Playing;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class StageController : MonoBehaviour
{
    private StageModel mStageModel;
    private PlayerInput mPlayerInput;

    private bool isLevelEnd;
    public bool IsLevelEnd
    {
        get => isLevelEnd;
        set
        {
            isLevelEnd = value;
            if(value == true)
            {
                //level end callback
                mPlayerInput.enabled = false;
                OnLevelEnd?.Invoke();
            }
        }
    }

    public UnityEvent OnLevelEnd, OnTouchGround;

    //public TetrisController ActiveBlock { get; set; }
    private TetrisController activeBlock;

    private GridBehavior currentGrid;
    public GridBehavior CurrentGrid
    {
        get
        {
            if (!currentGrid)
            {
                currentGrid = GameManager.Instance.Register.grid;
            }

            return currentGrid;
        }
    }

    private void Awake()
    {
        mStageModel = GetComponent<StageModel>();
        mPlayerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        GameManager.Instance.Register.stageController = this;
    }

    private void Start()
    {
        
        RandomCreateTetrisBlock();
        mStageModel.Score = 0;
        
        OnLevelEnd.AddListener(() =>
        {
            mPlayerInput.enabled = false;
        });

        OnTouchGround.AddListener(() =>
        {
            CheckLineClear();
            CheckLevelFail();
            RandomCreateTetrisBlock();
        });
        //Time.timeScale = 10f;
    }

    private void OnDisable()
    {
        //auto unregister when destroy
        GameManager.Instance.Register.stageController = null;
    }

    private void OnDrawGizmosSelected()
    {
        if ((EditorApplication.isPlaying))
        {
            Gizmos.color = Color.red;
            var start = CurrentGrid.CellToWorld(new Vector3Int(0, mStageModel.FailLine));
            var end = CurrentGrid.CellToWorld(new Vector3Int(CurrentGrid.width-1, mStageModel.FailLine));
            Gizmos.DrawLine(start, end);
        }
    }

    private void FixedUpdate()
    {
        if(!isLevelEnd) mStageModel.LevelTimer += Time.fixedDeltaTime;
        if(activeBlock != null) activeBlock.OnFixedUpdate();
    }

    public void RandomCreateTetrisBlock()
    {
        if (IsLevelEnd) return;
        
        var pos = new Vector3Int(mStageModel.initBlockPos.x, mStageModel.initBlockPos.y, 0);
        var go =
            mStageModel.GetRandomTetrisBlock()
            .Instantiate()
            .SetPosition(CurrentGrid.CellToWorld(pos));

        //var tetris = go.GetComponent<TetrisController>();
        activeBlock = new TetrisController(go);
        activeBlock.OnTouchGround = OnTouchGround;
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
        
        int[] dropCntArray = new int[blocks.GetLength(1)];

        //clear
        for (int j = 0; j < blocks.GetLength(1); j++)
        {
            if (toClearArray[j])
            {
                //logic1 - remove block of line ${j}
                for (int i = 0; i < blocks.GetLength(0); i++)
                {
                    CurrentGrid.RemoveContent(new Vector3Int(i, j, 0), true);
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
        //�����Ƿ�ȸ�λ�ø���
        var blocks = CurrentGrid.GetContentContainer();
        var failLine = mStageModel.FailLine;
        
        for (int i = blocks.GetLength(1) - 1; i >= 0; i--)
        {
            if (i < failLine) break; //end check
            if (IsLevelEnd) break;

            for (int j = 0; j < blocks.GetLength(0); j++)
            {
                if (blocks[j, i]) { IsLevelEnd = true; break; }
            }
        }

        if (IsLevelEnd) Debug.Log("LevelEnd");
    }

    #region Input

    public void OnMoveLeftInput(InputAction.CallbackContext ctx)
    {
        if(ctx.phase == InputActionPhase.Started)
            activeBlock.MoveLeft();
    }

    public void OnMoveRightInput(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
            activeBlock.MoveRight();
    }

    public void OnMoveDownInput(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
            activeBlock.MoveDown();
    }

    public void OnRotateInput(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
            activeBlock.Rotate();
    }

    public void OnDropToGroundInput(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
            activeBlock.DropToGround();
    }

    #endregion

}