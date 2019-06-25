using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.ViewModel;


namespace Wlst.Cr.CoreMims.NodeServices
{

    /// <summary>
    /// 单属性的get set
    /// </summary>
    public partial class ObservableObjectInfo : ObservableObject
    {


        #region  单属性的get set

        /// <summary>
        /// propoeryname  value
        /// </summary>
        private Dictionary<string, object> dic = new Dictionary<string, object>();

        /// <summary>
        /// 默认值为 Empty
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="propertyExpresssion"></param>
        /// <returns></returns>
        public string GetZstring<X>(Expression<Func<X>> propertyExpresssion)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            var gt = GetZ(propertyName);
            if (gt == null) return string.Empty;
            return gt.ToString();
        }

        /// <summary>
        /// 默认值为0
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="propertyExpresssion"></param>
        /// <returns></returns>
        public int GetZint<X>(Expression<Func<X>> propertyExpresssion)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            var gt = GetZ(propertyName);
            if (gt == null) return 0;
            int rtn = 0;
            Int32.TryParse(gt.ToString(), out rtn);
            return rtn;
        }

        /// <summary>
        /// 默认值为 当前时间
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="propertyExpresssion"></param>
        /// <returns></returns>
        public DateTime GetZdatetime<X>(Expression<Func<X>> propertyExpresssion)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            var gt = GetZ(propertyName);
            if (gt == null) return DateTime.Now;
            DateTime rtn = DateTime.Now;
            DateTime.TryParse(gt.ToString(), out rtn);
            return rtn;
        }

        /// <summary>
        /// 默认值为0
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="propertyExpresssion"></param>
        /// <returns></returns>
        public double GetZdouble<X>(Expression<Func<X>> propertyExpresssion)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            var gt = GetZ(propertyName);
            if (gt == null) return 0;
            double rtn = 0;
            double.TryParse(gt.ToString(), out rtn);
            return rtn;
        }

        /// <summary>
        /// 默认值为 null
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="propertyExpresssion"></param>
        /// <returns></returns>
        public object GetZobject<X>(Expression<Func<X>> propertyExpresssion)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            var gt = GetZ(propertyName);
            return gt;
        }

        /// <summary>
        /// 默认值为0
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="propertyExpresssion"></param>
        /// <returns></returns>
        public long GetZlong<X>(Expression<Func<X>> propertyExpresssion)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            var gt = GetZ(propertyName);
            if (gt == null) return 0L;
            long rtn = 0;
            long.TryParse(gt.ToString(), out rtn);
            return rtn;
        }

        /// <summary>
        /// 默认为 false
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="propertyExpresssion"></param>
        /// <returns></returns>
        public bool GetZbool<X>(Expression<Func<X>> propertyExpresssion)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            var gt = GetZ(propertyName);
            if (gt == null) return false;
            bool rtn = false;
            bool.TryParse(gt.ToString(), out rtn);
            return rtn;
        }

        /// <summary>
        /// 默认为 visible
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="propertyExpresssion"></param>
        /// <returns></returns>
        public Visibility GetZVisibility<X>(Expression<Func<X>> propertyExpresssion)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            var gt = GetZ(propertyName);
            if (gt == null) return Visibility.Collapsed;
            Visibility rtn = Visibility.Visible;
            Enum.TryParse(gt.ToString(), out rtn);
            return rtn;
        }

        /// <summary>
        /// 默认值为 ObservableCollection<X>   
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="propertyExpresssion"></param>
        /// <returns></returns>
        public ObservableCollection<X> GetZcollection<X>(Expression<Func<ObservableCollection<X>>> propertyExpresssion)
            where X : ObservableObject
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            var gt = GetZ(propertyName);
            if (gt == null)
            {
                SetZ(propertyName, new ObservableCollection<X>());
            }

            gt = GetZ(propertyName);
            if (gt != null)
            {
                try
                {
                    return gt as ObservableCollection<X>;

                }
                catch (Exception ex)
                {

                }
            }

            return null;
        }


        /// <summary>
        /// 默认值为 null 继承ObservableObject 可以使用 
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="propertyExpresssion"></param>
        /// <returns></returns>
        public W GetZclass<W>(Expression<Func<W>> propertyExpresssion) where W : ObservableObject
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            var gt = GetZ(propertyName);
            if (gt == null) return null;
            try
            {
                return gt as W;

            }
            catch (Exception ex)
            {

            }

            return null;
        }


        ///// <summary>
        ///// 默认值为 null 继承ObservableObject 可以使用 
        ///// </summary>
        ///// <typeparam name="X"></typeparam>
        ///// <param name="propertyExpresssion"></param>
        ///// <returns></returns>
        //public W GetZclass<X, W>(Expression<Func<X>> propertyExpresssion) where W : ObservableObject
        //{
        //    var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
        //    var gt = GetZ(propertyName);
        //    if (gt == null) return null;
        //    try
        //    {
        //        return gt as W;

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return null;
        //}

        ///// <summary>
        ///// 默认值为 null 继承ObservableObject 可以使用 
        ///// </summary>
        ///// <typeparam name="X"></typeparam>
        ///// <param name="propertyExpresssion"></param>
        ///// <returns></returns>
        //public ObservableCollection<W> GetZcollection<X, W>(Expression<Func<X>> propertyExpresssion)
        //    where W : ObservableObject
        //{
        //    var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
        //    var gt = GetZ(propertyName);
        //    if (gt == null)
        //    {
        //        SetZ(propertyName, new ObservableCollection<W>());
        //    }

        //    gt = GetZ(propertyName);
        //    if (gt != null)
        //    {
        //        try
        //        {
        //            return gt as ObservableCollection<W>;

        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }

        //    return null;
        //}

        /// <summary>
        /// 获取 给定属性的值 ，无值则返回默认值 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private object GetZ(string name)
        {
            if (dic.ContainsKey(name))
            {
                return dic[name];
            }

            return null;
        }

        /// <summary>
        /// 设置 给定属性的值 ，并触发RaisePropertyChanged 事件
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="propertyExpresssion"></param>
        /// <param name="value"></param>
        public void SetZ<X>(Expression<Func<X>> propertyExpresssion, object value)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            SetZ(propertyName, value);
        }


        /// <summary>
        /// 程序在构造的时候 某些时候不需要触发此时间  ，触发此时间等待时间比较长
        /// 
        /// </summary>
        public bool IsRaisePropertyChanged = true;

        /// <summary>
        /// 设置 给定属性的值 ，并触发RaisePropertyChanged 事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private void SetZ(string name, object value)
        {
            if (dic.ContainsKey(name))
            {

                if (dic[name] != null && value != null && dic[name] == value) return;
                dic[name] = value;

                if (IsRaisePropertyChanged)
                    this.RaisePropertyChanged(name);
            }
            else
            {
                dic.Add(name, value);
                if (IsRaisePropertyChanged)
                    this.RaisePropertyChanged(name);
            }
        }

        #endregion



    }


    /// <summary>
        /// 实现INotifyPropertyChanged接口的底层类，可继承直接使用OnPropertyChanged等函数
        /// </summary>
        [Serializable]
        public abstract class ObservableObject : INotifyPropertyChanged, IDataErrorInfo//, IDataErrorInfo
        {
            /// <summary>
            /// IDataErrorInfo成员
            /// </summary>
            public string Error
            {
                get
                {
                    return "";
                }
            }



            /// <summary>
            /// 获取错误属性
            /// </summary>
            /// <param name="columnName"></param>
            public string this[string columnName]
            {
                get
                {
                    {
                        try
                        {
                            //var vc = new ValidationContext(this, null, null);
                            //vc.MemberName = columnName;
                            //var res = new List<ValidationResult>();
                            //var obj = this.GetType().GetProperty(columnName).GetValue(this, null);
                            //var result =  Validator.TryValidateProperty(obj ,  vc, res);
                            //if (res.Count > 0)
                            //{
                            //    return string.Join(Environment.NewLine, res.Select(r => r.ErrorMessage).ToArray());
                            //}
                        }
                        catch (Exception ex)
                        {
                            return string.Empty; //"错误:" + ex;
                        }
                        return string.Empty;

                    }
                }

            }


            [field: NonSerialized]
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                var handler = this.PropertyChanged;
                if (handler != null)
                {
                    handler(this, e);
                }
            }

            internal virtual void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpresssion)
            {
                var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
                this.RaisePropertyChanged(propertyName);
            }

            protected void RaisePropertyChanged(String propertyName)
            {
                VerifyPropertyName(propertyName);
                OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }

            /// <summary>
            /// Warns the developer if this Object does not have a public property with
            /// the specified name. This method does not exist in a Release build.
            /// </summary>
            [Conditional("DEBUG")]
            [DebuggerStepThrough]
            public void VerifyPropertyName(String propertyName)
            {
                // verify that the property name matches a real,  
                // public, instance property on this Object.
                if (TypeDescriptor.GetProperties(this)[propertyName] == null)
                {
                    Debug.Fail("Invalid property name: " + propertyName);
                }
            }

            #region IDataErrorInfo 成员

            //[Browsable(false)]
            //public string Error
            //{
            //    get
            //    {
            //        string strReturn = "";
            //        foreach (KeyValuePair<string, String> keyValuePar in Errors)
            //        {
            //            try
            //            {
            //                strReturn += keyValuePar.Value;
            //            }
            //            catch (Exception ex) { ex.ToString(); }
            //        }
            //        return strReturn;
            //    }
            //}

            //[Browsable(false)]
            //public string this[string propertyName]
            //{

            //    get
            //    {
            //        if (Errors.ContainsKey(propertyName))
            //        {
            //            return Errors[propertyName];
            //        }
            //        return "has err but not write in errors container ...";
            //    }
            //}

            #endregion

            //protected Dictionary<String, String> Errors = new Dictionary<string, string>();
            //protected void AddError(string propertyName, string error)
            //{
            //    if (!Errors.ContainsKey(propertyName))
            //    {
            //        Errors.Add(propertyName, error);
            //        RaisePropertyChanged(propertyName);
            //        RaisePropertyChanged("Error");
            //    }
            //}
            //protected void RemoveError(string propertyName, string error)
            //{
            //    if (Errors.ContainsKey(propertyName))
            //    {
            //        Errors.Remove(propertyName);
            //        RaisePropertyChanged(propertyName);
            //        RaisePropertyChanged("Error");
            //    }
            //}




        }
}
