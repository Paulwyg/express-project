using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wlst.client;


namespace Wlst.Ux.WJ3005Module.ZZhaoCe.ZhaoCeRtuWeekSetViewModel.ViewModel
{
   public  class OneLoopOneWeekTimeViewModel:Wlst .Cr .Core .CoreServices .ObservableObject
   {
       public OneLoopOneWeekTimeViewModel(){}

       public OneLoopOneWeekTimeViewModel(Wlst.client.ZhaoCeInfo.ZhaoCeOneLoopOneWeekTime oneLoopOneWeek,TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem timeTableItem ,bool flg)
       {

           this.LoopId = oneLoopOneWeek.LoopId;
           //this.MondayTime = oneLoopOneWeek.MondayTime.Insert(2, ":").Insert(8, ":");
           //this.TuesdayTime = oneLoopOneWeek.TuesdayTime.Insert(2, ":").Insert(8, ":");
           //this.WednesdayTime = oneLoopOneWeek.WednesdayTime.Insert(2, ":").Insert(8, ":");
           //this.ThursdayTime = oneLoopOneWeek.ThursdayTime.Insert(2, ":").Insert(8, ":");
           //this.FridayTime = oneLoopOneWeek.FridayTime.Insert(2, ":").Insert(8, ":");
           //this.SaturdayTime = oneLoopOneWeek.SaturdayTime.Insert(2, ":").Insert(8, ":");
           //this.SundayTime = oneLoopOneWeek.SundayTime.Insert(2, ":").Insert(8, ":");
           DateTimeRecevie = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
           for (int i = 0; i < 7; i++)
           {
               this.ZhaoCeRtuItems.Add(new ZhaoCeRtuItemsStyle());
               this.ZhaoCeRtuItems[i].ZhaoCeWeek = i;
               //this.ZhaoCeRtuItems[i].DateTimeRecevie = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
               this.ZhaoCeRtuItems[i].ZhaoCeDate = DateTime.Now.AddDays(i - (int)DateTime.Now.DayOfWeek).ToString("yyyy.MM.dd");

           }



           this.ZhaoCeRtuItems[1].ZhaoCeTimeOnOffOne = oneLoopOneWeek.MondayTime.Insert(2, ":").Insert(8, ":");
           this.ZhaoCeRtuItems[2].ZhaoCeTimeOnOffOne = oneLoopOneWeek.TuesdayTime.Insert(2, ":").Insert(8, ":");
           this.ZhaoCeRtuItems[3].ZhaoCeTimeOnOffOne = oneLoopOneWeek.WednesdayTime.Insert(2, ":").Insert(8, ":");
           this.ZhaoCeRtuItems[4].ZhaoCeTimeOnOffOne = oneLoopOneWeek.ThursdayTime.Insert(2, ":").Insert(8, ":");
           this.ZhaoCeRtuItems[5].ZhaoCeTimeOnOffOne = oneLoopOneWeek.FridayTime.Insert(2, ":").Insert(8, ":");
           this.ZhaoCeRtuItems[6].ZhaoCeTimeOnOffOne = oneLoopOneWeek.SaturdayTime.Insert(2, ":").Insert(8, ":");
           this.ZhaoCeRtuItems[0].ZhaoCeTimeOnOffOne = oneLoopOneWeek.SundayTime.Insert(2, ":").Insert(8, ":");

           this.ZhaoCeIsOverOne = new List<bool>() {false, false, false};
           this.ZhaoCeType = 300;
           this.ZhaoCeScroll = "Disabled";

           //var str1 = "25:00-25:00";
           //var ruleitems =
           //    new List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableSectionRule>();
           //if (flg == false)
           //{
           //    if (timeTableItem.RuleItems != null)
           //    {
           //        ruleitems = timeTableItem.RuleItems;
           //        foreach (var t in ruleitems)
           //        {
           //            for (int i = 0; i < 7; i++)
           //            {
           //                this.ZhaoCeRtuItems[i].ZhaoCeBak = new List<string>();
           //                if (t.DayOfWeekUsed.Contains(i))
           //                {
           //                    int sunrise = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
           //                        int.Parse(this.ZhaoCeRtuItems[i].ZhaoCeDate.Substring(5, 2)), int.Parse(this.ZhaoCeRtuItems[i].ZhaoCeDate.Substring(8, 2))).time_sunrise;
           //                    int sunset = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
           //                        int.Parse(this.ZhaoCeRtuItems[i].ZhaoCeDate.Substring(5, 2)), int.Parse(this.ZhaoCeRtuItems[i].ZhaoCeDate.Substring(8, 2))).time_sunset;
           //                    str1 = CalculateTimeOnOff(timeTableItem.LightOnOffset, timeTableItem.LightOffOffset, t.TimeOn, t.TimeOff, sunset, sunrise, t.TypeOn, t.TypeOff);


           //                }
           //                else
           //                {
           //                    str1 = "25:00-25:00";
           //                }


           //                if (this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne == str1)
           //                {
           //                    this.ZhaoCeRtuItems[i].ZhaoCeBak.Add("");
           //                }
           //                else
           //                {
           //                    this.ZhaoCeRtuItems[i].ZhaoCeBak.Add("Red");
           //                }


           //            }
           //        }
           //    }
           //    else
           //    {
           //        for (int i = 0; i < 7; i++)
           //        {
           //            this.ZhaoCeRtuItems[i].ZhaoCeBak = new List<string>();
           //            if (this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne == str1)
           //            {
           //                this.ZhaoCeRtuItems[i].ZhaoCeBak.Add("");
           //            }
           //            else
           //            {
           //                this.ZhaoCeRtuItems[i].ZhaoCeBak.Add("Red");
           //            }
           //        }
           //    }

           //}
           //else
           //{
           //    for (int i = 0; i < 7; i++)
           //    {
           //        this.ZhaoCeRtuItems[i].ZhaoCeBak = new List<string>();
           //        this.ZhaoCeRtuItems[i].ZhaoCeBak.Add("");
           //    }
           //}

           //for (int i = 0; i < 7; i++)
           //{
           //    try
           //    {
           //        if (Convert.ToInt32(this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(0, 2)) * 100 + Convert.ToInt32(this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(3, 2))
           //            > Convert.ToInt32(this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(6, 2)) * 100 + Convert.ToInt32(this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(9, 2))
           //            && Convert.ToInt32(this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(6, 2)) != 25)
           //        {
           //            this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne = this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne + "(明)";
           //        }
           //    }
           //    catch (Exception)
           //    {}

           //    if (this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(0, 2) == "25")
           //    {
           //        this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne = "25:00-25:00" +
           //                                                    this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(5, 6);
           //    }
           //    if (this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(6, 2) == "25")
           //    {
           //        this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne = this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(0, 6) + "25:00-25:00";
           //    }
           //}
           var str1 = "25:00-25:00";
           var ruleitems =
               new List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableSectionRule>();
           if (flg == false)
           {
               if (timeTableItem.RuleItems != null)
               {
                   ruleitems = timeTableItem.RuleItems;
                   var daylst = new List<int>() { 0, 1, 2, 3, 4, 5, 6 };
                   foreach (var t in ruleitems)
                   {
                       for (int i = 0; i < 7; i++)
                       {
                           this.ZhaoCeRtuItems[i].ZhaoCeBak = new List<string>();
                           if (t.DayOfWeekUsed.Contains(i))
                           {
                               int sunrise = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                                   int.Parse(this.ZhaoCeRtuItems[i].ZhaoCeDate.Substring(5, 2)), int.Parse(this.ZhaoCeRtuItems[i].ZhaoCeDate.Substring(8, 2))).time_sunrise;
                               int sunset = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                                   int.Parse(this.ZhaoCeRtuItems[i].ZhaoCeDate.Substring(5, 2)), int.Parse(this.ZhaoCeRtuItems[i].ZhaoCeDate.Substring(8, 2))).time_sunset;
                               str1 = CalculateTimeOnOff(timeTableItem.LightOnOffset, timeTableItem.LightOffOffset, t.TimeOn, t.TimeOff, sunset, sunrise, t.TypeOn, t.TypeOff);

                               if (this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne == str1)
                               {
                                   this.ZhaoCeRtuItems[i].ZhaoCeBak.Add("");
                               }
                               else
                               {
                                   this.ZhaoCeRtuItems[i].ZhaoCeBak.Add("Red");
                               }

                               if (daylst.Contains(i)) daylst.Remove(i);
                           }
                       }
                   }

                   foreach (var t in daylst)
                   {
                       str1 = "25:00-25:00";
                       if (this.ZhaoCeRtuItems[t].ZhaoCeTimeOnOffOne == str1)
                       {
                           this.ZhaoCeRtuItems[t].ZhaoCeBak.Add("");
                       }
                       else
                       {
                           this.ZhaoCeRtuItems[t].ZhaoCeBak.Add("Red");
                       }
                   }

               }
               else
               {
                   for (int i = 0; i < 7; i++)
                   {
                       this.ZhaoCeRtuItems[i].ZhaoCeBak = new List<string>();
                       if (this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne == str1)
                       {
                           this.ZhaoCeRtuItems[i].ZhaoCeBak.Add("");
                       }
                       else
                       {
                           this.ZhaoCeRtuItems[i].ZhaoCeBak.Add("Red");
                       }
                   }
               }

           }
           else
           {
               for (int i = 0; i < 7; i++)
               {
                   this.ZhaoCeRtuItems[i].ZhaoCeBak = new List<string>();
                   this.ZhaoCeRtuItems[i].ZhaoCeBak.Add("");
               }
           }

