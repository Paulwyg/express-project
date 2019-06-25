using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Windows.Data;
using I36N.DataHolding;
using I36N.Helper;
using Wlst.Cr.Core.UtilityFunction;


namespace I36N.Services
{
    /// <summary>
    /// 汉化数据转换，注意：只提供后台到台前的数据转换，前台提交数据到后台未处理 
    /// </summary>
    public class I36N : IValueConverter, INotifyPropertyChanged
    {
        /// <summary>
        /// 前台绑定字段
        /// </summary>
        public static string AString
        {
            get { return "AString"; }
        }

        #region Constructor


        static I36N _myself;

        /// <summary>
        /// Save a reference to myself for changing cultures
        /// </summary>
        public static I36N Myself
        {
            get
            {
                if (_myself == null)
                {
                    new I36N();
                }
                return _myself;
            }

        }

        /// <summary>
        /// 是否及时更新
        /// </summary>
        //public static bool UpdateImmediately = false;


        public I36N()
        {

            if (_myself == null)
            {
                _myself = this;
                this.ChangeCulture(GetCulturefromDb(), true);
            }
        }

        /// <summary>
        /// 获取当前数据库中的汉化内容
        /// </summary>
        /// <returns></returns>
        public  string GetCulturefromDb()
        {
            var ssss = System.Convert.ToInt32(SqlLiteHelper.ExecuteQuery(
                "SELECT COUNT(*) as count FROM sqlite_master WHERE type='table' and name= 'i36n_culture'").
                                                  Tables[0].Rows[0][0].ToString().Trim());


            if (ssss < 1)
            {
                SqlLiteHelper.ExecuteQuery(
                    "CREATE TABLE 'i36n_culture' ('culture' text)");
            }


            try
            {
                DataSet ds = SqlLiteHelper.ExecuteQuery("select * from i36n_culture", null);
                if (ds == null) return "zh-CN";
                int mCount = ds.Tables[0].Rows.Count;
                for (int i = 0; i < mCount; i++)
                {
                    string name = ds.Tables[0].Rows[i]["culture"].ToString().Trim();
                    return name;
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    "Class I36N Function loadItem from SQLlite table i36n_culture  Occer an Error:" +
                    ex.ToString());
            }
            return "zh-CN";
        }

        /// <summary>
        /// 将汉化将使用的语言写入数据库 下次启动时自动加载汉化版本
        /// </summary>
        /// <param name="culture"></param>
        public  void SetCultureToDb(string culture)
        {
            try
            {
                SqlLiteHelper.ExecuteNonQuery("delete from i36n_culture");
                string strUpdateDirectroy = "insert into i36n_culture(culture) values ('" + culture.Trim() + "')";
                SqlLiteHelper.ExecuteNonQuery(strUpdateDirectroy);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        #endregion Constructor



        /// <summary>
        /// String resource manager
        /// </summary>
        //private static I36NdataHolding _stringResourceManager;

        private static I36NdataHolding _i36NdataHolding = new I36NdataHolding();


        #region

        public static bool MoniterIdAndDes = false;
        public static Dictionary<string , string> MoniterIdAndDesInfo = new Dictionary<string , string>(); 

        #endregion

        #region IValueConverter

        /// <summary>
        /// Converter to go find a string based on the UI culture
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value == null) || !(value is string))
                return "set: Binding Path=AString!";

            string par = parameter as string;
            if (string.IsNullOrEmpty(par)) return "ConverterParameter is Null";
            string[] sp = par.Split('#');
            if (sp.Length > 2 || sp.Length == 0) return "ConverterParameter Should be Value#Description";
            if (string.IsNullOrEmpty(sp[0])) return "Value Is Null";
            //StringResourceManager.RegisterNewInfoInChinese(sp[0], sp[1], sp[2]);
            string nametmp = _i36NdataHolding.GetName(sp[0]);

            return string.IsNullOrEmpty(nametmp) ? "Missing" : nametmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("No reason to do this.");
            //return "No reason to do this.";
        }

