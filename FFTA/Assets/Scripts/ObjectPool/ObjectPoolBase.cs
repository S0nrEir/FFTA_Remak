using AquilaFramework.Common.Tools;
using AquilaFramework.ExceptionEx;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AquilaFramework.Common.Define.DelegateDefine;

namespace AquilaFramework.ObjectPool
{
    /// <summary>
    /// 默认对象池基类，通常继承此类即可，若有自己的需求，则需实现IObjectPool接口并实现自己的ObjectPoolBase类
    /// </summary>
    public abstract class ObjectPoolBase<T> : IObjectPool where T : class, new()
    {
        //public delegate void PoolHandler (T obj);
        //public Action<T> PoolHandler;
        /// <summary>
        /// 对象池存储器
        /// </summary>
        private Queue<T> _pool;

        /// <summary>
        /// 保存对象回收状态的map，如果对象被获取，则标记为false，在池中为true
        /// </summary>
        private Dictionary<T, bool> _objectMap;

        /// <summary>
        /// 创建对象回调
        /// </summary>
        protected Action<T> _onGenDel = null;

        /// <summary>
        /// 回收对象回调
        /// </summary>
        protected Action<T> _onResycleDel = null;

        /// <summary>
        /// 释放对象回调
        /// </summary>
        protected Action<T> _onReleaseDel = null;

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

        public ObjectPoolBase () 
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public ObjectPoolBase
            (
                Action<T> onGenDel,
                Action<T> onDisDelint,
                Action<T> onReleaseDel,
                int capacity = DEFAULT_CAPACITY,
                float expireTime = DEFAULT_EXPIRE_TIME,
                float releaseTime = DEFAULT_RELEASE_TIME
            )
        {
            Capacity = capacity;
            ExpireTime = expireTime;
            ReleaseTime = releaseTime;
            _onGenDel = onGenDel;
            _onResycleDel = onDisDelint;
            _onReleaseDel = onReleaseDel;
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
            ResycleAll();
            ReleaseAllUnused();
            _onGenDel     = null;
            _onReleaseDel = null;
            _onResycleDel = null;
        }

        public void Create ()
        {
            Capacity = DEFAULT_CAPACITY;
            ExpireTime = DEFAULT_EXPIRE_TIME;
            ReleaseTime = DEFAULT_RELEASE_TIME;
            _onGenDel = null;
            _onResycleDel = null;
            _onReleaseDel = null;

            _pool = new Queue<T>( DEFAULT_CAPACITY );
            _objectMap = new Dictionary<T, bool>( DEFAULT_CAPACITY );
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        public T Get()
        {
            T obj;
            if (_pool.Count == 0)
            {
                obj = new T();
                _pool.Enqueue( obj );
                AddObjectReference( obj, false );

                _onGenDel?.Invoke( obj );

                return obj;
            }

            obj = _pool.Dequeue();
            //if (!_objectMap.ContainsKey( obj ))
            //{
            //    Log.Warnning($"ObjectMap does not have Object:{obj}");
            //    return null;
            //}
            SetObjectReferenceState( obj, false );

            return obj;
        }

        /// <summary>
        /// 回收所有对象到池中
        /// </summary>
        public void ResycleAll ()
        {
            var itor = _objectMap.GetEnumerator();
            while (itor.MoveNext())
            {
                var pair = itor.Current;
                if (pair.Value)
                    continue;

                var obj = pair.Key;
                _onResycleDel( obj );
                SetObjectReferenceState( obj, true );
            }
        }

        /// <summary>
        /// 释放所有未使用的Object
        /// </summary>
        public void ReleaseAllUnused ()
        {
            while (_pool.Count != 0)
            {
                var obj = _pool.Dequeue();
                DeleteObjectReference( obj );
                _onReleaseDel( obj );
            }
        }

        #region Set
        /// <summary>
        /// 设置对象过期时间
        /// </summary>
        public void SetExpireTime (float time)
        {
            if (time <= 0)
            {
                Log.Error("SetExpireTime--->time <= 0 !!!");
                return;
            }

            ExpireTime = time;
        }

        /// <summary>
        /// #todo设置回收间隔，需要配合协程实现
        /// </summary>
        public void SetReleaseTime (float time)
        {
            if (time <= 0)
            {
                Log.Error( "SetReleaseTime--->time <= 0 !!!" );
                return;
            }

            ReleaseTime = time;
        }

        /// <summary>
        /// 设置释放对象回调
        /// </summary>
        public void SetOnReleaseDel (Action<T> handler) => _onReleaseDel = handler;

        /// <summary>
        /// 设置生成对象回调
        /// </summary>
        public void SetOnGenDel (Action<T> handler) => _onGenDel = handler;

        /// <summary>
        /// 设置回收回调
        /// </summary>
        public void SetOnResycleDel (Action<T> handler) => _onResycleDel = handler;

        /// <summary>
        /// 设置对象池数量
        /// </summary>
        public void SetCapacity (int capacity) => Capacity = capacity;

        /// <summary>
        /// 设置对象的引用状态
        /// </summary>
        protected void SetObjectReferenceState(T obj,bool useState)
        {
            if (obj is null)
                throw new AquilaException( "SetObjectRefereceState ---> obj is null" );

            if (_objectMap.ContainsKey( obj ))
                _objectMap[obj] = useState;
        }

        /// <summary>
        /// 删除对象引用
        /// </summary>
        protected void DeleteObjectReference (T obj)
        {
            if(obj is null)
                throw new AquilaException( "DeleteObjectReference ---> obj is null" );

            if (_objectMap.ContainsKey( obj ))
                _objectMap.Remove( obj );
        }

        /// <summary>
        /// 增加对象引用
        /// </summary>
        protected void AddObjectReference(T obj,bool defaultState = false)
        {
            if(obj is null)
                throw new AquilaException( "AddObjectReference ---> obj is null" );

            if(_objectMap.ContainsKey(obj))
                throw new AquilaException( "AddObjectReference ---> contains object!!!" );

            _objectMap.Add( obj, defaultState );
        }

        public Type GetObjectType ()
        {
            return typeof( T );
        }

        #endregion

        #endregion
    }
}
