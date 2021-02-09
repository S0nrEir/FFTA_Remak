using AquilaFramework.Common.Tools;
using System.Collections.Generic;

namespace AquilaFramework.Common.Event
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    public class EventMgr : Singleton<EventMgr>
    {
        /// <summary>
        /// 触发事件
        /// </summary>
        public void FireEvent (EventID id,object[] args = null)
        {
            if (!_eventDic.TryGetValue( id, out var lst ))
            {
                Log.Warnning("EventMgr.FireEvent--->dose not contain id:"+id.ToString());
                return;
            }

            foreach (var arg in lst)
                arg.CallBack?.Invoke(args);
        }

        /// <summary>
        /// 事件注册
        /// </summary>
        public void RegisterEvent (EventID id,EventArgs arg)
        {
            List<EventArgs> lst;
            if (!_eventDic.ContainsKey( id ))
            {
                lst = new List<EventArgs>();
                _eventDic.Add( id, lst );
            }

            _eventDic.TryGetValue( id, out lst );
            lst.Add( arg );

            //大于一个以上的回调链，重新排序
            if (lst.Count > 1)
                ReSortEventDic( id );
        }

        /// <summary>
        /// 事件注销
        /// </summary>
        public void UnRegisterEvent (EventID id,object listener)
        {
            if (!_eventDic.TryGetValue( id, out var lst ))
            {
                Log.Warnning( "EventMgr.UnRegisterEvent--->dose not contain id:" + id.ToString() );
                return;
            }

            if (listener is null)
            {
                Log.Warnning( "EventMgr.UnRegisterEvent--->listener is null,id:"+id.ToString());
                return;
            }

            EventArgs arg = null;
            var cnt = lst.Count;
            for (int i = 0; i < cnt; i++)
            {
                arg = lst[i];
                if (!ReferenceEquals( listener, arg.Listener ))
                    continue;

                //todo待优化：避免遍历找到对应的listener
                lst.Remove( arg );
                ReSortEventDic( id );
                break;
            }
        }

        /// <summary>
        /// 事件回调集合
        /// </summary>
        private Dictionary<EventID, List<EventArgs>> _eventDic = new Dictionary<EventID, List<EventArgs>>();

        /// <summary>
        /// 根据回调顺序重新排序
        /// </summary>
        private void ReSortEventDic (EventID id)
        {
            if (!_eventDic.TryGetValue( id ,out var lst))
            {
                Log.Warnning( "EventMgr.ReSortEventDic--->dost not contains id:" + id.ToString() );
                return;
            }

            lst.Sort((x,y) => 
            {
                if (x.Order > y.Order)
                    return 1;

                if (x.Order < y.Order)
                    return -1;

                return 0;
            } );
        }

        /// <summary>
        /// 事件ID序列，仅作标识用
        /// </summary>
        private int _eventID = int.MaxValue;
    }
}
