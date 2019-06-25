using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using SpeechLib;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.ViewModel
{
  
    //[Export(typeof(IIEquipmentAllFaultOnTabViewModel))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentAllFaultOnTabViewModel :
        EventHandlerHelperExtendNotifyProperyChanged,
        Services.IIEquipmentAllFaultOnTabViewModel
    {
        #region tab

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public string Title
        {
            get { return "最新故障"; }
        }

        #endregion

        protected const int MaxRecordHold = 999;

        private ObservableCollection<OneTmlExistFaultViewModel> _record;

        public ObservableCollection<OneTmlExistFaultViewModel> Records
        {
            get
            {
                if (_record == null)
                    _record = new ObservableCollection<OneTmlExistFaultViewModel>();
                return _record;
            }
        }

        private OneTmlExistFaultViewModel _curr;

        public OneTmlExistFaultViewModel CurrentSelectTml
        {
            get { return _curr; }
            set
            {
                if (value == _curr) return;
                _curr = value;
                this.RaisePropertyChanged(() => this.CurrentSelectTml);
                if (value != null && value.EquipmentRtuId  > 0)
                {
                    var args = new PublishEventArgs
                    {
                        EventType = PublishEventType.Core,
                        EventId =
                            Sr.EquipmentInfoHolding.Services.EventIdAssign.
                            EquipmentSelected,
                    };
                    args.AddParams(value.EquipmentRtuId );

                    EventPublisher.EventPublish(args);
                }
            }
        }


        public EquipmentAllFaultOnTabViewModel()
        {
            this.InitEvent();
            threadRunVoiceReoprt = new Thread(VoiceReportFun);
           threadRunVoiceReoprt.Start();
           
           
        }


        #region 语音报警
        

        private static bool _isVoiceReport=true;
        //private static bool _isOnReport = false;
        private static ConcurrentQueue<string> _voiceReportItems = new ConcurrentQueue<string>();

        private Thread threadRunVoiceReoprt;
        private static void VoiceReportFun()
        {
            try
            {


                SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFDefault; //.SVSFlagsAsync;

                var voice = new SpVoice();


                while (_isVoiceReport)
                {
                    try
                    {
                        if (_voiceReportItems.Count > 0)
                        {
                            string speaktext = "";
                            if (_voiceReportItems.TryDequeue(out speaktext))
                            {
                                if (string.IsNullOrEmpty(speaktext)) continue;
                                voice.Speak(speaktext, SpFlags);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("语音报警出错:" + ex);
                    }
                    Thread.Sleep(1000);
                }
                //System.Speech.Synthesis.SpeechSynthesizer
                //var  speechSynthesizer = new SpeechSynthesizer();
                // speechSynthesizer.SetOutputToDefaultAudioDevice();


                // while (_isVoiceReport)
                //{
                //    try
                //    {
                //        if (_voiceReportItems.Count > 0)
                //        {
                //            string speaktext = "";
                //            if (_voiceReportItems.TryDequeue(out speaktext))
                //            {
                //                if (string.IsNullOrEmpty(speaktext))
                //                    continue;
                //                speechSynthesizer.Speak(speaktext);

                //            }

                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("语音报警出错:" + ex);
                //    }
                //    Thread.Sleep(1000);
                //}

            }
            catch (Exception)
            {

            }
        }

        public void ClearVoiceReportItems()
        {
            string speaktext = "";
            while (_voiceReportItems.Count > 0)
                _voiceReportItems.TryDequeue(out speaktext);

        }

        #endregion
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class EquipmentAllFaultOnTabViewModel
    {
        public void OnRequestServerData(OneTmlExistFaultViewModel info)
        {
            if (info == null) return;
            Sr.EquipemntLightFault.Services.PreErrorServices.RequestDataWhenErrorHappen(info.EquipmentRtuId, info.LoopId,
                                                                                        info.DateCreateId);
        }

        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId,
                                    PublishEventType.Core);
            this.AddEventFilterInfo(Sr.EquipemntLightFault.Services.EventIdAssign.EquipementExistFaultDeleteId,
                                    PublishEventType.Core);
            this.AddEventFilterInfo(Wlst.Ux.EquipemntLightFault.Services.EventIdAssign.VoiceAlarmClosed,
                                    PublishEventType.Core); 
        }

        private DateTime _voicealarmclosedtime;

        public override void ExPublishedEvent(
            Microsoft.Practices.Prism.MefExtensions.Event.EventHelper.PublishEventArgs args)
        {
            //base.ExPublishedEvent(args);
            switch (args.EventId)
            {
                case Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId:
                    var info = args.GetParams()[0] as List<int>;
                    if (info == null) return;
                    _isVoiceReport = info.Count <= 5;
                    foreach (var t in info)
                    {
                        AddErrorInfo(t);
                       
                    }
                    if (info.Count < 3)
                    {
                        if (Wlst.Sr.EquipmentInfoHolding.Services.Others .IsShowThisViewOnNewErrArriveInfo) //选项中设定
                            //Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.ViewModel.
                            //    EquipmentFaultRecordQueryViewModel.IsShowThisViewOnNewErrArriveInfo)
                        {
                            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(
                                EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultRecordQueryViewId, -1);
                        }
                    }
                    break;
                case Sr.EquipemntLightFault.Services.EventIdAssign.EquipementExistFaultDeleteId:
                    var infos = args.GetParams()[0] as List<int>;
                    if (infos == null) return;
                    DeleteTmlFault(infos);
                    break;
                //case Wlst.Ux.EquipemntLightFault.Services.EventIdAssign.VoiceAlarmClosed:
                //    _isVoiceReport = false ;
                //    Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("null", 8888, DateTime.Now.Ticks , 180, Ac,null,1);
                //    //if (DateTime.Now.Ticks - _voicealarmclosedtime.Ticks == 180000000) _isVoiceReport = true;
                   
                    
                //    break;

            }
        }

        void Ac(object obj)
        {
            _isVoiceReport = true;
        }


        private void AddErrorInfo(int id)
        {
            var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(id);
            if (error == null) return;

            //Add Voice Report
            if (_isVoiceReport)
            {
                int alarmNum = 0;
                string temp;
                while (error.AlarmTimes > alarmNum)
                {
                    string tt = error.FaultName;
                    if (tt.Contains("防盗")) tt = "被盗";
                    if (tt.Contains("门")) tt = "打开";
                    temp = error.RtuName + error.LoopId + "        " + tt;
                    _voiceReportItems.Enqueue( temp);
                    alarmNum++;
                }
            }

            var infovm = new OneTmlExistFaultViewModel(error);

            if (error.IsShowAtTop > 0)
                Records.Insert(0, infovm);
            else Records.Add(infovm);

            if (Records.Count > MaxRecordHold)
            {
                Records.RemoveAt(MaxRecordHold - 1);
            }
        }

        private void DeleteTmlFault(List<int> info)
        {
            var lst = new List<OneTmlExistFaultViewModel>();

            foreach (var t in Records)
            {
                if (info.Contains(t.Id))
                {
                    lst.Add(t);
                }
            }

            foreach (var t in lst)
            {
                try
                {
                  //  var tmpssss = new OneTmlExistFaultViewModel(t);
                  ////  Records.Remove(t);
                  ////  t.FaultRemak = "消警";
                  ////  t.CreatTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                  ////  t.FaultName += " 消警";
                  //  Records.Insert(0,tmpssss);
                    if(Records.Contains(t))
                    Records.Remove(t);
                }
                catch (Exception ex)
                {
                }
            }

        }

    }
}
