using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Sr.PrivilegesCrl.PrivilegeInfo;
using Wlst.client;

namespace Wlst.Sr.PrivilegesCrl.Services
{
    public class PrivilegeInfoServer
    {
        private static PrivilegeInfo.PrivilegeInfomation _info = new PrivilegeInfomation();

        public PrivilegeInfoServer()
        {
            _info.InitStart();
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="deleteUserName">需要删除的用户名</param>
        public static void DeleteUserInfomaton(string deleteUserName)
        {
            _info.DeleteUserInfomaiton(deleteUserName);
        }


        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="info"></param>
        public static long ModflyUserInfo(UserInfomation info)
        {
           return _info.ModflyUserInfo(info);
        }

        /// <summary>
        /// AddUserInfo
        /// </summary>
        /// <param name="info"></param>
        public static long AddUserInfo(UserInfomation info)
        {
           return _info.AddUserInfo(info);
        }

        /// <summary>
        /// 使用本账户请求所有的用户信息
        /// </summary>
        public static void RequestAllUserInfomation()
        {
            _info.RequestAllUserInfomation();
        }


    }
}