        #endregion IValueConverter

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Change the culture for the application.
        /// </summary>
        /// <param name="culture">Full culture name</param>
        /// <param name="updateImmediately"> 是否立即应用该culture到程序 </param>
        public void ChangeCulture(string culture, bool updateImmediately = false)
        {
            //if (!System.Threading.Thread.CurrentThread.CurrentCulture.Name.Equals(culture))
            //{
            //    SetCultureToDb(culture);
            //}
            if (updateImmediately)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    new System.Globalization.CultureInfo(culture);
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    System.Threading.Thread.CurrentThread.CurrentUICulture;
                // Strings.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture;

                _i36NdataHolding.CurrentCultureInfo = System.Threading.Thread.CurrentThread.CurrentUICulture;

                // notify that the culture has changed
                PropertyChangedEventHandler handler = PropertyChanged;


                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs("AString"));
                }

                foreach (var t in RegisterBindingNamess)
                {
                    if (t != null)
                        t.RaiseFatherPropertyChanged();
                }

            }
        }

        #endregion


        /// <summary>
        /// 后台代码调用实现绑定信息解析 ，多语言汉化
        /// </summary>
        /// <param name="id">id值</param>
        /// <param name="pars">参数，如果有的话</param>
        /// <returns>解析后的信息 包含出错信息</returns>
        public static string ConvertByCoding(string id, params object[] pars)
        {
            if (MoniterIdAndDes)
            {
                if (!MoniterIdAndDesInfo.ContainsKey(id)) MoniterIdAndDesInfo.Add(id, "id ");
            }

            try
            {
                if (string.IsNullOrEmpty(id)) return "Value Is Null";
                string nametmp = _i36NdataHolding.GetName(id);
                if (string.IsNullOrEmpty(nametmp)) return "Missing";

                string strReturn = nametmp;
                if (pars != null && pars.Length > 0)
                {
                    try
                    {
                        strReturn = string.Format(nametmp, pars);
                    }
                    catch (Exception ex)
                    {
                        strReturn = "string.Format error:" + ex;
                    }
                }
                try
                {
                    strReturn = System.Text.RegularExpressions.Regex.Replace(strReturn, @"\{[^\]]+\}", "");
                }
                catch (Exception ex)
                {
                    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
                }
                return strReturn;
            }
            catch (Exception ex)
            {
                return "Error" + ex;
            }
        }

        /// <summary>
        /// 后台代码调用实现绑定信息解析 ，多语言汉化
        /// </summary>
        /// <param name="id">id值</param>
        /// <param name="description">描述 </param>
        /// <param name="pars">参数，如果有的话</param>
        /// <returns>解析后的信息 包含出错信息</returns>
        public static string ConvertByCodingOne(string id, string description, params object[] pars)
        {

            if (MoniterIdAndDes)
            {
                if (!MoniterIdAndDesInfo.ContainsKey(id)) MoniterIdAndDesInfo.Add(id, description);
            }
            return ConvertByCoding(id, pars);
        }

        /// <summary>
        /// 后台代码调用实现绑定信息解析 ，多语言汉化
        /// </summary>
        /// <param name="id">id值</param>
        /// <param name="description">描述 </param>
        /// <param name="pars">参数，如果有的话</param>
        /// <returns>解析后的信息 包含出错信息</returns>
        public static string ConvertByCodingOne(int id, string description, params object[] pars)
        {
            if (MoniterIdAndDes)
            {
                if (!MoniterIdAndDesInfo.ContainsKey(id.ToString()))
                    MoniterIdAndDesInfo.Add(id.ToString(), description);
            }

            return ConvertByCoding(id.ToString(), pars);
        }


        private static List<Sstring> RegisterBindingNamess = new List<Sstring>();

        /// <summary>
        /// 注册属性 使用即时更新才有效果
        /// </summary>
        /// <param name="propertyName"></param>
        public static void RegisterPropertyName(Sstring propertyName)
        {
            if (!RegisterBindingNamess.Contains(propertyName)) RegisterBindingNamess.Add(propertyName);
        }

    }
}
