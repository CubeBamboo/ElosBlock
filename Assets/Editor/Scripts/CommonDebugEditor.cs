using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ElosBlock
{
    [CustomEditor(typeof(CameraShakeController))]
    public class CommonDebugEditor : Editor
    {
        CameraShakeController targetObj;

        private void OnEnable()
        {
            targetObj = target as CameraShakeController;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Trigger"))
            {
                targetObj.ShakeTrigger();
            }
        }
    }
}