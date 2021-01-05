﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common
{

    /// <summary>
    /// 单例类，懒加载启动，实现该类型的子类都可作为单例使用，禁止用作通用的多类型派生类，只可作为管理/工具/通用属性类的实现；
    /// </summary>
    public class Singleton<T> where T:class,new()
    {
        /// <summary>
        /// 单例字段
        /// </summary>
        private static T _ins;

        /// <summary>
        /// 单例
        /// </summary>
        public static T I
        {
            get
            {
                if (_ins is null)
                    _ins = new T();

                return _ins;
            }
        }
    }
}