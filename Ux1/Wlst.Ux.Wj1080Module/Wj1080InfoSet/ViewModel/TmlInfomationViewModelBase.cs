using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.client;


namespace Wlst.Ux.Wj1080Module.Wj1080InfoSet.ViewModel
{

  public partial class TmlInfomationViewModelBase : Wlst .Cr .Core .CoreServices .ObservableObject    
  {


      private Wlst .Sr .EquipmentInfoHolding .Model .Wj1080Lux  _wj1080TerminalInformation;

      
      /// <summary>
      /// 
      /// </summary>
      /// <param name="luxId"></param>
      public void NavOnLoadByBase( int  luxId)
      {
          if (luxId > 0)
          {
              SelectedTmlChange(luxId);
          }
      }

      #region
      private bool _ismainequipment;

      /// <summary>
      ///   
      /// </summary>
      public bool IsMainEquipment
      {
          get { return _ismainequipment; }
          set
          {
              if (value != _ismainequipment)
              {
                  _ismainequipment = value;
                  this.RaisePropertyChanged(() => this.IsMainEquipment);
              }
          }
      }


      private bool _isattachequipment;

      /// <summary>
      ///   
      /// </summary>
      public bool IsAttachEquipment
      {
          get { return _isattachequipment; }
          set
          {
              if (value != _isattachequipment)
              {
                  _isattachequipment = value;
                  this.RaisePropertyChanged(() => this.IsAttachEquipment);
              }
          }
      }
      #endregion

      #region
      

      /// <summary>
      /// 提供外界更改终端
      /// </summary>
      /// <param name="rtuId">终端地址</param>
      public void SelectedTmlChange(int rtuId)
      {
          _wj1080TerminalInformation  = null;
          if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuId))
          {
              var tm =
                  Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId] as
                  Wlst.Sr.EquipmentInfoHolding.Model.Wj1080Lux;

              _wj1080TerminalInformation = new Wj1080Lux(new EquipmentParameter()
                                                             {
                                                                 RtuArgu = tm.RtuArgu,
                                                                 RtuFid = tm.RtuFid,
                                                                 RtuGisX = tm.RtuFid,
                                                                 RtuGisY = tm.RtuGisY,
                                                                 RtuId = tm.RtuId,
                                                                 RtuInstallAddr = tm.RtuInstallAddr,
                                                                 RtuMapX = tm.RtuMapX,
                                                                 RtuMapY = tm.RtuMapY,
                                                                 RtuModel = tm.RtuModel,
                                                                 RtuName = tm.RtuName,
                                                                 RtuPhyId = tm.RtuPhyId,
                                                                 RtuRemark = tm.RtuRemark,
                                                                 RtuStateCode = tm.RtuStateCode

                                                             }, new LuxParameter()
                                                                    {
                                                                        LuxCommTypeCode = tm.WjLux.LuxCommTypeCode,
                                                                        LuxWorkMode = tm.WjLux.LuxWorkMode,
                                                                        LuxPort = tm.WjLux.LuxPort,
                                                                        LuxRange = tm.WjLux.LuxRange
                                                                    });

