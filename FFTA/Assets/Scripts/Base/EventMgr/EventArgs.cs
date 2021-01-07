using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.Common.Define.DelegateDefine;

namespace Game.Common.Event
{
    /// <summary>
    /// Event参数类，标识了该事件调用的优先级，回调和事件ID
    /// </summary>
    public class EventArgs
    {
        public EventArgs (EventID id, Void_Objects_Del callBack, object listener, EventOrderEnum order = EventOrderEnum.DEFAULT)
        {
            EventID = id;
            CallBack = callBack;
            Order = (int)order;
            Listener = listener;
        }

        /// <summary>
        /// default Constructor
        /// </summary>
        public EventArgs ()
        {
            EventID = EventID.None;
            CallBack = null;
            Order = (int)EventOrderEnum.DEFAULT;
            Listener = null;
        }

        /// <summary>
        /// eventID
        /// </summary>
        public EventID EventID { get; private set; }

        /// <summary>
        /// 回调
        /// </summary>
        public Void_Objects_Del CallBack { get; private set; }

        /// <summary>
        /// 回调顺序
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// 监听器
        /// </summary>
        public object Listener { get; private set; }
    }


    /// <summary>
    /// event回调
    /// </summary>
    //public delegate void EventCallBack (object[] args);

    /// <summary>
    /// 回调顺序
    /// </summary>
    public enum EventOrderEnum
    {
        /// <summary>
        /// 数据类
        /// </summary>
        DATA = 0,

        /// <summary>
        /// 场景
        /// </summary>
        SCENE,
        
        /// <summary>
        /// sprite
        /// </summary>
        SPRITE,

        /// <summary>
        /// 音频
        /// </summary>
        AUDIO,

        /// <summary>
        /// UI
        /// </summary>
        UI,

        /// <summary>
        /// 默认处于最后的调用顺序
        /// </summary>
        DEFAULT = 999,
    }
}


