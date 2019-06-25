using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;


using Wlst.Cr.Core.CommandCore;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;

namespace Wlst.Ux.StateBarModule.AreaSet
{

    [Export(typeof (AreaSetVm))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class AreaSetVm : Wlst.Cr.Core.CoreInterface.IINavOnLoad, Wlst.Cr.Core.CoreInterface.IITab
    {
        #region tab
        public int Index
        {
            get { return 1; }
        }
        public bool CanClose
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public string Title
        {
            get { return "主界面布局设置"; }
        }

        #endregion

        #region CmdApply

        private ICommand _CmdApply;

        public ICommand CmdApply
        {
            get
            {

                if (_CmdApply == null)
                {
                    _CmdApply = new CommandRelay(ExCmdApply);
                }
                return _CmdApply;
            }
        }

        private void ExCmdApply()
        {

            //var tm1 = new Tuple<string, string, string, string, string, string, string>(this.Color1, this.Color2,
            //                                                                            this.Color3,
            //                                                                            this.Color4, this.Color5,
            //                                                                            this.ColorBottom, this.ColorMenu);
            //var tm2 = new Tuple<int, int, int, int, int, int>(DataArea, MsgArea, Area1Wide, Area35Wide, Area45Height,
            //                                                  MainArea);



            var dic = new Dictionary<int, string>();
            var desc = new Dictionary<int, string>();
            dic.Add(1, Color1);
            desc.Add(1, "树区域1背景色");

            dic.Add(2, Color2);
            desc.Add(2, "主设置区域2背景色");

            dic.Add(3, Color3);
            desc.Add(3, "区域4底部数据区域背景色");

            dic.Add(4, Color4);
            desc.Add(4, "区域5右下角背景色");

            dic.Add(5, Color5);
            desc.Add(5, "区域3背景色");


            //ColorBottom = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 6, Colors.Transparent.ToString());
            //ColorMenu = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 7, Colors.Transparent.ToString());

            dic.Add(6, ColorBottom);
            desc.Add(6, "左侧系统扼要信息背景色");

            dic.Add(7, ColorMenu);
            desc.Add(7, "菜单区域背景色");

            //DataArea = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 8, 4);
            //MsgArea = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 9, 5);
            //MainArea = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 10, 23);
            //Area1Wide = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 11, 200);

            dic.Add(8, DataArea + "");
            desc.Add(8, "设备最新数据占区域几");

            dic.Add(9, MsgArea + "");
            desc.Add(9, "信息区域占区域几");


            dic.Add(10, MainArea + "");
            desc.Add(10, "主设置区域占区域几");


            dic.Add(11, Area1Wide + "");
            desc.Add(11, "设备树区域1宽度");


            //Area45Height = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 12, 4);
            //Area35Wide = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 13, 5);
            //Area3Wide = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 14, 23);


            dic.Add(12, Area45Height + "");
            desc.Add(12, "最新数据区域4与5的高度");


            dic.Add(13, Area35Wide + "");
            desc.Add(13, "区域5宽度");


            dic.Add(14, Area3Wide + "");
            desc.Add(14, "区域3宽度");

            dic.Add(15, Area0Wide + "");
            desc.Add(15, "左侧扼要信息宽度");

            dic.Add(16, IsNewView?"1":"0");
            desc.Add(16, "界面显示在新窗口中");

            dic.Add(17, IsNewViewUseDefaultWin ?"1":"0");
            desc.Add(17, "弹出窗口使用默认系统样式");

            Wlst.Cr.CoreOne.Services.OptionXmlSvr.SaveXml(3301, dic, desc, "\\SystemColorAndFont");

            //MainWindow.update.windowsset
            //newdateheight
            var info = new PublishEventArgs() {EventType = "MainWindow.update.windowsset"};
            //info.AddParams(tm1);
            //info.AddParams(tm2);
            EventPublish.PublishEvent(info);
        }

        #endregion


        public void NavOnLoad(params object[] parsObjects)
        {
            this.OnLoad();
        }
        //private const string XmlSetPath = "CETC50_DemoAreaSet";
        private void OnLoad()
        {
            //Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 1);
            //var info = Elysium.ThemesSet.Common.ReadSave.Read(XmlSetPath);
           
            //if (info.ContainsKey("Color1")) Color1 = info["Color1"];
            //else Color1 = Colors.Transparent.ToString();

            Color1 = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 1, Colors.Transparent.ToString(), "\\SystemColorAndFont");
            Color2 = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 2, Colors.Transparent.ToString(),"\\SystemColorAndFont");
            Color3 = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 3, Colors.Transparent.ToString(), "\\SystemColorAndFont");
            Color4 = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 4, Colors.Transparent.ToString(), "\\SystemColorAndFont");
            Color5 = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 5, Colors.Transparent.ToString(), "\\SystemColorAndFont");

            //if (info.ContainsKey("Color2")) Color2 = info["Color2"];
            //else Color2 = Colors.Transparent.ToString();

            //if (info.ContainsKey("Color3")) Color3 = info["Color3"];
            //else Color3 = Colors.Transparent.ToString();

            //if (info.ContainsKey("Color4")) Color4 = info["Color4"];
            //else Color4 = Colors.Transparent.ToString();

            //if (info.ContainsKey("Color5")) Color5 = info["Color5"];
            //else Color5 = Colors.Transparent.ToString();


            ColorBottom = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 6, Colors.Transparent.ToString(), "\\SystemColorAndFont");
            ColorMenu = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 7, Colors.Transparent.ToString(), "\\SystemColorAndFont");

            //if (info.ContainsKey("ColorBottom")) ColorBottom = info["ColorBottom"];
            //else ColorBottom = Colors.Transparent.ToString();

            //if (info.ContainsKey("ColorMenu")) ColorMenu = info["ColorMenu"];
            //else ColorMenu = Colors.Transparent.ToString();


            DataArea = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 8, 4,"\\SystemColorAndFont");
            MsgArea = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 9, 5, "\\SystemColorAndFont");
            MainArea = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 10, 23, "\\SystemColorAndFont");
            Area1Wide = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 11, 200, "\\SystemColorAndFont");


            //try
            //{
            //    if (info.ContainsKey("DataArea")) DataArea = Int32.Parse(info["DataArea"]);
            //    else DataArea = 4;
            //}
            //catch (Exception ex)
            //{
            //}


            //try
            //{
            //    if (info.ContainsKey("MsgArea")) MsgArea = Int32.Parse(info["MsgArea"]);
            //    else MsgArea = 5;
            //}
            //catch (Exception ex)
            //{
            //}
        
            //try
            //{
            //    if (info.ContainsKey("MainArea")) MainArea = Int32.Parse(info["MainArea"]);
            //    else MainArea = 23;
            //}
            //catch (Exception ex)
            //{
            //}
            //try
            //{
            //    if (info.ContainsKey("Area1Wide")) Area1Wide = Int32.Parse(info["Area1Wide"]);
            //    else Area1Wide = 200;
            //}




            Area45Height = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 12, 4, "\\SystemColorAndFont");
            Area35Wide = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 13, 5, "\\SystemColorAndFont");
            Area3Wide = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 14, 23, "\\SystemColorAndFont");
            Area0Wide = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 15, 60, "\\SystemColorAndFont");

            IsNewView = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3301, 16, "\\SystemColorAndFont") == true;
            IsNewViewUseDefaultWin = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3301, 17, "\\SystemColorAndFont") == true;

            //catch (Exception ex)
            //{
            //}
            //try
            //{
            //    if (info.ContainsKey("Area45Height")) Area45Height = Int32.Parse(info["Area45Height"]);
            //    else Area45Height = 250;
            //}
            //catch (Exception ex)
            //{
            //}
            //try
            //{
            //    if (info.ContainsKey("Area35Wide")) Area35Wide = Int32.Parse(info["Area35Wide"]);
            //    else Area35Wide = 580;
            //}
            //catch (Exception ex)
            //{
            //}

        }



    }

    public partial class AreaSetVm : INotifyPropertyChanged
    {
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
    }

    public partial class AreaSetVm
    {

        #region Color1

        private string _Color1;

        public string Color1
        {
            get { return _Color1; }
            set
            {
                if (_Color1 != value)
                {
                    _Color1 = value;
                    this.OnPropertyChanged("Color1");
                }
            }
        }


        #endregion

        #region Color2

        private string _Color2;

        public string Color2
        {
            get { return _Color2; }
            set
            {
                if (_Color2 != value)
                {
                    _Color2 = value;
                    this.OnPropertyChanged("Color2");
                }
            }
        }


        #endregion

        #region Color3

        private string _Color3;

        public string Color3
        {
            get { return _Color3; }
            set
            {
                if (_Color3 != value)
                {
                    _Color3 = value;
                    this.OnPropertyChanged("Color3");
                }
            }
        }


        #endregion

        #region Color4

        private string _Color4;

        public string Color4
        {
            get { return _Color4; }
            set
            {
                if (_Color4 != value)
                {
                    _Color4 = value;
                    this.OnPropertyChanged("Color4");
                }
            }
        }


        #endregion

        #region Color5

        private string _Color5;

        public string Color5
        {
            get { return _Color5; }
            set
            {
                if (_Color5 != value)
                {
                    _Color5 = value;
                    this.OnPropertyChanged("Color5");
                }
            }
        }


        #endregion

        #region ColorBottom

        private string _colorBottom;

        public string ColorBottom
        {
            get { return _colorBottom; }
            set
            {
                if (_colorBottom != value)
                {
                    _colorBottom = value;
                    OnPropertyChanged("ColorBottom");
                }
            }
        }

        #endregion


        #region ColorMenu

        private string _colorBColorMenuottom;

        public string ColorMenu
        {
            get { return _colorBColorMenuottom; }
            set
            {
                if (_colorBColorMenuottom != value)
                {
                    _colorBColorMenuottom = value;
                    OnPropertyChanged("ColorMenu");
                }
            }
        }

        #endregion

        #region DataArea  4 5 45  数据区域位置

        private int _DataArea;

        public int DataArea
        {
            get { return _DataArea; }
            set
            {
                if (_DataArea != value)
                {
                    _DataArea = value;
                    this.OnPropertyChanged("DataArea");
                }
            }
        }


        #endregion

        #region MsgArea 3 4 5  信息区域位置

        private int _MsgArea;

        public int MsgArea
        {
            get { return _MsgArea; }
            set
            {
                if (_MsgArea != value)
                {
                    _MsgArea = value;
                    this.OnPropertyChanged("MsgArea");
                }
            }
        }


        #endregion

        #region MainArea 2 23 24 2345 主设置区域

        private int _mainArea;

        public int MainArea
        {
            get { return _mainArea; }
            set
            {
                if (_mainArea != value)
                {
                    _mainArea = value;
                    this.OnPropertyChanged("MainArea");
                }
            }
        }


        #endregion

        #region Area1Wide 设备树

        private int _Area1Wide;

        public int Area1Wide
        {
            get { return _Area1Wide; }
            set
            {
                if (_Area1Wide != value)
                {
                    if (value < 0) value = 0;
                    if (value > 500) value = 500;
                    _Area1Wide = value;
                    this.OnPropertyChanged("Area1Wide");
                }
            }
        }


        #endregion

        #region Area45Height 底部最新数据高度

        private int _Area45Height;

        public int Area45Height
        {
            get { return _Area45Height; }
            set
            {
                if (_Area45Height != value)
                {
                    if (value < 100) value = 100;
                    if (value > 950) value = 950;
                    _Area45Height = value;
                    this.OnPropertyChanged("Area45Height");
                }
            }
        }


        #endregion

        #region Area35Wide 左侧信息

        private int _Area35Wide;

        public int Area35Wide
        {
            get { return _Area35Wide; }
            set
            {
                if (_Area35Wide != value)
                {
                    if (value < 100) value = 100;
                    if (value > 950) value = 950;
                    _Area35Wide = value;
                    this.OnPropertyChanged("Area35Wide");
                }
            }
        }


        #endregion

        #region Area3Wide 左侧信息

        private int _Area3Wide;

        public int Area3Wide
        {
            get { return _Area3Wide; }
            set
            {
                if (_Area3Wide != value)
                {
                    if (value < 100) value = 100;
                    if (value > 950) value = 950;
                    _Area3Wide = value;
                    this.OnPropertyChanged("Area3Wide");
                }
            }
        }


        #endregion

        #region _Area0Wide 左侧扼要信息宽度

        private int _Area0Wide;

        public int Area0Wide
        {
            get { return _Area0Wide; }
            set
            {
                if (_Area0Wide != value)
                {
                    // if (value < 40) value = 100;
                    if (value > 250) value = 250;
                    _Area0Wide = value;
                    this.OnPropertyChanged("Area0Wide");
                }
            }
        }


        #endregion

        #region IsNewView


        private bool _isNewView;

        public bool IsNewView
        {
            get { return _isNewView; }
            set
            {
                if (value != _isNewView)
                {
                    _isNewView = value;
                    this.OnPropertyChanged("IsNewView");
                }
            }
        }



        #endregion

        #region IsNewViewUseDefaultWin


        private bool _isNewViewUseDefaultWin;

        public bool IsNewViewUseDefaultWin
        {
            get { return _isNewViewUseDefaultWin; }
            set
            {
                if (value != _isNewViewUseDefaultWin)
                {
                    _isNewViewUseDefaultWin = value;
                    this.OnPropertyChanged("IsNewViewUseDefaultWin");
                }
            }
        }



        #endregion


    }
}
