using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Controls;

namespace Wlst.Cr.CoreOne.Services
{
    public class LoadSaveDisplayIndex
    {
        public static void Create_DisplayIndex_Folder()
        {
            string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig\\DisplayIndex";

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }

        public static void SaveDisplayIndex(Telerik.Windows.Controls.GridViewColumnCollection _column, string xmlFileName)
        {
            Dictionary<string, string> _columnsDisplayIndex = new Dictionary<string, string>();

            foreach (var g in _column)
            {
                _columnsDisplayIndex.Add(g.Header.ToString(), g.DisplayIndex.ToString());
            }


            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(_columnsDisplayIndex, xmlFileName);
        }

        public static void LoadDisplayIndex(Telerik.Windows.Controls.GridViewColumnCollection _column, string xmlFileName)
        {
            Create_DisplayIndex_Folder();

            Dictionary<string, string> _columnsDisplayIndex = new Dictionary<string, string>();

            _columnsDisplayIndex.Clear();

            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(xmlFileName);
            foreach (var g in info)
            {
                _columnsDisplayIndex.Add(g.Key, g.Value);
            }

            if (_columnsDisplayIndex == null || _columnsDisplayIndex.Count == 0) return;
            foreach (var g in _column)
            {
                foreach (var j in _columnsDisplayIndex)
                {
                    if (g.Header.ToString() == j.Key)
                    {
                        g.DisplayIndex = int.Parse(j.Value);
                        break;
                    }
                }
            }
        }
    }
}
