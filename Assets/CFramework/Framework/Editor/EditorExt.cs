using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Framework.Editor
{
    public class EditorExt : MonoBehaviour
    {
        //[MenuItem("Custom/Init Map")]
        //public static void InitMap()
        //{
        //    int lineCnt = 5;
        //    int columnCnt = 7;
        //    Vector2 startPoint = Vector2.zero;
        //    GameObject SquarePrefab = null;
        //    float lineGap = 1.6f;
        //    float columnGap = 0f;

        //    for (int j = 0; j < lineCnt; j++)
        //    {
        //        for (int i = 0; i < columnCnt; i++)
        //        {
        //            Vector2 pos = startPoint + new Vector2(i * lineGap, j * columnGap);
        //            var go = Instantiate(SquarePrefab, pos, Quaternion.identity);
        //            go.name = $"Square({i},{j})";
        //            //mapDict.Add(new Core.CustomStruct.Point(i, j), go.GetComponent<MapBlock>());
        //        }
        //    }
        //}
    }
}