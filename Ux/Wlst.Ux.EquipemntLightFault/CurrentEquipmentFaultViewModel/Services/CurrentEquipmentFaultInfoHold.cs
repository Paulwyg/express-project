using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Services
{
    public class CurrentEquipmentFaultInfoHold
    {


        //写xml文件
        public static void WriteRules(ObservableCollection<CurrentItemViewModel> lst, int type,int visi)
        {
            try
            {

                string dir = Directory.GetCurrentDirectory() + "\\config";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                //string path = dir + "\\" + "\\FaultRules.xml";
                string path = dir + "\\" + "FaultRules.xml";
                if (File.Exists(path)) File.Delete(path);

                try
                {
                    XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
                    writer.Formatting = Formatting.Indented; //使用自动缩进便于阅读
                    writer.WriteStartDocument(); //XML声明
                    writer.WriteStartElement("Rules"); //书写根元素

                    foreach (var t in lst)
                    {
                        //if (string.IsNullOrEmpty(t.Id.ToString( ) ) || string.IsNullOrEmpty(t.Value)) continue;

                        writer.WriteStartElement("node" + t.Id.ToString("D1")); //开始一个元素
                        writer.WriteAttributeString("序号", t.Id.ToString("D1"));
                        if (t.IsEnable == true)
                        {
                            writer.WriteAttributeString("使用", "1");
                        }
                        else
                        {
                            writer.WriteAttributeString("使用", "0");
                        }
                        writer.WriteAttributeString("起始时间", t.StTime.ToString("D1"));
                        writer.WriteAttributeString("结束时间", t.EndTime.ToString("D1"));

                        foreach (var f in t.SelectedFaults)
                        {
                            var str = "";
                            foreach (var g in f.Value) str += g + ",";
                            writer.WriteAttributeString("L"+f.Key + "", str);
                        }
                        for (int i = 1; i < 7;i++ )
                        {
                            writer.WriteAttributeString(  "N"+i , t.Names [i-1].Name );
                        }


                            //if (t.SelectedFault1 != null)
                            //    writer.WriteAttributeString("故障一", t.SelectedFault1.Id.ToString("D1"));
                            //else
                            //    writer.WriteAttributeString("故障一", "0");
                            //if (t.SelectedFault2 != null)
                            //    writer.WriteAttributeString("故障二", t.SelectedFault2.Id.ToString("D1"));
                            //else
                            //    writer.WriteAttributeString("故障二", "0");
                            //if (t.SelectedFault3 != null)
                            //    writer.WriteAttributeString("故障三", t.SelectedFault3.Id.ToString("D1"));
                            //else
                            //    writer.WriteAttributeString("故障三", "0");
                            //if (t.SelectedFault4 != null)
                            //    writer.WriteAttributeString("故障四", t.SelectedFault4.Id.ToString("D1"));
                            //else
                            //    writer.WriteAttributeString("故障四", "0");
                            //if (t.SelectedFault5 != null)
                            //    writer.WriteAttributeString("故障五", t.SelectedFault5.Id.ToString("D1"));
                            //else
                            //    writer.WriteAttributeString("故障五", "0");


                            writer.WriteAttributeString("模式", type.ToString("D1"));

                            writer.WriteAttributeString("显示城区局", visi.ToString("D1"));

                        //int count = 1;
                        //foreach(var ttt in t.FaultComboBox )
                        //{
                        //    if(ttt.IsChecked == true)
                        //    {
                        //        if (count > 5) break;
                        //        writer.WriteAttributeString("故障"+count, ttt.Id.ToString("D1"));
                        //        count++;
                        //    }
                        //}
                        writer.WriteEndElement(); // 关闭元素
                    }
                    //在节点间添加一些空

                    writer.Close();

                }
                catch
                {
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static List<int> GetLst(string data)
        {
            var rtn = new List<int>();
            try
            {
                var sp = data.Split(',');
                foreach (var f in sp)
                {
                    int x = 0;
                    if (Int32.TryParse(f, out x))
                    {
                        rtn.Add(x);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return rtn;
        }

        //读xml文件
        public static Tuple<Dictionary<int, CurrentItemViewModel>, int,int> ReadRules()
        {
            var info = new Dictionary<int, CurrentItemViewModel>();
            int type = 1;
            string dir = Directory.GetCurrentDirectory() + "\\config";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\FaultRules.xml";
            if (!File.Exists(path))
            {
             return  new Tuple<Dictionary<int, CurrentItemViewModel>, int,int>(info, type,0);
               
            }

            var visi = 0;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(path);

                XmlNode root = xmlDoc.SelectSingleNode("Rules");
                if (root != null)
                {

                    var nodelist = root.ChildNodes;

                    foreach (XmlNode nodeType in nodelist)
                    {
                        XmlElement element = (XmlElement) nodeType;
                        if (element != null)
                        {
                            try
                            {
                                int id = Convert.ToInt32(element.GetAttribute("序号"));
                                int stTime = Convert.ToInt32(element.GetAttribute("起始时间"));
                                int endTime = Convert.ToInt32(element.GetAttribute("结束时间"));
                                //string selectedRules = element.GetAttribute("已选故障");
                                bool isenable = false;
                                if (Convert.ToInt32(element.GetAttribute("使用")) == 1)
                                {
                                    isenable = true;
                                }



                                var infoutem = new CurrentItemViewModel()
                                                   {
                                                       Id = id,

                                                       StTime = stTime,
                                                       EndTime = endTime,
                                                       IsEnable = isenable,



                                                   };

                                //List<int> faultid = new List<int>();
                                //faultid.Add(Convert.ToInt32(element.GetAttribute("1")));
                                //faultid.Add(Convert.ToInt32(element.GetAttribute("2")));
                                //faultid.Add(Convert.ToInt32(element.GetAttribute("3")));
                                //faultid.Add(Convert.ToInt32(element.GetAttribute("4")));
                                //faultid.Add(Convert.ToInt32(element.GetAttribute("5")));
                                for (int i = 1; i < 7; i++)
                                {
                                    var ntg = element.GetAttribute("L"+i );
                                    infoutem.SelectedFaults.Add(i, GetLst(ntg));
                                }
                                for (int i = 1; i < 7; i++)
                                {
                                    var ntg = element.GetAttribute("N" + i);
                                    if (infoutem.Names.Count >= i)
                                        infoutem.Names[i - 1] = new NameIntBool() {Name = ntg};
                                    else  infoutem.Names.Add(  new NameIntBool() {Name = ntg});
                                }

                                type = Convert.ToInt32(element.GetAttribute("模式"));

                                ////List<int> chosenFaults = new List<int>();
                                //var combobox =
                                //    CurrentEquipmentFaultViewModel.ViewModel.CurrentEquipmentFaultViewModel.
                                //        LoadFaultType();
                                //var combobox2 = new ObservableCollection<FaultItemViewModel>();



                                //foreach (var f in combobox) combobox2.Add(f);
                                try
                                {
                                    visi = Convert.ToInt32(element.GetAttribute("显示城区局"));
                                }
                                catch (Exception)
                                {
                                    visi = 0;
                                }


                                info.Add(id, infoutem);



                            }
                            catch (Exception ex)
                            {
                                Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("bug1", "配置文件有误，请重新载入。",
                                                                                               UMessageBoxButton.Ok);
                                if (File.Exists(path)) File.Delete(path);
                                break;
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {


                info.Clear();
                info.Add(1, new CurrentItemViewModel()
                                {
                                    Id = 1,
                                    StTime = -1500,
                                    EndTime = -1500,
                                    IsEnable = true,


                                });
                for (int i = 2; i < 7; i++)
                {
                    info.Add(i, new CurrentItemViewModel()
                                    {
                                        Id = i,
                                        StTime = 0,
                                        EndTime = 0,
                                        IsEnable = false,


                                    });
                }
                Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("bug2", "配置文件为空。", UMessageBoxButton.Ok);
            }
            return new Tuple<Dictionary<int, CurrentItemViewModel>, int, int>(info, type, visi);
        }




    }
}
