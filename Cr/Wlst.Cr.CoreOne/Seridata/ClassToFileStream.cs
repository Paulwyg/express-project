using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Wlst.Cr.CoreOne.Seridata
{
    public class ClassToFileStream
    {
        //    protected static string ConfFilePath = "d:\fff";

        #region 读取保存

        /// <summary>
        /// Save the object in XML format to a stream
        /// </summary>
        /// <param name="s">Stream to save the object to</param>
        /// <param name="obj"> </param>
        protected static void SaveAsXml(Stream s, object obj)
        {
            IFormatter iformat = new BinaryFormatter();
            iformat.Serialize(s, obj);
        }

        public static void SaveAsXml(string filepath, object obj)
        {
            if (File.Exists(filepath)) File.Delete(filepath);
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    SaveAsXml(fs, obj);
                }
            }
            catch
            {

                throw;
            }
        }


        public static void DeleteXml(string filepath)
        {

            try
            {
                if (File.Exists(filepath)) File.Delete(filepath);
            }
            catch
            {
            }
        }

        public static T LoadFromXml<T>(string filepath)
        {
            //  ConfFilePath = filepath;
            if (!File.Exists(filepath))
            {
                return default(T);
                ;
            }
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    var result = LoadFromXml<T>(fs);
                    return result;
                }
            }
            catch
            {
                return default(T);
                ;
            }
        }

        /// <summary>
        /// Create a new object loading members from the stream in XML format.
        /// Derived class should call this from a static method i.e.:
        /// return (ComDerivedSettings)LoadFromXml(s, typeof(ComDerivedSettings));
        /// </summary>
        /// <param name="s">Stream to load the object from</param>
        /// <returns></returns>
        protected static T LoadFromXml<T>(Stream s)
        {
            IFormatter iformat = new BinaryFormatter();
            try
            {
                return (T)iformat.Deserialize(s);
            }
            catch
            {
                s.Close();
                s.Dispose();
                return default(T);
            }
        }

        #endregion
    }
}
