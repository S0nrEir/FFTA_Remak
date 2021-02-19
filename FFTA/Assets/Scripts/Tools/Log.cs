using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AquilaFramework.Common.Tools
{
    /// <summary>
    /// Log类
    /// </summary>
    public class Log
    {
        public static void Orange (string str)
        {
            Debug.Log( $"<color=orange>${str}</color>" );
        }

        public static void Purple (string str)
        {
            Debug.Log($"<color=purple>${str}</color>");
        }

        /// <summary>
        /// 消息
        /// </summary>
        public static void Info (string str)
        {
            Debug.Log( $"<color=green>${str}</color>" );
        }

        /// <summary>
        /// 警告
        /// </summary>
        public static void Warnning (string str)
        {
            Debug.Log( $"<color=yellow>${str}</color>" );
        }

        /// <summary>
        /// 错误
        /// </summary>
        public static void Error (string str)
        {
            Debug.Log( $"<color=red>${str}</color>" );
        }

        /// <summary>
        /// 消息
        /// </summary>
        public static void Msg (string str)
        {
            Debug.Log( $"<color=white>${str}</color>" );
        }
    }
}
