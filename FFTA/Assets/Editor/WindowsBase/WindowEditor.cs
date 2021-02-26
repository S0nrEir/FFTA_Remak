using AquilaFramework.Common.UI;
using UnityEditor;
using UnityEngine;

namespace AquilaFramework.EditorExtension
{
    /// <summary>
    /// WindowsBase编辑类
    /// </summary>
    [CustomEditor( typeof( WindowBase ) )]//指定当WindowBase的GameObject选中时执行以下编辑器脚本，
    public class WindowBaseEditor : Editor
    {
        //字段引用-下拉类型
        private GameObject[] _fieldsArr;
        private int _objFieldsCount = 0;

        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( "object reference picker" );
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            //add referece
            var verticalRect = EditorGUILayout.BeginVertical();
            if (GUI.Button( new Rect( 0, 0, 100, 50 ), "添加引用" ))
            {
                Debug.Log("123");
            }
            EditorGUILayout.EndVertical();
            
        }
    }
}