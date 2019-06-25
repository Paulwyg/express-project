namespace Wlst.Cr.CoreMims.CoreIdAssign
{
    /// <summary>
    /// 全局Id说明
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// <para> 本模块的全局事件发布Id 3100000 + 1*100;</para> 
        /// <para>每个模块均有自己的独立的全局事件发布Id地址，地址跟随模块的模块地址而分配；</para> 
        /// <para>全局地址使用范围为3 100 000~3 999 999；前十万地址保留；</para> 
        /// <para>每个模块的使用全局Id范围为 3 100 000+ ModuleId*100 ~ 3 100 000+ ModuleId*100 +99, </para> 
        /// <para>每个模块均发放99个全局事件Id值。</para> 
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 1*100;


       
        /// <summary>
        /// 程序设备模型更新，携带参数：该设备模型的关键字
        /// </summary>
        public const int EquipmentModelComponentUpdate = EventIdAssignBaseId + 4;

        /// <summary>
        /// 程序设备模型删除，携带参数：该设备模型的关键字
        /// </summary>
        public const int EquipmentModelComponentDelete = EventIdAssignBaseId + 5;

        /// <summary>
        /// 程序设备模型增加，携带参数：该设备模型的关键字
        /// </summary>
        public const int EquipmentModelComponentAdd = EventIdAssignBaseId + 6;
    }
}
