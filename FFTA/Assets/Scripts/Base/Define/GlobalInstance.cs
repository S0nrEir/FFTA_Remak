﻿using AquilaFramework.Common.Tools;
using UnityEngine;

namespace AquilaFramework.Common.Define
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

            //uiCamera
            var uiCamGo = GameObject.Find("UI_Camra");
            _uiCamera = uiCamGo.GetComponent<Camera>();

            //UIRoot
            var uiGo = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "UIRoot" );
            var guideGo = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "GuideLayer" );
            var normalGo = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "NormalLayer" );
            _uiRoot = uiGo.transform;
            _guideUIRoot = guideGo.transform;
            _normalUIRoot = normalGo.transform;

            //hold all
            DontDestroyOnLoad
                (
                    uiCamGo,
                    mainCamGo,
                    uiGo,
                    guideGo,
                    normalGo
                );
        }

        /// <summary>
        /// 保留gameObject
        /// </summary>
        private static void DontDestroyOnLoad (params GameObject[] gos)
        {
            foreach (var go in gos)
                GameObject.DontDestroyOnLoad( go );
        }

        #region camera

        /// <summary>
        /// UI相机
        /// </summary>
        private static Camera _uiCamera = null;

        /// <summary>
        /// UI相机
        /// </summary>
        private static Camera UICamera
        {
            get
            {
                if (_uiCamera is null)
                {
                    var go = GameObject.Find("UI_Camera");
                    _uiCamera = go.GetComponent<Camera>();
                    GameObject.DontDestroyOnLoad( _uiCamera );
                }
                return _uiCamera;
            }
        }

        /// <summary>
        /// 主相机，场景相机
        /// </summary>
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
        private static Transform _uiRoot = null;

        /// <summary>
        /// UI根节点
        /// </summary>
        public static Transform UIRoot
        {
            get
            {
                if (_uiRoot is null)
                {
                    //var roots = GameObject.FindGameObjectsWithTag( "UIRoot" );
                    //todo查找优化，尽量避免Find
                    var go = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "UIRoot" );
                    _uiRoot = go.transform;
                    //_uiRoot = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "UIRoot" );
                    GameObject.DontDestroyOnLoad( _uiRoot );
                }
                return _uiRoot;
            }
        }

        private static Transform _normalUIRoot = null;

        /// <summary>
        /// 通常UI节点
        /// </summary>
        public static Transform NormalUILayer
        {
            get
            {
                if (_normalUIRoot is null)
                {
                    var go = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "NormalLayer" );
                    _normalUIRoot = go.transform;
                    //_normalUIRoot = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "NormalLayer" );
                    //GameObject.DontDestroyOnLoad( _uiRoot );
                }
                return _normalUIRoot;
            }
        }

        private static Transform _guideUIRoot = null;

        /// <summary>
        /// 引导层
        /// </summary>
        public static Transform GuideUILayer
        {
            get
            {
                if (_guideUIRoot is null)
                {
                    var go = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "GuideLayer" );
                    _guideUIRoot = go.transform;
                    //_guideUIRoot = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "GuideLayer" );

                }
                return _guideUIRoot;
            }
        }

        #endregion
    }
}

