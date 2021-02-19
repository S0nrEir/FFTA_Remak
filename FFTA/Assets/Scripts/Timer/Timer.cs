using System;
using UnityEngine;

namespace AquilaFramework.Common.Timer
{
    public partial class TimerMgr
    {

        /// <summary>
        /// 计时器类
        /// </summary>
        public class Timer
        {
            private Timer () { }

            public Timer (int id)
            {
                ID = id;
            }

            #region methods

            /// <summary>
            /// 开始该计时器
            /// </summary>
            public void Start ()
            {
                _isActive = true;
            }

            /// <summary>
            /// 暂停该计时器
            /// </summary>
            public void Pause ()
            {
                _isActive = false;
            }

            /// <summary>
            /// 停止该计时器
            /// </summary>
            public void Stop ()
            {
                Pause();
            }

            /// <summary>
            /// 重置计数器和内部计时器
            /// </summary>
            private void Reset ()
            {
                _currTimer = 0f;
                _counter = 0;
            }

            /// <summary>
            /// 手动销毁该计时器
            /// </summary>
            public void Destroy ()
            {
                Stop();
                _readyToDestroy = true;
            }

            /// <summary>
            /// 重新开始计时器
            /// </summary>
            public void ReStart ()
            {
                Stop();
                Reset();
                Start();
            }

            /// <summary>
            /// 开启一个重复m次，每次间隔n秒的回调
            /// </summary>
            public void StartTick (float n, int m, Action callBack)
            {
                if (m <= 0)
                    return;

                if (n <= 0f)
                    return;

                _isRepeat = true;
                _callBack = callBack;
                _repeatTime = m;
                _intervalTime = n;
                _currTimer = 0;
                _counter = 0;
                Start();
                Ins.Add( this );
            }

            /// <summary>
            /// n秒后开启一个回调
            /// </summary>
            public void StartCounting (float n, Action callBack)
            {
                if (n <= 0f)
                    return;

                _isRepeat = false;
                _callBack = callBack;
                _repeatTime = 0;
                _intervalTime = n;
                _currTimer = 0f;
                _counter = 0;
                Start();
                Ins.Add( this );
            }

            /// <summary>
            /// toString
            /// </summary>
            public override string ToString ()
            {
                return $"Timer ---> {ID}";
            }

            #endregion

            #region fields
            public bool Active => _isActive;

            /// <summary>
            /// 是否重复调用
            /// </summary>
            public bool _isRepeat;

            /// <summary>
            /// 是否激活
            /// </summary>
            private bool _isActive;

            /// <summary>
            /// 回调间隔
            /// </summary>
            public float _intervalTime;

            /// <summary>
            /// 重复次数
            /// </summary>
            public int _repeatTime;

            /// <summary>
            /// 当前自己的内部时间
            /// </summary>
            public float _currTimer = 0f;

            /// <summary>
            /// 销毁标记
            /// </summary>
            public bool ReadyToDestroy => _readyToDestroy;

            /// <summary>
            /// 销毁标记
            /// </summary>
            private bool _readyToDestroy = false;

            /// <summary>
            /// 计数器
            /// </summary>
            public int _counter = 0;

            /// <summary>
            /// id
            /// </summary>timer.
            public int ID;

            /// <summary>
            /// 回调
            /// </summary>
            public Action _callBack;

            /// <summary>
            /// 最大次数限制
            /// </summary>
            public const int MaxLimit = int.MaxValue;
            #endregion
        }
    }
}
