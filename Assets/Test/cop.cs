/*// InputHandler.cs
public class InputHandler : MonoBehaviour
{
    private TetrisController activeBlock;

    public void SetActiveBlock(TetrisController block)
    {
        activeBlock = block;
    }

    public void OnMoveLeftInput(InputAction.CallbackContext ctx)
    {
        // ...
    }

    // ... �������봦���� ...
}

// LevelManager.cs
public class LevelManager : MonoBehaviour
{
    private bool isLevelEnd;
    private StageModel mStageModel;

    public void StartLevel()
    {
        // ...
    }

    public void CheckLevelFail()
    {
        // ...
    }

    private IEnumerator SetLevelEnd()
    {
        // ...
    }
}

// BlockManager.cs
public class BlockManager : MonoBehaviour
{
    private GridBehavior currentGrid;
    private StageModel mStageModel;

    public void RandomCreateTetrisBlock()
    {
        // ...
    }

    public void CheckLineClear()
    {
        // ...
    }
}

// StageController.cs
public class StageController : MonoBehaviour
{
    private InputHandler mInputHandler;
    private LevelManager mLevelManager;
    private BlockManager mBlockManager;

    private void Awake()
    {
        mInputHandler = GetComponent<InputHandler>();
        mLevelManager = GetComponent<LevelManager>();
        mBlockManager = GetComponent<BlockManager>();
    }

    // ... �������� ...
}*/
