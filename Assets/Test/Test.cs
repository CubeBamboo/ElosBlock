using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Grid grid;
    public GameObject go;
    [SerializeField] private Vector3 cellPos;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Start()
    {
        //Debug.Log("GetLayoutCellCenter:" + grid.GetCellCenterWorld());
    }

    private void Update()
    {
        //cellPos = grid.WorldToLocal(go.transform.position);
    }
}
