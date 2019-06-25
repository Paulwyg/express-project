using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.client;
using Wlst.Ux.Wj2090Module.Services;

namespace Wlst.Ux.Wj2096Module.Zorder
{

    public class CtrlRightOperatorBase : MenuItemBase
    {
        /// <summary>
        /// 最底层开灯服务，无继承不可用 需实现LoopId赋值
        /// </summary>
        public CtrlRightOperatorBase()
        {
            Id = -1;
            Description = "最底层Wj2096控制器右键菜单。";
            Tag = "最底层Wj2096控制器右键菜单";
            this.Text = "Null";
            this.Tooltips = "最底层Wj2096控制器右键菜单，无继承不可用";
            //SluId = -1;
            //CtrlId = -1;
            //Xoperator = -1;
            this.Classic = "右键菜单-Wj2096控制器";
            base.Command = new RelayCommand(Ex, CanEx, true);
            //IsPrivilegLeave = true;


           
        }

     
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;

            var equipment = this.Argu as Tuple<int, int>;
            if (equipment == null ) return false;
            var areaId = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlAreaId(equipment.Item2);
            //var equipmentPara = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(equipment.Item1);
            //var areaId = equipmentPara.AreaId;
            return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
        }
        private bool CanEx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;

            //var equipment = this.Argu as Tuple<int, int>;
            //if (equipment == null) return false;
            
