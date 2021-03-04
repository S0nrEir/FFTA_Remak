using AquilaFramework.Common.Tools;
using AquilaFramework.Common.UI;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEditor;
using UnityEngine;

namespace AquilaFramework.EditorExtension
{
    /// <summary>
    /// WindowsBase编辑类
    /// </summary>
    [CustomEditor( typeof( WindowBase ) )]//指定当WindowBase的GameObject选中时执行以下编辑器脚本，
    //[System.Serializable]
    public class WindowBaseEditor : Editor
    {
        //字段引用-下拉类型
        private List<SerializObject> _list = new List<SerializObject>();

        private string _filePath;

        /// <summary>
        /// 类名
        /// </summary>
        private string _className;
        private SerializedProperty _referenceList;

        #region unity methods
        private void OnEnable ()
        {
            LoadOriginalObject();
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
            {
                Undo.RecordObject( target ,"tar changed");
                //EditorUtility.SetDirty( target );
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// 加载预设的序列化对象
        /// </summary>
        private void LoadOriginalObject ()
        {
            //load className
            var tmp = serializedObject.FindProperty( "_className" );
            _className = tmp.stringValue;
            _referenceList = serializedObject.FindProperty( "_components" );
            if (!_referenceList.isArray)
            {
                Log.Error( $"faild to find _list!" );
                return;
            }
            TurnPropertyArray2List( _referenceList );
        }

        /// <summary>
        /// 将读到的property数组转为list
        /// </summary>
        private void TurnPropertyArray2List (SerializedProperty refList)
        {
            if (refList is null)
            {
                Log.Error("refList is null");
                return;
            }
            if (_list is null) _list = new List<SerializObject>();

            SerializObject obj = null;
            var cnt = refList.arraySize;
            for (int i = 0; i < cnt; i++)
            {
                var sObj = refList.GetArrayElementAtIndex( i );
                var go = sObj.FindPropertyRelative( "go" );
                var type = sObj.FindPropertyRelative( "type" );
                var name = sObj.FindPropertyRelative( "name" );
                var stringValue = name.stringValue;
                if (go is null || type is null /*|| string.IsNullOrEmpty( name )*/)
                    continue;

                //_list.Add( new SerializObject( go.objectReferenceValue as GameObject, typeof( GameObject ), name ) );
            }
        }

        private void RefreshTarget ()
        {
            serializedObject.Update();
            //SerializedObject sObj = new SerializedObject( target );
            //sObj.Update();
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        private void SaveFile ()
        {

        }
        #endregion


        #region draw
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
                if (obj is null) obj = new SerializObject();
                EditorGUILayout.BeginHorizontal();

                if (obj.Setted)
                {
                    var tup = obj.Values;
                    EditorGUILayout.ObjectField( tup.name, tup.go,tup.typ, true );
                }
                else
                {
                    var selectedObj = EditorGUILayout.ObjectField( "objectPicker", obj.GameObj, typeof( GameObject ), true );
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
                _list.Add( new SerializObject() );
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
                //重新写入到propertyList
                var cnt = _list.Count;
                if (_referenceList is null)
                    return;

                _referenceList.ClearArray();
                _referenceList.arraySize = cnt;

                SerializObject sObj;
                for (int i = 0; i < cnt; i++)
                {
                    sObj = _list[i];
                    _referenceList.InsertArrayElementAtIndex( i );
                    var comp = _referenceList.GetArrayElementAtIndex( i );
                    var go = comp.FindPropertyRelative( "go" );
                    var type = comp.FindPropertyRelative( "type" );
                    var name = comp.FindPropertyRelative( "name" ).stringValue;
                    go.objectReferenceValue = sObj.GameObj;
                    name = go.name;
                    
                }
                Undo.RecordObject( target, "tar changed" );
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        #endregion
    }
}