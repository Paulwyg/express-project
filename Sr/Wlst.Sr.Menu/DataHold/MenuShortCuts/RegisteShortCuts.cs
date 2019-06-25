using System;
using System.Collections.Generic;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreOne.CoreInterface;

namespace Wlst.Sr.Menu.DataHold.MenuShortCuts
{

    /// <summary>
    /// 系统菜单有效快捷键保存位置 
    /// 1、注册快捷键
    /// 2、处理快捷键事件
    /// </summary>
    public class RegisteShortCuts
    {
        private class ClsMenuItem
        {
            public readonly IIMenuItem MenuItem;
            public readonly ModifierKeys ModifierKey;
            public readonly Key Key;

            /// <summary>
            /// 快捷键保存Item
            /// </summary>
            /// <param name="mi">菜单实例</param>
            /// <param name="modifierKey">指定修改键集</param>
            /// <param name="key">指定键盘上可能的键值</param>
            public ClsMenuItem(IIMenuItem mi, ModifierKeys modifierKey, Key key)
            {
                this.MenuItem = mi;
                this.ModifierKey = modifierKey;
                this.Key = key;
            }
        };

        private static List<ClsMenuItem> lstMenuHasShortCuts = new List<ClsMenuItem>();

        /// <summary>
        /// 
        /// </summary>
        public static RegisteShortCuts MySelf;

        /// <summary>
        /// 
        /// </summary>
        public RegisteShortCuts()
        {
            if (MySelf == null)
            {
                MySelf = this;
            }
        }

        public static void ClearLst()
        {
            lstMenuHasShortCuts.Clear();
        }


        /// <summary>
        /// 注册具有快捷键功能的MenuItem 注册后立即有效
        /// 其中MenuItem.InputGestureText属性携带快捷键信息
        /// 快捷键信息格式为  ModifierKeys+Key 或者 Key
        /// </summary>
        /// <param name="menuItem"></param>
        /// <param name="strWantedPressKys"> </param>
        public static void AddMenuItem(IIMenuItem menuItem, string strWantedPressKys)
        {
            //   ModifierKeys
            //// 摘要:
            ////     没有按下任何修饰符。
            //None = 0,
            ////
            //// 摘要:
            ////     The ALT key.
            //Alt = 1,
            ////
            //// 摘要:
            ////     The CTRL key.
            //Control = 2,
            ////
            //// 摘要:
            ////     Shift 键。
            //Shift = 4,
            ////
            //// 摘要:
            ////     Windows 徽标键。
            //Windows = 8,
            if (string.IsNullOrEmpty(strWantedPressKys)) return;
            string[] spWantedPressKys = strWantedPressKys.Split('+');
            for (var i = 0; i < spWantedPressKys.Length; i++)
            {
                spWantedPressKys[i] = spWantedPressKys[i].Trim();
            }

            switch (spWantedPressKys.Length)
            {
                case 1:
                    try
                    {
                        var key = Enum.Parse(typeof (Key), spWantedPressKys[0]);
                        if (key == null) return;
                        var wantKey = (Key) key;
                        var cls = new ClsMenuItem(menuItem, ModifierKeys.None, wantKey);
                        lstMenuHasShortCuts.Add(cls);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                        return;
                    }
                    break;
                case 2: // Intentionally ignore modifier keys
                    try
                    {
                        var modifierKeys = ModifierKeys.None;
                        //Control  Ctrl
                        switch (spWantedPressKys[0].ToUpper())
                        {
                            case "CONTROL":
                                modifierKeys = ModifierKeys.Control;
                                break;
                            case "CTRL":
                                modifierKeys = ModifierKeys.Control;
                                break;
                            case "ALT":
                                modifierKeys = ModifierKeys.Alt;
                                break;
                            case "SHIFT":
                                modifierKeys = ModifierKeys.Shift;
                                break;
                        }

                        var key = Enum.Parse(typeof (Key), spWantedPressKys[1]);
                        if (key == null) return;
                        var wantKey = (Key) key;

                        var cls = new ClsMenuItem(menuItem, modifierKeys, wantKey);
                        lstMenuHasShortCuts.Add(cls);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                        return;
                    }
                    break;
            }
        }



        /// <summary>
        /// 快捷键事件处理函数
        /// </summary>
        /// <param name="e"></param>
        public void OnKeyDown(KeyEventArgs e)
        {
            bool find = false;
            foreach (var clsMenuItem in lstMenuHasShortCuts)
            {
                if (clsMenuItem.ModifierKey == Keyboard.Modifiers && clsMenuItem.Key == e.Key)
                {
                    if (clsMenuItem.MenuItem.Command != null)
                    {
                        find = true;
                        clsMenuItem.MenuItem.Command.Execute(null);
                        break;
                    }
                }

            }
            if (find == false)
            {
                if (e.Key == Key.M)
                {
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(1101101,
                                                                                                Cr.Core.CoreServices.
                                                                                                    DocumentRegionName.
                                                                                                    DocumentRegion, 0);
                        Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(1101102,
                                                                                                Cr.Core.CoreServices.
                                                                                                    DocumentRegionName.
                                                                                                    DocumentRegion, 0);
                    }
                }
            }

        }


    }
}
