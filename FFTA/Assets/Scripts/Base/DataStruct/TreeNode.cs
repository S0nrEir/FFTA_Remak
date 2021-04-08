using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AquilaFramework.Common.Define.DelegateDefine;

namespace AquilaFramework.DataStruct
{

    public sealed class TreeNode<T>
    {
        #region consturctor

        public TreeNode (T value)
        {
            Value = value;
        }

        public TreeNode (T left,T right)
        {
            Left  = new TreeNode<T>();
            Right = new TreeNode<T>();
        }

        public TreeNode ()
        {
            Left  = null;
            Right = null;
        }
        #endregion

        #region fields

        /// <summary>
        /// value
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// left node
        /// </summary>
        public TreeNode<T> Left { get; private set; }

        /// <summary>
        /// right Node
        /// </summary>
        public TreeNode<T> Right { get; private set; }


        #endregion 
    }
}

