using System.Windows.Input;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.EquipmentGroupManage.Services;

namespace Wlst.Ux.EquipmentGroupManage.GrpMulitManageViewModel.Models
{
    public class PathItem : ObservableObject
    {
        private int  _path;

        public int  GrpId
        {
            get { return _path; }
            set
            {
                if (value != _path)
                {
                    _path = value;
                   // GetTheGrpName(_Path);
                    this.RaisePropertyChanged(() => this.GrpId);
                }
            }
        }

        //设备逻辑地址
        public int Id;

        private string _grpName;
        public string GrpName
        {
            get { return _grpName; }
            set
            {
                if(value!=_grpName)
                {
                    _grpName = value;
                    RaisePropertyChanged(()=>this.GrpName);
                }
            }
        }

        private int _areaId;

        public int AreaId
        {
            get { return _areaId; }
            set
            {
                if (value != _areaId)
                {
                    _areaId = value;
                    this.RaisePropertyChanged(() => this.AreaId);
                }
            }
        }
 
        public ICommand DeletePath
        {
            get { return new RelayCommand(ExDelete ); }
        }

        private void ExDelete()
        {
            
            var args = new PublishEventArgs()
                           {
                               EventType = PublishEventTypeLocal.Name,
                               EventId  = PublishEventTypeLocal.DeleteGrpPathByCommandBasicShowGroupManage
                           };
            args.AddParams(Id ,GrpId,GrpName,AreaId);
            EventPublish.PublishEvent(args);
        }
    }
}
