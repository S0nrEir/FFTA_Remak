using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AquilaFramework.ObjectPool
{
    /// <summary>
    /// 对象池基本接口，定义了一些Aquila对象池的基本规范，任何自定义对象池都应该实现此接口
    /// </summary>
    public interface IObjectPool
    {
        /// <summary>
        /// 清理并释放该对象池
        /// </summary>
        public void Dispose ();

        /// <summary>
        /// 创建池
        /// </summary>
        public void Create ();

        /// <summary>
        /// 释放所有池中未使用的object
        /// </summary>
        public void ReleaseAllUnused ();
    }
}
