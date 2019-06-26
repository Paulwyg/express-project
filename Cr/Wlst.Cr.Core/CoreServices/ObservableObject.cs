using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.Prism.ViewModel;
using System.Diagnostics;

namespace Wlst.Cr.Core.CoreServices
{
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
            get { return "";
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
                        var vc = new ValidationContext(this, null, null);
                        vc.MemberName = columnName;
                        var res = new List<ValidationResult>();
                        var obj = this.GetType().GetProperty(columnName).GetValue(this, null);
                        var result =  Validator.TryValidateProperty(obj ,  vc, res);
                        if (res.Count > 0)
                        {
                            return string.Join(Environment.NewLine, res.Select(r => r.ErrorMessage).ToArray());
                        }
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
