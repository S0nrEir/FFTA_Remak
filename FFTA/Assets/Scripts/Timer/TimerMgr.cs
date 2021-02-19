using AquilaFramework.Common.Tools;
using System;
using System.Collections.Generic;

namespace AquilaFramework.Common.Timer
{
    public partial class TimerMgr
    {
        #region//methods
        /// <summary>
        /// n秒后开启一个回调
        /// </summary>
        public Timer StartCounting (float n, Action callBack)
        {
            var timer = Gen();
            timer.StartCounting( n, callBack );
            return timer;
        }

        /// <summary>
        /// 开启一个重复m次，每次间隔n秒的回调
        /// </summary>
        public Timer StartTick (float n, int m, Action callBack)
        {
            var timer = Gen();
            timer.StartTick( n, m, callBack );
            return timer;
        }

        /// <summary>
        /// 添加计时器到计时队列中，添加失败返回false
        /// </summary>
        private bool Add (Timer timer)
        {
            var dic = Ins._regTimerDic;
            if (dic.ContainsKey( timer.ID ))
                return false;

            dic.Add( timer.ID, timer );
            return true;
        }


        /// <summary>
        /// 将待处理的计时器从计时队列中进行处理
        /// </summary>
        private void FinishUpdateTodoList ()
        {
            if (_regTimerDic != null && _regTimerDic.Count != 0)
            {
                foreach (var kv in _regTimerDic)
                {
                    if(_currTimerDic.ContainsKey(kv.Key))
                    {
                        Log.Warnning($"TimerMgr.FinishUpdateTodoList--->register contains id:{kv.Key}");
                        continue;
                    }

                    _currTimerDic.Add( kv.Key, kv.Value );
                }
                _regTimerDic.Clear();
            }



            if (_unRegTimerLst != null && _unRegTimerLst.Count != 0)
            {
                foreach (var id in _unRegTimerLst)
                {
                    if (!_currTimerDic.TryGetValue( id ,out var timer))
                    {
                        Log.Warnning( $"TimerMgr.FinishUpdateTodoList--->unRegister dosent contains id:{id}");
                        continue;
                    }

                    if (!timer.ReadyToDestroy)
                        continue;

                    _currTimerDic.Remove( id );
                }
                _unRegTimerLst.Clear();
            }

        }

        /// <summary>
        /// udate后进行清理操作
        /// </summary>
        private void LateUpdate ()
        {
            FinishUpdateTodoList();
        }

        /// <summary>
        /// 回调，考虑放到fixedUpdate中
        /// </summary>
        private void FixedUpdate (float deltaTime)
        {
            foreach (var kv in _currTimerDic)
            {
                var timer = kv.Value;
                if (!timer.Active)//没有激活的，跳过
                    continue;

                timer._currTimer += deltaTime;
                if (timer._currTimer < timer._intervalTime)//没有到回调时间的跳过
                    continue;

                //到了回调时间
                //重复调用
                if (timer._isRepeat)
                {
                    timer._counter++;
                    if (timer._counter >= timer._repeatTime)//回调次数达到上限
                    {
                        timer.Destroy();
                        continue;
                    }

                    timer._callBack?.Invoke();
                    timer._currTimer = 0;
                }
                else//n秒后的回调
                {
                    timer._callBack?.Invoke();
                    timer.Destroy();
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void EnsureInit ()
        {
            var frameIns = FrameController.I;
            if (!frameIns.Inited)
                frameIns.EnsureInit();

            frameIns.RegisterFixedUpdateFuncs( Ins, FixedUpdate );
            frameIns.RegisterLateUpdateFuncs( Ins, LateUpdate );

            Ins.IDPool = int.MaxValue;
        }

        /// <summary>
        /// 反初始化
        /// </summary>
        public void DeInit ()
        {
            var frameIns = FrameController.I;
            frameIns.UnRegisterFixedUpdateFuncs( Ins );
            frameIns.UnRegisterLateUpdateFuncs( Ins );
        }

        #endregion

        /// <summary>
        /// 当前的timer列表
        /// </summary>
        private Dictionary<int, Timer> _currTimerDic = new Dictionary<int, Timer>();

        /// <summary>
        /// 待注册的timer列表
        /// </summary>
        private Dictionary<int, Timer> _regTimerDic = new Dictionary<int, Timer>();

        /// <summary>
        /// 待注销的timer列表
        /// </summary>
        private List<int> _unRegTimerLst = new List<int>();

        #region//fields

        /// <summary>
        /// ID池
        /// </summary>
        public int IDPool
        {
            get => _idPool;
            private set
            {
                _idPool = value;
            }
        }

        /// <summary>
        /// ID池
        /// </summary>
        private int _idPool = int.MaxValue;

        #endregion

        /// <summary>
        /// 生成一个Timer
        /// </summary>
        public static Timer Gen ()
        {
            return new Timer( Ins.IDPool-- )
            {
                _callBack = null,
                _isRepeat = false,
                _intervalTime = 0f,
                _repeatTime = 0,
                _currTimer = 0f,
                _counter = 0,
            };
        }

        /// <summary>
        /// 单例
        /// </summary>
        private static TimerMgr _ins = null;

        /// <summary>
        /// 生成单例
        /// </summary>
        private static TimerMgr GetIns ()
        {
            _ins = new TimerMgr();
            return _ins;
        }

        /// <summary>
        /// 单例
        /// </summary>
        public static TimerMgr Ins => _ins ?? GetIns();
    }
}
