using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.WJ3005Module.Wj3005TmlInfoSetViewModel.ViewModel;

namespace Wlst.Ux.WJ3005Module.Wj3005TmlInfoSetViewModel.Services
{
    public interface IITmlInformationViewModel:IINavOnLoad,IITab ,IIOnHideOrClose 
    {
        ///// <summary>
        ///// 开关量输入参数
        ///// </summary>
        //ObservableCollection<RtuParaSwitchInViewModel> RtuParaSwitchInViewModels { get; }

        /// <summary>
        /// 开关量输出参数
        /// </summary>
        ObservableCollection<RtuParaSwitchOutViewModel> RtuParaSwitchOutViewModels { get; }

        /// <summary>
        /// 回路参数
        /// </summary>
        ObservableCollection<RtuLoopInfoVm> RtuParaAnalogueAmpViewModels { get; }

        //void SelectedTmlChange(int rtuId);
        object SelectedObject { get; }
    
        //ICommand SaveAllCommand { get; }

        //ICommand AddAnalogueAmpViewModelCommand { get; }

        //RtuParaAnalogueAmpViewModel CurrentSelectLoopViewModel { get; set; }

        //RtuParaSwitchOutViewModel CurrentSelectSwitchoutViewModel { get; set; }

        //RtuParaSwitchInViewModel CurrentSelectSwitchInViewModel { get; set; }

       // VectorSampleViewModel CurrentSelectVectorSampleViewModel { get; set; }

   //     AttachEquipmentModuleViewModel CurrentSelectAttachEquimentModule { get; set; }
//
      //  AttachEquipmentViewModel CurrentSelectAttachEquiment { get; set; }

       // void SetSwitchInputState(byte state, bool loops);
        void AddModule(int mouduleKey);
        void DeleteAttachInstances(int instancesId);
        void ResetCm(int instancesId);

    };
}