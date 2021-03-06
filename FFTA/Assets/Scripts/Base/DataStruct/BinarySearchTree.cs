﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static AquilaFramework.Common.Define.DelegateDefine;

namespace AquilaFramework.DataStruct
{
    public class BinarySearchTree<T> : BinaryTree<T>
    {
        public BinarySearchTree (TreeNode<T> node , TreeNodeDiffDel<T> del) : base( node )
        {
            _diffDel = del;
        }

        public BinarySearchTree (List<TreeNode<T>> list) : base( list )
        {

        }

        #region override

        public override void Add (T item)
        {
            if (_diffDel is null)
                return;


        }

        public override void Del (T item)
        {
            if (_diffDel is null)
                return;


        }

        public override void Add (T item, TreeNodeDiffDel<T> del)
        {
            throw new NotImplementedException();
        }

        public override void Del (T item, TreeNodeDiffDel<T> del)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// 最左子子节点
        /// </summary>
        public TreeNode<T> Left => GetLeft();

        /// <summary>
        /// 最右子节点
        /// </summary>
        public TreeNode<T> Right => GetRight();

        /// <summary>
        /// 获取二叉搜索树最左侧节点
        /// </summary>
        private TreeNode<T> GetLeft ()
        {
            if (_diffDel is null)
                return null;

            if (Root is null)
                return null;

            var tempRoot = Root;
            while (tempRoot != null)
            {
                if (tempRoot.Left != null &&_diffDel( tempRoot ) == TreeChildNodeDiffEnum.Left)
                    tempRoot = tempRoot.Left;
            }

            return tempRoot;
        }

        /// <summary>
        /// 获取最右侧节点
        /// </summary>
        private TreeNode<T> GetRight ()
        {
            if (_diffDel is null)
                return null;

            if (Root is null)
                return null;

            var tempRoot = Root;
            while (tempRoot != null)
            {
                if (tempRoot.Right != null && _diffDel( tempRoot ) == TreeChildNodeDiffEnum.Right)
                    tempRoot = tempRoot.Right;
            }

            return tempRoot;
        }

        private TreeNodeDiffDel<T> _diffDel = null;
    }
}