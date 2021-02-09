using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AquilaFramework.Common.UI
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
        StartUpUI = 10,

        /// <summary>
        /// 存储
        /// </summary>
        SaveAndLoadUI = 20,

        /// <summary>
        /// 任务板
        /// </summary>
        MissionBoardUI = 30,

        /// <summary>
        /// 队伍概览
        /// </summary>
        TeamOverViewUI = 40,

        /// <summary>
        /// 角色详情
        /// </summary>
        CharacterDetailUI = 50,

        /// <summary>
        /// 购买/出售
        /// </summary>
        BuyAndSaleUI = 60,

        None = 999,
    }
}
