using AquilaFramework.Common.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AquilaFramework.EditorExtension
{
    public class ResourceMissingFounder : EditorWindow
    {
        public ResourceMissingFounder () => Init();

        private void Init ()
        {
            titleContent = new GUIContent( "Missing Reference Founder" );
            _pathList = new List<string>( 1 );
            _pathList.Add( string.Empty );
            _pathCount = _pathList.Count;
            maxSize = new Vector2( 500, 700 );
            minSize = new Vector2( 500, 700 );
        }

        #region GUI

        private void OnFocus ()
        {
            maximized = true;
        }

        private void OnGUI ()
        {
            DrawTitle();
            DrawPathFields();
            DrawCheckButton();
        }

        #endregion

        #region draw

        private void DrawPathFields ()
        {
            GUILayout.BeginVertical();

            var cnt = _pathList.Count;
            for (int i = 0; i < cnt; i++)
            {
                GUILayout.BeginHorizontal();

                GUILayout.TextField( _pathList[i], _textFiedOption );

                if (GUILayout.Button( "选择" ))
                {
                    //重写一遍，跨调用效果不好，不如用自带的
                    var folder = EditorUtility.OpenFolderPanel( "选择文件夹", Application.dataPath, "" );
                    //返回来的绝对路径要手动处理一下，蛋疼
                    _pathList[i] = SubPath( folder );

                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void DrawCheckButton ()
        {
            GUILayout.Space( 20 );
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button( "-" ))
            {
                if (_pathList.Count <= 1)
                    return;

                _pathList.RemoveAt( _pathList.Count - 1 );

            }

            if (GUILayout.Button( "+" ))
                _pathList.Add( string.Empty );

            if (GUILayout.Button( "检查以上目录资源引用" ))
                Chk();

            if (GUILayout.Button( "Clear" ))
            {
                var cnt = _pathList.Count;
                for (int i = 0; i < cnt; i++)
                    _pathList[i] = string.Empty;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DrawTitle ()
        {
            GUI.skin.label.fontSize = 10;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label( "Missing Resources Founder" );
        }
        #endregion

        #region 检查函数
        /// <summary>
        /// 检查指定目录下所有美术资源的引用情况，将丢引用的预设打印出来
        /// </summary>
        [UnityEditor.MenuItem( "AquilaFrameWork/ResTools/ResourceMissingFounder" )]
        public static void OpenWindow ()
        {
            EditorWindow.GetWindow( typeof( ResourceMissingFounder ) );
        }

        private string SubPath (string originalPath)
        {
            if (string.IsNullOrEmpty( originalPath ))
                return originalPath;

            var temp = originalPath;
            var idx = temp.IndexOf( "Assets" );
            temp = temp.Substring( idx );

            return temp;
        }

        private void Chk ()
        {
            //        var standardPath = new string[]
            //{
            //                "Assets/MLDJ/GameRes/Prefab/Effect",
            //                "Assets/Res_MS/Effects/Data",
            //                "Assets/Res_MS/Prefab",
            //};

            if (_pathList.Count == 0)
                return;

            var test = _pathList.ToArray();
            var guidArr = AssetDatabase.FindAssets( "t:Prefab", _pathList.ToArray() );
            List<string> resultLst = new List<string>();
            Debug.Log( "资源数量:" + guidArr.Length );

            foreach (var guid in guidArr)
            {
                var path = AssetDatabase.GUIDToAssetPath( guid );
                var obj = AssetDatabase.LoadAssetAtPath( path, typeof( GameObject ) );

                if (obj is null)
                    continue;

                var res = LoadPrefab( obj as GameObject, path );
                if (string.IsNullOrEmpty( res ))
                    continue;

                resultLst.Add( res );
            }

            if (resultLst.Count == 0)
                Debug.Log( "<color=green>检查结束，未检查到相应目录下丢失材质引用的meshRender或skinMesh</color>" );
            else
            {
                PrintResult2DeskTop( resultLst, out var fileName );
                Debug.Log( $"<color=green>检查结束，已将检查结果输出至桌面{fileName}文件</color>" );
            }
        }

        /// <summary>
        /// 将检查结果生成Txt放在桌面上
        /// </summary>
        private static void PrintResult2DeskTop (List<string> pathLst, out string fileName)
        {
            var time = DateTime.Now;
            fileName = $"{time.Year}_{time.Month}_{time.Day}_{time.Hour}_{time.Minute}_{time.Second}_{GUID.Generate().ToString()}";//带一串随机的GUID，防止重名
            var deskTopPath = Environment.GetFolderPath( Environment.SpecialFolder.DesktopDirectory );
            deskTopPath += @"/";
            using (var file = File.Create( deskTopPath + @fileName + ".txt" ))
            {
                var sw = new StreamWriter( file, encoding: System.Text.Encoding.UTF8 );
                foreach (var path in pathLst)
                    sw.WriteLine( path );

                sw.Flush();
                sw.Close();
            }
        }

        /// <summary>
        /// 检查Prefab
        /// </summary>
        private static string LoadPrefab (GameObject prefab, string path)
        {
            if (prefab is null)
                return string.Empty;

            var res = string.Empty;
            var tran = prefab.transform;
            var childCnt = tran.childCount;
            if (childCnt != 0)
            {
                for (int i = 0; i < childCnt; i++)
                {
                    var obj = tran.GetChild( i );
                    if (obj is null)
                        continue;

                    res += LoadPrefab( obj.gameObject, path );
                }
            }

            if (IsResourceRefMissing<MeshRenderer>( prefab ))
                res += $"MeshRender Missing:{GetGameObjectFullPath( prefab )},\n文件路径:{path}\n\n";

            if (IsResourceRefMissing<SkinnedMeshRenderer>( prefab ))
                res += $"SkinnedMesh Missing:{GetGameObjectFullPath( prefab )},\n文件路径:{path}\n\n";

            if (IsResourceRefMissing<Animation>( prefab ))
                res += $"AnimClip Missing:{GetGameObjectFullPath( prefab )},\n文件路径：{path}\n\n";

            if (IsResourceRefMissing<Animator>( prefab ))
                res += $"Animator Missing:{GetGameObjectFullPath( prefab )}，\n文件路径:{path}\n\n";

            if (IsResourceRefMissing<ParticleSystemRenderer>( prefab ))
                res += $"Particle Missing:{GetGameObjectFullPath( prefab )}，\n文件路径:{path}\n\n";

            return res;
        }

        /// <summary>
        /// 检查一组资源是有否丢失，是返回true
        /// </summary>
        private static bool IsResourceRefMissing<T> (GameObject go) where T : Component
        {
            var comp = go.GetComponent<T>();
            if (comp == null)
                return false;

            Component tempComp = null;
            string propertyName = string.Empty;
            switch (comp.GetType())
            {
                case var _ when comp is MeshRenderer:
                    tempComp = comp as MeshRenderer;
                    propertyName = "m_Materials";
                    break;
                case var _ when comp is SkinnedMeshRenderer:
                    tempComp = comp as SkinnedMeshRenderer;
                    propertyName = "m_Materials";
                    break;
                case var _ when comp is Animation:
                    propertyName = "m_Animations";
                    tempComp = comp as Animation;
                    break;
                case var _ when comp is Animator:
                    propertyName = "m_Avatar";
                    tempComp = comp as Animator;
                    break;
                case var _ when comp is ParticleSystemRenderer:
                    tempComp = comp as ParticleSystemRenderer;
                    propertyName = "m_Materials";
                    break;
                default:
                    return false;
            }

            SerializedObject sObj = new SerializedObject( tempComp );

            var sp = sObj.FindProperty( propertyName );

            var refMethod = typeof( SerializedProperty ).GetProperty( "objectReferenceStringValue",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Public );

            if (sp != null && sp.isArray)
            {
                var cnt = sp.arraySize;
                if (cnt == 0)
                    return false;

                for (int i = 0; i < cnt; i++)
                {
                    var elmt = sp.GetArrayElementAtIndex( i );
                    if (elmt is null)
                        continue;

                    var refString = (string)refMethod.GetGetMethod( true ).Invoke( elmt, null );
                    if (refString.StartsWith( "Miss" ))
                        return true;
                }
            }


            return false;
        }

        /// <summary>
        /// 获取GameObject的全路径
        /// </summary>
        private static string GetGameObjectFullPath (GameObject prefab)
        {
            var temp = prefab;
            var path = temp.name;
            while (temp.transform.parent != null)
            {
                path = $"{temp.transform.parent.gameObject.name}/{path}";
                temp = temp.transform.parent.gameObject;
            }

            return path;
        }
        #endregion

        #region fields

        /// <summary>
        /// 要检查的路径总数
        /// </summary>
        private int _pathCount = 0;

        /// <summary>
        /// 保存选择路径
        /// </summary>
        private List<string> _pathList;

        private GUILayoutOption[] _textFiedOption = new GUILayoutOption[]
        {
            GUILayout.Height(20),
            GUILayout.Width(450),
        };


        #endregion

    }
}