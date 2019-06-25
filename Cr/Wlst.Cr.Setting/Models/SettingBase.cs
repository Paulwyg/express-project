namespace Wlst.Cr.Setting.Models
{

    /// <summary>
    /// 如果为setting 需要实现 Id Value以及path 参数赋值
    /// </summary>
    public class SettingBase : Interfaces .IISetting
    {
        /// <summary>
        /// 如果为GlobalSetting 需要实现 Id 以及path 参数赋值
        /// defaule Key =GlobalSetting
        /// </summary>
        public SettingBase()
        {
            Key = "GlobalSetting";
            _pathSetting = string.Empty;
            Value = null;
            Id = 0;
            Describle = "Null";
        }

        private string _key;

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string _pathSetting;

        /// <summary>
        /// Setting#SetName
        /// </summary>
        public string PathSetting
        {
            get { return _pathSetting; }
            set { _pathSetting = value; }
        }

        private object _value;

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private int _id;

        /// <summary>
        /// Setting Id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _viewid;

        /// <summary>
        /// View  Id
        /// </summary>
        public int ViewId
        {
            get { return _viewid; }
            set { _viewid = value; }
        }

        private string _describle;

        public string Describle
        {
            get { return _describle; }
            set { _describle = value; }
        }

        private string _tooltips;

        public string Tooltips
        {
            get { return _tooltips; }
            set { _tooltips = value; }
        }

        private object _tag;

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
    }
}
