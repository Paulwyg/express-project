using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using Wlst.Cr.Core.Modularity;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.CoreModuelConfig.Services;

namespace Wlst.Ux.CoreModuelConfig.ViewModel
{


    [Export(typeof(IICoreMoudleConfig))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CoreMoudleLoadConfigViewModel : IICoreMoudleConfig, INotifyPropertyChanged
    {
        // private  _moduleLoadManager;

        #region IITab
      
        public int  Index
        {
            get { return 0; }
        }

        /// <summary>
        /// 当显示在主界面的tab页面时 显示的title
        /// </summary>
        public string Title
        {
            get { return "模块加载"; }
        }

        /// <summary>
        /// 当显示在主界面tab时是否允许用户关闭  地图不运行关闭
        /// </summary>
        public bool CanClose
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion


        private ObservableCollection<ModuleItemInfoModel> mObservableCollection;

        public ObservableCollection<ModuleItemInfoModel> ItemsModules
        {
            get
            {
                if (mObservableCollection == null)
                    mObservableCollection = new ObservableCollection<ModuleItemInfoModel>();
                return mObservableCollection;
            }
        }

        public CoreMoudleLoadConfigViewModel()
        {
            //_moduleLoadManager = new ModuleLoadManager();
            //  CETC50_Core .Services .ModuleManage .ModuleMangeInatance 
            ModuleManage.ModuleMangeInatance.OnModuleLoadedStateChanged += OnModuleLoadItemStateChange;


            //this.NavOnLoad();
            _canReflesh = true;
        }

        #region CmdAddNew

        private ICommand iCmdAddNew;

        public ICommand CmdAddNew
        {
            get
            {
                if (iCmdAddNew == null) iCmdAddNew = new RelayCommand(ExAddNew);
                return iCmdAddNew;
            }
        }


        private void ExAddNew()
        {
            // Configure open file dialog box
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".dll"; // Default file extension
            dlg.Filter = "Modules (.dll)|*.dll|(.mdl)|*.mdl"; // Filter files by extension

            // Show open file dialog box
            var result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == DialogResult.OK)
            {
                // Open document
                string filename = dlg.FileName;
                var assembly = Assembly.LoadFile(filename);
                var assemblyInfo = new AssemblyInfo(assembly);
                if (!assemblyInfo.CompanyInfo.Company.Contains("CETC"))
                {
                    MessageBox.Show("Company is not CETC50,Error", "Company Error");
                }
                else
                {
                    string moduleName = filename.Split('\\')[filename.Split('\\').Length - 1];


                    foreach (var t in ItemsModules)
                    {
                        if (t.Name.Equals(moduleName))
                        {
                            MessageBox.Show("Already add a same name module!!!", "Name already has");
                            return;
                        }
                    }



                    int id = ModuleManage.ModuleMangeInatance.AddModuelItem(filename);
                    if (id > 0)
                    {
                        foreach (var t in ModuleManage.ModuleMangeInatance.ModuleItems)
                        {
                            if (t.AssemblyConfigInfo != null && t.AssemblyConfigInfo.ModuleId == id)
                            {
                                this.AddItem(t);
                                break;
                            }
                        }
                    }

                    //this.addItem(filename, false);
                }
            }
        }

        #endregion

        #region CmdSave

        private ICommand iCmdSave;

        public ICommand CmdSave
        {
            get
            {
                if (iCmdSave == null) iCmdSave = new RelayCommand(ExCmdSave);
                return iCmdSave;
            }
        }


        private void ExCmdSave()
        {
            ModuleManage.ModuleMangeInatance.SaveConfig();
        }

        #endregion

        #region CmdFlesh

        private bool _canReflesh;
        private ICommand iCmdFlesh;

        public ICommand CmdFlesh
        {
            get
            {
                if (iCmdFlesh == null) iCmdFlesh = new RelayCommand(ExCmdFlesh, CanReFlesh, false);
                return iCmdFlesh;
            }
        }

        bool CanReFlesh()
        {
            return _canReflesh;
        }


        private void ExCmdFlesh()
        {
            // _moduleLoadManager.SaveConfig();
            this.NavOnLoad();
            _canReflesh = false;
        }

        #endregion


        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        public void RiseProperyChange(string properyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(properyName));

        }

