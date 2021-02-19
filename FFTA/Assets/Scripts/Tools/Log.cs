using AquilaFramework.Common.Define;
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
        private static void DoLog (string str)
        {
            if (!GlobalVar.IsOpenLog)
                return;

            Debug.Log( str );
        }

        public static void Orange (string str)
        {
            DoLog( $"<color=orange>${str}</color>" );
        }

        public static void Purple (string str)
        {
            DoLog( $"<color=purple>${str}</color>");
        }

        /// <summary>
        /// 消息
        /// </summary>
        public static void Info (string str)
        {
            DoLog( $"<color=green>${str}</color>" );
        }

        /// <summary>
        /// 警告
        /// </summary>
        public static void Warnning (string str)
        {
            DoLog( $"<color=yellow>${str}</color>" );
        }

        /// <summary>
        /// 错误
        /// </summary>
        public static void Error (string str)
        {
            DoLog( $"<color=red>${str}</color>" );
        }

        /// <summary>
        /// 消息
        /// </summary>
        public static void Msg (string str)
        {
            DoLog( $"<color=white>${str}</color>" );
        }
    }
}
