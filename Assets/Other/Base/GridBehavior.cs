using ElosBlock;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    private CustomGrid currentGrid;
    public CustomGrid CurrentGrid
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
}
