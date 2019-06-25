using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HappyPrint.Enum;
using HappyPrint.Paginator;

namespace HappyPrint
{
    /// <summary>
    /// 分页起工厂...
    /// </summary>
    static class PaginatorFactory
    {
        public static DocumentPaginator GetDocumentPaginator(PrintConfig config)
        {
            switch (config.DataSourceType)
            {
                case DataSourceTypeDefine.DataTable:
                    return new DataTableDocumentPaginator(config);
                case DataSourceTypeDefine.Image:
                    return new ImageDocumentPaginator(config);
                case DataSourceTypeDefine.Control:
                    return new ImageDocumentPaginator(config);
                default:
                    return null;
            }
        }
    }
}
