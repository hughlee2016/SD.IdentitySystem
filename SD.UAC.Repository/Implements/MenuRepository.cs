﻿using System;
using System.Collections.Generic;
using System.Linq;
using SD.UAC.Domain.Entities;
using SD.UAC.Domain.IRepositories.Interfaces;
using ShSoft.Framework2016.Infrastructure.Repository.EntityFrameworkProvider;

namespace SD.UAC.Repository.Implements
{
    /// <summary>
    /// 菜单仓储实现
    /// </summary>
    public class MenuRepository : EFRepositoryProvider<Menu>, IMenuRepository
    {
        #region # 获取菜单集 —— IEnumerable<Menu> FindBySystemKind(string systemKindNo)
        /// <summary>
        /// 获取菜单集
        /// </summary>
        /// <param name="systemKindNo">信息系统类别编号</param>
        /// <returns>菜单集</returns>
        public IEnumerable<Menu> FindBySystemKind(string systemKindNo)
        {
            return base.Find(x => x.InfoSystemKind.Number == systemKindNo).AsEnumerable();
        }
        #endregion

        #region # 根据上级菜单Id判断菜单是否存在 —— bool Exists(Guid? parentId, string menuName)
        /// <summary>
        /// 根据上级菜单Id判断菜单是否存在
        /// </summary>
        /// <param name="parentId">上级菜单Id</param>
        /// <param name="menuName">菜单名称</param>
        /// <returns>菜单名称是否存在</returns>
        public bool Exists(Guid? parentId, string menuName)
        {
            if (parentId == null)
            {
                return base.Exists(x => x.IsRoot && x.Name == menuName);
            }
            return base.Exists(x => x.ParentNode != null && x.ParentNode.Id == parentId && x.Name == menuName);
        }
        #endregion
    }
}