        #endregion

        public void NavOnLoad(params object[] parsObjects)
        {
            this.ItemsModules.Clear();
            foreach (var t in ModuleManage.ModuleMangeInatance.ModuleItems)
            {
                this.AddItem(t);
            }
        }

        private void AddItem(IIModuleItemInfo t)
        {
            if (ItemsModules.Any(f => f.ModuleId == t.AssemblyConfigInfo.ModuleId))
            {
                return;
            }

            ModuleItemInfoModel item = new ModuleItemInfoModel();
            item.CatalogPath = "";
            item.Company = t.AssemblyInfo.CompanyInfo.Company;
            item.IsLoad = t.IsLoaded;
            item.ModuleId = t.AssemblyConfigInfo.ModuleId;
            item.ModuleType = t.AssemblyConfigInfo.AutoLoad.ToString();
            item.Name = t.AssemblyConfigInfo.ModuleName;
            item.ShouldLoadOnFirstRun = t.IsAutoLoadAfterLogin;
            item.IsLoadBySystemAuto =
                false; //t.AssemblyConfigInfo.AutoLoad == ModuleLoadSqu.AutoLoadBeforeLogin || t.AssemblyConfigInfo.AutoLoad == ModuleLoadSqu.AutoLoadAfterLogin ;

            item.OnCmdLoad += OnItemCmdLoad;
            item.OnCmdUnLoad += OnItemCmdUnLoad;
            item.OnCmdDelete += OnItemCmdDelete;
            item.OnShouldLoadOnFirstRunChange += OnItemRunOnLoadChange;
            this.ItemsModules.Add(item);
        }

        private void OnItemCmdLoad(object sender, EventArgs args)
        {
            var item = sender as ModuleItemInfoModel;
            if (item == null) return;
            ModuleManage.ModuleMangeInatance.LoadModuleItem(item.ModuleId);
        }

        private void OnItemCmdUnLoad(object sender, EventArgs args)
        {
            var item = sender as ModuleItemInfoModel;
            if (item == null) return;
            ModuleManage.ModuleMangeInatance.UnLoadModuleItem(item.ModuleId);
        }

        private void OnItemCmdDelete(object sender, EventArgs args)
        {
            var item = sender as ModuleItemInfoModel;
            if (item == null) return;
            item.OnCmdLoad -= OnItemCmdLoad;
            item.OnCmdUnLoad -= OnItemCmdUnLoad;
            item.OnCmdDelete -= OnItemCmdDelete;
            item.OnShouldLoadOnFirstRunChange -= OnItemRunOnLoadChange;
            var f = ModuleManage.ModuleMangeInatance.RemoveItem(item.ModuleId);
            if (f)
                foreach (var t in this.ItemsModules.Where(t => t.ModuleId == item.ModuleId))
                {
                    this.ItemsModules.Remove(t);
                    break;
                }
        }

        private void OnItemRunOnLoadChange(object sender, EventArgs args)
        {
            var item = sender as ModuleItemInfoModel;
            if (item == null) return;
            ModuleManage.ModuleMangeInatance.SetModuleItemAutoLoad(item.ModuleId, item.ShouldLoadOnFirstRun);
        }

        private void OnModuleLoadItemStateChange(object sender, EventArgs args)
        {

            var send = sender as ModuleItemInfo;
            if (send == null) return;
            if (send.AssemblyConfigInfo == null) return;
            foreach (var t in this.ItemsModules)
            {
                if (t.ModuleId == send.AssemblyConfigInfo.ModuleId)
                {
                    t.IsLoad = send.IsLoaded;
                    return;
                }
            }
        }
    }
}