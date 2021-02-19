using UnityEngine;
using static AquilaFramework.Common.Define.DelegateDefine;

namespace AquilaFramework.Common.UI
{
    /// <summary>
    /// UI的基类，所有UI实例继承于此，对于所有UI，禁止使用单例
    /// </summary>
    public abstract class UIBase
    {
        #region constructor
        /// <summary>
        /// 通过此构造UI实例
        /// </summary>
        public UIBase (UIParam param)
        {
            if (!Init( param ))
            {
                Tools.Log.Error( $"load faild:{GetType().Name}");
                return;
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public UIBase () 
        {
            Tools.Log.Info($"UIBase constructed:{GetType().Name}");
            //Tools.Log.Warnning("通过默认构造了一个UI实例");
        }
        #endregion

        #region private methods

        /// <summary>
        /// 带Window的UI初始化，初始化成功返回true
        /// </summary>
        public bool Init<T> (UIParam uiParam) where T:WindowBase
        {
            param = uiParam;
            //根据path加载
            var obj = Resources.Load<GameObject>( Path );
            if (obj is null)
                return false;

            UIGameObject = obj;
            UICanvas = UIGameObject.AddComponent<Canvas>();
            UIGameObject.AddComponent<CanvasGroup>();
            UICanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            //set UI name:
            UIGameObject.name = ID.ToString();

            ResetTransform();

            //处理Window的类型转换
            var tWindow = UIGameObject.GetComponent<T>();
            if (tWindow is null)
                Window = null;
            else
                Window = tWindow;

            return true;
        }

        /// <summary>
        /// 不带Window的UI初始化
        /// </summary>
        public bool Init (UIParam uiParam)
        {
            param = uiParam;
            //根据path加载
            var obj = Resources.Load<GameObject>( Path );
            if (obj is null)
                return false;

            UIGameObject = obj;
            UICanvas = UIGameObject.AddComponent<Canvas>();
            UIGameObject.AddComponent<CanvasGroup>();
            UICanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            ResetTransform();

            Window = null;

            return true;
        }

        /// <summary>
        /// 重设根Object的transform信息
        /// </summary>
        private void ResetTransform ()
        {
            if (UIGameObject is null)
            {
                Tools.Log.Error( "UIBase.ResetTransform--->UIGameObejct is null!" );
                return;
            }

            var tran = UIGameObject.transform;
            tran.localScale = new Vector3( 1, 1, 1 );
            tran.eulerAngles = new Vector3( 0, 0, 0 );
        }

        #endregion

        #region public methods

        /// <summary>
        /// showUI
        /// </summary>
        public void Show ()
        {
            IsShowed = true;
            UIGameObject.SetActive( true );
            OnEnable();
            OnShow();
        }

        /// <summary>
        /// closeUI
        /// </summary>
        public void Close ()
        {
            IsShowed = false;
            UIGameObject.SetActive( false );
            OnDisable();
            OnClose();
        }

        #endregion

        #region sub class impl

        public virtual void OnLoad()
        {
            if (Window is null)
                Tools.Log.Warnning( $"ui {UIGameObject.name} dosent have window sub class" );
        }

        protected virtual void OnEnable ()
        {
            RegisterEvent();
        }

        protected virtual void OnShow ()
        {
            
        }

        protected virtual void OnClose ()
        {
            
        }

        protected virtual void OnDisable ()
        {
            UnRegisterEvent();
        }

        protected virtual void OnDestroy ()
        {
            
        }

        protected virtual void RegisterEvent ()
        {
            
        }

        protected virtual void UnRegisterEvent ()
        {
            
        }


        #endregion

        /// <summary>
        /// UI层级
        /// </summary>
        public virtual UILayerEnum Layer { get; } = UILayerEnum.Normal;

        /// <summary>
        /// 触发后是否隐藏前一个UI实例
        /// </summary>
        public virtual bool IsHidePrevious { get; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShowed { get; private set; }

        /// <summary>
        /// 参数
        /// </summary>
        public UIParam param { get; private set; }

        /// <summary>
        /// UI实例唯一ID
        /// </summary>
        public UIIDEnum ID { get; }

        /// <summary>
        /// canvas对象
        /// </summary>
        protected Canvas UICanvas { get; private set; }

        /// <summary>
        /// 实例gameObject对象
        /// </summary>
        protected GameObject UIGameObject { get; private set; }

        /// <summary>
        /// Transform组件
        /// </summary>
        public Transform UITransform => UIGameObject is null ? null : UIGameObject.transform ;

        /// <summary>
        /// 是否需要每帧更新
        /// </summary>
        protected virtual bool IsUseUpdate { get; }

        /// <summary>
        /// 是否同步加载，true为是，false为异步加载
        /// </summary>
        protected virtual bool IsLoadAsync { get; }

        /// <summary>
        /// 实例路径
        /// </summary>
        protected abstract string Path { get; } 

        /// <summary>
        /// 窗口引用实例，任何对于Prefab元素/组件的操作都通过此字段实现
        /// </summary>
        protected WindowBase Window { get; private set; } = null;
    }

    /// <summary>
    /// UI参数类
    /// </summary>
    public class UIParam
    {
        /// <summary>
        /// 界面参数
        /// </summary>
        public object Param;

        /// <summary>
        /// 界面加载参数回调
        /// </summary>
        public Void_Void_Del OnShow;
    }
}
