using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.UI
{
    /// <summary>
    /// UI预设资源引用基类
    /// </summary>
    public class WindowBase : MonoBehaviour
    {
        /// <summary>
        /// 界面ID，ID和实例类型对应
        /// </summary>
        [SerializeField] private UIIDEnum _uiID = UIIDEnum.None;
        
        /// <summary>
        /// 根节点
        /// </summary>
        [SerializeField] private GameObject _root;

        /// <summary>
        /// 获取该Window对应的UIID
        /// </summary>
        public UIIDEnum GetUIID ()
        {
            return _uiID;
        }

    }
}
