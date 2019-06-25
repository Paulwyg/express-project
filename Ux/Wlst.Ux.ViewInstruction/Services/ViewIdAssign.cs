using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.ViewInstruction.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 70*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 70*100;

        public const int ViewInstructionId = ViewIdAssignBaseId + 1;
        public const string ViewInstructionViewAttachRegion = RegionNames.MsgRegion;
    }
}
