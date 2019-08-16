using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.ViewModel
{

   public class LoopInfoLeft : ObservableObject
   {
       public int Indexr = 0;
       public LoopInfoLeft(int _datarCount, int loopId, string loopName, string v, string a, string power, string brightrate, string powerfactor, string  isAttachShow,
           string attachShowInfo, string attachShowInfoName, string backgnd, string backgroundAttach, string refa, string upper, string lower, string ratio, string onlinerate, string isred, int isShieldLoop)
       {
           Indexr = _datarCount + 1;
           this.LoopId = loopId;
           this.A = a;
           this.Power = power;
           this.V = v;
           this.PowerFactor = powerfactor;
           this.SwitchInState = isAttachShow;
           this.BrightRate = brightrate;
           this.LoopName = loopName;
           this.RefA = refa;
           //this.YesterdayA = yesterdaya;
           //this.YesterdayP = yesterdayp;
           this.Upper = upper;
           this.Lower = lower;
           this.Ratio = ratio;
           this.OnlineRate = onlinerate;
           this.IsRed = isred;
           this.isShieldLoop = isShieldLoop;
           Backgroundx = backgnd;
           AttachInfo = attachShowInfo;
           AttachInfoName = attachShowInfoName;
           BackgroundAttach = backgroundAttach;

           this.ShieldLoopMarkx = isShieldLoop != 0 ? "屏蔽" : "正常";
           BackgroundShield = isShieldLoop != 0 ? "Red" : "Black";
       }

       public LoopInfoLeft (int loopid)
       {
           LoopId = loopid;
          
       }

       private int _loopId;
       /// <summary>
       /// 回路序号
       /// </summary>
       public int LoopId
       {
           get { return _loopId; }
           set
           {
               if (value != _loopId)
               {
                   _loopId = value;
                   this.RaisePropertyChanged(() => this.LoopId);
               }
           }
       }

       private string _loosdfpName;
       /// <summary>
       /// 回路名称
       /// </summary>
       public string Backgroundx
       {
           get { return _loosdfpName; }
           set
           {
               if (value != _loosdfpName)
               {
                   _loosdfpName = value;
                   this.RaisePropertyChanged(() => this.Backgroundx);
               }
           }
       }

       private string _loosdfpNasdfme;
       /// <summary>
       /// 回路名称
       /// </summary>
       public string BackgroundAttach
       {
           get { return _loosdfpNasdfme; }
           set
           {
               if (value != _loosdfpNasdfme)
               {
                   _loosdfpNasdfme = value;
                   this.RaisePropertyChanged(() => this.BackgroundAttach);
               }
           }
       }

       private string _loossdfsddfpName;

       /// <summary>
       /// 回路名称
       /// </summary>
       public string AttachInfo
       {
           get { return _loossdfsddfpName; }
           set
           {
               if (value != _loossdfsddfpName)
               {
                   _loossdfsddfpName = value;
                   this.RaisePropertyChanged(() => this.AttachInfo);
               }
           }
       }

       private string _loossddfpName;

       /// <summary>
       /// 回路名称
       /// </summary>
       public string AttachInfoName
       {
           get { return _loossddfpName; }
           set
           {
               if (value != _loossddfpName)
               {
                   _loossddfpName = value;
                   this.RaisePropertyChanged(() => this.AttachInfoName);
               }
           }
       }

       private string _loopName;
       /// <summary>
       /// 回路名称
       /// </summary>
       public string LoopName
       {
           get { return _loopName; }
           set
           {
               if (value != _loopName)
               {
                   _loopName = value;
                   this.RaisePropertyChanged(() => this.LoopName);
               }
           }
       }

        private string _vPhase;
        /// <summary>
        /// 回路电压相位
        /// </summary>
        public string Phase
        {
            get { return _vPhase; }
            set
            {
                if (value != _vPhase)
                {
                    _vPhase = value;
                    this.RaisePropertyChanged(() => this.Phase);
                }
            }
        }


        private string _v;
       /// <summary>
       /// 回路电压  或 所代表的门啥的状态
       /// </summary>
       public string V
       {
           get { return _v; }
           set
           {
               if (value != _v)
               {
                   _v = value;
                   this.RaisePropertyChanged(() => this.V);
               }
           }
       }

       private string _a;
       /// <summary>
       /// 回路电流
       /// </summary>
       public string A
       {
           get { return _a; }
           set
           {
               if (value != _a)
               {
                   _a = value;
                   this.RaisePropertyChanged(() => this.A);
               }
           }
       }

       private string _Power;
       /// <summary>
       /// 回路有功功率
       /// </summary>
       public string Power
       {
           get { return _Power; }
           set
           {
               if (value != _Power)
               {
                   _Power = value;
                   this.RaisePropertyChanged(() => this.Power);
               }
           }
       }



       private string _PowerFactor;
       /// <summary>
       /// 回路~~~
       /// </summary>
       public string PowerFactor
       {
           get { return _PowerFactor; }
           set
           {
               
               if (value != _PowerFactor)
               {
                   _PowerFactor = value;
                   this.RaisePropertyChanged(() => this.PowerFactor);
               }
           }
       }

       private string _BrightRate;
       /// <summary>
       /// 亮灯率~~~
       /// </summary>
       public string BrightRate
       {
           get { return _BrightRate; }
           set
           {
               if (value != _BrightRate)
               {
                   _BrightRate = value;
                   this.RaisePropertyChanged(() => this.BrightRate);
               }
           }
       }

       private string _SwitchInState;

       /// <summary>
       /// ~~~
       /// </summary>
       public string SwitchInState
       {
           get { return _SwitchInState; }
           set
           {
               if (value != _SwitchInState)
               {
                   _SwitchInState = value;
                   this.RaisePropertyChanged(() => this.SwitchInState);
               }
           }
       }


       private string _upper;

       /// <summary>
       /// 电流上限
       /// </summary>
       public string Upper
       {
           get { return _upper; }
           set
           {
               if (value != _upper)
               {
                   _upper = value;
                   this.RaisePropertyChanged(() => this.Upper);
               }
           }
       }

       private string _lower;

       /// <summary>
       /// 电流下限
       /// </summary>
       public string Lower
       {
           get { return _lower; }
           set
           {
               if (value != _lower)
               {
                   _lower = value;
                   this.RaisePropertyChanged(() => this.Lower);
               }
           }
       }

       private string _ratio;

       /// <summary>
       /// 互感器比值
       /// </summary>
       public string Ratio
       {
           get { return _ratio; }
           set
           {
               if (value != _ratio)
               {
                   _ratio = value;
                   this.RaisePropertyChanged(() => this.Ratio);
               }
           }
       }

       private string _refA;

       /// <summary>
       /// 参考电流
       /// </summary>
       public string RefA
       {
           get { return _refA; }
           set
           {
               if (value != _refA)
               {
                   _refA = value;
                   this.RaisePropertyChanged(() => this.RefA);
               }
           }
       }

       private string _onlineRate;

       /// <summary>
       /// 上线率
       /// </summary>
       public string OnlineRate
       {
           get { return _onlineRate; }
           set
           {
               if (value != _onlineRate)
               {
                   _onlineRate = value;
                   this.RaisePropertyChanged(() => this.OnlineRate);
               }
           }
       }

       private string _isRed;
       /// <summary>
       /// 接触器为吸合时标红
       /// </summary>
       public string IsRed
       {
           get { return _isRed; }
           set
           {
               if (value != _isRed)
               {
                   _isRed = value;
                   this.RaisePropertyChanged(() => this.IsRed);
               }
           }
       }

       private int _isShieldLoop;
       /// <summary>
       /// 是否屏蔽该回路 0不屏蔽  1屏蔽 2 屏蔽 并隐藏
       /// </summary>
       public int isShieldLoop
       {
           get { return _isShieldLoop; }
           set
           {
               if (value != _isShieldLoop)
               {
                   _isShieldLoop = value;
                   this.RaisePropertyChanged(() => this.isShieldLoop);
               }
           }
       }



       private string _shieldLoopMark;
       /// <summary>
       /// 屏蔽回路标识
       /// </summary>
       public string ShieldLoopMarkx
       {
           get { return _shieldLoopMark; }
           set
           {
               if (value != _shieldLoopMark)
               {
                   _shieldLoopMark = value;
                   this.RaisePropertyChanged(() => this.ShieldLoopMarkx);
               }
           }
       }

       private string _backgroundShield;
       /// <summary>
       /// 屏蔽标识颜色
       /// </summary>
       public string BackgroundShield
       {
           get { return _backgroundShield; }
           set
           {
               if (value != _backgroundShield)
               {
                   _backgroundShield = value;
                   this.RaisePropertyChanged(() => this.BackgroundShield);
               }
           }
       }


       private string _yesterdaya;

       /// <summary>
       /// 昨日电流
       /// </summary>
       public string YesterdayA
       {
           get { return _yesterdaya; }
           set
           {
               if (value != _yesterdaya)
               {
                   _yesterdaya = value;
                   this.RaisePropertyChanged(() => this.YesterdayA);
               }
           }
       }

       private string _yesterdayp;

       /// <summary>
       /// 昨日功率
       /// </summary>
       public string YesterdayP
       {
           get { return _yesterdayp; }
           set
           {
               if (value != _yesterdayp)
               {
                   _yesterdayp = value;
                   this.RaisePropertyChanged(() => this.YesterdayP);
               }
           }
       }


         private string _yesterdayv;

       /// <summary>
       /// 昨日功率
       /// </summary>
       public string YesterdayV
       {
           get { return _yesterdayv; }
           set
           {
               if (value != _yesterdayv)
               {
                   _yesterdayv = value;
                   this.RaisePropertyChanged(() => this.YesterdayV);
               }
           }
       }

       private string _yesterdaySwitchin;

       /// <summary>
       /// 昨日功率
       /// </summary>
       public string YesterdaySwitchin
       {
           get { return _yesterdaySwitchin; }
           set
           {
               if (value != _yesterdaySwitchin)
               {
                   _yesterdaySwitchin = value;
                   this.RaisePropertyChanged(() => this.YesterdaySwitchin);
               }
           }
       }

       private string _yesterdaySwitchinColor;

       /// <summary>
       /// 昨日功率
       /// </summary>
       public string YesterdaySwitchinColor
       {
           get { return _yesterdaySwitchinColor; }
           set
           {
               if (value != _yesterdaySwitchinColor)
               {
                   _yesterdaySwitchinColor = value;
                   this.RaisePropertyChanged(() => this.YesterdaySwitchinColor);
               }
           }
       }
   }
}