            return true;
        }

      
        public OpType op;

        private void Ex()
        {
            var terminalInfo = this.Argu as Tuple<int, int>;
            if (terminalInfo == null)
            {
                LogInfo.Log("无法执行关灯命令，参数错误....");
                return;
            }

            //if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(terminalInfo.Item1) == false) return;
         
            NextEx(terminalInfo.Item1, terminalInfo.Item2);

        }

        public virtual void NextEx(int sluId, int ctrlId)
        {
           
        }

        /// <summary>
        /// 混合控制或PWM操作
        /// </summary>
        /// <param name="sluId"></param>
        /// <param name="ctrlId"></param>
        /// <param name="isMix">是否为混合控制</param>
        /// <param name="mixOpe">如果为混合控制则控制的操作 1-开灯，2-1档节能，3-2档节能，4-关灯 </param>
        /// <param name="scalOpe">如果为pwm操作则 0-10 -> 0%-100%</param>
        protected static void SndOrderMix(int sluId, int ctrlId, bool isMix, int mixOpe, int scalOpe)
        {
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slu_sgl_right_operator;
            var data = new client.SluRightOperators.SluRightOperator();
            data .SluId = sluId;
            data.AddrType = 4;
            data.Addr = ctrlId;
            data.Addrs = new List<int>();
            data.Addrs.Add(ctrlId);
            data.CmdType = isMix ? 4 : 5;
            data.CmdMix = new List<int>() { mixOpe, mixOpe, mixOpe, mixOpe };
            data.CmdPwmField = new client.SluRightOperators.SluRightOperator.CmdPwm()
                                        {
                                            LoopCanDo = new List<int>() {1, 2, 3, 4},
                                            Scale = scalOpe
                                        };
            info.WstSluRightOperator .OperatorItems.Add(data);
            SndOrderServer.OrderSnd(info,0,0,true);
        }

    }


    #region 及时选测
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoZcArgss3 : CtrlRightOperatorBase
    {
        public CtrlRightOperatoZcArgss3()
        {

            Id = Wlst.Ux.Wj2096Module.Services.MenuIdAssign.MeasureControllerCtrlForMenuId;
            Text = "即时选测";
            Tag = "即时选测";
            this.Classic = "单灯控制器-右键菜单-20960";
            Description = "召测时间设置方案，ID 为" + Id +
                          "，类型为Wj2096控制器。即时选测。";
            Tooltips = "即时选测";
        }


        public override void NextEx(int sluId, int ctrlId)
        {
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slu_sgl_ctrl_measure;
            info.Args.Addr.Add(ctrlId);
            //cid 0:普通选测  1：召测基本参数  2：召测基本参数1
            info.Args.Cid = 0;  
            SndOrderServer.OrderSnd(info,0,0,true );
        }
    }

    #endregion



    #region 召测基本参数1
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoZcRunArgss : CtrlRightOperatorBase
    {
        public CtrlRightOperatoZcRunArgss()
        {

            Id = Wlst.Ux.Wj2096Module.Services.MenuIdAssign.MeasureControllerCtrlRunArgsForMenuId;
            Text = "召测基本参数";
            Tag = "召测基本参数";
            this.Classic = "单灯控制器-右键菜单-20960";
            Description = "召测基本参数，ID 为" + Id +
                          "，类型为Wj2096控制器。召测基本参数。";
            Tooltips = "召测基本参数";
        }


        public override void NextEx(int sluId, int ctrlId)
        {
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slu_sgl_ctrl_measure;
            info.Args.Addr.Add(ctrlId);
            //cid 0:普通选测  1：召测基本参数  2：召测基本参数1
            info.Args.Cid =1;
            SndOrderServer.OrderSnd(info, 0, 0, true);

            //lvf 记录 召测终端
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.ContainsKey(sluId) == false)
            {
                Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.Add(sluId, DateTime.Now);
            }
            else
            {
                Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus[sluId] = DateTime.Now;
            }


        }
    }

    #endregion


    #region 混合控制  5-8

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoMix1 : CtrlRightOperatorBase
    {
        public CtrlRightOperatoMix1()
        {

            Id = Wlst.Ux.Wj2096Module.Services.MenuIdAssign.OpenCloseChangeLightCtrlMenuId  + 1;
            Text = "开灯";
            Tag = "开灯";
            this.Classic = "单灯控制器-右键菜单-20960";
            Description = "开灯，ID 为" + Id +
                          "，类型为Wj2096控制器。开灯。";
            Tooltips = "开灯";
        }


        public override void NextEx(int sluId, int ctrlId)
        {
            
            SndOrderMix(sluId, ctrlId, true, 1, 0);
        }
    }

    #region 测试

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoTest : CtrlRightOperatorBase //测试代码
    {
        public CtrlRightOperatoTest() //测试代码
        {
            Id = Wlst.Ux.Wj2096Module.Services.MenuIdAssign.OpenCloseChangeLightCtrlMenuId + 19;
            Text = "测试5分钟";
            Tag = "测试5分钟";
            this.Classic = "测试5分钟";
            Description = "测试" + Id +
                          "测试。";
            Tooltips = "测试5分钟";
        }


        public override void NextEx(int sluId, int ctrlId)
        {

            if (flg1)
            {
                ThreadTestSnd = new Thread(TestSndThread);//测试代码
                ThreadTestSnd.Start();//测试代码
                flg1 = false;
            }
            else
            {
                ThreadTestSnd.Abort(); 
                flg1 = true;
            }
            
        }

        private bool flg1 = true;//测试代码

        private Thread ThreadTestSnd;
        void TestSndThread(object obj)  //测试代码
        {
            try
            {
                while (true)
                {
                    SndOrderMix(0, 8000007, true, 1, 0);
                    SndXC();

                    Thread.Sleep(300000);


                    SndOrderMix(0, 8000007, false, 0, 3);
                    Thread.Sleep(300000);


                    SndOrderMix(0, 8000007, true, 4, 0);
                    SndXC();

                    Thread.Sleep(300000);
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }

        public void SndXC() //测试代码
        {
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slu_sgl_ctrl_measure;
            info.Args.Addr.Add(8000007);
            info.WstSluMeasure.Type = 5;
            SndOrderServer.OrderSnd(info, 0, 0, true);
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoTest2 : CtrlRightOperatorBase //测试代码
    {
        public CtrlRightOperatoTest2() //测试代码
        {
            Id = Wlst.Ux.Wj2096Module.Services.MenuIdAssign.OpenCloseChangeLightCtrlMenuId + 18;
            Text = "测试1分钟";
            Tag = "测试1分钟";
            this.Classic = "测试1分钟";
            Description = "测试" + Id +
                          "测试。";
            Tooltips = "测试1分钟";
        }


        public override void NextEx(int sluId, int ctrlId)
        {

            if (flg1)
            {
                ThreadTestSnd2 = new Thread(TestSndThread);//测试代码
                ThreadTestSnd2.Start();//测试代码
                flg1 = false;
            }
            else
            {
                ThreadTestSnd2.Abort();
                flg1 = true;
            }

        }

        private bool flg1 = true;//测试代码

        private Thread ThreadTestSnd2;
        void TestSndThread(object obj)  //测试代码
        {
            try
            {
                while (true)
                {
                    SndOrderMix(0, 8000007, true, 1, 0);
                    Thread.Sleep(50000);

                    SndXC();
                    Thread.Sleep(50000);

                    SndOrderMix(0, 8000007, false, 0, 3);
                    Thread.Sleep(50000);

                    SndOrderMix(0, 8000007, true, 4, 0);

                    Thread.Sleep(50000);

                    SndXC();
                    Thread.Sleep(50000);
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }

        public void SndXC() //测试代码
        {
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slu_sgl_ctrl_measure;
            info.Args.Addr.Add(8000007);
            info.WstSluMeasure.Type = 5;
            SndOrderServer.OrderSnd(info, 0, 0, true);
        }
    }
    #endregion、

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoMix4 : CtrlRightOperatorBase
    {
        public CtrlRightOperatoMix4()
        {

            Id = Wlst.Ux.Wj2096Module.Services.MenuIdAssign.OpenCloseChangeLightCtrlMenuId + 4;
            Text = "关灯";
            Tag = "关灯";
            this.Classic = "单灯控制器-右键菜单-20960";
            Description = "关灯，ID 为" + Id + 
                          "，类型为Wj2096控制器。关灯。";
            Tooltips = "关灯";
        }


        public override void NextEx(int sluId, int ctrlId)
        {
            SndOrderMix(sluId, ctrlId, true, 4, 0);
        }
    }

#endregion
    
    #region 调光 9-19 使用

    public class CtrlRightOperatoPwm : CtrlRightOperatorBase
    {
        private int indexf;

        public CtrlRightOperatoPwm(int index)
        {
            indexf = index;
            Id = Wlst.Ux.Wj2096Module.Services.MenuIdAssign.OpenCloseChangeLightCtrlMenuId + 5 +index;
            Text = index == 0 ? "0%调光" : index + "0%调光";
            Tag = index == 0 ? "0%调光" : index + "0%调光";
            this.Classic = "单灯控制器-右键菜单-20960";
            Description = index == 0
                              ? "0%调光"
                              : index + "0%调光，ID 为" + Id +
                                "，类型为Wj2096控制器调光。";
            Tooltips = index == 0 ? "0%调光" : index + "0%调光";
        }


        public override void NextEx(int sluId, int ctrlId)
        {
            SndOrderMix(sluId, ctrlId, false, 0, indexf*10);   
        }
    }

    #region

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm0 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm0()
            : base(0)
        {
        }
    }


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm1 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm1()
            : base(1)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm2 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm2()
            : base(2)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm3 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm3()
            : base(3)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm4 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm4()
            : base(4)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm5 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm5()
            : base(5)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm6 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm6()
            : base(6)
        {
        }
    }
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm7 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm7()
            : base(7)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm8 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm8()
            : base(8)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm9 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm9()
            : base(9)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm10 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm10()
            : base(10)
        {
        }
    }
    #endregion


    #endregion

    #region 历史数据查询
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightHisDataQuery : CtrlRightOperatorBase
    {
        public CtrlRightHisDataQuery()
        {

            Id = Wlst.Ux.Wj2096Module.Services.MenuIdAssign.NavToCtrlDataQueryId;
            Text = "历史数据查询";
            Tag = "历史数据查询";
            this.Classic = "单灯控制器-右键菜单-20960";
            Description = "历史数据查询，ID 为" + Id +
                          "，类型为Wj2096控制器。历史数据查询。";
            Tooltips = "历史数据查询";
        }


        public override void NextEx(int sluId, int ctrlId)
        {
           sluId = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(ctrlId);

            ExNavWithArgs(
                ViewIdAssign.ControlDataQueryViewId,
                sluId, ctrlId);
        }
    }

    #endregion




}
