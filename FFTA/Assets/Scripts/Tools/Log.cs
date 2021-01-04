using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Tools
{
    /// <summary>
    /// 简易封装log类,先凑活用
    /// </summary>
    public class Log
    {
        public static void Info (string str)
        {
            Debug.Log(string.Format("<color=green>{0}</color>",str));
        }

        public static void Warnning (string str)
        {
            Debug.Log( string.Format( "<color=yellow>{0}</color>", str ) );
        }

        public static void Error (string str)
        {
            Debug.Log( string.Format( "<color=red>{0}</color>", str ) );
        }

        public static void Msg (string str)
        {
            Debug.Log( string.Format( "<color=white>{0}</color>", str ) );
        }
    }
}
