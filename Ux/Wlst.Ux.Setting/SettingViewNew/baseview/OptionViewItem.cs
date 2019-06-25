using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Wlst.Cr.CoreMims.NodeServices;

namespace Wlst.Ux.Setting.SettingViewNew.baseview
{
    public class OptionViewItem : ObservableObject
    {

        private string _name;

        //左侧显示名称
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        private int _Id;

        //唯一编号
        public int Id
        {
            get { return _Id; }
            set
            {
                if (value == _Id) return;
                _Id = value;
                this.RaisePropertyChanged("Id");
            }
        }


        private ContentControl _content;

        //显示的页面
        public ContentControl Content
        {
            get { return _content; }
            set
            {
                if (value == _content) return;
                _content = value;
                this.RaisePropertyChanged("Content");
            }
        }

        //第一次计算的 页面与scr距离
        public Point Point = new Point(0, 0);

        //鼠标移动的时候计算的 当前与 0 0 的距离
        public Point PointTmp = new Point(0, 0);


        private string _nameBackGround;

        //id显示的背景色
        public string BackGround
        {
            get { return _nameBackGround; }
            set
            {
                if (value == _nameBackGround) return;
                _nameBackGround = value;
                this.RaisePropertyChanged("BackGround");
            }
        }

        private string _nameBackGroundView;

        //视图显示的背景色
        public string BackGroundView
        {
            get { return _nameBackGroundView; }
            set
            {
                if (value == _nameBackGroundView) return;
                _nameBackGroundView = value;
                this.RaisePropertyChanged("BackGroundView");
            }
        }
    }
}
