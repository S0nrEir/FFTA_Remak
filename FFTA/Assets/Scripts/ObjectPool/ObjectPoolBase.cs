using AquilaFramework.Common.Tools;
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
    public abstract class ObjectPoolBase<T> : IObjectPool where T : class, new()
    {
        /// <summary>
        /// 对象池存储器
        /// </summary>
        private Queue<T> _pool;

        /// <summary>
        /// 保存对象回收状态的map，如果对象被获取，则标记为false，在池中为true
        /// </summary>
        private Dictionary<T, bool> _objectMap;

        /// <summary>
        /// 对象创建回调
        /// </summary>
        public delegate void PoolHandler (T obj);

        protected PoolHandler _onGenDel = null;

        /// <summary>
        /// 销毁对象回调
        /// </summary>
        protected PoolHandler _onResycleDel = null;

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

        }

        public ObjectPoolBase () 
        {
            _onGenDel = null;
            _onResycleDel = null;
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
            _onGenDel = onGenDel;
            _onResycleDel = onDisDelint;
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
            Capacity = DEFAULT_CAPACITY;
            ExpireTime = DEFAULT_EXPIRE_TIME;
            ReleaseTime = DEFAULT_RELEASE_TIME;
            _onGenDel = null;
            _onResycleDel = null;

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
                _objectMap.Add( obj, false );

                _onGenDel?.Invoke( obj );

                return obj;
            }

            obj = _pool.Dequeue();
            if (!_objectMap.ContainsKey( obj ))
            {
                Log.Warnning($"ObjectMap does not have Object:{obj}");
                return null;
            }
            _objectMap[obj] = false;

            return obj;
        }



        public void ReleaseAllUnused ()
        {
            throw new AquilaException( "" );
        }

        /// <summary>
        /// 设置对象的引用状态
        /// </summary>
        private void SetObjectReferenceState(T obj,bool useState)
        {
            if (obj is null)
                throw new AquilaException( "SetObjectRefereceState ---> obj is null" );

            if (_objectMap.ContainsKey( obj ))
                _objectMap[obj] = useState;
        }

        /// <summary>
        /// 回收
        /// </summary>
        public void Recycle ()
        {
            
        }
        #endregion
    }
}
