using AquilaFramework.Common.Tools;
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
            //var mainCamGo = GameObject.Find("Main_Camera");
            //_mainCamera = mainCamGo.GetComponent<Camera>();

            //uiCamera
            //var uiCamGo = GameObject.Find("UI_Camra");
            //_uiCamera = uiCamGo.GetComponent<Camera>();
            GenMainCamera();
            GenUICamera();

            //UIRoot
            GetUIRoot();
            GetNormalUiRoot();
            GetGuideUIRoot();
            //var uiGo = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "UIRoot" );
            var guideGo = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "GuideLayer" );
            //var normalGo = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "NormalLayer" );
            //_uiRoot = uiGo.transform;
            _guideUIRoot = guideGo.transform;
            //_normalUIRoot = normalGo.transform;
        }

        /// <summary>
        /// 保留gameObject
        /// </summary>
        //private static void DontDestroyOnLoad (params GameObject[] gos)
        //{
        //    foreach (var go in gos)
        //        GameObject.DontDestroyOnLoad( go );
        //}

        #region camera
        /// <summary>
        /// UI相机单例
        /// </summary>
        private static Camera GenUICamera ()
        {
            if (_uiCamera is null)
            {
                var go = GameObject.Find( "UI_Camera" );
                _uiCamera = go.GetComponent<Camera>();
                if (_uiCamera is null)
                {
                    Log.Error("_uiCamera is null!");
                    return null;
                }
                GameObject.DontDestroyOnLoad( _uiCamera );
            }

            return _uiCamera;
        }

        /// <summary>
        /// UI相机
        /// </summary>
        private static Camera _uiCamera = null;

        /// <summary>
        /// UI相机
        /// </summary>
        private static Camera UICamera => _uiCamera ?? GenUICamera();


        /// <summary>
        /// 获取主相机单例
        /// </summary>
        private static Camera GenMainCamera ()
        {
            if (_mainCamera is null)
            {
                var go = GameObject.Find( "Main_Camera" );
                _mainCamera = go.GetComponent<Camera>();
                if (_mainCamera is null)
                {
                    Log.Error("_mainCamera is null");
                    return null;
                }
                GameObject.DontDestroyOnLoad( go );
            }

            return _mainCamera;
        }

        /// <summary>
        /// 主相机，场景相机
        /// </summary>
        private static Camera _mainCamera = null;

        /// <summary>
        /// 主相机，场景相机
        /// </summary>
        public static Camera MainCamera => _mainCamera ?? GenMainCamera();
        #endregion

        #region UIRoot

        /// <summary>
        /// 获取UI节点单例
        /// </summary>
        private static Transform GetUIRoot ()
        {
            if (_uiRoot is null)
            {
                //var roots = GameObject.FindGameObjectsWithTag( "UIRoot" );
                //#todo查找优化，尽量避免Find
                var go = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "UIRoot" );
                _uiRoot = go.transform;
                //_uiRoot = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "UIRoot" );
                GameObject.DontDestroyOnLoad( _uiRoot );
            }

            return _uiRoot;
        }

        /// <summary>
        /// UI根节点
        /// </summary>
        private static Transform _uiRoot = null;

        /// <summary>
        /// UI根节点
        /// </summary>
        public static Transform UIRoot => _uiRoot ?? GetUIRoot();

        /// <summary>
        /// 获取通常UI节点
        /// </summary>
        private static Transform GetNormalUiRoot ()
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

        /// <summary>
        /// 通常UI节点
        /// </summary>
        private static Transform _normalUIRoot = null;

        /// <summary>
        /// 通常UI节点
        /// </summary>
        public static Transform NormalUILayer => _normalUIRoot ?? GetNormalUiRoot();

        /// <summary>
        /// 获取引导层单例
        /// </summary>
        private static Transform GetGuideUIRoot ()
        {
            if (_guideUIRoot is null)
            {
                var go = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "GuideLayer" );
                _guideUIRoot = go.transform;
                //_guideUIRoot = CommonTools.FindFromObjects( GameObject.FindGameObjectsWithTag( "UIRoot" ), "GuideLayer" );

            }
            return _guideUIRoot;
        }

        /// <summary>
        /// 引导层
        /// </summary>
        private static Transform _guideUIRoot = null;

        /// <summary>
        /// 引导层
        /// </summary>
        public static Transform GuideUILayer => _guideUIRoot ?? GetGuideUIRoot();
        #endregion
    }
}

