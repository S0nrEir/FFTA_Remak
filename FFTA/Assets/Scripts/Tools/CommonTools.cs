using UnityEngine;

namespace AquilaFramework.Common.Tools
{
    public class CommonTools
    {
        /// <summary>
        /// 找到一组集合中匹配名称的组件，找不到返回null
        /// </summary>
        public static GameObject FindFromObjects (GameObject[] objs, string name)
        {
            GameObject go = null;

            if (string.IsNullOrEmpty( name ))
            {
                Log.Info( "UITools.FindFromObjects--->name is empty!" );
                return go;
            }

            foreach (var item in objs)
            {
                if (item.name.Equals( name ))
                    return item;
            }

            return go;
        }

        /// <summary>
        /// 保留GameObject使其在切换场景时不被销毁
        /// </summary>
        public static void DontDestroyOnLoad (params GameObject[] gos)
        {
            foreach (var go in gos)
                GameObject.DontDestroyOnLoad( go );
        }
    }
}


