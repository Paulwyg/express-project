using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wlst.Cr.Core.CommandCore;
using Wlst.Cr.Core.ComponentHold;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreOne.ComponentHold;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Image = System.Windows.Controls.Image;

namespace Wlst.Sr.Menu.Services
{
    /// <summary>
    /// 提供生产具体菜单功能，需要提供设置好的菜单关键字。
    /// 使用提供好的菜单关键字调用具体函数即可完成菜单生成。
    /// </summary>
    public class MenuBuilding
    {

        private static  Dictionary<int, System.Windows.Controls.Image> _imagesourcesss = new Dictionary<int, Image>();
        private const int FileId = 1010;


        public static int ImageHeight = 24;
        /// <summary>
        /// 生成菜单
        /// </summary>
        /// <param name="menuInstanceKey">菜单关键字</param>
        /// <param name="hasShortCuts">是否具有快捷键</param>
        /// <param name="arguData">菜单参数 </param>
        /// <returns>菜单 不存在在返回空的list</returns>
        public static ObservableCollection<IIMenuItem> BulidCm(string menuInstanceKey, Boolean hasShortCuts,
                                                               object arguData)
        {
            if (hasShortCuts)
            {
                ServicesShortCuts.ClearRegisterLst();
            }
            var items = new ObservableCollection<IIMenuItem>();
            var lstMenu = ServerInstanceRelation.GetInstanceRelationsByfatherId(menuInstanceKey, 0);
            if (lstMenu == null || lstMenu.Count == 0) return items;

            foreach (var t in lstMenu)
            {
                if (t.FatherId == 0)
                {
                    if (t.Id >= MenuIdControlAssign.MenuFileGroupIdMin) //File
                    {
                        var menuItemFile = new MenuItemBase()
                                               {IsCheckable = false, IsEnabled = true,Id =t.Id , Visibility = Visibility.Visible};

                        BulidChild(menuItemFile, menuInstanceKey, t.Id, hasShortCuts, arguData);

                        menuItemFile.Text = t.Name;
                        menuItemFile.TextTmp = t.Name;
                        var textid = ImageSourceHelper.MySelf.GetTextById(t.Id);
                        if (!string.IsNullOrEmpty(textid))
                        {
                            menuItemFile.Text = textid;
                        }
                        if (menuItemFile.CmItems.Count > 0)
                        {
                            items.Add(menuItemFile);
                        }
                        else menuItemFile = null;
                    }
                    else //末端 菜单 功能性部件
                    {
                        if (t.Id >= MenuIdControlAssign.MenuIdMin && t.Id <= MenuIdControlAssign.MenuIdMax)
                        {
                            //var eng = I36N.Services.I36N.ConvertByCoding( t.Id.ToString());
                            var menuItem = MenuComponentHolding.GetMenuItemById(t.Id);
                           
                            if (menuItem != null) //末端 菜单 功能性部件
                            {
                                menuItem.InitDataWhenBeforeUse(arguData);
                                bool rwx = menuItem.IsCanBeShowRwx();
                                if (rwx == false) continue;

                                if (_imagesourcesss.ContainsKey(t.Id))
                                {
                                    menuItem.Image = _imagesourcesss[t.Id];
                                }
                                else
                                {
                                    var imagesource = //ImageSourceHelper.MySelf.GetImageSourceById(1001);
                                        ImageSourceHelper.MySelf.GetImageSourceById(t.Id);
                                    if (imagesource != null)
                                    {

                                        var img = new System.Windows.Controls.Image
                                                      {
                                                          Source = imagesource
                                                      };
                                        img.Stretch = Stretch.UniformToFill;
                                        img.Height = ImageHeight;
                                        img.Width = ImageHeight;


                                        if (!_imagesourcesss.ContainsKey(t.Id)) _imagesourcesss.Add(t.Id, img);
                                        menuItem.Image = img;

                                    }
                                    var textid = ImageSourceHelper.MySelf.GetTextById( t.Id);
                                    if(!string .IsNullOrEmpty( textid ))
                                    {
                                        menuItem.Text = textid;
                                    }
                                }

                               
                                //if (!string.IsNullOrEmpty(eng) && !eng.Contains("issing")) //汉化内容
                                //{
                                //    menuItem.Text = eng;
                                //}
                              
                               
                                menuItem.Tag = "";
                                if (string.IsNullOrEmpty(menuItem.ExText))
                                    menuItem.TextTmp = menuItem.Text;
                                else
                                    menuItem.TextTmp = menuItem.Text + " - " + menuItem.ExText;
                                if (hasShortCuts)
                                {
                                    var shortcut = ServicesShortCuts.GetShortCutValue(t.Id);
                                    if (!string.IsNullOrEmpty(shortcut)) //注册快捷键
                                    {
                                        menuItem.ShortCuts = shortcut;
                                        ServicesShortCuts.AddRegisterMenuItem(menuItem, shortcut);
                                    }
                                }
                                items.Add(menuItem);
                            }
                        }
                    }
                }
            }
            return items;




        }

