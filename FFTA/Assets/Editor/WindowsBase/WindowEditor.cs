using AquilaFramework.Common.Tools;
using AquilaFramework.Common.UI;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEditor;
using UnityEngine;

namespace AquilaFramework.EditorExtension
{
    /// <summary>
    /// 内部类，表示面板形式的SerializObject
    /// </summary>
    internal class InspectorSerializObjct
    {
        public WindowInspectorObj _serializObject;
        public int _selectTypeIdx;
        public InspectorSerializObjct () { }
    }

    /// <summary>
    /// WindowsBase编辑类
    /// </summary>
    [CustomEditor( typeof( WindowBase ) )]//指定当WindowBase的GameObject选中时执行以下编辑器脚本，
    //[System.Serializable]
    public class WindowBaseEditor : UnityEditor.Editor
    {
        //字段引用-下拉类型
        private List<InspectorSerializObjct> _list = new List<InspectorSerializObjct>();

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

            //load object reference
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
            if (_list is null) _list = new List<InspectorSerializObjct>();

            WindowInspectorObj obj = null;
            var cnt = refList.arraySize;
            for (int i = 0; i < cnt; i++)
            {
                var sObj = refList.GetArrayElementAtIndex( i );
                var go = sObj.FindPropertyRelative( "go" );
                var type = sObj.FindPropertyRelative( "type" ).stringValue;
                var name = sObj.FindPropertyRelative( "name" ).stringValue;
                if (go is null || string.IsNullOrEmpty( type ) || string.IsNullOrEmpty( name ))
                    continue;

                var serialObj = new WindowInspectorObj( go.objectReferenceValue as GameObject, type, name );
                var index = GetSelectedIndex( serialObj.Type, GetGameobjetsCompTypes( serialObj.GameObj ) );
                var inspectObj = new InspectorSerializObjct()
                {
                    _serializObject = serialObj,
                    _selectTypeIdx = index,
                };
                _list.Add( inspectObj );
            }
        }

        /// <summary>
        /// 刷新Inspector面板
        /// </summary>
        private void RefreshTarget ()
        {
            serializedObject.Update();
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
                if (_list[i] is null)
                    continue;

                var windowObj = _list[i]._serializObject;
                var types = GetGameobjetsCompTypes( windowObj.GameObj );
                var selectedIdx = GetSelectedIndex( windowObj.Type, types );

                //draw object picker
                EditorGUILayout.BeginHorizontal();
                if (windowObj.Setted)
                {
                    var tup = windowObj.Values;
                    EditorGUILayout.ObjectField( tup.name, tup.go,typeof(GameObject), true );
                }
                else
                {
                    var selectedObj = EditorGUILayout.ObjectField( "objectPicker", windowObj.GameObj, typeof( GameObject ), true );
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

                    windowObj.Set( go, types[_list[i]._selectTypeIdx] , go.name );//popup的value
                    Debug.Log("setted");
                }

                //draw type selector
                _list[i]._selectTypeIdx = EditorGUILayout.Popup( selectedIdx, types );

                //draw name
                var fieldName = EditorGUILayout.TextField( windowObj.Name);
                windowObj.SetName( fieldName );

                EditorGUILayout.EndHorizontal();
            }//end for

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 获取一个gameObject上所有组件的string类型表示
        /// </summary>
        private string[] GetGameobjetsCompTypes(GameObject go)
        {
            var compArr = go.GetComponents<Component>();
            var len = compArr.Length;
            var typeArr = new string[len + 1];
            typeArr[0] = typeof(GameObject).Name;//默认第0项是GameObject
            for (int i = 1; i < len; i++)
                typeArr[i] = compArr[i].GetType().Name;

            return typeArr;
        }

        /// <summary>
        /// SerializObjct上指定类型在其GameObject上的组件下标，没有返回0（GameObject）
        /// </summary>
        private int GetSelectedIndex (string target,string[] types)
        {
            var len = types.Length;
            for (int i = 0; i < len; i++)
                if (types[i] == target)
                    return i;

            return 0;
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
            if (GUILayout.Button( "SaveFile" ))
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
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button( " + " ))
                _list.Add( new InspectorSerializObjct() );

            if (GUILayout.Button( " - " ))
            {
                var cnt = _list.Count;
                if (cnt == 0)
                    return;

                _list.RemoveAt( cnt - 1 );
            }

            if (GUILayout.Button( "Save Reference" ))
            {
                //重新写入到propertyList
                var cnt = _list.Count;
                if (_referenceList is null)
                    return;

                _referenceList.ClearArray();
                _referenceList.arraySize = cnt;

                InspectorSerializObjct sObj;
                for (int i = 0; i < cnt; i++)
                {
                    sObj = _list[i];
                    _referenceList.InsertArrayElementAtIndex( i );
                    var comp = _referenceList.GetArrayElementAtIndex( i );
                    var go = comp.FindPropertyRelative( "go" );
                    var type = comp.FindPropertyRelative( "type" );
                    var name = comp.FindPropertyRelative( "name" ).stringValue;
                    //go.objectReferenceValue = sObj._serializObject;
                    name = go.name;
                    
                }
                //Undo.RecordObject( target, "tar changed" );
                serializedObject.ApplyModifiedProperties();
            }

            if (GUILayout.Button( "Clear Reference" ))
            {
                _referenceList?.ClearArray();
                _list?.Clear();
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        #endregion
    }
}