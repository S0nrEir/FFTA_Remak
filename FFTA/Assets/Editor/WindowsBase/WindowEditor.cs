using AquilaFramework.Common.UI;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEditor;
using UnityEngine;

namespace AquilaFramework.EditorExtension
{
    public class ObjectPickerValue
    {
        public GameObject Go;
        public System.Type Type;
        public string Name;

        public void SetGo (GameObject go) => Go = go;
        public void SetType (System.Type type) => this.Type = type;
        public void SetName (string name) => this.Name = name;

        public void Set (GameObject go, System.Type typ, string name)
        {
            SetGo( go );
            SetType( typ );
            SetName( name );
        }

        public bool Setted => Go != null && !string.IsNullOrEmpty( Name );
    }

    /// <summary>
    /// WindowsBase编辑类
    /// </summary>
    [CustomEditor( typeof( WindowBase ) )]//指定当WindowBase的GameObject选中时执行以下编辑器脚本，
    //[System.Serializable]
    public class WindowBaseEditor : Editor
    {
        //字段引用-下拉类型
        private List<ObjectPickerValue> _list = new List<ObjectPickerValue>();

        private string _filePath;


        private void OnEnable ()
        {
            Debug.Log("on enable") ;
        }

        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI();
            #region no use
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField( "object reference picker" );
            //EditorGUILayout.EndHorizontal();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //GUIContent content = new GUIContent( "dropDownButton" );
            //if (EditorGUI.DropdownButton( new Rect( 0, 0, 100, 50 ), content,FocusType.Keyboard ))
            //{
            //    Debug.Log("drop down button");
            //}
            #endregion
            RefreshTarget();
            DrawFileTextField();
            DrawObjectPicker();
            DrawFilePathField();
            DrawReferenceButton();
            if (GUI.changed)
                EditorUtility.SetDirty( target );
        }

        private void RefreshTarget ()
        {
            SerializedObject sObj = new SerializedObject( target );
            sObj.Update();
        }

        /// <summary>
        /// 绘制对象选择器
        /// </summary>
        private void DrawObjectPicker ()
        {
            EditorGUILayout.BeginVertical();
            var cnt = _list.Count;

            for (int i = 0; i < cnt; i++)
            {
                var obj = _list[i];
                if (obj is null) obj = new ObjectPickerValue();
                EditorGUILayout.BeginHorizontal();

                if (obj.Setted)
                {
                    EditorGUILayout.ObjectField( "objectPicker", obj.Go,obj.Type, true );
                }
                else
                {
                    var selectedObj = EditorGUILayout.ObjectField( "objectPicker", obj.Go, typeof( GameObject ), true );
                    if (selectedObj is null)
                    {
                        EditorGUILayout.EndHorizontal();
                        continue;
                    }
                    var go = selectedObj as GameObject;
                    if (go is null)
                        return;

                    var sObj = new SerializedObject( go );

                    sObj.ApplyModifiedProperties();
                    sObj.Update();

                    obj.Set( go, typeof( GameObject ), go.name );
                    Debug.Log("setted");
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制文件文本插件
        /// </summary>
        private void DrawFileTextField ()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("object referece picker");
            EditorGUILayout.EndVertical();
        }

        private void DrawFilePathField ()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField( "filePath:" );
            EditorGUILayout.TextField( string.Empty);
            if (GUILayout.Button( "保存文件" ))
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = $"{_filePath}|*.cs";
                if (dlg.ShowDialog() == DialogResult.Yes)//保存成功
                {
                    
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制引用相关按钮
        /// </summary>
        private void DrawReferenceButton ()
        {
            var verticalRect = EditorGUILayout.BeginVertical();
            var horizRect = EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button( "增加引用" ))
            {
                _list.Add( new ObjectPickerValue() );
            }

            if (GUILayout.Button( "删除引用" ))
            {
                var cnt = _list.Count;
                if (cnt == 0)
                    return;

                _list.RemoveAt( cnt - 1 );
            }

            if (GUILayout.Button( "保存引用" ))
            {
                Undo.RecordObject( target, "tar changed" );
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void SaveFile ()
        {
            
        }
    }
}