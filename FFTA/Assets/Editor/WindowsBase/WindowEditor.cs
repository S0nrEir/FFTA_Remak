using AquilaFramework.Common.Tools;
using AquilaFramework.Common.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using UnityEditor;
using UnityEngine;

//210330更新：将UI编辑器和WindowBase类分开来，编辑器类专门做预设引用拖拽和类的设置，
//WindowBase作为单独的一个类型，以一个字段的形式表示在UIBase中，基类Init方法，子类实现，传入一个表示该UI根节点的GameObject，然后WindowBase的子类实现，根据该GameObject找到相应的字段引用
namespace AquilaFramework.EditorExtension
{
    /// <summary>
    /// 内部类，表示面板形式的SerializObject
    /// </summary>
    internal class InspectorSerializObjct
    {
        public WindowInspectorObj _serializObject = null;
        public int _selectTypeIdx = -1;
        public string _objectNodePath = string.Empty;
        public InspectorSerializObjct () 
        {
            _serializObject = new WindowInspectorObj();

            //WindowBase t = new Game.UI.TestWindow();
            //Game.UI.TestWindow tt = new WindowBase() as Game.UI.TestWindow;
        }
    }

    /// <summary>
    /// WindowsBase编辑类
    /// </summary>
    [CustomEditor( typeof( WindowBase) )]//指定当WindowBase的GameObject选中时执行以下编辑器脚本，
    //[System.Serializable]
    public class WindowBaseEditor : UnityEditor.Editor
    {
        //字段引用-下拉类型
        private List<InspectorSerializObjct> _list = new List<InspectorSerializObjct>();

        /// <summary>
        /// 类文件路径
        /// </summary>
        private string _filePath;

        /// <summary>
        /// 类名
        /// </summary>
        private string _className;

        /// <summary>
        /// 组件引用列表
        /// </summary>
        private SerializedProperty _referenceList;

        #region unity methods
        private void OnEnable ()
        {
            LoadOriginalObject();
        }

        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI();

            RefreshTarget();
            //DrawFileTextField();
            DrawObjectPicker();
            DrawFilePathField();
            DrawReferenceButton();
            if (GUI.changed)
            {
                Undo.RecordObject( target, "tar changed" );
                //EditorUtility.SetDirty( target );
            }
        }

        private void OnDisable ()
        {
            //离开的时候走一遍保存的流程
            
        }
        #endregion

        #region private methods

        /// <summary>
        /// 生成类文件
        /// </summary>
        private void GenerateClassFile ()
        {
            
        }

