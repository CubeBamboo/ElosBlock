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
                OnLevelEnd?.Invoke();
            }

            Framework.Timer.Instance.SetTimer(RestartLevel, 1f);
        }
    }

    public UnityEvent OnLevelEnd, OnTouchGround;

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

    public Transform blockParent;

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
        //init event
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

        //init game
        StartLevel();
    }

    private void OnDisable()
    {
        //unregister when disable
        GameManager.Instance.Register.stageController = null;
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if ((EditorApplication.isPlaying))
        {
            Gizmos.color = Color.red;
            var start = CurrentGrid.CellToWorld(new Vector3Int(0, mStageModel.FailLine));
            var end = CurrentGrid.CellToWorld(new Vector3Int(CurrentGrid.width-1, mStageModel.FailLine));
            Gizmos.DrawLine(start, end);
        }
#endif
    }

    private void FixedUpdate()
    {
        if(!isLevelEnd) mStageModel.LevelTimer += Time.fixedDeltaTime;
        activeBlock?.OnFixedUpdate();
    }

    public void StartLevel()
    {
        isLevelEnd = false;
        mPlayerInput.enabled = true;
        mStageModel.Score = 0;
        mStageModel.LevelTimer = 0;
        RandomCreateTetrisBlock();
    }

    public void RandomCreateTetrisBlock()
    {
        if (IsLevelEnd) return;
        
        var pos = new Vector3Int(mStageModel.initBlockPos.x, mStageModel.initBlockPos.y, 0);
        var go =
            mStageModel.GetRandomTetrisBlock()
            .Instantiate()
            .SetPosition(CurrentGrid.CellToWorld(pos))
            .SetParent(blockParent);

        activeBlock = new TetrisController(go);
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
        //方块是否比该位置更高
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

    public void RestartLevel()
    {
        CurrentGrid.ClearAllContent();
        StartLevel();
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
