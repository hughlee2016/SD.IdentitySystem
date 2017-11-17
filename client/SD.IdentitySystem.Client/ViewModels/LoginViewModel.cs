﻿using Caliburn.Micro;
using SD.IdentitySystem.Client.Commons;
using SD.IdentitySystem.IAppService.Interfaces;
using SD.Infrastructure.Constants;
using SD.IOC.Core.Mediator;
using System;
using System.Diagnostics;

namespace SD.IdentitySystem.Client.ViewModels
{
    /// <summary>
    /// 登录ViewModel
    /// </summary>
    public class LoginViewModel : Screen
    {
        #region # 依赖注入构造器

        /// <summary>
        /// 身份认证服务接口
        /// </summary>
        private readonly IAuthenticationContract _authenticationContract;

        /// <summary>
        /// 窗体管理器
        /// </summary>
        private readonly IWindowManager _windowManager;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        /// <param name="authenticationContract">身份认证服务接口</param>
        /// <param name="windowManager">窗体管理器</param>
        public LoginViewModel(IAuthenticationContract authenticationContract, IWindowManager windowManager)
        {
            this._authenticationContract = authenticationContract;
            this._windowManager = windowManager;

            //默认值
            base.DisplayName = null;
        }

        #endregion

        #region # 属性

        #region 用户名 —— string LoginId
        /// <summary>
        /// 用户名
        /// </summary>
        private string _loginId;

        /// <summary>
        /// 用户名
        /// </summary>
        public string LoginId
        {
            get { return this._loginId; }
            private set { this.Set(ref this._loginId, value); }
        }
        #endregion

        #region 密码 —— string Password
        /// <summary>
        /// 密码
        /// </summary>
        private string _password;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return this._password; }
            private set { this.Set(ref this._password, value); }
        }
        #endregion

        #endregion

        #region # 方法

        #region 访问我的码云 —— void LaunchGitOsc()
        /// <summary>
        /// 访问我的码云
        /// </summary>
        public void LaunchGitOsc()
        {
            Process.Start("https://gitee.com/lishilei0523");
        }
        #endregion

        #region 登录 —— void Login()
        /// <summary>
        /// 登录
        /// </summary>
        public void Login()
        {
            LoginInfo loginInfo = this._authenticationContract.Login(this.LoginId, this.Password);

            //存入Session
            AppDomain.CurrentDomain.SetData(SessionKey.CurrentUser, loginInfo);

            //跳转到主窗体
            IElementManager elementManager = ResolveMediator.Resolve<IElementManager>();
            this._windowManager.ShowWindow(elementManager);

            //关闭当前窗口
            this.TryClose();
        }
        #endregion

        #endregion
    }
}