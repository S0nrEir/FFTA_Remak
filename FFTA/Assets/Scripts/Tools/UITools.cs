using AquilaFramework.Common.Define;
using UnityEngine;

namespace AquilaFramework.Common.Tools
{
    /// <summary>
    /// UI工具类
    /// </summary>
    public class UITools
    {
        /// <summary>
        /// 获取GameObject的节点全路径
        /// </summary>
        public static string GetGameObjectFullPath (GameObject tar)
        {
            var temp = tar;
            var path = temp.name;
            while (temp.transform.parent != null)
            {
                path = $"{temp.transform.parent.gameObject.name}/{path}";
                temp = temp.transform.parent.gameObject;
            }

            return path;
        }

        /// <summary>
        /// 返回不带根节点的目标GameObject全路径
        /// </summary>
        /// <returns></returns>
        public static string GetGameObjectChildPath (GameObject tar)
        {
            var temp = tar;
            var path = temp.name;
            while (temp.transform.parent != null)
            {
                path = $"{temp.transform.parent.gameObject.name}/{path}";
                temp = temp.transform.parent.gameObject;
            }

            var fullPathArr = path.Split( '/' );
            var len = fullPathArr.Length;
            path = string.Empty;
            
            var last = len - 1;
            for (int i = 1; i < len; i++)
            {
                if(i == last)
                    path += $"{fullPathArr[i]}";

                path += $"{fullPathArr[i]}/";
            }

            return path;
        }

        /// <summary>
        /// 设置UI实例到指定的UI节点下
        /// </summary>
        public static void SetToUIRoot (UI.UIBase ui)
        {
            var tran = ui.UITransform;
            switch (ui.Layer)
            {
                case UI.UILayerEnum.Normal:
                    tran.SetParent( GlobalInstance.NormalUILayer );
                    break;
                case UI.UILayerEnum.Guide:
                    tran.SetParent( GlobalInstance.GuideUILayer );
                    break;
                default:
                    tran.SetParent( GlobalInstance.UIRoot );
                    break;
            }

            tran.localScale = Vector3.one;
            tran.localPosition = Vector3.zero;
        }

        /// <summary>
        /// 根据路径拿到gameObject上相应的组件，拿不到返回NULL
        /// </summary>
        public static T GetComponent<T> (GameObject go,string path) where T : Component
        {
            if (string.IsNullOrEmpty( path ))
                return null;

            var tran = go.transform.Find( path );
            if (tran is null)
                return null;

            return tran.gameObject.GetComponent<T>();
        }

        /// <summary>
        /// 根据路径获取子节点，拿不到返回NULL
        /// </summary>
        public static GameObject GetChildObject (GameObject go, string path)
        {
            if (string.IsNullOrEmpty( path ))
                return null;

            var tran = go.transform.Find( path );
            return tran is null ? null : tran.gameObject;
        }

    }
}
