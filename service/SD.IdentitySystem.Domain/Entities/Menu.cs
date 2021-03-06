﻿using SD.Infrastructure.Constants;
using SD.Infrastructure.EntityBase;
using SD.Toolkits.Recursion.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SD.IdentitySystem.Domain.Entities
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class Menu : AggregateRootEntity, ISortable, ITree<Menu>
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected Menu()
        {
            //初始化导航属性
            this.SubNodes = new HashSet<Menu>();
            this.Authorities = new HashSet<Authority>();
        }
        #endregion

        #region 02.创建菜单构造器
        /// <summary>
        /// 创建菜单构造器
        /// </summary>
        /// <param name="systemNo">信息系统编号</param>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="menuName">菜单名称</param>
        /// <param name="sort">菜单排序</param>
        /// <param name="url">链接地址</param>
        /// <param name="path">路径</param>
        /// <param name="icon">图标</param>
        /// <param name="parentNode">上级菜单</param>
        public Menu(string systemNo, ApplicationType applicationType, string menuName, int sort, string url, string path, string icon, Menu parentNode)
            : this()
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(menuName))
            {
                throw new ArgumentNullException("menuName", "菜单名称不可为空！");
            }
            if (parentNode != null && parentNode.SystemNo != systemNo)
            {
                throw new InvalidOperationException("子级菜单的信息系统必须与父级菜单一致！");
            }
            if (parentNode != null && parentNode.Authorities.Any())
            {
                throw new InvalidOperationException("已关联权限的菜单不可增加子菜单！");
            }

            #endregion

            base.Name = menuName;
            this.Sort = sort;
            this.SystemNo = systemNo;
            this.ApplicationType =
                parentNode?.ApplicationType == null
                    ? applicationType
                    : parentNode.ApplicationType == ApplicationType.Complex
                        ? applicationType
                        : parentNode.ApplicationType;
            this.Url = url;
            this.Path = path;
            this.Icon = icon;
            this.ParentNode = parentNode;
            this.IsRoot = parentNode == null;

            //初始化关键字
            this.InitKeywords();
        }
        #endregion

        #endregion

        #region # 属性

        #region 信息系统编号 —— string SystemNo
        /// <summary>
        /// 信息系统编号
        /// </summary>
        public string SystemNo { get; private set; }
        #endregion

        #region 应用程序类型 —— ApplicationType ApplicationType
        /// <summary>
        /// 应用程序类型
        /// </summary>
        public ApplicationType ApplicationType { get; private set; }
        #endregion

        #region 链接地址(Web适用) —— string Url
        /// <summary>
        /// 链接地址(Web适用)
        /// </summary>
        public string Url { get; private set; }
        #endregion

        #region 路径(Windows适用) —— string Path
        /// <summary>
        /// 路径(Windows适用)
        /// </summary>
        public string Path { get; private set; }
        #endregion

        #region 图标 —— string Icon
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; private set; }
        #endregion

        #region 排序 —— int Sort
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; private set; }
        #endregion

        #region 是否是根级节点 —— bool IsRoot
        /// <summary>
        /// 是否是根级节点
        /// </summary>
        public bool IsRoot { get; private set; }
        #endregion

        #region 只读属性 - 是否是叶子级节点 —— bool IsLeaf
        /// <summary>
        /// 只读属性 - 是否是叶子级节点
        /// </summary>
        public bool IsLeaf
        {
            get { return !this.SubNodes.Any(); }
        }
        #endregion

        #region 导航属性 - 父级菜单 —— Menu ParentNode
        /// <summary>
        /// 导航属性 - 父级菜单
        /// </summary>
        public virtual Menu ParentNode { get; private set; }
        #endregion

        #region 导航属性 - 子级菜单集 ——ICollection<Menu> SubNodes
        /// <summary>
        /// 导航属性 - 子级菜单集
        /// </summary>
        public virtual ICollection<Menu> SubNodes { get; private set; }
        #endregion

        #region 导航属性 - 权限集 —— ICollection<Authority> Authorities
        /// <summary>
        /// 导航属性 - 权限集
        /// </summary>
        public virtual ICollection<Authority> Authorities { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 修改菜单信息 —— void UpdateInfo(string menuName, int sort, string url...
        /// <summary>
        /// 修改菜单信息
        /// </summary>
        /// <param name="menuName">菜单名称</param>
        /// <param name="sort">菜单排序</param>
        /// <param name="url">链接地址</param>
        /// <param name="path">路径</param>
        /// <param name="icon">图标</param>
        public void UpdateInfo(string menuName, int sort, string url, string path, string icon)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(menuName))
            {
                throw new ArgumentNullException("menuName", "菜单名称不可为空！");
            }

            #endregion

            base.Name = menuName;
            this.Sort = sort;
            this.Url = url;
            this.Path = path;
            this.Icon = icon;

            //初始化关键字
            this.InitKeywords();
        }
        #endregion

        #region 关联权限 —— void RelateAuthorities(IEnumerable<Authority> authorities)
        /// <summary>
        /// 关联权限
        /// </summary>
        /// <param name="authorities">权限集</param>
        public void RelateAuthorities(IEnumerable<Authority> authorities)
        {
            #region # 验证

            if (!this.IsLeaf)
            {
                throw new InvalidOperationException("非叶子级菜单不可关联权限！");
            }

            #endregion

            //先清空
            this.ClearRelations();

            foreach (Authority authority in authorities)
            {
                this.Authorities.Add(authority);
                authority.MenuLeaves.Add(this);
            }
        }
        #endregion

        #region 清空关联 —— void ClearRelations()
        /// <summary>
        /// 清空关联
        /// </summary>
        public void ClearRelations()
        {
            foreach (Authority authority in this.Authorities.ToArray())
            {
                this.Authorities.Remove(authority);
                authority.MenuLeaves.Remove(this);
            }
        }
        #endregion

        #region 初始化关键字 —— void InitKeywords()
        /// <summary>
        /// 初始化关键字
        /// </summary>
        private void InitKeywords()
        {
            StringBuilder keywordsBuilder = new StringBuilder();
            keywordsBuilder.Append(this.GetTreePath());

            base.SetKeywords(keywordsBuilder.ToString());
        }
        #endregion

        #region 重写ToString()方法
        /// <summary>
        /// 重写ToString()方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
        #endregion

        #endregion
    }
}