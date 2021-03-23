using AquilaFramework.Common.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AquilaFramework.Editor
{
    public class ResourceMissingFounder
    {
        /// <summary>
        /// 查找指定路径下的丢失资源
        /// </summary>
        [MenuItem( "AuilaTools/FindMissingResource" )]
        public static void FindMissingResource ()
        {
            //另一种思路读prefab拿GUID然后查文件存在，有时间再弄吧
            //var str = AssetDatabase.GUIDToAssetPath( "88d93511830e83e4e9028a7a56353077" );
            //if (!File.Exists( str ))
            //    Debug.Log( 1 );
            //else
            //    Debug.Log( 2 );

            //var sstr = AssetDatabase.GUIDToAssetPath( "38c6cb7f8bec66d4f98b88955ada6099" );
            //if (!File.Exists( sstr ))
            //    Debug.Log( 1 );
            //else
            //    Debug.Log( 2 );

            var standardPath = new string[]
                {
                    "Assets/MLDJ/GameRes/Prefab/Effect",
                    "Assets/Res_MS/Effects/Data",
                    "Assets/Res_MS/Prefab",
                };

            var guidArr = AssetDatabase.FindAssets( "t:Prefab", standardPath );
            List<string> resultLst = new List<string>();
            Log.Info( $"资源数量{guidArr.Length}" );

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
                res += $"MeshRender Missing:{GetGameObjectFullPath( prefab )},文件路径:{path}\n\n";

            if (IsResourceRefMissing<SkinnedMeshRenderer>( prefab ))
                res += $"SkinnedMesh Missing:{GetGameObjectFullPath( prefab )},文件路径:{path}\n\n";

            if (IsResourceRefMissing<Animation>( prefab ))
                res += $"AnimClip Missing:{GetGameObjectFullPath( prefab )},文件路径：{path}\n\n";

            if (IsResourceRefMissing<Animator>( prefab ))
                res += $"Animator Missing:{GetGameObjectFullPath( prefab )}，文件路径:{path}\n\n";

            if (IsResourceRefMissing<ParticleSystemRenderer>( prefab ))
                res += $"Particle Missing:{GetGameObjectFullPath( prefab )}，文件路径:{path}\n\n";

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

            //if (tempComp.GetType() == typeof( ParticleSystemRenderer ))
            //{
            //    var tsp = sObj.GetIterator();
            //    while (tsp.NextVisible( true ))
            //    {
            //        Debug.Log( $"<color=green>type:{tempComp.GetType().ToString()},disName:{tsp.displayName},internalName:{tsp.name}</color>" );
            //    }
            //}

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

            #region
            //while (tSp.NextVisible( true ))
            //{
            //var tt = sp.FindPropertyRelative( "objectReferenceStringValue" );


            //var refMethod = typeof( SerializedProperty ).GetProperty( "objectReferenceStringValue",
            //    System.Reflection.BindingFlags.Instance |
            //    System.Reflection.BindingFlags.NonPublic |
            //    System.Reflection.BindingFlags.Public );

            //var tempStr = (string)refMethod.GetGetMethod( true ).Invoke( sp, null );
            //Debug.Log( sp.stringValue + "," + tempStr );

            ////var stringValue = refMethod == null ? string.Empty : (string)refMethod.GetGetMethod( true ).Invoke( sp, new object[] { } );
            //if (sp.propertyType != SerializedPropertyType.ObjectReference)
            //    continue;

            //if (sp.objectReferenceValue == null && sp.objectReferenceInstanceIDValue != 0)
            //    return true;

            ////if (stringValue.StartsWith( "Missing" ))
            ////    return true;
            //}
            #endregion

            return false;
        }

        private bool HandlerParticelSystem ()
        {
            return true;
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
    }

}