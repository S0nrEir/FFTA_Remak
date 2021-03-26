using UnityEngine;
using static AquilaFramework.Common.Define.DelegateDefine;
using UnityEngine.EventSystems;

namespace AquilaFramework.Common.UI
{
    [System.Serializable]
    public class WindowInspectorObj
    {
        [SerializeField] private GameObject go;
        //#TODO 这里有个问题：对于System.Type类型不可以直接映射到SerializedProperty类型的任何表示类型的字段，所以要用string做转换处理
        [SerializeField] private string type;
        [SerializeField] private string name;

        public (GameObject go, string typ, string name) Values => (go, type, name);

        public WindowInspectorObj (GameObject go, string typ, string name)
            => Set( go, typ, name );

        public WindowInspectorObj () { }

        public void SetGo (GameObject go) => this.go = go;
        public void SetType (string type) => this.type = type;
        public void SetName (string name) => this.name = name;

        public void Set (GameObject go, string typ, string name)
        {
            SetGo( go );
            SetType( typ );
            SetName( name );
        }

        public bool Setted => go != null && !string.IsNullOrEmpty( name );

        public GameObject GameObj => go;
        public string Name => name;

        public string Type => type;
    }

    /// <summary>
    /// UI预设资源引用基类
    /// </summary>
    public class WindowBase : MonoBehaviour
    {
        //UI保存思路：
        //数组保存GameObject
        //对于每一项，前指定引用，后指定类型
        //输入框，保存类名
        //buttton，保存脚本

        #region InspectorGUI Fields
        /// <summary> 
        /// 组件对象
        /// </summary>
        [HideInInspector][SerializeField] private WindowInspectorObj[] _components; 

        /// <summary>
        /// 生成保存的类名
        /// </summary>
        [HideInInspector][SerializeField] private string _className = "WindowBaseClass";

        /// <summary>
        /// 类文件路径
        /// </summary>
        [HideInInspector] [SerializeField] private string _classFilePath = "classFilePath";

        #endregion

        /// <summary>
        /// 界面ID，ID和实例类型对应
        /// </summary>
        [SerializeField] private UIIDEnum _uiID = UIIDEnum.None;
        
        #region uiHandler
        private Void_Void_Del _onMouseEnterDel = null;
        private Void_Void_Del _onMouseExitDel = null;
        //private void OnMouseEnter () => _onMouseEnterDel?.Invoke();
        //private void OnMouseExit () => _onMouseExitDel?.Invoke();

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
