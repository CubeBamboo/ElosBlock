using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ElosBlock.Wave
{
    [CustomEditor(typeof(WaveShapeController))]
    public class WaveControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            WaveShapeController waveController = target as WaveShapeController;

            if (GUILayout.Button("ÐÞ¸Ä¶¥µã"))
            {
                waveController.DebugControlMesh();
            }

            //if(EditorApplication.isPlaying)
            //    waveController.DivideCount = EditorGUILayout.IntSlider("DivideCount", waveController.divideCount, 3, 600);
        }
    }
}