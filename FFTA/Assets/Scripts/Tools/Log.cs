using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Tools
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

        public static void Info (string str)
        {
            Debug.Log( $"<color=green>${str}</color>" );
        }

        public static void Warnning (string str)
        {
            Debug.Log( $"<color=yellow>${str}</color>" );
        }

        public static void Error (string str)
        {
            Debug.Log( $"<color=red>${str}</color>" );
        }

        public static void Msg (string str)
        {
            Debug.Log( $"<color=white>${str}</color>" );
        }
    }
}
