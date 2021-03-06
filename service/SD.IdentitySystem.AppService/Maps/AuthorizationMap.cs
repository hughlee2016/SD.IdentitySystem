﻿using SD.Common.PoweredByLee;
using SD.IdentitySystem.Domain.Entities;
using SD.IdentitySystem.IAppService.DTOs.Outputs;
using SD.Infrastructure.MemberShip;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SD.IdentitySystem.AppService.Maps
{
    /// <summary>
    /// 权限相关映射工具类
    /// </summary>
    public static class AuthorizationMap
    {
        #region # 信息系统映射 —— static InfoSystemInfo ToDTO(this InfoSystem infoSystem)
        /// <summary>
        /// 信息系统映射
        /// </summary>
        /// <param name="infoSystem">信息系统领域模型</param>
        /// <returns>信息系统数据传输对象</returns>
        public static InfoSystemInfo ToDTO(this InfoSystem infoSystem)
        {
            InfoSystemInfo systemInfo = Transform<InfoSystem, InfoSystemInfo>.Map(infoSystem);

            return systemInfo;
        }
        #endregion

        #region # 信息系统登录信息映射 —— static LoginSystemInfo ToLoginSystemInfo(this InfoSystem infoSystem)
        /// <summary>
        /// 信息系统登录信息映射
        /// </summary>
        public static LoginSystemInfo ToLoginSystemInfo(this InfoSystem infoSystem)
        {
            return new LoginSystemInfo(infoSystem.Number, infoSystem.Name, infoSystem.ApplicationType, infoSystem.Index);
        }
        #endregion

        #region # 菜单映射 —— static MenuInfo ToDTO(this Menu menu...
        /// <summary>
        /// 菜单映射
        /// </summary>
        /// <param name="menu">菜单领域模型</param>
        /// <param name="systemInfos">信息系统数据传输对象字典</param>
        /// <returns>菜单数据传输对象</returns>
        public static MenuInfo ToDTO(this Menu menu, IDictionary<string, InfoSystemInfo> systemInfos)
        {
            MenuInfo menuInfo = Transform<Menu, MenuInfo>.Map(menu);

            menuInfo.InfoSystemInfo = systemInfos[menu.SystemNo];
            menuInfo.ParentMenuId = menu.ParentNode == null ? (Guid?)null : menu.ParentNode.Id;

            return menuInfo;
        }
        #endregion

        #region # 菜单登录信息映射 —— static LoginMenuInfo ToNode(this Menu menu)
        /// <summary>
        /// 菜单登录信息映射
        /// </summary>
        public static LoginMenuInfo ToNode(this Menu menu)
        {
            return new LoginMenuInfo
            {
                SystemNo = menu.SystemNo,
                ApplicationType = menu.ApplicationType,
                ParentId = menu.ParentNode?.Id,
                Id = menu.Id,
                Name = menu.Name,
                Sort = menu.Sort,
                Icon = menu.Icon,
                Path = menu.Path,
                Url = menu.Url
            };
        }
        #endregion

        #region # 菜单树映射 —— static ICollection<LoginMenuInfo> ToTree(this...
        /// <summary>
        /// 菜单树映射
        /// </summary>
        public static ICollection<LoginMenuInfo> ToTree(this IEnumerable<Menu> menus, Guid? parentId)
        {
            //验证
            menus = menus?.ToArray() ?? new Menu[0];

            //声明容器
            ICollection<LoginMenuInfo> loginMenuInfos = new HashSet<LoginMenuInfo>();

            //判断父级菜单Id是否为null
            if (!parentId.HasValue)
            {
                //从根级开始遍历
                foreach (Menu menu in menus.OrderBy(x => x.Sort).Where(x => x.IsRoot))
                {
                    LoginMenuInfo menuInfo = menu.ToNode();
                    loginMenuInfos.Add(menuInfo);
                    menuInfo.SubMenuInfos = menus.ToTree(menuInfo.Id);
                }
            }
            else
            {
                //从给定Id向下遍历
                foreach (Menu menu in menus.OrderBy(x => x.Sort).Where(x => x.ParentNode != null && x.ParentNode.Id == parentId.Value))
                {
                    LoginMenuInfo menuInfo = menu.ToNode();
                    loginMenuInfos.Add(menuInfo);
                    menuInfo.SubMenuInfos = menus.ToTree(menuInfo.Id);
                }
            }

            return loginMenuInfos;
        }
        #endregion

        #region # 权限映射 —— static AuthorityInfo ToDTO(this Authority authority...
        /// <summary>
        /// 权限映射
        /// </summary>
        /// <param name="authority">权限领域模型</param>
        /// <param name="systemInfos">信息系统数据传输对象字典</param>
        /// <returns>权限数据传输对象</returns>
        public static AuthorityInfo ToDTO(this Authority authority, IDictionary<string, InfoSystemInfo> systemInfos)
        {
            AuthorityInfo authorityInfo = Transform<Authority, AuthorityInfo>.Map(authority);

            authorityInfo.InfoSystemInfo = systemInfos[authority.SystemNo];

            return authorityInfo;
        }
        #endregion

        #region # 权限登录信息映射 —— static LoginAuthorityInfo ToLoginAuthorityInfo(this Authority authority)
        /// <summary>
        /// 权限登录信息映射
        /// </summary>
        public static LoginAuthorityInfo ToLoginAuthorityInfo(this Authority authority)
        {
            return new LoginAuthorityInfo(authority.Name, authority.EnglishName, authority.AuthorityPath);
        }
        #endregion

        #region # 角色映射 —— static RoleInfo ToDTO(this Role role...
        /// <summary>
        /// 角色映射
        /// </summary>
        /// <param name="role">角色领域模型</param>
        /// <param name="systemInfos">信息系统数据传输对象字典</param>
        /// <returns>角色数据传输对象</returns>
        public static RoleInfo ToDTO(this Role role, IDictionary<string, InfoSystemInfo> systemInfos)
        {
            RoleInfo roleInfo = Transform<Role, RoleInfo>.Map(role);

            roleInfo.InfoSystemInfo = systemInfos[role.SystemNo];

            return roleInfo;
        }
        #endregion
    }
}