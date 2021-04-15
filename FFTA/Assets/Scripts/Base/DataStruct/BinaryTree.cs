using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AquilaFramework.DataStruct
{
    public abstract class BinaryTree<T>
    {
        public BinaryTree (TreeNode<T> root)
        {
            Root = root;
        }

        /// <summary>
        /// 构造list转tree
        /// </summary>
        public BinaryTree (List<TreeNode<T>> list)
        {
            
        }

        /// <summary>
        /// add
        /// </summary>
        public abstract void Add (T item);

        /// <summary>
        /// Add
        /// </summary>
        public abstract void Add (T item, TreeNodeDiffDel<T> del);

        /// <summary>
        /// delete
        /// </summary>
        public abstract void Del (T item);

        /// <summary>
        /// delete
        /// </summary>
        public abstract void Del (T item, TreeNodeDiffDel<T> del);

        /// <summary>
        /// 获取tree节点数量
        /// </summary>
        private int GetCount ()
        {
            var count = 1;
            TreeNode<T> node = Root;
            Stack<TreeNode<T>> stack = new Stack<TreeNode<T>>();

            while (node != null || !(stack.Count == 0))
            {
                while (node != null)
                {
                    stack.Push( node );
                    node = node.Left;
                    count++;
                }

                if (!(stack.Count == 0))
                {
                    node = stack.Pop();
                    node = node.Right;
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// tree节点数量
        /// </summary>
        public int Count => GetCount();

        public TreeNode<T> Root { get; private set; }
    }

    /// <summary>
    /// 对于二叉树做子节点比较时的自定义委托，可基于此定义自定义的二叉树左右子节点比较函数
    /// </summary>
    public delegate TreeChildNodeDiffEnum TreeNodeDiffDel<T>(TreeNode<T> node);

    /// <summary>
    /// 二叉树子节点自定义比较返回结果
    /// </summary>
    public enum TreeChildNodeDiffEnum
    {
        Left,
        Right,
    }
}
