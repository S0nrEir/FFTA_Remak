using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AquilaFramework.ExceptionEx
{
    /// <summary>
    /// Aquila异常类
    /// </summary>
    public class AquilaException : Exception
    {
        public AquilaException () : base()
        { }

        public AquilaException (string msg) : base( msg )
        { }
    }
}