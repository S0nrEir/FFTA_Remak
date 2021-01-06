using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.UI
{
    /// <summary>
    /// UI管理器，包含UI的加载和基于quene的管理机制
    /// </summary>
    public class UIMgr
    {
        /// <summary>
        /// UI池
        /// </summary>
        private static Queue<UIBase> _uiPool = new Queue<UIBase>();

        /// <summary>
        /// 引用管理集合
        /// </summary>
        private static Dictionary<UIIDEnum, UIBase> _refDic = new Dictionary<UIIDEnum, UIBase>();

        /// <summary>
        /// 从缓存队列中获取指定的UI实例，如果没有返回null
        /// </summary>
        private static UIBase GetFromCache<T> (T ui) where T : UIBase
        {
            //var contains = _uiPool.Contains( ui );
            var contains = _refDic.TryGetValue( ui.ID ,out var tempUI);
            if (contains)
                return tempUI as T;

            return null;
        }

        /// <summary>
        /// 从缓存集合中获取指定的UI实例，如果没有返回NULL
        /// </summary>
        private static UIBase GetFromCache<T> (UIIDEnum id) where T : UIBase
        {
            if (_refDic.TryGetValue( id, out var ui ))
                return ui as T;

            return null;
        }

        /// <summary>
        /// 加载新UI
        /// </summary>
        private static UIBase LoadUI<T,W> (UIIDEnum id, UIParam param = null) where T : UIBase, new()//todo不觉得这传泛型的方式是一种好的处理方式
                                                                              where W:WindowBase
        {
            var newUI = new T();
            if (!newUI.Init<W>( param ))
                return null;

            return newUI;
        }

        /// <summary>
        /// 加载并显示一个UI实例
        /// </summary>
        public static void ShowUI<T,W> (UIIDEnum uiID,UIParam param = null) where T : UIBase, new()
                                                                            where W:WindowBase
        {
            //检查缓存队列里有没有已经打开的此类UI实例
            var ui = GetFromCache<T>( uiID );
            if (ui is null)//拿不到，加载新的
            {
                ui = LoadUI<T,W>( uiID, param );
                if (ui is null)//todo这里处理的其实不太好
                {
                    Tools.Log.Error("UIMgr--->faild to load ui:"+ typeof(T).Name);
                    return;
                }

                CheckClosePrevUI( _uiPool.Peek(), ui );

                _uiPool.Enqueue( ui );
                _refDic.Add( uiID, ui );

                ui.OnLoad();
                ui.Show();
            }
            else//能拿到，直接显示
                ui.Show();

        }

        /// <summary>
        /// 检查是否需要关闭前一级UI
        /// </summary>
        private static void CheckClosePrevUI (UIBase prevUI,UIBase currUI)
        {
            if (currUI is null)
                return;

            if (!currUI.IsHidePrevious)
                return;

            if (!ReferenceEquals( null, prevUI ))
                prevUI.Close();
        }

        /// <summary>
        /// 加载并显示一个UI实例
        /// </summary>
        public static void ShowUI<T,W> (int id, UIParam param = null) where T : UIBase, new()
                                                                      where W:WindowBase
        {
            ShowUI<T,W>( (UIIDEnum)id, param );
        }


    }
}
