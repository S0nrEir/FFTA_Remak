using UnityEngine;
using static AquilaFramework.Common.Define.DelegateDefine;
using UnityEngine.EventSystems;

namespace AquilaFramework.Common.UI
{
    [System.Serializable]
    public class ObjectPickerValue
    {
        public GameObject Go;
        public System.Type Type;
        public string Name;

        public ObjectPickerValue (GameObject go, System.Type typ, string name)
            => Set( go, typ, name );

        public ObjectPickerValue () { }

        public void SetGo (GameObject go) => Go = go;
        public void SetType (System.Type type) => this.Type = type;
        public void SetName (string name) => this.Name = name;

        public void Set (GameObject go, System.Type typ, string name)
        {

            SetGo( go );
            SetType( typ );
            SetName( name );
        }

        public bool Setted => Go != null && !string.IsNullOrEmpty( Name );
    }

    /// <summary>
    /// UI预设资源引用基类
    /// </summary>
    [System.Serializable]
    public class WindowBase : MonoBehaviour
    {
        //UI保存思路：
        //数组保存GameObject
        //对于每一项，前指定引用，后指定类型
        //输入框，保存类名
        //buttton，保存脚本


        #region GUILayout
        /// <summary> 
        /// 组件对象
        /// </summary>
        [HideInInspector]
        [SerializeField] public ObjectPickerValue[] _components; 


        /// <summary>
        /// 生成保存的类名
        /// </summary>
        [HideInInspector]
        public string _className = "test class name";

        #endregion

        /// <summary>
        /// 界面ID，ID和实例类型对应
        /// </summary>
        [SerializeField] private UIIDEnum _uiID = UIIDEnum.None;
        
        #region uiHandler
        private Void_Void_Del _onMouseEnterDel = null;
        private Void_Void_Del _onMouseExitDel = null;
        private void OnMouseEnter () => _onMouseEnterDel?.Invoke();
        private void OnMouseExit () => _onMouseExitDel?.Invoke();

        #endregion

        #region 周期函数，预留接口

        private Void_Void_Del _onAwakeDel = null;
        private Void_Void_Del _onEnableDel = null;
        private Void_Void_Del _onStartDel = null;
        private Void_Void_Del _onDisableDel = null;
        private Void_Void_Del _onDestryDel = null;

        void Awake () => _onAwakeDel?.Invoke();
        void OnEnable () => _onEnableDel?.Invoke();
        void Start () => _onStartDel?.Invoke();
        void OnDisable () => _onDisableDel?.Invoke();
        void OnDestroy () => _onDestryDel?.Invoke();

        #endregion

        /// <summary>
        /// 获取该Window对应的UIID
        /// </summary>
        public UIIDEnum GetUIID () => _uiID;


    }
}
