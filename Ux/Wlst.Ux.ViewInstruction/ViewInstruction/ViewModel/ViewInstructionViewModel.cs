using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.ViewInstruction.Services;
using Wlst.Ux.ViewInstruction.ViewInstruction.Services;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.ViewInstruction.ViewInstruction.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    [Export(typeof(IIViewInstruction))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ViewInstructionViewModel : Cr.Core.CoreServices.ObservableObject, IIViewInstruction
    {
        private readonly Dictionary<int, string> _info = new Dictionary<int, string>();
        private readonly bool _isShowThisView;
        /// <summary>
        /// 
        /// </summary>
        public ViewInstructionViewModel()
        {
             _isShowThisView = ReadViewInstructions.ReadInstructionConfig(); //读取配置文件，是否显示该界面
            if(_isShowThisView){
                InitialEvent();
                if (_info.Count == 0)
                {
                    _info = ReadViewInstructions.ReadInstructions("ViewInstructions.xml");  //加载界面说明文件
                }
            }

        }

        #region Instructions

        private string _instructions;
        /// <summary>
        /// 
        /// </summary>
        public string Instructions
        {
            get { return _instructions; }
            set
            {
                if (_instructions == value) return;
                _instructions = value;
                RaisePropertyChanged(() => Instructions);
            }

        }

        #endregion

        private string _temp;


        #region IITab
        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "界面说明"; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class ViewInstructionViewModel
    {
        private void InitialEvent()
        {
           EventPublish.AddEventTokener( Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
        }
        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.Core && args.EventId == Cr.CoreOne.CoreIdAssign.EventIdAssign.ShowViewInstructionEventId)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        public void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.Core && args.EventId ==Cr.CoreOne.CoreIdAssign.EventIdAssign.ShowViewInstructionEventId)
                {
                    if (args.GetParams()[0] != null)
                    {
                        int id = Convert.ToInt32(args.GetParams()[0]);
                        if (id > 0)
                        {
                            if (_info.ContainsKey(id))
                            {
                                Instructions = "               ";
                                _temp = _info[id];
                                var tt = _temp.Split('。');
                                for (int t = 0; t < tt.Length;t++ )
                                {
                                    if (string.IsNullOrEmpty(tt[t])) continue;
                                    if (t == 0)
                                    {
                                        Instructions += tt[t] + Environment.NewLine+Environment.NewLine;
                                        continue;
                                    }
                                    Instructions += tt[t] + "。" + Environment.NewLine;
                                }
                                Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
ViewIdAssign.ViewInstructionId, true);
                            }
                            else
                            {
                                Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
ViewIdAssign.ViewInstructionId,false);
                            }

                        }
                    }
                }

            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("ReSetAnimation error in FundEventHandlers:ex:" + xe);
            }
        }
    }
}
