using System.Data;
using System.Printing;
using System.Windows;
using System.Windows.Media;
using HappyPrint.Enum;

namespace HappyPrint
{
    public class PrintConfig
    {
        /// <summary>
        /// 文本设置
        /// </summary>
        public Typeface Typeface
        {
            get;
            set;
        }

        /// <summary>
        /// 表头文本设置
        /// </summary>
        public Typeface HeaderTypeface
        {
            get;
            set;
        }

        /// <summary>
        /// 标题文本设置
        /// </summary>
        public Typeface TitleTypeface
        {
            get;
            set;
        }

        /// <summary>
        /// 表头字体大小
        /// </summary>
        public double HeaderFontSize
        {
            get;
            set;
        }

        /// <summary>
        /// 数据字体大小
        /// </summary>
        public double FontSize
        {
            get;
            set;
        }

        /// <summary>
        /// Title字体大小
        /// </summary>
        public double TitleFontSize
        {
            get;
            set;
        }

        /// <summary>
        /// 页面类型名称
        /// </summary>
        public PageMediaSizeName? PageSizeName
        {
            get;
            set;
        }

        /// <summary>
        /// 打印数据源
        /// </summary>
        public object DataSource
        {
            get;
            set;
        }

        public DataSourceTypeDefine DataSourceType
        {
            get;
            set;
        }

        /// <summary>
        /// 打印方向
        /// </summary>
        public PageOrientation PageOrientation
        {
            get;
            set;
        }

        public PrintConfig()
        {
            Typeface = new Typeface("Ya Hei");

            HeaderTypeface = new Typeface(new FontFamily("Ya Hei"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);

            TitleTypeface = new Typeface("Ya Hei");

            TitleFontSize = 16;

            HeaderFontSize = 14;

            FontSize = 12;

            PageSizeName = PageMediaSizeName.ISOA4;

            PageOrientation = PageOrientation.Portrait;
        }
    }
}
