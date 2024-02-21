using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    public int width, height;
    private GridLayout mGrid { get; set; } //TODO:自己实现一套
    private GameObject[,] contents;

    public Vector3 cellSize => mGrid.cellSize;

    [Tooltip("if is null, gizmos will not draw")]
    public Transform OriginPoint; //origin point

    private void Awake()
    {
        mGrid = GetComponentInChildren<GridLayout>();
        GameManager.Instance.Register.grid = this;
        mGrid.transform.position = OriginPoint.position;
        contents = new GameObject[width, height];
    }

    //private void OnDrawGizmos()
    private void OnDrawGizmosSelected()
    {
        if (!OriginPoint) return;
        
        Gizmos.color = Color.yellow;
        for (int i = 0; i < width; i++)
        {   
            for(int j = 0; j < height; j++)
            {
                var cellSize = new Vector3(1, 1, 1); var offset = OriginPoint.position + cellSize/2;
                var pos = offset + cellSize.x * new Vector3(i, j, 0);
                Gizmos.DrawWireCube(pos, cellSize);
            }
        }

        //if (EditorApplication.isPlaying)
        //{
        //    Gizmos.color = Color.red;
        //    for (int i = 0; i < width; i++)
        //    {
        //        for (int j = 0; j < height; j++)
        //        {
        //            if (contents[i, j])
        //            {
        //                var cellSize = new Vector3(1, 1, 1); var offset = new Vector3(-4.25f, -7.55f) + cellSize / 2;
        //                var pos = offset + cellSize.x * new Vector3(i, j, 0);
        //                Gizmos.DrawWireCube(pos, cellSize);
        //            }
        //        }
        //    }
        //}
    }

    /// <summary>
    /// not recommend because you'll miss the check of OutOfBound Warning.
    /// </summary>
    public GameObject[,] GetContentContainer() => contents;

    public GameObject GetContent(Vector3Int pos)
    {
        if (IsOutOfBound(pos))
        {
            Debug.LogWarning($"Grid IndexOutOfRange: {pos}");
            return null;
        }

        return contents[pos.x, pos.y];
    }

    public bool IsOutOfBound(Vector3Int pos) => pos.x < 0 || pos.y < 0 || pos.x >= width || pos.y >= height;
    
    public bool IsHaveBlock(Vector3Int pos)
    {
        if (IsOutOfBound(pos))
        {
            Debug.LogWarning($"Grid IndexOutOfRange: {pos}");
            return false;
        }

        return contents[pos.x, pos.y] != null;
    }

    public Vector3Int WorldToCell(Vector3 pos) => mGrid.WorldToCell(pos - mGrid.cellSize/2);
    public Vector3 CellToWorld(Vector3Int pos) => mGrid.CellToWorld(pos) + mGrid.cellSize/2;

    public bool AddContent(Vector3Int pos, GameObject go)
    {
        if (IsOutOfBound(pos))
        {
            Debug.LogWarning($"Grid IndexOutOfRange: {pos}");
            return false;
        }

        contents[pos.x, pos.y] = go;
        return true;
    }

    public bool RemoveContent(Vector3Int pos, bool isDestroy=true)
    {
        if (IsOutOfBound(pos))
        {
            Debug.LogWarning($"Grid IndexOutOfRange: {pos}");
            return false;
        }

        //not exist
        if(!contents[pos.x, pos.y]) return false;

        if(isDestroy) Destroy(contents[pos.x, pos.y]);
        contents[pos.x, pos.y] = null;
        return true;
    }

    public bool MoveContent(Vector3Int pos, Vector3Int newPos)
    {
        if (IsOutOfBound(pos))
        {
            Debug.LogWarning($"Grid IndexOutOfRange: {pos}");
            return false;
        }

        if (GetContent(newPos))
        {
            Debug.LogWarning($"Grid MoveContent(): already have a gameobject in {newPos} and it may lead to some bugs.");
        }

        contents[newPos.x, newPos.y] = contents[pos.x, pos.y];
        contents[newPos.x, newPos.y].SetPosition(CellToWorld(newPos));
        contents[pos.x, pos.y] = null;

        return true;
    }

    /// <summary>
    /// Get maximum row which have content in giving column. (return -1 if can't find)
    /// </summary>
    public int GetMaximumRowHaveContent(int column)
    {
        for (int i = height-1; i >= 0; i--)
        {
            if (GetContent(new Vector3Int(column, i))) return i;
        }

        return -1;
    }

    public int GetMaximumRowHaveContent(int column, int startRow)
    {
        startRow = Mathf.Clamp(startRow, 0, height-1);

        for (int i = startRow; i >= 0; i--)
        {
            if (GetContent(new Vector3Int(column, i))) return i;
        }

        return -1;
    }

    /// <summary>
    /// Get minimal row which have content in giving column. (return -1 if can't find)
    /// </summary>
    public int GetMinimalRowHaveContent(int column)
    {
        for (int i = 0; i < height; i++)
        {
            if (!GetContent(new Vector3Int(column, i))) return i-1;
        }

        return height-1;
    }

    public void ClearAllContent(bool isDestroy = true)
    {
        for (int i = 0; i < contents.GetLength(0); i++)
        {
            for (int j = 0; j < contents.GetLength(1); j++)
            {
                if (isDestroy && contents[i, j] != null) Destroy(contents[i, j]);
                contents[i, j] = null;
            }
        }
    }

}