using AquilaFramework.Common;
using AquilaFramework.Common.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AquilaFramework.ObjectPool
{
    /// <summary>
    /// 对象池管理器
    /// </summary>
    public class ObjectPoolMgr : Singleton<ObjectPoolMgr>
    {
        /// <summary>
        /// 对象池管理集合
        /// </summary>
        private Dictionary<Type, IObjectPool> _objectPoolDic;

        #region public interface

        /// <summary>
        /// 对象池数量
        /// </summary>
        public int Count => _objectPoolDic.Count;

        public IObjectPool CreateObjectPool<T> () where T : IObjectPool, new()
        {
            var type = typeof( T );
            if (_objectPoolDic.TryGetValue( type, out var pool ))
            {
                Log.Info($"ObjectPoolManager have objectPool of type {type.ToString()}");
                return pool;
            }

            pool = new T();
            pool.Create();
            _objectPoolDic.Add( type, pool );

            return pool;
        }

        /// <summary>
        /// 获取指定类型的ObjectPool，拿不到返回空
        /// </summary>
        public IObjectPool GetObjectPool<T> () where T : IObjectPool
        {
            var type = typeof( T );
            if (!_objectPoolDic.ContainsKey( type ))
            {
                Log.Error($"ObjectPoolMgr doesnt have pool of type {type.ToString()}");
                return null;
            }

            if (!_objectPoolDic.TryGetValue( type, out var pool ))
            {
                Log.Error($"faild to get pool of type {type.ToString()}");
                return null;
            }

            return pool;
        }


        #endregion

        #region private methods

        #endregion


        public override void Init ()
        {
            _objectPoolDic = new Dictionary<Type, IObjectPool>();
        }

        public override void DeInit ()
        {
            _objectPoolDic?.Clear();
            _objectPoolDic = null;
        }

    }
}
