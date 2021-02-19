using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AquilaFramework.Common.Define
{
    /// <summary>
    /// 一些依据运行时或开发者指定标记和全局变量
    /// </summary>
    public class GlobalVar
    {
        public static void Init (bool isOpenLog)
        {
            IsOpenLog = isOpenLog;
        }

        /// <summary>
        /// log开关,true = 打开log
        /// </summary>
        public static bool IsOpenLog { get; private set; } = false;
    }
}

