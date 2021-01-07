using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.UI
{
    /// <summary>
    /// UI实例的唯一ID
    /// </summary>
    public enum UIIDEnum
    {
        Test = 0,

        /// <summary>
        ///  初始UI
        /// </summary>
        StartUpUI,

        /// <summary>
        /// 存储
        /// </summary>
        SaveAndLoadUI,

        /// <summary>
        /// 任务板
        /// </summary>
        MissionBoardUI,

        /// <summary>
        /// 队伍概览
        /// </summary>
        TeamOverViewUI,

        /// <summary>
        /// 角色详情
        /// </summary>
        CharacterDetailUI,

        /// <summary>
        /// 购买/出售
        /// </summary>
        BuyAndSaleUI,

        None = 999,
    }
}
