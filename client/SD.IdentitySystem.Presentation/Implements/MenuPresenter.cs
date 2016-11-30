﻿using System;
using System.Collections.Generic;
using System.Linq;
using SD.IdentitySystem.IAppService.DTOs.Outputs;
using SD.IdentitySystem.IAppService.Interfaces;
using SD.IdentitySystem.IPresentation.Interfaces;
using SD.IdentitySystem.IPresentation.ViewModels.Formats.EasyUI;
using SD.IdentitySystem.IPresentation.ViewModels.Outputs;
using SD.IdentitySystem.Presentation.Maps;

namespace SD.IdentitySystem.Presentation.Implements
{
    /// <summary>
    /// 菜单呈现器实现
    /// </summary>
    public class MenuPresenter : IMenuPresenter
    {
        #region # 字段及构造器

        /// <summary>
        /// 权限服务接口
        /// </summary>
        private readonly IAuthorizationContract _authorizationContract;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        /// <param name="authorizationContract">权限服务接口</param>
        public MenuPresenter(IAuthorizationContract authorizationContract)
        {
            this._authorizationContract = authorizationContract;
        }

        #endregion

        #region # 获取菜单列表 —— IEnumerable<MenuView> GetMenus(string systemKindNo)
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="systemKindNo">信息系统类别编号</param>
        /// <returns>菜单列表</returns>
        public IEnumerable<MenuView> GetMenus(string systemKindNo)
        {
            IEnumerable<MenuInfo> menuInfos = this._authorizationContract.GetMenus(systemKindNo);

            IEnumerable<MenuView> menuViews = menuInfos.Select(x => x.ToViewModel());

            return menuViews;
        }
        #endregion

        #region # 获取菜单树 —— IEnumerable<Node> GetMenuTree(string systemKindNo)
        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <param name="systemKindNo">信息系统类别编号</param>
        /// <returns>菜单树</returns>
        public IEnumerable<Node> GetMenuTree(string systemKindNo)
        {
            IEnumerable<MenuView> menuViews = this.GetMenus(systemKindNo);

            return menuViews.ToTree(null);
        }
        #endregion

        #region # 获取菜单 —— MenuView GetMenu(Guid menuId)
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="menuId">菜单Id</param>
        /// <returns>菜单</returns>
        public MenuView GetMenu(Guid menuId)
        {
            //MenuInfo menuInfo=this._authorizationContract.GetMenus()

            //TODO 添加服务接口

            return null;
        }
        #endregion
    }
}
