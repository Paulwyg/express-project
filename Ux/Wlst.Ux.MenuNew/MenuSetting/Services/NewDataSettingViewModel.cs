using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.ComponentHold;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Models;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.MenuNew.MenuSetting.Services;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.MenuNew.MenuSetting.Services
{
    [Export(typeof(IINewDataSetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewDataSettingViewModel : ObservableObject, IINewDataSetting
    {
       
        public void NavOnLoad(params object[] parsObjects)
        {
            
            TreeItems = new ObservableCollection<TreeItemViewMode>();
            LoadBase();
            LoadNow();
        }


        private void LoadBase()
        {
            TreeItems.Clear();
            var ssss = Convert.ToInt32(SqlLiteHelper.ExecuteQuery(
                "SELECT COUNT(*) as count FROM sqlite_master WHERE type='table' and name= 'menu_instances_relation'").
                                           Tables[0].Rows[0][0].ToString().Trim());


            if (ssss < 1)
            {
                SqlLiteHelper.ExecuteQuery(
                    "CREATE TABLE 'menu_instances_relation' ('father_id' integer,'id' integer,'sort_index' integer,'name' text,'" +
                    "instances_id' integer)");
            }


            try
            {

                DataSet ds = SqlLiteHelper.ExecuteQuery("select * from menu_instances_relation", null);
                if (ds == null) return;
                int mCount = ds.Tables[0].Rows.Count;
                for (int i = 0; i < mCount; i++)
                {
                    try
                    {
                        // (id integer NOT NULL,tag text,name text NOT NULL,tooltips text)
                        int fatherId = Convert.ToInt32(ds.Tables[0].Rows[i]["father_id"].ToString().Trim());
                        int id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString().Trim());

                        if ((id >= MenuIdControlAssign.MenuFileGroupIdMin &&
                             id <= MenuIdControlAssign.MenuFileGroupIdMax) ||
                            (id >= MenuIdControlAssign.MenuIdMin &&
                             id <= MenuIdControlAssign.MenuIdMax))
                        {
                            int sortIndex = Convert.ToInt32(ds.Tables[0].Rows[i]["sort_index"].ToString().Trim());
                            string name = ds.Tables[0].Rows[i]["name"].ToString().Trim();
                            int instancesId = Convert.ToInt32(ds.Tables[0].Rows[i]["instances_id"].ToString().Trim());

                         if (instancesId == 2920022) //moban weizhi 

                            {
                                if (fatherId ==0)
                                {
                    
                                    if ( !DicType.ContainsKey(id))
                                    {
                                        DicType.Add(id, name);
                                    }
                                    else
                                    {
                                        DicType[id] = name;
                                    }
                                    

                                }
                                else 
                                {
                                    if (DicType.ContainsKey(fatherId))
                                    {
                                        TreeItems.Add(new TreeItemViewMode()
                                                          {
                                                              Id = id,
                                                              Name = name,
                                                              IsChecked = false,
                                                              Description = DicType[fatherId],
                                                              FatherId = fatherId
                                                          });
                                    }

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogError(ex.ToString());
                    }
                }


            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    "Class MenuInstanceRelationHoldingExtend Function loadItem from SQLlite table menu_instances_relation  Occer an Error:" +
                    ex.ToString());
            }
            var args = new PublishEventArgs()
            {
                EventId =
                    EventIdAssign.MenuInstanceRelationLoadUpdate,
                EventType = PublishEventType.Core
            };

            EventPublish.PublishEvent(args);


            //var lst = new List<MenuInstancesRelation>();
            //foreach (var f in TreeItems)
            //    lst.Add(new MenuInstancesRelation()
            //                {
            //                    FatherId = f.FatherId,
            //                    Id = f.Id,
            //                    InstancesId = 2920022,
            //                    Name = f.Name,
            //                    SortIndex =0,
            //                });
            //ServerInstanceRelation.UpdateMenuInstanceRelation(2920022, "MainMenuMa", lst);
        }

        private void LoadNow()
        {
            var ssss = Convert.ToInt32(SqlLiteHelper.ExecuteQuery(
                "SELECT COUNT(*) as count FROM sqlite_master WHERE type='table' and name= 'menu_instances_relation'").
                                           Tables[0].Rows[0][0].ToString().Trim());


            if (ssss < 1)
            {
                SqlLiteHelper.ExecuteQuery(
                    "CREATE TABLE 'menu_instances_relation' ('father_id' integer,'id' integer,'sort_index' integer,'name' text,'" +
                    "instances_id' integer)");
            }


            try
            {
                DataSet ds = SqlLiteHelper.ExecuteQuery("select * from menu_instances_relation", null);
                if (ds == null) return;
                int mCount = ds.Tables[0].Rows.Count;
                for (int i = 0; i < mCount; i++)
                {
                    try
                    {
                        int fatherId = Convert.ToInt32(ds.Tables[0].Rows[i]["father_id"].ToString().Trim());
                        int id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString().Trim());

                        if ((id >= MenuIdControlAssign.MenuFileGroupIdMin &&
                             id <= MenuIdControlAssign.MenuFileGroupIdMax) ||
                            (id >= MenuIdControlAssign.MenuIdMin &&
                             id <= MenuIdControlAssign.MenuIdMax))
                        {
                            int sortIndex = Convert.ToInt32(ds.Tables[0].Rows[i]["sort_index"].ToString().Trim());
                            string name = ds.Tables[0].Rows[i]["name"].ToString().Trim();
                            int instancesId = Convert.ToInt32(ds.Tables[0].Rows[i]["instances_id"].ToString().Trim());

                            if (instancesId == 2920002)
                            {
                                foreach (var t in TreeItems)
                                {
                                    if (t.Id == id && t.FatherId == fatherId)
                                    {
                                        t.IsChecked = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogError(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    "Class MenuInstanceRelationHoldingExtend Function loadItem from SQLlite table menu_instances_relation  Occer an Error:" +
                    ex.ToString());
            }
            var args = new PublishEventArgs()
            {
                EventId =
                    EventIdAssign.MenuInstanceRelationLoadUpdate,
                EventType = PublishEventType.Core
            };

            EventPublish.PublishEvent(args);

         
        }

        #region Save

        private DateTime _dtSaveEx;
        private RelayCommand _relaySaveCommand;
        public ICommand CmdSave
        {
            get { return _relaySaveCommand ?? (_relaySaveCommand = new RelayCommand(SaveEx, CanSaveEx, true)); }
        }


        private void SaveEx()
        {
            _dtSaveEx = DateTime.Now;

            var lst = new List<MenuInstancesRelation>();
            var index = new Dictionary<int, int>();
            var lstmain = new List<int>();
            index.Add(0,0);

            foreach (var t in TreeItems)
            {
                if (t.IsChecked)
                {
                    if (!lstmain.Contains(t.FatherId) && DicType.ContainsKey(t.FatherId)) 
                    {
                        lstmain.Add(t.FatherId);
                        lst.Add(new MenuInstancesRelation()
                                    {
                                        FatherId = 0,
                                        Id = t.FatherId,
                                        InstancesId = 2920002,
                                        Name = DicType[t.FatherId],
                                        SortIndex = index[0]
                                    });
                        index[0]++;
                    }

                    if (index.ContainsKey(t.FatherId)) index[t.FatherId]++;
                    else index.Add(t.FatherId, 0);
                   
                    lst.Add(new MenuInstancesRelation()
                    {
                        FatherId = t.FatherId,
                        Id = t.Id,
                        InstancesId = 2920002,
                        Name = t.Name,
                        SortIndex = index[t.FatherId]
                    });
                    
                }
            }

           ServerInstanceRelation.UpdateMenuInstanceRelation(2920002, "MainMenu" ,lst);

            
        }




        private bool CanSaveEx()
        {
            return DateTime.Now.Ticks - _dtSaveEx.Ticks > 30000000;
        }
       
        #endregion

        #region

        public Dictionary<int, string> DicType = new Dictionary<int, string>();

        private ObservableCollection<TreeItemViewMode> _treeitems;

        public ObservableCollection<TreeItemViewMode> TreeItems
        {
            get { return _treeitems; }
            set
            {
                if (value != _treeitems)
                {
                    _treeitems = value;
                    this.RaisePropertyChanged(() => this.TreeItems);
                }
            }
        }

        public class TreeItemViewMode : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private int _id;
            public int Id
            {
                get { return _id; }
                set
                {
                    if (_id != value)
                    {
                        _id = value;
                        this.RaisePropertyChanged(() => this.Id);
                    }
                }
            }

            private int _fatherid;
            public int FatherId
            {
                get { return _fatherid; }
                set
                {
                    if (_fatherid != value)
                    {
                        _fatherid = value;
                        this.RaisePropertyChanged(() => this.FatherId);
                    }
                }
            }

            private string _description;
            public string Description
            {
                get { return _description; }
                set
                {
                    if (value != _description)
                    {
                        _description = value;
                        this.RaisePropertyChanged(() => this.Description);
                    }
                }
            }

            private string _name;
            public string Name
            {
                get { return _name; }
                set
                {
                    if (value != _name)
                    {
                        _name = value;
                        this.RaisePropertyChanged(() => this.Name);
                    }
                }
            }

            private bool _ischecked;
            public bool IsChecked
            {
                get { return _ischecked; }
                set
                {
                    if (value != _ischecked)
                    {
                        _ischecked = value;
                        this.RaisePropertyChanged(() => this.IsChecked);
                    }
                }
            }

            private ObservableCollection<TreeItemViewMode> _childtreeitems;
            public ObservableCollection<TreeItemViewMode> ChildTreeItems
            {
                get { return _childtreeitems; }
                set
                {
                    if (value != _childtreeitems)
                    {
                        _childtreeitems = value;
                        this.RaisePropertyChanged(() => this.ChildTreeItems);
                    }
                }
            }
        }

        #endregion
    }
    
}
