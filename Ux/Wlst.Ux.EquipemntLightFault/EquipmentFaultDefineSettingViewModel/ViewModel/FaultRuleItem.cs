using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreOne.Models;
using Wlst.client;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.ViewModel
{
    public class FaultRuleItem : ObservableObject //, IIEquipmentFaultType
    {

        public FaultRuleItem(Wlst.client.FaultTypes.FaultSettingRuleOne rule,
                             ObservableCollection<TmlFaultTypeViewModel> allfaults)
        {
            RuleId = rule.RuleId;
            Rule_name = rule.RuleName;
            target_equipment = rule.TargetEquipment;
            Op = rule.Op;
            Op_extend = rule.OpExtend;
            OpTimeSet = rule.AlarmTimeSet + 1;


            if (OpTimeSet == 2 || OpTimeSet == 3)
            {
                DtStart = rule.AlarmTimeStart + "";
                DtEnd = rule.AlarmTimeEnd + "";

            }
            if (OpTimeSet == 1)
            {
                DtStart = gettostr(rule.AlarmTimeStart);
                DtEnd = gettostr(rule.AlarmTimeEnd);
            }

            if (OpTimeSet == 4 || OpTimeSet == 5)
            {
                DtStart = rule.AlarmTimeStart + "";
                DtEnd = rule.AlarmTimeEnd + "";

            }

            for (int i = 0; i < 5; i++)
            {
                if (rule.ProperyContainKey.Count > i)
                {
                    ProperyContainKey[i].Name = rule.ProperyContainKey[i];
                }
                else
                {
                    ProperyContainKey[i].Name = "";

                }
            }

            ItemsAddto.Clear();
            ItemsRemoveto.Clear();

            var ntr = (from t in allfaults where t.IsEnable select t).ToList();
            foreach (var f in ntr)
            {
                ItemsRemoveto.Add(new NameIntBool()
                                      {
                                          IsSelected = rule.FaultsRemoveOff.Contains(f.FaultId),
                                          Name =
                                              string.IsNullOrEmpty(f.FaultNameByDefine)
                                                  ? f.FaultName
                                                  : f.FaultNameByDefine,
                                          Value = f.FaultId
                                      });
            }
            UpdateProCkey();
            UpdateRemoveOffStr();

        }

        public FaultRuleItem(int id, ObservableCollection<TmlFaultTypeViewModel> allfaults)
        {
            RuleId = id;
            Rule_name = "屏蔽报警";
            target_equipment = 1;
            Op = 1;
            Op_extend = 1;
            OpTimeSet = 1;

            //for (int i = 0; i < 5; i++)
            //{
            //    if (rule.ProperyContainKey.Count > i)
            //    {
            //        ProperyContainKey[i].Name = rule.ProperyContainKey[i];
            //    }
            //    else
            //    {
            //        ProperyContainKey[i].Name = "";

            //    }
            //}

            ItemsAddto.Clear();
            ItemsRemoveto.Clear();

            var ntr = (from t in allfaults where t.IsEnable && (t.FaultId < 25 || t.FaultId > 80) select t).ToList();
            foreach (var f in ntr)
            {
                ItemsRemoveto.Add(new NameIntBool()
                                      {
                                          IsSelected = false,
                                          Name =
                                              string.IsNullOrEmpty(f.FaultNameByDefine)
                                                  ? f.FaultName
                                                  : f.FaultNameByDefine,
                                          Value = f.FaultId
                                      });
            }

        }

        public static bool IsNumberic(string _string)
        {
            if (string.IsNullOrEmpty(_string))
                return false;
            foreach (char c in _string)
            {
                if (!char.IsDigit(c))
                    //if(c<'0' c="">'9')//最好的方法,在下面测试数据中再加一个0，然后这种方法效率会搞10毫秒左右
                    return false;
            }
            return true;
        }

        public Wlst.client.FaultTypes.FaultSettingRuleOne BackTo()
        {
            var d1 = 0;
            var d2 = 0;

            if (OpTimeSet == 2)
            {
                if (!IsNumberic(DtStart) || !IsNumberic(DtEnd)) return null;
                d1 = Convert.ToInt32(DtStart.Trim());//gettostr(DtStart);
                d2 = Convert.ToInt32(DtEnd.Trim());
                //d1 = DtS2;
                //StrTip = "开灯后设置为分钟数值";

            }
            else if (OpTimeSet == 3)
            {
                if (!IsNumberic(DtStart) || !IsNumberic(DtEnd)) return null;
                d1 = Convert.ToInt32(DtStart.Trim());//gettostr(DtStart);
                d2 = Convert.ToInt32(DtEnd.Trim());
                //d1 = DtS2;
                //StrTip = "关灯后设置为分钟数值";
            }
            else if (OpTimeSet == 1)
            {
                d1 = getStrInt(DtStart);
                d2 = getStrInt(DtEnd);
                //StrTip = "全天时间 为几点几分";
            }
            else if (OpTimeSet == 4)
            {
                //lvf 负数 判断
                if (DtStart.StartsWith("-")) //先判断是否以-开头
                {
                    string subdtStartStr = DtStart.Substring(1); //取-剩下的字符串
                    if (!IsNumberic(subdtStartStr)) return null;
                }
                if (DtEnd.StartsWith("-")) //先判断是否以-开头
                {
                    string subdtEndStr = DtEnd.Substring(1); //取-剩下的字符串
                    if (!IsNumberic(subdtEndStr)) return null;
                }
                //if (!IsNumberic(DtStart) || !IsNumberic(DtEnd)) return null;
                d1 = Convert.ToInt32(DtStart.Trim()); //gettostr(DtStart);
                d2 = Convert.ToInt32(DtEnd.Trim());
                //StrTip = "日出-日落偏移量 为分钟数值";
            }
            else if (OpTimeSet == 5)
            {
                if (DtStart.StartsWith("-")) //先判断是否以-开头
                {
                    string subdtStartStr = DtStart.Substring(1); //取-剩下的字符串
                    if (!IsNumberic(subdtStartStr)) return null;
                }
                if (DtEnd.StartsWith("-")) //先判断是否以-开头
                {
                    string subdtEndStr = DtEnd.Substring(1); //取-剩下的字符串
                    if (!IsNumberic(subdtEndStr)) return null;
                }
                //if (!IsNumberic(DtStart) || !IsNumberic(DtEnd)) return null;
                d1 = Convert.ToInt32(DtStart.Trim());//gettostr(DtStart);
                d2 = Convert.ToInt32(DtEnd.Trim());
                //StrTip = "日落-日出偏移量 为分钟数值";
            }

            //int optimeset = OpTimeSet;
            //if (OpTimeSet < 4) optimeset = OpTimeSet - 1;
            var rtn = new Wlst.client.FaultTypes.FaultSettingRuleOne()
                          {
                              FaultsAddTo = new List<int>(),
                              AlarmTimeEnd = d2 ,
                              AlarmTimeSet = OpTimeSet -1,
                              AlarmTimeStart = d1 ,
                              FaultsRemoveOff = (from t in ItemsRemoveto where t.IsSelected select t.Value).ToList(),
                              ProperyContainKey =
                                  (from t in ProperyContainKey where string.IsNullOrEmpty(t.Name) == false select t.Name)
                                  .ToList(),
                           
                                                        Op = Op,
                                                        OpExtend = Op_extend,
                                                   
                              RuleId = RuleId,
                              RuleName = Rule_name,
                              TargetEquipment = 1
                          };
            return rtn;
        }

        private int _faultId;

        public int RuleId
        {
            get { return _faultId; }
            set
            {
                if (_faultId != value)
                {
                    _faultId = value;
                    this.RaisePropertyChanged(() => this.RuleId);


                }
            }
        }


        private string rule_name;

        public string Rule_name
        {
            get { return rule_name; }
            set
            {
                if (rule_name != value)
                {
                    rule_name = value;
                    this.RaisePropertyChanged(() => this.Rule_name);


                }
            }
        }


        private int target_equipment;

        /// <summary>
        ///  1、终端设备，2、单灯设备，3、防盗设备，4、漏电设备，5、光控设备，6、节能设备
        /// </summary>
        public int Target_equipment
        {
            get { return target_equipment; }
            set
            {
                if (target_equipment != value)
                {
                    target_equipment = value;
                    this.RaisePropertyChanged(() => this.Target_equipment);


                }
            }
        }


        private int _OpId;

        /// <summary>
        ///  设置的选项:1、终端名称(包含字样)，2、终端识别码(包含字样)，3、终端设备类型 3005类，4、终端备注(包含字样)，5、终端设备类型 路灯、亮化，6、回路名称(包含字样)
        /// </summary>
        public int Op
        {
            get { return _OpId; }
            set
            {
                if (_OpId != value)
                {
                    _OpId = value;
                    this.RaisePropertyChanged(() => this.Op);
                    //if ((value == 3 || value == 5)&& Op_extend !=4 ) IsCheckedCon = false;
                    //else IsCheckedCon = true;

                    UpdateIsCheckedCon(value, Op_extend);
                    if (Op == 1) OpStr = "终端名称";
                    else if (Op == 2) OpStr = "终端识别码";
                    else if (Op == 3) OpStr = "终端类型";
                    else if (Op == 4) OpStr = "终端备注";
                    else if (Op == 5) OpStr = "终端设备类型";
                    else if (Op == 6) OpStr = "回路名称";
                    else if (Op == 11) OpStr = "全部终端";
                    else if (Op == 12) OpStr = "全部回路";
                    else OpStr = "未知";

                    IsCheckedLoop = (Op == 6 || Op == 12);
                    IsCheckedRtu = !IsCheckedLoop;
                }
            }
        }


       private bool rule_IIsCheckedLoopsCheckedConname;

        public bool IsCheckedLoop
        {
            get { return rule_IIsCheckedLoopsCheckedConname; }
            set
            {
                if (rule_IIsCheckedLoopsCheckedConname != value)
                {
                    rule_IIsCheckedLoopsCheckedConname = value;
                    this.RaisePropertyChanged(() => this.IsCheckedLoop);


                }
            }
        }

        private bool _isCheckedRtu;

        public bool IsCheckedRtu
        {
            get { return _isCheckedRtu; }
            set
            {
                if (_isCheckedRtu != value)
                {
                    _isCheckedRtu = value;
                    this.RaisePropertyChanged(() => this.IsCheckedRtu);


                }
            }
        }

        private bool rule_IsCheckedConname;

        public bool IsCheckedCon
        {
            get { return rule_IsCheckedConname; }
            set
            {
                if (rule_IsCheckedConname != value)
                {
                    rule_IsCheckedConname = value;
                    this.RaisePropertyChanged(() => this.IsCheckedCon);


                }
            }
        }
            private bool rule_IsEnabledRb;

            public bool IsEnabledRb
        {
            get { return rule_IsEnabledRb; }
            set
            {
                if (rule_IsEnabledRb != value)
                {
                    rule_IsEnabledRb = value;
                    this.RaisePropertyChanged(() => this.IsEnabledRb);


                }
            }
        }

            private int rule_OpTimeSet;

        public int OpTimeSet
        {
            get { return rule_OpTimeSet; }
            set
            {
                if (rule_OpTimeSet != value)
                {
                    rule_OpTimeSet = value;
                    this.RaisePropertyChanged(() => this.OpTimeSet);

                    if (rule_OpTimeSet == 1)
                    {
                        StrTip = "结束时间:";
                        StrTitle = "起始时间：";
                    }
                    else if (rule_OpTimeSet == 2)
                    {
                        StrTip = "结束分钟:";
                        StrTitle = "起始分钟：";
                    }
                    else if (rule_OpTimeSet == 3)
                    {
                        StrTip = "结束分钟:";
                        StrTitle = "起始分钟：";
                    }
                    else if (rule_OpTimeSet == 4)
                    {
                        StrTip = "日落偏移:";
                        StrTitle = "日出偏移：";
                    }
                    else if (rule_OpTimeSet == 5)
                    {
                        StrTip = "日出偏移:";
                        StrTitle = "日落偏移：";
                    }
                }
            }
        }

        private string _rule_DtStart;

                 public string DtStart
                 {
                     get { return _rule_DtStart; }
                     set
                     {
                         if (_rule_DtStart != value)
                         {
                             _rule_DtStart = value;
                             this.RaisePropertyChanged(() => this.DtStart);


                         }
                     }
                 }

                 private string _rule_DtEnd;

                 public string DtEnd
                 {
                     get { return _rule_DtEnd; }
                     set
                     {
                         if (_rule_DtEnd != value)
                         {
                             _rule_DtEnd = value;
                             this.RaisePropertyChanged(() => this.DtEnd);


                         }
                     }
                 }


                 private string _strTip;

                 public string StrTip
                 {
                     get { return _strTip; }
                     set
                     {
                         if (_strTip != value)
                         {
                             _strTip = value;
                             this.RaisePropertyChanged(() => this.StrTip);


                         }
                     }
                 }

                 private string _strTitle;

                 public string StrTitle
                 {
                     get { return _strTitle; }
                     set
                     {
                         if (_strTitle != value)
                         {
                             _strTitle = value;
                             this.RaisePropertyChanged(() => this.StrTitle);


                         }
                     }
                 }



            string getstr(string data)
            {
                var xr = getStrInt(data);
                return (xr/60) + ":" + (xr%60);

            }

            int getStrInt(string data)
            {
                if (string.IsNullOrEmpty(data)) return 0;
                int xr = 0;
                if (data.Contains(":") || data.Contains("：") || data.Contains(".") || data.Contains("。") ||
                    data.Contains("，") || data.Contains(","))
                {
                    var sps = data.Split(':', '：', ',', '.', '，', '。');
                    int x1 = 0;
                    int x2 = 0;

                    if (Int32.TryParse(sps[0], out x1)) xr = x1*60;
                    if (Int32.TryParse(sps[1], out x2)) xr += x2;
                }
                else
                {
                    int xr1 = 0;
                    if (Int32.TryParse(data, out xr1)) xr = xr1*60;
                }
                return xr;
                //return (xr / 60) + ":" + (xr % 60);

            }
            string gettostr(int xr)
            {
                return (xr/60) + ":" + (xr%60);
            }


        private string rule_DtS4;

        public string DtS4
        {
            get { return rule_DtS4; }
            set
            {
                if (rule_DtS4 != value)
                {
                    rule_DtS4 = getstr(value);
                    this.RaisePropertyChanged(() => this.DtS4);


                }
            }
        }

        private string _OpIdstr;

        /// <summary>
        ///  设置的选项:1、终端名称(包含字样)，2、终端识别码(包含字样)，3、终端设备类型 3005类，4、终端备注(包含字样)，5、终端设备类型 路灯、亮化，6、回路名称(包含字样)
        /// </summary>
        public string OpStr
        {
            get { return _OpIdstr; }
            set
            {
                if (_OpIdstr != value)
                {
                    _OpIdstr = value;
                    this.RaisePropertyChanged(() => this.OpStr);


                }
            }
        }


        private int _Op_extend;

        /// <summary>
        ///  Op3设备类型：1、3005,2、3090,3、3006，4、其他设备类型  填入 propery_contain_key
        /// Op5设备类型  1、路灯，2、亮化，3、广告，4、其他 若有填入propery_contain_key 无则不填
        /// </summary>
        public int Op_extend
        {
            get { return _Op_extend; }
            set
            {
                if (_Op_extend != value)
                {
                    _Op_extend = value;
                    this.RaisePropertyChanged(() => this.Op_extend);

                    UpdateIsCheckedCon(Op , value );
                    //if ((value == 3 || value == 5) && Op_extend != 4)
                    //{
                    //    IsCheckedCon = false;
                    //    if (value ==3 )
                    //    {
                    //        if (Op_extend == 1) Ex_OpStr = "3005型设备";
                    //        else if (Op_extend == 2) Ex_OpStr = "3090型设备";
                    //        else if (Op_extend == 3) Ex_OpStr = "3060型设备";
                    //        else Ex_OpStr = "其他";
                    //    }
                    //    if (value == 4)
                    //    {
                    //        if (Op_extend == 1) Ex_OpStr = "路灯";
                    //        else if (Op_extend == 2) Ex_OpStr = "亮化";
                    //        else if (Op_extend == 3) Ex_OpStr = "广告";
                    //        else Ex_OpStr = "其他";
                    //    }
                    //}
                    //else
                    //{
                    //    Ex_OpStr = "--";
                    //    IsCheckedCon = true;
                    //}


                }
            }
        }

        void UpdateIsCheckedCon(int op, int opextemd)
        {


            if ((op == 3 || op == 5) && opextemd != 4)
            {
                IsCheckedCon = false;
                if (op == 3)
                {
                    if (opextemd == 1) Ex_OpStr = "3005型设备";
                    else if (opextemd == 2) Ex_OpStr = "3090型设备";
                    else if (opextemd == 3) Ex_OpStr = "3006型设备";
                    else Ex_OpStr = "其他";
                }
                if (op == 4)
                {
                    if (opextemd == 1) Ex_OpStr = "路灯";
                    else if (opextemd == 2) Ex_OpStr = "亮化";
                    else if (opextemd == 3) Ex_OpStr = "广告";
                    else Ex_OpStr = "其他";
                }
            }
            else
            {

                Ex_OpStr = "--";
                if (op == 11 || op == 12) IsCheckedCon = false;
                else
                    IsCheckedCon = true;
            }


            IsEnabledRb = op == 6;

        }



        private string propery_contain_key;

        /// <summary>
        /// 可有多个包含字样 以;分割
        /// </summary>
        public string Ex_OpStr
        {
            get { return propery_contain_key; }
            set
            {
                if (propery_contain_key != value)
                {
                    propery_contain_key = value;
                    this.RaisePropertyChanged(() => this.Ex_OpStr);


                }
            }
        }



        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool> _recordrr;

        public ObservableCollection<NameIntBool> ProperyContainKey
        {
            get
            {
                if (_recordrr == null)
                {
                    _recordrr = new ObservableCollection<NameIntBool>();
                    for (int i = 0; i < 5; i++)
                    {
                        _recordrr.Add(new NameIntBool()
                                          {
                                              Name = ""
                                          });
                    }
                }
                return _recordrr;
            }
            set
            {
                if (value == _recordrr) return;
                _recordrr = value;
                this.RaisePropertyChanged(() => ProperyContainKey);
            }
        }

        private string _OpIProperyContainKeyStrdstr;

        public string ProperyContainKeyStr
        {
            get { return _OpIProperyContainKeyStrdstr; }
            set
            {
                if (_OpIProperyContainKeyStrdstr != value)
                {
                    _OpIProperyContainKeyStrdstr = value;
                    this.RaisePropertyChanged(() => this.ProperyContainKeyStr);


                }
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool> _record;

        public ObservableCollection<NameIntBool> ItemsAddto
        {
            get { return _record ?? (_record = new ObservableCollection<NameIntBool>()); }
            set
            {
                if (value == _record) return;
                _record = value;
                this.RaisePropertyChanged(() => ItemsAddto);
            }
        }


        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool> _recordx;

        public ObservableCollection<NameIntBool> ItemsRemoveto
        {
            get { return _recordx ?? (_recordx = new ObservableCollection<NameIntBool>()); }
            set
            {
                if (value == _recordx) return;
                _recordx = value;
                this.RaisePropertyChanged(() => ItemsRemoveto);
            }
        }

        public List<int> Addto;
        public List<int> Removeoff;

        public void UpdateProCkey()
        {
            ProperyContainKeyStr = "";
            foreach (var f in ProperyContainKey)
            {
                if (string.IsNullOrEmpty(f.Name)) continue;
                ProperyContainKeyStr += f.Name + ";";
            }
        }

        public void UpdateRemoveOffStr()
        {
            int rouc = (from t in ItemsRemoveto where t.IsSelected select t).Count();
            var str = "";
            int xcount = 0;
            foreach (var f in ItemsRemoveto)
            {
                if (f.IsSelected)
                {
                    xcount++;
                    str += f.Name + ";";
                }
                if (xcount > 1) break;
            }
            if (rouc < 3) RemoveOffStr = str;
            else
            {
                RemoveOffStr = "共 "+rouc + " 种:  " + str + " 等.";
            }
        }

        private string propery_contaRemoveOffStrin_key;

        /// <summary>
        /// 可有多个包含字样 以;分割
        /// </summary>
        public string RemoveOffStr
        {
            get { return propery_contaRemoveOffStrin_key; }
            set
            {
                if (propery_contaRemoveOffStrin_key != value)
                {
                    propery_contaRemoveOffStrin_key = value;
                    this.RaisePropertyChanged(() => this.RemoveOffStr);


                }
            }
        }

    }
}
