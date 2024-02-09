using UnityEditor;
using UnityEngine;

namespace CoinSortClone.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ProgressManager), true)]
    public class ProgressManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ProgressManager progressManager = (ProgressManager) target;
            GUIStyle guiStyle = EditorStyles.miniButtonLeft;
            guiStyle.margin = new RectOffset(0, 0, 10, 0);

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(EditorGUIUtility.labelWidth);

                if (GUILayout.Button("Clear Data", EditorStyles.miniButtonMid))
                {
                    progressManager.ClearData();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}