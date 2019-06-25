

namespace Infrastructure.IdAssign
{

    /// <summary>
    /// Assembly Name                         Module Name                   Module Value
    /// Wlst.Cr.Core                          CrCore                        1  Company=Cetc50;ModuleName=CrCore;ModuleId=1;AutoLoad=1;DependsOnModuleNames=
    /// Wlst.Cr.CoreOne                       CrCoreOne                     2  Company=Cetc50;ModuleName=CrCoreOne;ModuleId=2;AutoLoad=1;DependsOnModuleNames=CrCore
    /// Wlst.Cr.CoreMims                      CrCoreMims                    3  Company=Cetc50;ModuleName=CrCoreMims;ModuleId=3;AutoLoad=2;DependsOnModuleNames=CrCore
    /// Wlst.Cr.Setting                       CrSetting                     4  Company=Cetc50;ModuleName=CrSetting;ModuleId=4;AutoLoad=2;DependsOnModuleNames=CrCore
    /// Wlst.Cr.Wj1050Module                  CrWj1050Module                44 Company=Cetc50;ModuleName=CrWj1050Module;ModuleId=44;AutoLoad=1;DependsOnModuleNames=CrCore
    /// Wlst.Cr.Wj1080Module                  CrWj1080Module                45 Company=Cetc50;ModuleName=CrWj1080Module;ModuleId=45;AutoLoad=1;DependsOnModuleNames=CrCore
    /// Wlst.Cr.Wj1090Module                  CrWj1090Module                46 Company=Cetc50;ModuleName=CrWj1090Module;ModuleId=46;AutoLoad=1;DependsOnModuleNames=CrCore
    /// Wlst.Cr.Wj3005Module                  CrWj3005Module                43 Company=Cetc50;ModuleName=CrWj3005Module;ModuleId=43;AutoLoad=1;DependsOnModuleNames=CrCore
    /// Wlst.Cr.Wj3090Module                  CrWj3090Module                47 Company=Cetc50;ModuleName=CrWj3090Module;ModuleId=47;AutoLoad=1;DependsOnModuleNames=CrCore
    /// Wlst.Cr.Wj6005Module                  CrWj6005Module                48 Company=Cetc50;ModuleName=CrWj6005Module;ModuleId=48;AutoLoad=1;DependsOnModuleNames=CrCore
    /// Wlst.Cr.WjEquipmentBaseModels         CrWjEquipmentModels           just server
    /// Wlst.Sr.EquipemntLightFault           SrEquipemntLightFault         49 Company=Cetc50;ModuleName=SrEquipemntLightFault;ModuleId=49;AutoLoad=2;DependsOnModuleNames=CrCore
    /// Wlst.Sr.EquipmentGroupInfoHolding     SrEquipmentGroupInfoHolding   41 Company=Cetc50;ModuleName=SrEquipmentGroupInfoHolding;ModuleId=41;AutoLoad=2;DependsOnModuleNames=CrCore
    /// Wlst.Sr.EquipmentInfoHolding          SrEquipmentInfoHolding        21 Company=Cetc50;ModuleName=SrEquipmentInfoHolding;ModuleId=21;AutoLoad=2;DependsOnModuleNames=CrCore
    /// Wlst.Sr.EquipmentNewData              SrEquipmentNewData            50 Company=Cetc50;ModuleName=SrEquipmentNewData;ModuleId=50;AutoLoad=2;DependsOnModuleNames=CrCore
    /// Wlst.Sr.Menu                          SrMenu                        10 Company=Cetc50;ModuleName=SrMenu;ModuleId=10;AutoLoad=2;DependsOnModuleNames=CrCore
    /// Wlst.Sr.TimeTableSystem               SrTimeTableSystem             51 Company=Cetc50;ModuleName=SrTimeTableSystem;ModuleId=51;AutoLoad=2;DependsOnModuleNames=CrCore
    /// Wlst.Ux.CoreDataEventMonitor          UxCoreMonitor                 5  Company=Cetc50;ModuleName=UxCoreMonitor;ModuleId=5;AutoLoad=3;DependsOnModuleNames=CrCore
    /// Wlst.Ux.CoreModuelConfig              UxCoreModuleConfig            7  Company=Cetc50;ModuleName=UxCoreModuleConfig;ModuleId=7;AutoLoad=1;DependsOnModuleNames=CrCore
    /// Wlst.Ux.EquipemntLightFault           UxEquipemntLightFault         36 Company=Cetc50;ModuleName=UxEquipemntLightFault;ModuleId=36;AutoLoad=3;DependsOnModuleNames=CrCore,SrEquipemntLightFault
    /// Wlst.Ux.EquipemntTree                 UxEquipemntTree               40 Company=Cetc50;ModuleName=UxEquipemntTree;ModuleId=40;AutoLoad=2;DependsOnModuleNames=CrCore,SrEquipmentInfoHolding,SrEquipmentGroupInfoHolding
    /// Wlst.Ux.EquipmentDataQuery            UxEquipmentDataQuery          37 Company=Cetc50;ModuleName=UxEquipmentDataQuery;ModuleId=37;AutoLoad=3;DependsOnModuleNames=CrCore,SrEquipmentNewData
    /// Wlst.Ux.EquipmentGroupManage          UxEquipmentGroupManage        42 Company=Cetc50;ModuleName=UxEquipmentGroupManage;ModuleId=42;AutoLoad=3;DependsOnModuleNames=CrCore,SrEquipmentGroupInfoHolding,SrEquipmentInfoHolding
    /// Wlst.Ux.EquipmentNewData              UxEquipmentNewData            38 Company=Cetc50;ModuleName=UxEquipmentNewData;ModuleId=38;AutoLoad=3;DependsOnModuleNames=CrCore,SrEquipmentNewData
    /// Wlst.Ux.Menu                          UxMenuModule                  34 Company=Cetc50;ModuleName=UxMenuModule;ModuleId=34;AutoLoad=2;DependsOnModuleNames=CrCore,SrMenu
    /// Wlst.Ux.MenuManage                    UxMenuManage                  11 Company=Cetc50;ModuleName=UxMenuManage;ModuleId=11;AutoLoad=1;DependsOnModuleNames=CrCore,SrMenu
    /// Wlst.Ux.RadMapJpeg                    UxRadMapJpeg                  35 Company=Cetc50;ModuleName=UxRadMapJpeg;ModuleId=35;AutoLoad=3;DependsOnModuleNames=CrCore
    /// Wlst.Ux.StateBarModule                UxStateBarModule              33 Company=Cetc50;ModuleName=UxStateBarModule;ModuleId=33;AutoLoad=2;DependsOnModuleNames=CrCore
    /// Wlst.Ux.TimeTableSystem               UxTimeTableSystemModule       32 Company=Cetc50;ModuleName=UxTimeTableSystemModule;ModuleId=32;AutoLoad=3;DependsOnModuleNames=CrCore,SrTimeTableSystem
    /// Wlst.Ux.Wj1050Module                  UxWj1050Module                25 Company=Cetc50;ModuleName=UxWj1050Module;ModuleId=25;AutoLoad=2;DependsOnModuleNames=CrCore,CrWj1050Module
    /// Wlst.Ux.Wj1080Module                  UxWj1080Module                26 Company=Cetc50;ModuleName=UxWj1080Module;ModuleId=26;AutoLoad=2;DependsOnModuleNames=CrCore,CrWj1080Module
    /// Wlst.Ux.Wj1090Module                  UxWj1090Module                27 Company=Cetc50;ModuleName=UxWj1090Module;ModuleId=27;AutoLoad=2;DependsOnModuleNames=CrCore,CrWj1090Module
    /// Wlst.Ux.WJ3005Module                  UxWj3005Module                28 Company=Cetc50;ModuleName=UxWj3005Module;ModuleId=28;AutoLoad=2;DependsOnModuleNames=CrCore,CrWj3005Module
    /// Wlst.Ux.Wj3090Module                  UxWj3090Module                29 Company=Cetc50;ModuleName=UxWj3090Module;ModuleId=29;AutoLoad=2;DependsOnModuleNames=CrCore,CrWj3090Module
    /// Wlst.Ux.Wj6005Module                  UxWj6005Module                30 Company=Cetc50;ModuleName=UxWj6005Module;ModuleId=30;AutoLoad=2;DependsOnModuleNames=CrCore,CrWj6005Module
    /// Login                                 LoginModule                   22 Company=Cetc50;ModuleName=LoginModule;ModuleId=22;AutoLoad=1;DependsOnModuleNames=CrCore
    /// Wlst.Ux.AddMainEquipment              UxAddMainEquipment            52 Company=Cetc50;ModuleName=UxAddMainEquipment;ModuleId=52;AutoLoad=2;DependsOnModuleNames=CrCore,SrEquipmentInfoHolding
    /// Wlst.Ux.Setting                       UxSetting                     53 Company=Cetc50;ModuleName=UxSetting;ModuleId=53;AutoLoad=2;DependsOnModuleNames=CrCore,CrSetting
    /// Wlst.Ux.PartolWjEquipment             UxPartolWjEquipment           54 Company=Cetc50;ModuleName=UxPartolWjEquipment;ModuleId=54;AutoLoad=3;DependsOnModuleNames=CrCore,SrEquipmentInfoHolding,SrEquipmentNewData
    ///
    /// Wlst.Ux.WjGetInfomation               UxWjGetInfomation             55 Company=Cetc50;ModuleName=UxWjGetInfomation;ModuleId=55;AutoLoad=3;DependsOnModuleNames=CrCore,SrEquipmentInfoHolding
    /// Wlst.Sr.PrivilegesCrl                 SrPrivilegesCrl               56 Company=Cetc50;ModuleName=SrPrivilegesCrl;ModuleId=56;AutoLoad=2;DependsOnModuleNames=CrCore
    /// Wlst.Ux.PrivilegesManage              UxPrivilegesManage            57 Company=Cetc50;ModuleName=UxPrivilegesManage;ModuleId=57;AutoLoad=2;DependsOnModuleNames=CrCore,SrPrivilegesCrl
    /// Wlst.Ux.MenuShortCut                  UxMenuShortCut                58 Company=Cetc50;ModuleName=UxMenuShortCut;ModuleId=58;AutoLoad=2;DependsOnModuleNames=CrCore,SrMenu
    /// Wlst.Ux.TopDataModule                 UxTopDataModule               59 Company=Cetc50;ModuleName=UxTopDataModule;ModuleId=59;AutoLoad=2;DependsOnModuleNames=CrCore
    /// Infrastructure                        InfrastructureModule          23 Company=Cetc50;ModuleName=InfrastructureModule;ModuleId=23;AutoLoad=2;DependsOnModuleNames=CrCore
    /// Wlst.Ux.Wj2090Module                  Wj2090Module                  60 Company=Cetc50;ModuleName=Wj2090Module;ModuleId=60;AutoLoad=2;DependsOnModuleNames=CrCore
    /// Wlst.Ux.Wj2090Module                  Wj2090Module                 560 Company=Cetc50;ModuleName=Wj2090Module;ModuleId=60;AutoLoad=2;DependsOnModuleNames=CrCore  菜单地址不够使用 添加500段 分配使用
    /// Wlst.Ux.ExtendYixinEsu                ExtendYixinEsu               401 Company=Cetc50;ModuleName=UxExtendYixinEsu;ModuleId=401;AutoLoad=2;DependsOnModuleNames=CrCore  宜兴节能扩展模块
    internal class Xgtx
    {

    }
}