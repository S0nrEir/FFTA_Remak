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


        public override void OnInspectorGUI ()
        {
            //base.OnInspectorGUI();
            //GUILayout.Space( 10 );

            //(GameObject)EditorGUI.ObjectField( new Rect( 3, 3,, 20 ), "Find Dependency", obj, typeof( GameObject ) );

            //var option = new GUILayoutOption();

            //_fieldsArr = (GameObject) EditorGUILayout.ObjectField("fields", _fieldsArr, true,);
        }
    }
}