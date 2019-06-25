using System;
using System.Windows;
using Wlst.Cr.CoreMims.TopDataInfo;
using Wlst.client;


namespace Wlst.Cr.CoreMims.Services
{
    [Serializable]
    public class UserInfo
    {
        public static string SelfLoginIndefyLoginNamex = "";
        public static string SelfLoginIndefyLoginPsw = "";
        //private static UserInfomation _userLoginInfo;

        private static Wlst.client.LoginReplyMsg _userLoginInfo;
        /// <summary>
        /// 登陆成功后保存的信息
        /// </summary>
        public static Wlst.client.LoginReplyMsg UserLoginInfo
        {
            get
            {
                if (_userLoginInfo 
                    == null) _userLoginInfo = new LoginReplyMsg();
                return _userLoginInfo;
            }
            set
            {
                _userLoginInfo = value;
                if (value != null)
                UpdatUserInfo();
            }
        }

  
        public static bool CanX(int areaId)
        {
            if (UserLoginInfo == null) return false;
            if (UserLoginInfo.D) return true;
            return  UserLoginInfo.AreaX  .Contains(areaId );
 
   
        }
        public static bool CanW(int areaId)
        {
            if (UserLoginInfo == null) return false;
            if (UserLoginInfo.D) return true;
            return UserLoginInfo.AreaW .Contains(areaId);
 
        }
        public static bool CanR(int areaId)
        {
            if (UserLoginInfo == null) return false;
            if (UserLoginInfo.D) return true;
            return UserLoginInfo.AreaR.Contains(areaId);
 
        }
        /// <summary>
        /// 超级管理员
        /// </summary>
        /// <returns></returns>
        public static bool Cand()
        {
            if (UserLoginInfo == null) return false;
            return UserLoginInfo.D;
        }


        /// <summary>
        /// 
        /// </summary>
        public static   DateTime DtLoginTime=DateTime .Now ;


        private static void UpdatUserInfo()
        {
            if (_userLoginInfo != null)
            {
                double x1 = SystemParameters.PrimaryScreenWidth; //得到屏幕整体宽度
                double y1 = SystemParameters.PrimaryScreenHeight; //得到屏幕整体高度

                var strattach = "";
                //if (x1 < 1800 || y1 < 1000) strattach = "建议屏幕分辨率为:1920*1080  ";


                var tooptips = Environment.NewLine +
                               "登陆时间:" + (DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine +
                               "用户名为:" + _userLoginInfo.UserName + Environment.NewLine +
                               "真实姓名:" + _userLoginInfo.UserRealName + Environment.NewLine;
                TopDataInfoServers.MySelf.UpdateDataInfo(strattach + "当前用户: " + _userLoginInfo.UserName,
                                                         tooptips, 0);
            }
        }
    };


}