              if (
                  Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuFid ==
                  0)
              {
                  IsMainEquipment = true;
                  IsAttachEquipment = false;
              }
              else
              {
                  IsAttachEquipment = true;
                  IsMainEquipment = false;
              }
          }


          if (_wj1080TerminalInformation == null) return;
          //var ffff = t.Clone();
        //  _wj1080TerminalInformation = ffff as Wj1080TerminalInformation;
          //属性自动生成
         
          InitViewModel();
      }


      /// <summary>
      /// 初始化 需要显示的回路、输出、输入信息
      /// </summary>
      private void InitViewModel()
      {

          if (_wj1080TerminalInformation == null) return;
          InitVm(_wj1080TerminalInformation);
      }

      /// <summary>
      /// 将回路信息、输入信、输出信息还原为 终端信息
      /// </summary>
      /// <returns></returns>
      protected  Wlst .Sr .EquipmentInfoHolding .Model .Wj1080Lux  BackViewModelToTerminalInformation()
      {
          BackToTerminal(_wj1080TerminalInformation);
          return _wj1080TerminalInformation;
      }

      #endregion



  };

  public partial class TmlInfomationViewModelBase  
  {
      private void InitVm(Wlst.Sr.EquipmentInfoHolding.Model.Wj1080Lux lux)
      {
          // LuxWorkMode 光控工作模式 0 每隔5秒主报，1 选测应答 ，2 根据设定的时间主动山包，默认10秒，GPRS通信，3 根据设定的时间主动上报，默认10秒，485通信   7 8 9


          this.RtuId = lux.RtuId;
          this.RtuName = lux.RtuName;
          this.PhyId = lux .RtuPhyId;
          this.AttachRtuId = lux.RtuFid ;
          this.RtuCommucationType = lux.WjLux .LuxCommTypeCode;
          if(IsAttachEquipment )
          {
              this.RtuCommucationType = 3;
          }
          this.LuxLocation = lux.RtuInstallAddr;
          this.LuxRange = lux.WjLux.LuxRange;
          this.LuxPort = lux.WjLux.LuxPort;
          this.LuxWorkMode = lux.WjLux.LuxWorkMode;

          // this.LuxWorkMode = lux.LuxWorkMode;
          //this.RaisePropertyChanged(() => this.CollectionRtuCommucationType);

      }

      private void BackToTerminal(Wlst.Sr.EquipmentInfoHolding.Model.Wj1080Lux ter)
      {
          ter.RtuId = this.RtuId;
          ter.RtuName = this.RtuName;
          ter.RtuPhyId = this.PhyId;
          ter.RtuFid = this.AttachRtuId;
          ter.WjLux.LuxCommTypeCode = this.RtuCommucationType;
          ter.RtuInstallAddr = this.LuxLocation;
          ter.WjLux.LuxRange = this.LuxRange;
          ter.WjLux.LuxPort = this.LuxPort;

          if (this.LuxWorkMode == 1)
          {
              ter.WjLux.LuxWorkMode = 0;
          }
          else if (this.LuxWorkMode == 2)
          {
              ter.WjLux.LuxWorkMode = 1;
          }
          else
          {
              ter.WjLux.LuxWorkMode = 2;
          }
          //ter.LuxWorkMode = this.LuxWorkMode;

      }

      private int _rtuId;

      /// <summary>
      /// 光控逻辑地址  
      /// </summary>
      public int RtuId
      {
          get { return _rtuId; }
          set
          {
              if (value != _rtuId)
              {
                  _rtuId = value;
                  this.RaisePropertyChanged(() => this.RtuId);
              }
          }
      }

      private string _rtuName;

      /// <summary>
      /// 光控名称
      /// </summary>
      [StringLength(30,ErrorMessage="光控名称不能超过30个字节")]
      [Required(ErrorMessage ="输入不能为空")]
      public string RtuName
      {
          get { return _rtuName; }
          set
          {
              if (value != _rtuName)
              {
                  _rtuName = value;
                  this.RaisePropertyChanged(() => this.RtuName);
              }
          }
      }

      private string _luxLocation;

      /// <summary>
      /// 光控安装位置 
      /// </summary>
      public string LuxLocation
      {
          get { return _luxLocation; }
          set
          {
              if (value != _luxLocation)
              {
                  _luxLocation = value;
                  this.RaisePropertyChanged(() => this.LuxLocation);
              }
          }
      }

      private int _rtuCommucationType;

      /// <summary>
      /// 通信方式 0 保留，1 电台，2 串口232，3 串口485，4 Zigbee，5 电力载波，6 Socket
      /// </summary>
      public int RtuCommucationType
      {
          get { return _rtuCommucationType; }
          set
          {

              _rtuCommucationType = value;
              this.RaisePropertyChanged(() => this.RtuCommucationType);
          }
      }


      private int _luxPort;
      /// <summary>
      /// 光控端口号
      /// </summary>
      public int LuxPort
      {
          get { return _luxPort; }
          set
          {

              _luxPort = value;
              this.RaisePropertyChanged(() => this.LuxPort);
          }
      }


      private int _luxWorkMode;
      /// <summary>
      /// 光控工作模式 0 每隔5秒主报，1 选测应答 ，2 根据设定的时间主动  4 根据设定的时间主动上报
      /// </summary>
      public int LuxWorkMode
      {
          get { return _luxWorkMode; }
          set
          {

              _luxWorkMode = value;
              this.RaisePropertyChanged(() => this.LuxWorkMode);
          }
      }





      private int _attachRtuId;

      /// <summary>
      /// 如果连接终端 则终端地址  不允许修改
      /// </summary>
      public int AttachRtuId
      {
          get { return _attachRtuId; }
          set
          {
              if (value != _attachRtuId)
              {
                  _attachRtuId = value;
                  this.RaisePropertyChanged(() => this.AttachRtuId);
                  var inso =
                      Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(_attachRtuId);
                      //Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentInfo(
                         // _attachRtuId);
                  if (inso != null)
                  {
                      AttachRtuName = inso.RtuName;
                  }
              }
          }
      }



      private string  _attachRtuName;

      /// <summary>
      /// 如果连接终端 则终端地址  不允许修改
      /// </summary>
      
      public string  AttachRtuName
      {
          get { return _attachRtuName; }
          set
          {
              if (value != _attachRtuName)
              {
                  _attachRtuName = value;
                  this.RaisePropertyChanged(() => this.AttachRtuName);
              }
          }
      }


      private int _luxRange;

      /// <summary>
      /// 光控量程   100; 10000
      /// </summary>
      public int LuxRange
      {
          get { return _luxRange; }
          set
          {
              if (value != _luxRange)
              {
                  _luxRange = value;
                  this.RaisePropertyChanged(() => this.LuxRange);
              }
          }
      }




      private int _phyId;

      /// <summary>
      /// 光控地址，此地址为光控设备上传数据自带的光控终端地址
      /// </summary>
      [Range(1,10000.99,ErrorMessage="物理地址介于1到10000之间")]
      public int PhyId
      {
          get { return _phyId; }
          set
          {
              if (value != _phyId)
              {
                  _phyId = value;
                  this.RaisePropertyChanged(() => this.PhyId);
              }
          }
      }
  }
}
