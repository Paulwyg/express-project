using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HappyPrint.Enum
{
    /// <summary>
    /// 数据源类型
    /// </summary>
    public enum DataSourceTypeDefine
    {
        [Description("数据表")]
        DataTable,
        [Description("图片")]
        Image,
        [Description("控件")]
        Control
    }
}
