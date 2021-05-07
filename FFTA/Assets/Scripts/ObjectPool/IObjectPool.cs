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
        void Dispose ();

        /// <summary>
        /// 创建池
        /// </summary>
        void Create ();

        /// <summary>
        /// 释放所有池中未使用的object
        /// </summary>
        void ReleaseAllUnused ();

        /// <summary>
        /// 回收所有Object到池中
        /// </summary>
        void ResycleAll ();

        /// <summary>
        /// 设置对象池容量
        /// </summary>
        void SetCapacity (int capacity);

        /// <summary>
        /// 设置对象过期时间
        /// </summary>
        void SetExpireTime (float time);

        /// <summary>
        /// 设置回收间隔
        /// </summary>
        void SetReleaseTime (float time);

        /// <summary>
        /// 获取对象池存储对象类型
        /// </summary>
        System.Type GetObjectType ();
    }
}
