namespace Editor
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using UnityEngine.UIElements;
    [CustomEditor(typeof(GenerateLevel))]
    public class MyCustomEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            GenerateLevel myScript = (GenerateLevel)target;

            // Hiển thị các trường thông thường khác của component
            DrawDefaultInspector();

            // Tạo nút bấm
            if (GUILayout.Button("Generate Level"))
            {
                // Debug.Log("Generate Level");
                myScript.GenLevel();
            }
        }

    
    }
}