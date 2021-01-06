using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Tools
{
    /// <summary>
    /// UI工具类
    /// </summary>
    public class UITools
    {
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
