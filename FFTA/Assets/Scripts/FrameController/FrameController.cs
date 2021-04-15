using AquilaFramework.Common.Tools;
using System;
using System.Collections.Generic;

namespace AquilaFramework.Common
{
    /// <summary>
    /// 帧脚本更新管理器
    /// </summary>
    public class FrameController : Singleton<FrameController>
    {
        #region public methods

        /// <summary>
        /// 每秒回调
        /// </summary>
        public void RegisterPerSecUpdateFuncs (object obj, Action callBack)
        {
            if (_perSecRegDic is null || _perSecRegDic.ContainsKey( obj ))
            {
                Log.Warnning( $"_perSecRegDic has value:{obj.ToString()}" );
                return;
            }

            _perSecRegDic.Add( obj, callBack );
        }

        public void UnRegisterPerSecUpdateFuncs (object obj)
        {
            if (_perSecUnRegLst is null)
                return;

            if (_perSecUnRegLst.Contains( obj ))
                return;

            _perSecUnRegLst.Add( obj );
        }

        /// <summary>
        /// lateUpdate注册
        /// </summary>
        public void RegisterLateUpdateFuncs (object obj, Action callBack)
        {
            if (_lateUpdateRegDic is null || _lateUpdateRegDic.ContainsKey( obj ))
            {
                Log.Warnning( $"fixedUpdate has value:{obj.ToString()}");
                return;
            }

            _lateUpdateRegDic.Add( obj, callBack );
        }

        /// <summary>
        /// lateUpdate注销
        /// </summary>
        public void UnRegisterLateUpdateFuncs (object obj)
        {
            if (_lateUpdateUnRegLst is null)
                return;

            if (_lateUpdateUnRegLst.Contains( obj ))
                return;

            _lateUpdateUnRegLst.Add( obj );
        }

        public void RegisterFixedUpdateFuncs (object obj, Action<float> callBack)
        {
            if (_fixedUpdateRegDic is null || _fixedUpdateRegDic.ContainsKey( obj ))
            {
                Log.Warnning( $"fixedUpdate has value:{obj.ToString()}");
                return;
            }

            _fixedUpdateRegDic.Add( obj, callBack );
        }

        /// <summary>
        /// FixedUpdate注销
        /// </summary>
        public void UnRegisterFixedUpdateFuncs (object obj)
        {
            if (_fixedUpdateUnRegLst is null)
                return;

            if (_fixedUpdateUnRegLst.Contains( obj ))
                return;

            _fixedUpdateUnRegLst.Add( obj );
        }

        /// <summary>
        /// Update回调注册
        /// </summary>
        public void RegisterUpdateFuncs (object obj, Action<float> action)
        {
            //if (_updateFuncDic is null)
            //    return;

            //if (_updateFuncDic.ContainsKey( obj ))
            //    return;

            if (_updateRegDic is null || _updateRegDic.ContainsKey( obj ))
            {
                Log.Warnning( $"update has value:{obj.ToString()}");
                return;
            }

            _updateRegDic.Add( obj, action );
        }

        /// <summary>
        /// Update回调注销
        /// </summary>
        public void UnRegisterUpdateFuncs (object obj)
        {
            if (_updateUnRegLst.Contains( obj ))
                return;

            _updateUnRegLst.Add( obj );
        }


        #endregion

        #region private methods

        private void FinishTodoList ()
        {
            FinishUpdateTodoList();
            FinishFixedUpdateTodoList();
            FinishLateUpdateTodoList();
            FinishPerSecTodoList();
        }

        private void FinishUpdateTodoList ()
        {
            //add
            if (!ReferenceEquals( null, _updateRegDic ))
            {
                foreach (var kv in _updateRegDic)
                {
                    if (_updateFuncDic.ContainsKey( kv.Key ))
                        continue;

                    _updateFuncDic.Add( kv.Key, kv.Value );
                }
                _updateRegDic.Clear();
            }

            //remove
            if (!ReferenceEquals( null, _updateUnRegLst ))
            {
                foreach (var obj in _updateUnRegLst)
                {
                    _updateFuncDic.Remove( obj );
                    //if (_updateFuncDic.ContainsKey( obj ))
                    //    _updateFuncDic.Remove( obj );
                }
                _updateUnRegLst.Clear();
            }
        }

        private void FinishFixedUpdateTodoList ()
        {
            //add
            if (!ReferenceEquals( null, _fixedUpdateRegDic ))
            {
                foreach (var kv in _fixedUpdateRegDic)
                {
                    if (_fixedUpdateFuncDic.ContainsKey( kv.Key ))
                        continue;

                    _fixedUpdateFuncDic.Add( kv.Key, kv.Value );
                }
                _fixedUpdateRegDic.Clear();
            }

            //remove
            if (!ReferenceEquals( null, _fixedUpdateUnRegLst ))
            {
                foreach (var obj in _fixedUpdateUnRegLst)
                    _fixedUpdateFuncDic.Remove( obj );

                _fixedUpdateUnRegLst.Clear();
            }
        }

        /// <summary>
        /// 更新操作集合
        /// </summary>
        private void FinishLateUpdateTodoList ()
        {
            if (!ReferenceEquals( null, _lateUpdateRegDic ))
            {
                foreach (var kv in _lateUpdateRegDic)
                {
                    if (_lateUpdateFuncDic.ContainsKey( kv.Key ))
                        continue;

                    _lateUpdateFuncDic.Add( kv.Key, kv.Value );
                }
                _lateUpdateRegDic.Clear();
            }

            if (!ReferenceEquals( null, _lateUpdateUnRegLst ))
            {
                foreach (var obj in _lateUpdateUnRegLst)
                    _lateUpdateFuncDic.Remove( obj );

                _lateUpdateUnRegLst.Clear();
            }
        }

