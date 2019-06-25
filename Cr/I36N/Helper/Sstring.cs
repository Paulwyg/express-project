using System;
using System.ComponentModel;

namespace I36N.Helper
{
    /// <summary>
    /// 此处返回的熟悉值仅能从汉化包中提取  不提供赋值功能
    /// </summary>
    public class Sstring : INotifyPropertyChanged
    {
        private string _nameId;
        private object[] _namepars;
        private string _propertyName;
        private Action<string> _action;

        /// <summary>
        /// 属性构造函数
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="action"> PropertyChanged</param>
        public Sstring(string propertyName, Action<string> action)
        {
            Services.I36N.RegisterPropertyName(this);
            _propertyName = propertyName;
            _action = action;
        }

        /// <summary>
        /// 属性名称发生变化
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="description">描述 仅供开发者查看</param>
        /// <param name="pars">参数</param>
        public void NameChanged(string id, string description, params object[] pars)
        {
            _nameId = id;
            _namepars = pars;
            this.RaisePropertyChanged();
        }

        private string _msg;

        /// <summary>
        /// 属性名称,从语言包中获取数据 ，不允许设置
        /// </summary>
        public string Name
        {
            get
            {
                _msg =Services.I36N.ConvertByCoding(_nameId, _namepars);
                return _msg;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;


        private void RaisePropertyChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public void RaiseFatherPropertyChanged()
        {
            if (_action != null)
            {
                _action(_propertyName);
            }
        }

        #endregion
    }
}