        /// <summary>
        /// 加载预设的序列化对象
        /// </summary>
        private void LoadOriginalObject ()
        {
            //load className
            _className = serializedObject.FindProperty( "_className" ).stringValue;
            _filePath = serializedObject.FindProperty( "_classFilePath" ).stringValue;

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
                Log.Error( "refList is null" );
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
                    _selectTypeIdx  = index,
                    _objectNodePath = UITools.GetGameObjectChildPath(serialObj.GameObj),
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
        private void SaveFile (string filePath)
        {
            //把老的删掉 重新生成
            if (File.Exists( filePath ))
                File.Delete( filePath );

            var builder = new StringBuilder();
            //var attBuilder = new StringBuilder("\t\t");
            var fieldsInitStr = string.Empty;
            using (var fs = File.Create( filePath ))
            {
                var sw = new StreamWriter( fs,encoding : Encoding.UTF8);
                //using
                builder.Append( "using UnityEngine;\n" );
                builder.Append( "using AquilaFramework.Common.Tools;\n" );

                //nameSpace
                builder.Append( "namespace AquilaFramework.Common.UI\n{");

                //class
                builder.Append( $"\n\tpublic class {_className} : WindowBase\n" );
                builder.Append( "\t{\n" );
                //fields:
                foreach (var item in _list)
                {
                    var serializedObj = item._serializObject;
                    if (serializedObj is null || serializedObj.GameObj is null)
                    {
                        Log.Error("serializedObj is null");
                        continue;
                    }
                    builder.Append( "\t\t#region fields\n" );
                    builder.Append($"\t\tprivate {serializedObj.Type} { serializedObj.Name };\n");
                    builder.Append( $"\t\t{GetFieldAttrName( serializedObj.Name )} => {serializedObj.Name}" );
                    builder.Append( "\n\t\t#endregion\n\n" );
                    //attLst.Add( $"{ GetFieldAttrName( serializedObj.Name ) } => {serializedObj.Name};\n" );

                    //#todo
                    //引用获取没有什么好的方案，暂时想到的是保存节点引用路径名
                    fieldsInitStr += $"\t\t\t{serializedObj.Name} = UITools.GetComponent<{serializedObj.Type}>({serializedObj.Name},{item._objectNodePath});\n";
                }
                //reference init
                builder.Append( "\t\tpublic override void Init()\n" );
                builder.Append( "\t\t{\n" );
                builder.Append( fieldsInitStr );
                builder.Append( "\t\t}" );

                builder.Append( "\n\t}" ); 
                builder.Append( "\n}" );

                sw.Write( builder.ToString() );
                sw.Flush();
                sw.Close();

            }
        }

        /// <summary>
        /// 处理字段对应的属性名
        /// </summary>
        private string GetFieldAttrName (string source)
        {
            //var firstChar = source[0];
            //if (firstChar >= 'A' && firstChar <= 'Z')
            //{
            //    firstChar = (char)(firstChar | 0x20);
            //    source = $"{firstChar}{}"
            //}

            //if (!source.StartsWith( "_" ))
            //    source = $"_{source}";

            //return source;
            return $"m_{source}";
        }

        /// <summary>
        /// 写入类
        /// </summary>
        private void WriteClass ()
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
                //draw object picker
                EditorGUILayout.BeginHorizontal();
                if (windowObj != null && windowObj.Setted)
                {
                    var tup = windowObj.Values;
                    EditorGUILayout.ObjectField( tup.name, tup.go, typeof( GameObject ), true, _objectPickerOptionArr );
                }
                else
                {
                    var selectedObj = EditorGUILayout.ObjectField( string.Empty, windowObj.GameObj, 
                        typeof( GameObject ),true, _objectPickerOptionArr );

                    if (selectedObj is null)
                    {
                        EditorGUILayout.EndHorizontal();
                        continue;
                    }
                    var go = selectedObj as GameObject;
                    if (go is null)
                        return;

                    var sObj = new SerializedObject( go );

                    windowObj.SetGo( go );
                    var typeStr = GetGameobjetsCompTypes( windowObj.GameObj );
                    windowObj.Set( go, typeStr[_list[i]._selectTypeIdx], go.name );//popup的value

                    sObj.ApplyModifiedProperties();
                    sObj.Update();
                    Debug.Log( "setted" );
                }

                var types = GetGameobjetsCompTypes( windowObj.GameObj );
                var selectedIdx = GetSelectedIndex( windowObj.Type, types );

                //draw type selector
                var idx = EditorGUILayout.Popup( selectedIdx, types/*, _popOptionArr*/ );
                _list[i]._selectTypeIdx = idx;
                _list[i]._serializObject.SetType(types[idx] );

                //draw name
                var fieldName = EditorGUILayout.TextField( windowObj.Name );
                windowObj.SetName( fieldName );

                EditorGUILayout.EndHorizontal();
            }//end for

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 获取一个gameObject上所有组件的string类型表示
        /// </summary>
        private string[] GetGameobjetsCompTypes (GameObject go)
        {
            var compArr = go.GetComponents<Component>();
            var len = compArr.Length;
            var typeArr = new string[len + 1];
            typeArr[0] = typeof( GameObject ).Name;//默认第0项是GameObject
            for (int i = 1; i < len; i++)
                typeArr[i] = compArr[i].GetType().Name;

            return typeArr;
        }

        /// <summary>
        /// SerializObjct上指定类型在其GameObject上的组件下标，没有返回0（GameObject）
        /// </summary>
        private int GetSelectedIndex (string target, string[] types)
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
            EditorGUILayout.LabelField( "-------------------object referece picker-------------------" );
            EditorGUILayout.EndVertical();
        }

        private void DrawFilePathField ()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField( "filePath:" ,new GUILayoutOption[] { GUILayout.Width(50) } );
            _filePath = EditorGUILayout.TextField( _filePath );

            //检查类名
            if (string.IsNullOrEmpty( _className ) || _className.Contains( " " ))
            {
                Log.Error("类名不合法");
                return;
            }

            if (GUILayout.Button( "SaveFile" ))
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "|*.cs";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var uiLogicPath = $@"{UnityEngine.Application.dataPath}/UI";
                    if (!Directory.Exists(uiLogicPath))
                        Directory.CreateDirectory( uiLogicPath );

                    SaveFile(dlg.FileName);
                }
                //reset filePath;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField( "ClassName:" , new GUILayoutOption[] { GUILayout.Width(100) } );

            _className = EditorGUILayout.TextField( _className, new GUILayoutOption[] { GUILayout.Width( 100 ) } );

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
                    var name = comp.FindPropertyRelative( "name" );
                    go.objectReferenceValue = sObj._serializObject.GameObj;
                    type.stringValue = sObj._serializObject.Type;
                    name.stringValue = sObj._serializObject.Name;
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

        #region fields

        private GUILayoutOption[] _objectPickerOptionArr = new GUILayoutOption[]
            {
                GUILayout.Width(200),
                GUILayout.MaxWidth(200),
            };

        //private GUILayoutOption[] _popOptionArr = new GUILayoutOption[]
        //    {
        //                GUILayout.Width(150),
        //                GUILayout.MaxWidth(150),
        //    };

        #endregion
    }
}