﻿// ReSharper disable once CheckNamespace
namespace SD.UAC.Common
{
    /// <summary>
    /// Session键
    /// </summary>
    public static class SessionKey
    {
        /// <summary>
        /// 当前登录用户Session键
        /// </summary>
        public static string CurrentUser = "CurrentUser";

        /// <summary>
        /// 当前公钥Session键
        /// </summary>
        public static string CurrentPublishKey = "CurrentPublishKey";

        /// <summary>
        /// 当前用户菜单树Session键
        /// </summary>
        public static string CurrentMenuTree = "CurrentMenus";

        /// <summary>
        /// 当前用户权限集Session键
        /// </summary>
        public static string CurrentAuthorities = "CurrentAuthorities";
    }
}