using System;
using System.Collections;
using System.Collections.Generic;
namespace AquilaFramework.Common.Define
{
    /// <summary>
    /// 常用类型委托定义类;
    /// 类型生命规则为：下划线分割，第一部分为返回值类型，其后为参数类型，对于集合or数组做s结尾
    /// </summary>
    public class DelegateDefine
    {
        /// <summary>
        /// 参数：无；
        /// 返回值：无
        /// </summary>
        //public Action Void_Void_Del;

        /// <summary>
        /// 参数：float；
        /// 返回值：无
        /// </summary>
        public delegate void Void_Float_Del (float f);

        //考虑到统一格式，没有使用Action
        /// <summary>
        /// 参数：无；
        /// 返回值：无
        /// </summary>
        public delegate void Void_Void_Del ();

        /// <summary>
        /// 参数：object[]
        /// 返回值：无
        /// </summary>
        public delegate void Void_ObjectArr_Del (object[] args);
    }
}
