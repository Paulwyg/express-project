using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.AddMainEquipment.ViewModels;

namespace Wlst.Ux.AddMainEquipment.Services
{
    public interface IIAddMainEquipment : IITab, IINavOnLoad 
    {
        ObservableCollection<EquipmentViewItem> EquipmentModules { get; }
        EquipmentViewItem CurrentSelectEquipmentMoudle { get; set; }
        ICommand AddNew { get; }
        int PhyId { get; set; }
        void SetButton(Button btn);
    }
}
