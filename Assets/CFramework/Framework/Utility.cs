using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class Utility : MonoBehaviour
    {

        public static class Creator
        {
            public static GameObject CreateGO(GameObject go, Vector3 pos, Transform parent = null)
            {
                return CreateGO(go, pos, Quaternion.identity, parent);
            }

            public static GameObject CreateGO(GameObject go, Vector3 pos, Quaternion rot, Transform parent = null)
            {
                return GameObject.Instantiate(go, pos, rot, parent);
            }

            /// <summary>
            /// create with auto destroy
            /// </summary>
            public static void CreateGO(GameObject go, Vector3 pos, float destroyDelay)
            {
                CreateGO(go, pos, Quaternion.identity, destroyDelay);
            }

            /// <summary>
            /// create with auto destroy
            /// </summary>
            public static void CreateGO(GameObject go, Vector3 pos, Quaternion rot, float destroyDelay)
            {
                var obj = GameObject.Instantiate(go, pos, rot);
                GameObject.Destroy(obj, destroyDelay);
            }
        }

        public static class Math
        {
            public static Vector3 Vec2ToVec3(Vector2 before, float defalutVal=0, bool isYEmpty = true)
            {
                if (isYEmpty) return new Vector3(before.x, defalutVal, before.y);
                else return new Vector3(before.x, before.y, defalutVal);
            }
        }

        public static class UI
        {
            
        }
    }
}