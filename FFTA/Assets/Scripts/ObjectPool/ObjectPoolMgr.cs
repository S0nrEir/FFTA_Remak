using AquilaFramework.Common;
using AquilaFramework.Common.Tools;
using System;
using System.Reflection;
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
        /// 拥有对象池数量
        /// </summary>
        public int Count => _objectPoolDic.Count;

        /// <summary>
        /// 创建对象池
        /// </summary>
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
        /// 创建默认的Auila框架实现的对象池，传入的回调禁止使用匿名函数
        /// </summary>
        public T CreateDefaultObjectPool<T> 
            (
                int capacity,
                float expireTime,
                float releaseTime, 
                Action<T> onGenDel,
                Action<T> onResycleDel,
                Action<T> onReleaseDel
            ) where T : class, IObjectPool, new()
        {
            var type = typeof( T );

            //if (type as ObjectPoolBase == null)
            //{
            //    Log.Error( "CreateDefaultObjectPool ---> type case faild!!!" );
            //    return null;
            //}

            if (_objectPoolDic.TryGetValue( type, out var pool ))
            {
                Log.Info( $"ObjectPoolManager have objectPool of type {type.ToString()}" );
                return (T)pool;
            }
            pool = new T();

            pool.SetCapacity( capacity );
            pool.SetExpireTime( expireTime );
            pool.SetReleaseTime( releaseTime );

            var objType = pool.GetObjectType();
            

            //#todo 直接使用ObjectBase或继承自他的类型作为对象池而不需要类型转换
            //var poolBase = pool as ObjectPoolBase<>;
            
            //pool.SetOnGenDel( onGenDel );
            //pool.SetOnResycleDel( onResycleDel );
            //pool.SetOnReleaseDel( onReleaseDel );

            return (T)pool;
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
