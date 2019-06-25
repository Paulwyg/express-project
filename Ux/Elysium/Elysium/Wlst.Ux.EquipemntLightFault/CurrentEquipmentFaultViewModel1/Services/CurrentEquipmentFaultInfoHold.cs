using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Services
{
    public class CurrentEquipmentFaultInfoHold
    {
       
        
        //写xml文件
        public static void WriteRules(ObservableCollection<CurrentItemViewModel> lst,int type)
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
                        if (t.SelectedFault1 != null)
                            writer.WriteAttributeString("故障一", t.SelectedFault1.Id.ToString("D1"));
                        else
                            writer.WriteAttributeString("故障一", "0");
                        if (t.SelectedFault2 != null)
                            writer.WriteAttributeString("故障二", t.SelectedFault2.Id.ToString("D1"));
                        else
                            writer.WriteAttributeString("故障二", "0");
                        if (t.SelectedFault3 != null)
                            writer.WriteAttributeString("故障三", t.SelectedFault3.Id.ToString("D1"));
                        else
                            writer.WriteAttributeString("故障三", "0");
                        if (t.SelectedFault4 != null)
                            writer.WriteAttributeString("故障四", t.SelectedFault4.Id.ToString("D1"));
                        else
                            writer.WriteAttributeString("故障四", "0");
                        if (t.SelectedFault5 != null)
                            writer.WriteAttributeString("故障五", t.SelectedFault5.Id.ToString("D1"));
                        else
                            writer.WriteAttributeString("故障五", "0");
                        writer.WriteAttributeString("模式", type.ToString("D1"));
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

        //读xml文件
        public static Tuple<Dictionary<int, CurrentItemViewModel>,int> ReadRules()
        {
            var info = new Dictionary<int, CurrentItemViewModel>();
            int type = 1;
            string dir = Directory.GetCurrentDirectory() + "\\config";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\FaultRules.xml";
            if (!File.Exists(path)) return new Tuple<Dictionary<int, CurrentItemViewModel>,int>(info,type);

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
                        XmlElement element = (XmlElement)nodeType;
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

                                List<int> faultid = new List<int>();
                                faultid.Add(Convert.ToInt32(element.GetAttribute("故障一")));
                                faultid.Add(Convert.ToInt32(element.GetAttribute("故障二")));
                                faultid.Add(Convert.ToInt32(element.GetAttribute("故障三")));
                                faultid.Add(Convert.ToInt32(element.GetAttribute("故障四")));
                                faultid.Add(Convert.ToInt32(element.GetAttribute("故障五")));

                                type = Convert.ToInt32(element.GetAttribute("模式"));

                                //List<int> chosenFaults = new List<int>();
                                var combobox =
                                    CurrentEquipmentFaultViewModel.ViewModel.CurrentEquipmentFaultViewModel.
                                        LoadFaultType();
                                var combobox2 = new ObservableCollection<FaultItemViewModel>();

                                //List<int> selectfaultid = new List<int>();

                                //for (int j = 0; j < 5; j++)
                                //{
                                //    foreach (var tt in combobox)
                                //    {
                                //        if (faultid[j] == tt.Id)
                                //        {
                                //            selectfaultid.Add(tt.Index);
                                //        }
                                //    }
                                //    if (selectfaultid.Count < j+1)
                                //    {
                                //        selectfaultid.Add(0);
                                //    }
                                //}






                                //var selectedfault = new List<FaultItemViewModel>();
                                //for (int i = 0; i < 5; i++)
                                //{
                                //    foreach (var f in combobox)
                                //    {
                                //        if (f.Id == faultid[i])
                                //        {
                                //            selectedfault.Add(new FaultItemViewModel()
                                //            {
                                //                Id = f.Id,
                                //                FaultName = f.FaultName
                                //            });
                                //         }
                                //    }
                                //    selectedfault.Add(nofault);
                                //}


                                foreach (var f in combobox) combobox2.Add(f);


                                var infoutem = new CurrentItemViewModel()
                                {
                                    Id = id,
                                    FaultComboBox = combobox2,
                                    StTime = stTime,
                                    EndTime = endTime,
                                    IsEnable = isenable,
                                    Fault1 = faultid[0],
                                    Fault2 = faultid[1],
                                    Fault3 = faultid[2],
                                    Fault4 = faultid[3],
                                    Fault5 = faultid[4],
                                    

                                };
                                info.Add(id, infoutem);



                            }
                            catch (Exception ex)
                            {
                                Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("bug1","配置文件有误，请重新载入。" , UMessageBoxButton.Ok);
                                if (File.Exists(path)) File.Delete(path);
                                break;
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                var combobox3 =
                          CurrentEquipmentFaultViewModel.ViewModel.CurrentEquipmentFaultViewModel.
                        LoadFaultType();
                var combobox4 = new ObservableCollection<FaultItemViewModel>();

                //var nofault1 = new FaultItemViewModel()
                //{
                //    Id = 0,
                //    FaultName = "无"
                //};
                foreach (var f in combobox3) combobox4.Add(f);

                info.Clear();
                info.Add(1, new CurrentItemViewModel()
                {
                    Id = 1,
                    FaultComboBox = combobox4,
                    StTime = -1500,
                    EndTime = -1500,
                    IsEnable = true,
                    Fault1 = -1,
                    Fault2 = -1,
                    Fault3 = -1,
                    Fault4 = -1,
                    Fault5 = -1,

                });
                for (int i = 2; i < 7; i++)
                {
                    info.Add(i, new CurrentItemViewModel()
                    {
                        Id = i,
                        StTime = 0,
                        EndTime = 0,
                        FaultComboBox = combobox4,
                        IsEnable = false,
                        Fault1 = -1,
                        Fault2 = -1,
                        Fault3 = -1,
                        Fault4 = -1,
                        Fault5 = -1,

                    });
                }
                Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("bug2", "配置文件为空。", UMessageBoxButton.Ok);
            }
            return new Tuple<Dictionary<int, CurrentItemViewModel>, int>(info,1);
        }


        

    }
}