           for (int i = 0; i < 7; i++)
           {
               try
               {
                   if (Convert.ToInt32(this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(0, 2)) * 100 + Convert.ToInt32(this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(3, 2))
                       > Convert.ToInt32(this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(6, 2)) * 100 + Convert.ToInt32(this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(9, 2))
                       && Convert.ToInt32(this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(6, 2)) != 25)
                   {
                       this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne = this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne + "(明¡Â)";
                   }
               }
               catch (Exception)
               { }

               //if (this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(0, 2) == "25")
               //{
               //    this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne = "25:00-25:00" +
               //                                                this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(5, 6);
               //}
               //if (this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(6, 2) == "25")
               //{
               //    this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne = this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne.Substring(0, 6) + "25:00-25:00";
               //}
           }

           
         
       
           
       }

       public OneLoopOneWeekTimeViewModel(List<List<ZhaoCeInfo.ZhaoCeWeekSetYear>> oneLoopOneWeek, int maxsection,TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem timeTableItem,int loopid)
       {
           try
           {
               DateTimeRecevie = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");;

               for (int i = 0; i < 7; i++)
               {
                   this.ZhaoCeRtuItems.Add(new ZhaoCeRtuItemsStyle());
                   this.ZhaoCeRtuItems[i].ZhaoCeWeek = i;
                   //this.ZhaoCeRtuItems[i].DateTimeRecevie = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                   this.ZhaoCeRtuItems[i].ZhaoCeDate = DateTime.Now.AddDays(i - (int)DateTime.Now.DayOfWeek).ToString("yyyy.MM.dd");
               }

               var str = new Dictionary<Tuple<int, int>, string>();

               if (timeTableItem.RuleItems != null)
               {
                   foreach (var t in timeTableItem.RuleItems)
                   {
                       for (int i = 0; i < 7; i++)
                       {
                           if (t.DayOfWeekUsed.Contains(i))
                           {
                               int sunrise = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                               int.Parse(this.ZhaoCeRtuItems[i].ZhaoCeDate.Substring(5, 2)), int.Parse(this.ZhaoCeRtuItems[i].ZhaoCeDate.Substring(8, 2))).time_sunrise;
                               int sunset = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                                   int.Parse(this.ZhaoCeRtuItems[i].ZhaoCeDate.Substring(5, 2)), int.Parse(this.ZhaoCeRtuItems[i].ZhaoCeDate.Substring(8, 2))).time_sunset;
                               var str1 = CalculateTimeOnOff( timeTableItem.LightOnOffset, timeTableItem.LightOffOffset,  t.TimeOn, t.TimeOff, sunset, sunrise,t.TypeOn,t.TypeOff);
                       
                               var tu = new Tuple<int, int>(i, t.TimetableSectionId);
                               str.Add(tu, str1);
                           }

                       }
                   }
               }
               else
               {
                   for (int i = 0; i < 7; i++)
                   {
                       var str1 = "25:00-25:00";
                       var tu = new Tuple<int, int>(i, 1);
                       str.Add(tu, str1);
                   }
               }


               var flg = 0;
               foreach (var t in oneLoopOneWeek)
               {
                   if (t.Count>flg)
                   {
                       flg = t.Count;
                   }
               }


               if (flg > 0)
               {

                   this.LoopId = loopid;

                   if (maxsection > 0)
                   {
                       for (int i = 0; i < 7; i++)
                       {
                           this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne = CalculateTimeAndBak(i, 1, oneLoopOneWeek, str).Item1;
                           this.ZhaoCeRtuItems[i].ZhaoCeBak = new List<string>() { CalculateTimeAndBak(i, 1, oneLoopOneWeek, str).Item2 };
                       }

                       this.ZhaoCeIsOverOne = new List<bool>() { false, false, false };
                       this.ZhaoCeType = 300;
                       this.ZhaoCeScroll = "Disabled";




                       if (maxsection > 1)
                       {
                           for (int i = 0; i < 7; i++)
                           {
                               this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffTwo = CalculateTimeAndBak(i, 2, oneLoopOneWeek, str).Item1;
                               this.ZhaoCeRtuItems[i].ZhaoCeBak.Add(CalculateTimeAndBak(i, 2, oneLoopOneWeek, str).Item2);
                           }

                           this.ZhaoCeIsOverOne = new List<bool>() { true, false, false };
                           this.ZhaoCeType = 150;
                           this.ZhaoCeScroll = "Disabled";

                           if (maxsection > 2)
                           {
                               for (int i = 0; i < 7; i++)
                               {
                                   this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffThree = CalculateTimeAndBak(i, 3, oneLoopOneWeek, str).Item1;
                                   this.ZhaoCeRtuItems[i].ZhaoCeBak.Add(CalculateTimeAndBak(i, 3, oneLoopOneWeek, str).Item2);
                               }

                               this.ZhaoCeIsOverOne = new List<bool>() { true, true, false };
                               this.ZhaoCeScroll = "Auto";

                               if (maxsection == 4)
                               {
                                   for (int i = 0; i < 7; i++)
                                   {
                                       this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffFour = CalculateTimeAndBak(i, 4, oneLoopOneWeek, str).Item1;
                                       this.ZhaoCeRtuItems[i].ZhaoCeBak.Add(CalculateTimeAndBak(i, 4, oneLoopOneWeek, str).Item2);
                                   }

                                   this.ZhaoCeIsOverOne = new List<bool>() { true, true, true };
                               }
                           }
                       }


                   }
               }
               else
               {
                   for (int i = 0; i < 7; i++)
                   {
                       this.ZhaoCeRtuItems[i].ZhaoCeTimeOnOffOne = "25:00-25:00";
                       this.ZhaoCeRtuItems[i].ZhaoCeBak = new List<string>()
                                                              {CalculateTimeAndBak(i, 1, oneLoopOneWeek, str).Item2};
                   }

                   this.ZhaoCeIsOverOne = new List<bool>() {false, false, false};
                   this.ZhaoCeType = 300;
                   this.ZhaoCeScroll = "Disabled";
               }
           }
           catch (Exception)
           {

           }



       }

       public string MainCalculateTime(int time)
       {
           try
           {
               var inttime = time;
               int hour = inttime / 60;
               int minute = inttime % 60;

               if (hour == 25)
                   return "-";
               return hour.ToString("D2") + ":" + minute.ToString("D2");
           }
           catch (Exception ex)
           {
               Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
           }
           return "25:00";
       }

       public string CalculateTimeOnOff(int luxonset, int luxoffset,int timeon, int timeoff ,int sunset,int sunrise,int typeon,int typeoff)
       {
           try
           {
               string stron = "";
               string stroff = "";
               if (typeon == 1||typeon==2) stron = CalculateTime(sunset + luxonset);
               else if (typeon == 3) stron = CalculateTime(timeon);

               if (typeoff == 1 || typeoff == 2) stroff = CalculateTime(sunrise + luxoffset);
               else if (typeoff == 3) stroff = CalculateTime(timeoff);

               return stron + "-" + stroff;
           }
           catch (Exception ex)
           {
               Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
           }
           return "25:00-25:00";
       }



       public string CalculateTime(int time)
       {
                int hour = time / 60;
                int minute = time % 60;

                if (hour == 25)
                    return "25:00";
                return hour.ToString("D2") + ":" + minute.ToString("D2");
       }

       public Tuple<string, string> CalculateTimeAndBak(int week, int rectionid, List<List<ZhaoCeInfo.ZhaoCeWeekSetYear>> oneLoopOneWeek,Dictionary<Tuple<int,int>,string> str1)
       {
           string str = "";
           if (oneLoopOneWeek[week].Count>0)
           {
               str = CalculateTime(oneLoopOneWeek[week][rectionid-1].OpenTime) + "-" + CalculateTime(oneLoopOneWeek[week][rectionid-1].CloseTime);
           }
           else
           {
               str = "25:00-25:00";
           }

           var colour = "";
           var tu = new Tuple<string, string>(str, "");
           if (str1.ContainsKey(new Tuple<int, int>(week, rectionid)))
           {
               if (str1[new Tuple<int,int>(week,rectionid)]==str)
               {
                   if (Convert.ToInt32(str.Substring(0, 2)) * 100 + Convert.ToInt32(str.Substring(3, 2))
                       > Convert.ToInt32(str.Substring(6, 2)) * 100 + Convert.ToInt32(str.Substring(9, 2))
                       && Convert.ToInt32(str.Substring(6, 2)) != 25)
                   {
                       str = str + "(明)";
                   }
                   colour = "";
               }
               else
               {
                   if (Convert.ToInt32(str.Substring(0, 2)) * 100 + Convert.ToInt32(str.Substring(3, 2))
                       > Convert.ToInt32(str.Substring(6, 2)) * 100 + Convert.ToInt32(str.Substring(9, 2))
                       && Convert.ToInt32(str.Substring(6, 2)) != 25)
                   {
                       str = str + "(明)";
                   }
                   colour = "Red";
               }
           }
           else
           {
               var str2 = "25:00-25:00";
               if (str2 == str)
               {
                   colour = "";
               }
               else
               {
                   colour = "Red";
               }
           }

           if (Convert.ToInt32(str.Substring(0, 2)) == 25)
           {
               str = "25:00" + str.Substring(5, 6);
           }

           if (Convert.ToInt32(str.Substring(6, 2)) == 25)
           {
               str = str.Substring(0, 6) + "25:00";
           }

           tu = new Tuple<string, string>(str, colour);
           return tu;
       }


       

       #region general ar

       private string _datatime;

       /// <summary>
       /// 接收时间  
       /// </summary>
       public string DateTimeRecevie
       {
           get { return _datatime; }
           set
           {
               if (_datatime != value)
               {
                   _datatime = value;
                   this.RaisePropertyChanged(() => this.DateTimeRecevie);
               }
           }
       }

       private int _loopId;
       /// <summary>
        /// 回路地址
        /// </summary>
        public int LoopId
       {
           get { return _loopId; }
           set
           {
               if (_loopId != value)
               {
                   _loopId = value;
                   this.RaisePropertyChanged(() => this.LoopId);
               }
           }
       }

        string _mondayTime;
        /// <summary>
        /// 周一开关灯时间；开关时间 hhmm-hhmm
        /// </summary>
        public string MondayTime
        {
            get { return _mondayTime; }
            set
            {
                if (_mondayTime != value)
                {
                    _mondayTime = value;
                    this.RaisePropertyChanged(() => this.MondayTime);
                }
            }
        }
        string _tuesdayTime;
        /// <summary>
        /// 周二开关灯时间；开关时间 hhmm-hhmm
        /// </summary>
        public string TuesdayTime
        {
            get { return _tuesdayTime; }
            set
            {
                if (_tuesdayTime != value)
                {
                    _tuesdayTime = value;
                    this.RaisePropertyChanged(() => this.TuesdayTime);
                }
            }
        }
        string _wednesdayTime;
        /// <summary>
        /// 周三开关灯时间；开关时间 hhmm-hhmm
        /// </summary>
        public string WednesdayTime
        {
            get { return _wednesdayTime; }
            set
            {
                if (_wednesdayTime != value)
                {
                    _wednesdayTime = value;
                    this.RaisePropertyChanged(() => this.WednesdayTime);
                }
            }
        }
        string _thursdayTime;
        /// <summary>
        /// 周四开关灯时间；开关时间 hhmm-hhmm
        /// </summary>
        public string ThursdayTime
        {
            get { return _thursdayTime; }
            set
            {
                if (_thursdayTime != value)
                {
                    _thursdayTime = value;
                    this.RaisePropertyChanged(() => this.ThursdayTime);
                }
            }
        }
        string _fridayTime;
        /// <summary>
        /// 周五开关灯时间；开关时间 hhmm-hhmm
        /// </summary>
        public string FridayTime
        {
            get { return _fridayTime; }
            set
            {
                if (_fridayTime != value)
                {
                    _fridayTime = value;
                    this.RaisePropertyChanged(() => this.FridayTime);
                }
            }
        }
        string _saturdayTime;
        /// <summary>
        /// 周六开关灯时间；开关时间 hhmm-hhmm
        /// </summary>
        public string SaturdayTime
        {
            get { return _saturdayTime; }
            set
            {
                if (_saturdayTime != value)
                {
                    _saturdayTime = value;
                    this.RaisePropertyChanged(() => this.SaturdayTime);
                }
            }
        }
        string _sundayTime;
        /// <summary>
        /// 周日开关灯时间；开关时间 hhmm-hhmm
        /// </summary>
        public string SundayTime
        {
            get { return _sundayTime; }
            set
            {
                if (_sundayTime != value)
                {
                    _sundayTime = value;
                    this.RaisePropertyChanged(() => this.SundayTime);
                }
            }
        }





       #endregion


       #region For4

        private ObservableCollection<ZhaoCeRtuItemsStyle> _zhaocertuitems;

        public ObservableCollection<ZhaoCeRtuItemsStyle> ZhaoCeRtuItems
        {
            get
            {
                if (_zhaocertuitems == null)
                {
                    _zhaocertuitems = new ObservableCollection<ZhaoCeRtuItemsStyle>();
                }
                return _zhaocertuitems;
            }
            set
            {
                if (_zhaocertuitems != value)
                {
                    _zhaocertuitems = value;
                    this.RaisePropertyChanged(() => this.ZhaoCeRtuItems);
                }
            }

        }

        private List<bool> _zhaoceisoverone;
        public List<bool> ZhaoCeIsOverOne
        {
            get
            {
                return _zhaoceisoverone;
            }
            set
            {
                if (_zhaoceisoverone != value)
                {
                    _zhaoceisoverone = value;
                    this.RaisePropertyChanged(() => this.ZhaoCeIsOverOne);
                }
            }
        }

        private int _zhaocetype;
        public int ZhaoCeType
        {
            get
            {
                return _zhaocetype;
            }
            set
            {
                if (_zhaocetype != value)
                {
                    _zhaocetype = value;
                    this.RaisePropertyChanged(() => this.ZhaoCeType);
                }
            }
        }

        private string _zhaocescroll;
        public string ZhaoCeScroll
        {
            get
            {
                return _zhaocescroll;
            }
            set
            {
                if (_zhaocescroll != value)
                {
                    _zhaocescroll = value;
                    this.RaisePropertyChanged(() => this.ZhaoCeScroll);
                }
            }
        }

       #endregion
   }


   public class ZhaoCeRtuItemsStyle : Wlst.Cr.Core.CoreServices.ObservableObject
   {
       private int _zhaoceweek;
       public int ZhaoCeWeek
       {
           get
           {
               return _zhaoceweek;
           }
           set
           {
               if (_zhaoceweek != value)
               {
                   _zhaoceweek = value;
                   this.RaisePropertyChanged(() => this.ZhaoCeWeek);
               }
           }
       }

       private string _zhaocedate;
       public string ZhaoCeDate
       {
           get
           {
               return _zhaocedate;
           }
           set
           {
               if (_zhaocedate != value)
               {
                   _zhaocedate = value;
                   this.RaisePropertyChanged(() => this.ZhaoCeDate);
               }
           }
       }

       private string _zhaocetimeonoffone;
       public string ZhaoCeTimeOnOffOne
       {
           get
           {
               return _zhaocetimeonoffone;
           }
           set
           {
               if (_zhaocetimeonoffone != value)
               {
                   _zhaocetimeonoffone = value;
                   this.RaisePropertyChanged(() => this.ZhaoCeTimeOnOffOne);
               }
           }
       }

       private string _maintimeonofftwo;
       public string ZhaoCeTimeOnOffTwo
       {
           get
           {
               return _maintimeonofftwo;
           }
           set
           {
               if (_maintimeonofftwo != value)
               {
                   _maintimeonofftwo = value;
                   this.RaisePropertyChanged(() => this.ZhaoCeTimeOnOffTwo);
               }
           }
       }

       private string _maintimeonoffthree;
       public string ZhaoCeTimeOnOffThree
       {
           get
           {
               return _maintimeonoffthree;
           }
           set
           {
               if (_maintimeonoffthree != value)
               {
                   _maintimeonoffthree = value;
                   this.RaisePropertyChanged(() => this.ZhaoCeTimeOnOffThree);
               }
           }
       }

       private string _maintimeonofffour;
       public string ZhaoCeTimeOnOffFour
       {
           get
           {
               return _maintimeonofffour;
           }
           set
           {
               if (_maintimeonofffour != value)
               {
                   _maintimeonofffour = value;
                   this.RaisePropertyChanged(() => this.ZhaoCeTimeOnOffFour);
               }
           }
       }

       //private string _datetimeReceive;
       //public string DateTimeRecevie
       //{
       //    get
       //    {
       //        return _datetimeReceive;
       //    }
       //    set
       //    {
       //        if (_datetimeReceive != value)
       //        {
       //            _datetimeReceive = value;
       //            this.RaisePropertyChanged(() => this.DateTimeRecevie);
       //        }
       //    }
       //}

       private List<string> _zhaocebak;
       public List<string> ZhaoCeBak
       {
           get
           {
               return _zhaocebak;
           }
           set
           {
               if (_zhaocebak != value)
               {
                   _zhaocebak = value;
                   this.RaisePropertyChanged(() => this.ZhaoCeBak);
               }
           }
       }


   
   }
}
