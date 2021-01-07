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
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init ()
        {
            //MainCamera
            var mainCamGo = GameObject.Find("Main_Camera");
            _mainCamera = mainCamGo.GetComponent<Camera>();

            //UIRoot
            _uiRoot = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "UIRoot" );
            _guideUIRoot = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "GuideLayer" );
            _normalUIRoot = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "NormalLayer" );

            //hold all
            DontDestroyOnLoad
                (
                    mainCamGo,
                    _uiRoot,
                    _guideUIRoot,
                    _normalUIRoot
                );
        }

        private static void DontDestroyOnLoad (params GameObject[] gos)
        {
            foreach (var go in gos)
                GameObject.DontDestroyOnLoad( go );
        }

        #region camera

        private static Camera _mainCamera = null;

        /// <summary>
        /// 主相机，场景相机
        /// </summary>
        public static Camera MainCamera
        {
            get
            {
                if (_mainCamera is null)
                {
                    var go = GameObject.Find( "Main_Camera" );
                    _mainCamera = go.GetComponent<Camera>();
                    GameObject.DontDestroyOnLoad( go );
                }
                return _mainCamera;
            }
        }

        #endregion

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
                    _normalUIRoot = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "NormalLayer" );
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
                if (_guideUIRoot is null)
                    _guideUIRoot = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "GuideLayer" );
                
                return _uiRoot;
            }
        }

        #endregion
    }
}