        private static void BulidChild(MenuItemBase menuItemFather, string menuInstanceKey, int rootId,
                                       Boolean hasShortCuts, object arguData)
        {
            var lstMenu = ServerInstanceRelation.GetInstanceRelationsByfatherId(menuInstanceKey, rootId);
            if (lstMenu == null || lstMenu.Count == 0) return;

            foreach (var t in lstMenu)
            {
                if (t.FatherId == rootId)
                {
                    if (t.Id >= MenuIdControlAssign.MenuFileGroupIdMin) //File
                    {
                        var menuItemFile = new MenuItemBase()
                                               {IsCheckable = false, IsEnabled = true,Id =t.Id , Visibility = Visibility.Visible};

                        BulidChild(menuItemFile, menuInstanceKey, t.Id, hasShortCuts, arguData);

                        menuItemFile.Text = t.Name;
                        menuItemFile.TextTmp = t.Name;
                        var textid = ImageSourceHelper.MySelf.GetTextById(t.Id);
                        if (!string.IsNullOrEmpty(textid))
                        {
                            menuItemFile.Text = textid;
                        }
                        if (menuItemFile.CmItems.Count > 0)
                        {
                            menuItemFather.CmItems.Add(menuItemFile);
                        }
                        else menuItemFile = null;
                    }
                    else //末端 菜单 功能性部件
                    {
                        if (t.Id >= MenuIdControlAssign.MenuIdMin && t.Id <= MenuIdControlAssign.MenuIdMax)
                        {
                            // var eng = EngHolding.GetEngValue(t.Value);
                            //var eng = I36N.Services.I36N.ConvertByCoding(t.Id.ToString());
                            var menuItemChild = MenuComponentHolding.GetMenuItemById(t.Id);
                            if (menuItemChild != null) //末端 菜单 功能性部件
                            {  
                                menuItemChild.InitDataWhenBeforeUse(arguData);
                                bool rwx = menuItemChild.IsCanBeShowRwx();
                                if (rwx == false) continue;


                              
                                //if (eng != null) //汉化内容
                                //{
                                //if (!string.IsNullOrEmpty(eng) && !eng.Contains("Missing")) menuItemChild.Text = eng;
                                //var imagesource = 
                                // Wlst.Cr.CoreOne.Services.ImageSourceHelper.MySelf.GetImageSourceById(t.Id);
                                //if (imagesource != null) menuItemChild.Image = imagesource;
                                //if (!string.IsNullOrEmpty(eng.Item2)) menuItemChild.Tooltips = eng.Item2;
                                //}
                                if (_imagesourcesss.ContainsKey(t.Id))
                                {
                                    menuItemChild.Image = _imagesourcesss[t.Id];
                                }
                                else
                                {
                                    var imagesource = //ImageSourceHelper.MySelf.GetImageSourceById(1001);
                                        ImageSourceHelper.MySelf.GetImageSourceById(t.Id);
                                    if (imagesource != null)
                                    {

                                        var img = new System.Windows.Controls.Image
                                        {
                                            Source = imagesource
                                        };
                                        img.Stretch = Stretch.UniformToFill;
                                        img.Height = ImageHeight;
                                        img.Width = ImageHeight;


                                        if (!_imagesourcesss.ContainsKey(t.Id)) _imagesourcesss.Add(t.Id, img);
                                        menuItemChild.Image = img;

                                    }
                                }
                                var textid = ImageSourceHelper.MySelf.GetTextById(t.Id);
                                if (!string.IsNullOrEmpty(textid))
                                {
                                    menuItemChild.Text = textid;
                                }
                                menuItemChild.Tag = "";
                                if (string.IsNullOrEmpty(menuItemChild.ExText))
                                    menuItemChild.TextTmp = menuItemChild.Text;
                                else
                                    menuItemChild.TextTmp = menuItemChild.Text+" - "+menuItemChild .ExText ;
                                if (hasShortCuts)
                                {
                                    var shortcut = ServicesShortCuts.GetShortCutValue(t.Id);
                                    if (!string.IsNullOrEmpty(shortcut)) //注册快捷键
                                    {
                                        menuItemChild.ShortCuts = shortcut;
                                        ServicesShortCuts.AddRegisterMenuItem(menuItemChild, shortcut);
                                    }
                                }
                                menuItemFather.CmItems.Add(menuItemChild);
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 通过输入的数据来新建菜单集合；
        /// 如果为主菜单  则如果一级菜单下的所有叶子菜单均不可见则一级菜单页将不可见，
        /// 则即使权限发生变化下面的叶子菜单可见一级菜单依然不可见，无触发事件能让他可见 这是个debug
        /// </summary>
        /// <param name="t"></param>
        /// <param name="isVislWhenMenuItemEnableFault">当菜单不可见时是否添加菜单到菜单价  默认 false </param>
        /// <returns></returns>
        public static ObservableCollection<MenuItem> HelpCmm(ObservableCollection<IIMenuItem> t )
        {
            ObservableCollection<IIMenuItem> iimenuItems = new ObservableCollection<IIMenuItem>();
            ObservableCollection<MenuItem> miReturn = new ObservableCollection<MenuItem>();

            if (t == null) return new ObservableCollection<MenuItem>();
            foreach (var f in t)
            {
                if (f.CmItems != null && f.CmItems.Count > 0)
                {
                    var ggg = HelpCmmm(f, iimenuItems );
                    if ( ggg.Items.Count == 0) continue;
                    ggg.Header = f.Text;
                    //ggg.IsVisibleChanged += new DependencyPropertyChangedEventHandler(ggg_IsVisibleChanged);
               //     ggg.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(ggg_MouseDown);   
                    //ggg .PreviewMouseDown 

                    ggg .MinHeight = ImageHeight;
                    ggg .VerticalAlignment = VerticalAlignment.Center;
                    miReturn.Add(ggg);
                }
                else
                {
                 //   if ( f.Visibility != Visibility.Visible) continue;

                    iimenuItems.Add(f);
                    int index = iimenuItems.Count - 1;
                    MenuItem mii = new MenuItem();
                    if (!string.IsNullOrEmpty(f.ExText)) mii.Header = f.Text  + f.ExText;
                    else mii.Header = f.Text;
                    if (!string.IsNullOrEmpty(f.ShortCuts)) mii.Header += " " + f.ShortCuts;
                    mii.IsCheckable = f.IsCheckable;
                    //mii.IsEnabled = f.IsEnabled;
            //        mii.ToolTip = f.Tooltips;
                    mii.Command = f.Command;


                    mii.MinHeight = ImageHeight;
                    mii .VerticalAlignment=VerticalAlignment.Center;
                    if (f.Image != null)
                    {
                        var path = f.Image as System.Windows.Controls.Image;
                        if (path !=null )
                        {
                            mii.Icon = path;
                        }
                        //var img = new System.Windows.Controls.Image
                        //{
                        //    Source = path
                        //};
                        //img .Stretch=Stretch.UniformToFill;
                        //img.Height = ImageHeight;
                        //img.Width = ImageHeight;
                        //mii.Icon = img;
            
                    }



                    //Binding bidchkd = new Binding();
                    //bidchkd.Path = new PropertyPath("IsChecked");
                    //bidchkd.Source = iimenuItems[index];
                    //bidchkd.Mode = BindingMode.TwoWay;
                    //BindingOperations.SetBinding(mii, MenuItem.IsCheckedProperty, bidchkd);
                    //Binding bidchk = new Binding();
                    //bidchk.Path = new PropertyPath("IsCheckable");
                    //bidchk.Source = iimenuItems[index];
                    //bidchk.Mode = BindingMode.TwoWay;
                    //BindingOperations.SetBinding(mii, MenuItem.IsCheckableProperty, bidchk);
                    //Binding bidvisi = new Binding();
                    //bidvisi.Mode = BindingMode.TwoWay;
                    //bidvisi.Path = new PropertyPath("Visibility");
                    //bidvisi.Source = iimenuItems[index];
                    //bidvisi.Mode = BindingMode.TwoWay;
                    //BindingOperations.SetBinding(mii, MenuItem.VisibilityProperty, bidvisi);
                    //Binding biden = new Binding();
                    //biden.Mode = BindingMode.TwoWay;
                    //biden.Path = new PropertyPath("IsEnabled");
                    //biden.Source = iimenuItems[index];
                    //BindingOperations.SetBinding(mii, MenuItem.IsEnabledProperty, biden);
                    miReturn.Add(mii);
                }
            }
            return miReturn;
        }

        private static MenuItem HelpCmmm(IIMenuItem g, ObservableCollection<IIMenuItem> iimenuItems)
        {
            MenuItem mi = new MenuItem();
            foreach (var f in g.CmItems)
            {
                if (f.CmItems != null && f.CmItems.Count > 0)
                {
                    var ggg = HelpCmmm(f, iimenuItems );
                    if ( ggg.Items.Count == 0) continue;
                    ggg.Header = f.Text;
                    //ggg.IsVisibleChanged += new DependencyPropertyChangedEventHandler(ggg_IsVisibleChanged);


                    ggg .MinHeight = ImageHeight;
                    ggg .VerticalAlignment = VerticalAlignment.Center;
             
                    mi.Items.Add(ggg);
                }
                else
                {

                   
                        //if (f.Visibility != Visibility.Visible)
                        //    continue;
                    
                    iimenuItems.Add(f);
                    int index = iimenuItems.Count - 1;
                    MenuItem mii = new MenuItem();
                    if (!string.IsNullOrEmpty(f.ExText)) mii.Header = f.Text  + f.ExText;
                    else mii.Header = f.Text;

                    if (!string.IsNullOrEmpty(f.ShortCuts)) mii.Header += " "+f.ShortCuts;
                    mii.IsCheckable = f.IsCheckable;
                    //mii.IsEnabled = f.IsEnabled;
             //       mii.ToolTip = f.Tooltips;
                    mii.Command = f.Command;

                    
                    //mii .Command =new CommandRelay(f.Command .Execute ,f.Command .CanExecute );

                    mii.MinHeight = ImageHeight;
                    mii.VerticalAlignment = VerticalAlignment.Center;
                    if (f.Image != null)
                    {
                        var path = f.Image as System.Windows.Controls.Image;
                        if (path != null)
                        {
                            mii.Icon = path;
                        }

                        //var path = f.Image as ImageSource;
                        //var img = new System.Windows.Controls.Image
                        //{
                        //    Source = path
                        //};
                        //img.Stretch = Stretch.UniformToFill;
                        //img.Height = ImageHeight;
                        //img.Width = ImageHeight;
                        //mii.Icon = img;

                        //mii.Icon = new System.Windows.Controls.Image
                        //{
                        //    Source = path
                        //};
                    }



                    //Binding bidchkd = new Binding();
                    //bidchkd.Path = new PropertyPath("IsChecked");
                    //bidchkd.Source = iimenuItems[index];
                    //bidchkd.Mode = BindingMode.TwoWay;
                    //BindingOperations.SetBinding(mii, MenuItem.IsCheckedProperty, bidchkd);
                    //Binding bidchk = new Binding();
                    //bidchk.Path = new PropertyPath("IsCheckable");
                    //bidchk.Source = iimenuItems[index];
                    //bidchk.Mode = BindingMode.TwoWay;
                    //BindingOperations.SetBinding(mii, MenuItem.IsCheckableProperty, bidchk);
                    //Binding bidvisi = new Binding();
                    //bidvisi.Mode = BindingMode.TwoWay;
                    //bidvisi.Path = new PropertyPath("Visibility");
                    //bidvisi.Source = iimenuItems[index];
                    //bidvisi.Mode = BindingMode.TwoWay;
                    //BindingOperations.SetBinding(mii, MenuItem.VisibilityProperty, bidvisi);
                    //Binding biden = new Binding();
                    //biden.Mode = BindingMode.TwoWay;
                    //biden.Path = new PropertyPath("IsEnabled");
                    //biden.Source = iimenuItems[index];
                    //BindingOperations.SetBinding(mii, MenuItem.IsEnabledProperty, biden);
                    mi.Items.Add(mii);
                }
            }
            return mi;
        }

        //static void ggg_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    var item = sender as MenuItem;
        //    if (item == null) return;
        //    foreach (var t in item.Items)
        //    {
        //        var ite = t as MenuItem;
        //        if (ite == null) continue;
        //        if (ite.Items.Count > 0)
        //        {
        //            CheckChildMenuItemVisiToUpdateVis(ite);
        //        }
        //    }

        //    bool bolvisi = false;
        //    foreach (var t in item.Items)
        //    {
        //        var ite = t as MenuItem;
        //        if (ite == null) continue;
        //        if (ite.Visibility == Visibility.Visible)
        //        {
        //            bolvisi = true;
        //            break;
        //        }
        //    }
        //    if (bolvisi) item.Visibility = Visibility.Visible;
        //    else item.Visibility = Visibility.Collapsed;
        //}

        //static void CheckChildMenuItemVisiToUpdateVis(MenuItem item)
        //{
        //    if (item == null) return;
        //    if (item.Items.Count == 0) return;
        //    foreach (var t in item.Items)
        //    {
        //        var ite = t as MenuItem;
        //        if (ite == null) continue;
        //        if (ite.Items.Count > 0)
        //        {
        //            CheckChildMenuItemVisiToUpdateVis(ite);
        //        }
        //    }

        //    bool bolvisi = false;
        //    foreach (var t in item.Items)
        //    {
        //        var ite = t as MenuItem;
        //        if (ite == null) continue;
        //        if (ite.Visibility == Visibility.Visible)
        //        {
        //            bolvisi = true;
        //            break;
        //        }
        //    }
        //    if (bolvisi) item.Visibility = Visibility.Visible;
        //    else item.Visibility = Visibility.Collapsed;
        //}


    };

   
}

