using Game.Common.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Common.Define
{
    /// <summary>
    /// 游戏全局Object实例类
    /// </summary>
    public class GlobalInstance
    {
        #region UIRoot
        private static GameObject _uiRoot = null;

        /// <summary>
        /// UI根节点
        /// </summary>
        public static GameObject UIRoot
        {
            get
            {
                if (_uiRoot is null)
                {
                    //var roots = GameObject.FindGameObjectsWithTag( "UIRoot" );
                    //todo查找优化，尽量避免Find
                    _uiRoot = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "UIRoot" );
                    GameObject.DontDestroyOnLoad( _uiRoot );
                }
                return _uiRoot;
            }
        }

        private static GameObject _normalUIRoot = null;

        /// <summary>
        /// 通常UI节点
        /// </summary>
        public static GameObject NormalUIlLayer
        {
            get
            {
                if (_normalUIRoot is null)
                {
                    _uiRoot = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "NormalLayer" );
                    //GameObject.DontDestroyOnLoad( _uiRoot );
                }
                return _uiRoot;
            }
        }

        private static GameObject _guideUIRoot = null;

        /// <summary>
        /// 引导层
        /// </summary>
        public static GameObject GuideUILayer
        {
            get
            {
                if (_normalUIRoot is null)
                    _uiRoot = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "GuideLayer" );
                
                return _uiRoot;
            }
        }

        #endregion
    }
}

