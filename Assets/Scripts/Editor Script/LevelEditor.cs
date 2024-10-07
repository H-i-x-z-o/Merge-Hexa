using UnityEditor;
using UnityEngine;

namespace Editor.Test
{
    [CustomEditor(typeof(Level))]
    public class LevelEditor : UnityEditor.Editor
    {
        public Texture2D texture;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUIStyle customButtonStyle = new GUIStyle();
            customButtonStyle.normal.background = new Texture2D(50,50);
            if (GUILayout.Button("", customButtonStyle, GUILayout.Width(100), GUILayout.Height(100)))
            {
                Debug.Log("Button Clicked");
            }
            // GUILayout.Button("button", new GUIStyle(), GUILayout.Width(100), GUILayout.Height(100));
        }
    }
}
