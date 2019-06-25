namespace Wlst.Cr.Setting.Interfaces
{

    public interface IISetting
    {
        /// <summary>
        /// 部件Key defaulte setting
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// the path like  Setting#LightSet
        /// </summary>
        string PathSetting { get; set; }

        /// <summary>
        /// 部件值  
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// 部件ID值  全局唯一  以用标示部件
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// 此Id值为视图Id 混合使用以定位的
        /// </summary>
        int ViewId { get; set; }

        /// <summary>
        /// 菜单描述  
        /// </summary>
        string Describle { get; }

        /// <summary>
        /// 用户界面提示信息
        /// </summary>
        string Tooltips { get; set; }

        /// <summary>
        /// 标签 可有可无 
        /// </summary>
        object Tag { get; set; }


    }
}
