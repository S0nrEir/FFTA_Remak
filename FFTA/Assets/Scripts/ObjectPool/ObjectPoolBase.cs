using AquilaFramework.ExceptionEx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AquilaFramework.Common.Define.DelegateDefine;

namespace AquilaFramework.ObjectPool
{
    /// <summary>
    /// 对象池接口，任何对象池的实现都需实现此接口
    /// </summary>
    public abstract class ObjectPoolBase<T> : IObjectPool 
    {
        /// <summary>
        /// 对象池存储器
        /// </summary>
        private Queue<T> _pool;

        /// <summary>
        /// 对象创建回调
        /// </summary>
        public delegate void PoolHandler (T obj);

        protected PoolHandler _onGeneDel = null;

        /// <summary>
        /// 销毁对象回调
        /// </summary>
        protected PoolHandler _onDisDel = null;

        /// <summary>
        /// 池中对象释放间隔（秒）
        /// </summary>
        public float ReleaseTime { get; private set; } = DEFAULT_RELEASE_TIME;

        /// <summary>
        /// 容量
        /// </summary>
        public int Capacity { get; private set; } = DEFAULT_CAPACITY;

        /// <summary>
        /// 池中对象过期时间（秒）
        /// </summary>
        public float ExpireTime { get; private set; } = DEFAULT_EXPIRE_TIME;

        /// <summary>
        /// constructor
        /// </summary>
        public ObjectPoolBase (PoolHandler onGenDel, PoolHandler onDisDel)
        {
            _onGeneDel = onGenDel;
            _onDisDel = onDisDel;
            Capacity = DEFAULT_CAPACITY;
            ExpireTime = DEFAULT_EXPIRE_TIME;
            ReleaseTime = DEFAULT_RELEASE_TIME;
        }

        public ObjectPoolBase () 
        {
            _onGeneDel = null;
            _onDisDel = null;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public ObjectPoolBase
            (
                PoolHandler onGenDel,
                PoolHandler onDisDelint,
                int capacity = DEFAULT_CAPACITY,
                float expireTime = DEFAULT_EXPIRE_TIME,
                float releaseTime = DEFAULT_RELEASE_TIME
            )
        {
            Capacity = capacity;
            ExpireTime = expireTime;
            ReleaseTime = releaseTime;
            _onGeneDel = onGenDel;
            _onDisDel = onDisDelint;
        }

        /// <summary>
        /// 默认容量
        /// </summary>
        private const int DEFAULT_CAPACITY = 64;

        /// <summary>
        /// 默认对象过期时间
        /// </summary>
        private const float DEFAULT_EXPIRE_TIME = 60;

        /// <summary>
        /// 默认对象清理间隔
        /// </summary>
        private const float DEFAULT_RELEASE_TIME = 120;

        #region override
        public void Dispose ()
        {
            throw new AquilaException( "" );
        }

        public void Create ()
        {
            throw new AquilaException( "" );
        }

        public void ReleaseAllUnused ()
        {
            throw new AquilaException( "" );
        }
        #endregion
    }
}