        private void FinishPerSecTodoList ()
        {
            if (!ReferenceEquals( null, _perSecRegDic ))
            {
                foreach (var kv in _perSecRegDic)
                {
                    if (_perSecFuncDic.ContainsKey( kv.Key ))
                        continue;

                    _perSecFuncDic.Add( kv.Key, kv.Value );
                }
                _perSecRegDic.Clear();
            }

            if (!ReferenceEquals( null, _perSecUnRegLst ))
            {
                foreach (var obj in _perSecUnRegLst)
                    _perSecFuncDic.Remove( obj );

                _perSecUnRegLst.Clear();
            }
        }

        #endregion


        #region update

        public void Update (float deltaTime)
        {
            if (_updateFuncDic is null)
                return;

            foreach (var kv in _updateFuncDic)
                kv.Value?.Invoke( deltaTime );

            //在当前帧内完成所有的注册/注销操作，因为延迟关系到下一帧可能发生crash
            FinishUpdateTodoList();
        }

        public void FixedUpdate (float deltaTime)
        {
            if (_fixedUpdateFuncDic is null)
                return;

            foreach (var kv in _fixedUpdateFuncDic)
                kv.Value?.Invoke( deltaTime );

            //在当前帧内完成所有的注册/注销操作，因为延迟关系到下一帧可能发生crash
            FinishFixedUpdateTodoList();

            _timerPerSec += deltaTime;
            if (_timerPerSec >= Seconds_Interval)
            {
                _timerPerSec = 0f;
                foreach (var kv in _perSecRegDic)
                    kv.Value?.Invoke();

                FinishPerSecTodoList();
            }
        }

        public void LateUpdate ()
        {
            if (_lateUpdateFuncDic is null)
                return;

            foreach (var kv in _lateUpdateFuncDic)
                kv.Value?.Invoke();

            //在当前帧内完成所有的注册/注销操作，因为延迟关系到下一帧可能发生crash
            FinishLateUpdateTodoList();
        }

        #endregion


        #region init
        /// <summary>
        /// 初始化
        /// </summary>
        public void EnsureInit ()
        {
            Init();
        }

        public override void Init ()
        {
            if (Inited)
                return;

            _updateFuncDic = new Dictionary<object, Action<float>>();
            _fixedUpdateFuncDic = new Dictionary<object, Action<float>>();
            _lateUpdateFuncDic = new Dictionary<object, Action>();
            _perSecFuncDic = new Dictionary<object, Action>();

            _updateRegDic = new Dictionary<object, Action<float>>();
            _fixedUpdateRegDic = new Dictionary<object, Action<float>>();
            _lateUpdateRegDic = new Dictionary<object, Action>();
            _perSecRegDic = new Dictionary<object, Action>();

            _updateUnRegLst = new List<object>();
            _fixedUpdateUnRegLst = new List<object>();
            _lateUpdateUnRegLst = new List<object>();
            _perSecUnRegLst = new List<object>();

            Inited = true;
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void CleanUp ()
        {
            _updateFuncDic?.Clear();
            _fixedUpdateFuncDic?.Clear();
            _lateUpdateFuncDic?.Clear();
            _perSecFuncDic?.Clear();

            _updateRegDic?.Clear();
            _fixedUpdateRegDic?.Clear();
            _lateUpdateRegDic?.Clear();
            _perSecRegDic?.Clear();

            _updateUnRegLst?.Clear();
            _fixedUpdateUnRegLst?.Clear();
            _lateUpdateUnRegLst?.Clear();
            _perSecUnRegLst?.Clear();

            _timerPerSec = 0f;
        }

        /// <summary>
        /// 反初始化
        /// </summary>
        public override void DeInit ()
        {
            CleanUp();
            //callBack
            _updateFuncDic = null;
            _fixedUpdateFuncDic = null;
            _lateUpdateFuncDic = null;
            _perSecFuncDic = null;

            //reg
            _updateRegDic = null;
            _fixedUpdateRegDic = null;
            _lateUpdateRegDic = null;
            _perSecRegDic = null;

            //unReg
            _updateUnRegLst = null;
            _fixedUpdateUnRegLst = null;
            _lateUpdateUnRegLst = null;
            _perSecUnRegLst = null;

            //_timerPerSec = 0f;
            Inited = false;
        }
        #endregion

        #region fields

        /// <summary>
        /// 初始化标记
        /// </summary>
        public bool Inited { get; private set; } = false;

        /// <summary>
        /// update回调列表
        /// </summary>
        private Dictionary<object, Action<float>> _updateFuncDic;

        private Dictionary<object, Action<float>> _updateRegDic;

        private List<object> _updateUnRegLst;

        /// <summary>
        /// fixedUpdate回调列表
        /// </summary>
        private Dictionary<object, Action<float>> _fixedUpdateFuncDic;

        private Dictionary<object, Action<float>> _fixedUpdateRegDic;

        private List<object> _fixedUpdateUnRegLst;

        /// <summary>
        /// lateUpdate回调列表
        /// </summary>
        private Dictionary<object, Action> _lateUpdateFuncDic;

        private Dictionary<object, Action> _lateUpdateRegDic;

        private List<object> _lateUpdateUnRegLst;

        /// <summary>
        /// 每秒回调列表
        /// </summary>
        private Dictionary<object, Action> _perSecFuncDic;

        private Dictionary<object, Action> _perSecRegDic;

        private List<object> _perSecUnRegLst;

        #region timer

        /// <summary>
        /// 每秒计时器
        /// </summary>
        private static float _timerPerSec = 0f;

        #endregion

        #region const

        /// <summary>
        /// 每秒回调的内部计时
        /// </summary>
        private const float Seconds_Interval = 1f;

        #endregion

        #endregion
    }
}

