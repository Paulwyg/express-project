using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Wlst.Cr.CoreOne.Models
{
    /// <summary>
    /// 集合化属性参数 使用此方法时更新一个属性所有属性同时更新
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableObjectCollectionX<T> : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        // protected string[] StringCollection;
        protected int Number;

        public ObservableObjectCollectionX(int number)
        {
            Number = number;
            Info = new ArrayCollection<T>(number);
            Info.OnValueUpdated += new EventHandler(Info_OnValueUpdated);
        }

        private void Info_OnValueUpdated(object sender, EventArgs e)
        {
            this.RaisePropertyChanged(() => this.Info);
        }

         ~ObservableObjectCollectionX()
        {
            try
            {
                Info.OnValueUpdated -= new EventHandler(Info_OnValueUpdated);
                Info.Clean();
            }
            catch (Exception ex)
            {

            }
        }


        public void CleanAttri()
        {
            try
            {
                Info.OnValueUpdated -= new EventHandler(Info_OnValueUpdated);
                Info.Clean();
            }
            catch (Exception ex)
            {

            }
        }

        protected ArrayCollection<T> _info;

        public ArrayCollection<T> Info
        {
            get { return _info; }
            set
            {
                if (value == _info) return;
                _info = value;
                this.RaisePropertyChanged(() => this.Info);
            }
        }
    }

    public class ArrayCollection<T>
    {
        protected T[] StringCollection;

        public event EventHandler OnValueUpdated;

        public ArrayCollection(int number)
        {
            StringCollection = new T[number];
        }

        internal void Clean()
        {
            StringCollection = null;
        }

        public T this[int index]
        {
            get
            {
                if (StringCollection == null || index >= StringCollection.Length) return default(T);
                return StringCollection[index];
            }
            set
            {
                if (index >= StringCollection.Length) return;
                if (StringCollection[index] == null && value == null) return;
                if (StringCollection[index] != null && value != null && StringCollection[index].Equals(value)) return;

                StringCollection[index] = value;
                if (OnValueUpdated != null) OnValueUpdated(index, EventArgs.Empty);
            }
        }
    }


    /// <summary>
    /// 集合化属性参数 使用此方法时更新一个属性仅仅是更新此属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableObjectCollection<T> : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        protected T[] StringCollection;
        protected int Number;

        /// <summary>
        /// 初始化属性 最多不得多于30个属性
        /// </summary>
        /// <param name="number">不得多于30个属性</param>
        public ObservableObjectCollection(int number)
        {
            if (number >= 30) number = 30;
            Number = number;
            StringCollection = new T[number];
        }

        #region  手动写入的属性 get  set

        public T Value0
        {
            get
            {
                if (Number > 0) return StringCollection[0];
                return default(T);
            }
            set
            {
                if (Number <= 0) return;
                if (StringCollection[0] == null && value == null) return;
                if (StringCollection[0] != null && value != null && StringCollection[0].Equals(value)) return;
                StringCollection[0] = value;
                this.RaisePropertyChanged(() => this.Value0);
            }
        }


        public T Value1
        {
            get
            {
                if (Number > 1) return StringCollection[1];
                return default(T);
            }
            set
            {
                if (Number <= 1) return;
                if (StringCollection[1] == null && value == null) return;
                if (StringCollection[1] != null && value != null && StringCollection[1].Equals(value)) return;
                StringCollection[1] = value;
                this.RaisePropertyChanged(() => this.Value1);
            }
        }


        public T Value2
        {
            get
            {
                if (Number > 2) return StringCollection[2];
                return default(T);
            }
            set
            {
                if (Number <= 2) return;
                if (StringCollection[2] == null && value == null) return;
                if (StringCollection[2] != null && value != null && StringCollection[2].Equals(value)) return;
                StringCollection[2] = value;
                this.RaisePropertyChanged(() => this.Value2);
            }
        }


        public T Value3
        {
            get
            {
                if (Number > 3) return StringCollection[3];
                return default(T);
            }
            set
            {
                if (Number <= 3) return;
                if (StringCollection[3] == null && value == null) return;
                if (StringCollection[3] != null && value != null && StringCollection[3].Equals(value)) return;
                StringCollection[3] = value;
                this.RaisePropertyChanged(() => this.Value3);
            }
        }

        public T Value4
        {
            get
            {
                if (Number > 4) return StringCollection[4];
                return default(T);
            }
            set
            {
                if (Number <= 4) return;
                if (StringCollection[4] == null && value == null) return;
                if (StringCollection[4] != null && value != null && StringCollection[4].Equals(value)) return;
                StringCollection[4] = value;
                this.RaisePropertyChanged(() => this.Value4);
            }
        }

        public T Value5
        {
            get
            {
                if (Number > 5) return StringCollection[5];
                return default(T);
            }
            set
            {
                if (Number <= 5) return;
                if (StringCollection[5] == null && value == null) return;
                if (StringCollection[5] != null && value != null && StringCollection[5].Equals(value)) return;
                StringCollection[5] = value;
                this.RaisePropertyChanged(() => this.Value5);
            }
        }

        public T Value6
        {
            get
            {
                if (Number > 6) return StringCollection[6];
                return default(T);
            }
            set
            {
                if (Number <= 6) return;
                if (StringCollection[6] == null && value == null) return;
                if (StringCollection[6] != null && value != null && StringCollection[6].Equals(value)) return;
                StringCollection[6] = value;
                this.RaisePropertyChanged(() => this.Value6);
            }
        }

        public T Value7
        {
            get
            {
                if (Number > 7) return StringCollection[7];
                return default(T);
            }
            set
            {
                if (Number <= 7) return;
                if (StringCollection[7] == null && value == null) return;
                if (StringCollection[7] != null && value != null && StringCollection[7].Equals(value)) return;
                StringCollection[7] = value;
                this.RaisePropertyChanged(() => this.Value7);
            }
        }

        public T Value8
        {
            get
            {
                if (Number > 8) return StringCollection[8];
                return default(T);
            }
            set
            {
                if (Number <= 8) return;
                if (StringCollection[8] == null && value == null) return;
                if (StringCollection[8] != null && value != null && StringCollection[8].Equals(value)) return;
                StringCollection[8] = value;
                this.RaisePropertyChanged(() => this.Value8);
            }
        }

        public T Value9
        {
            get
            {
                if (Number > 9) return StringCollection[9];
                return default(T);
            }
            set
            {
                if (Number <= 9) return;
                if (StringCollection[9] == null && value == null) return;
                if (StringCollection[9] != null && value != null && StringCollection[9].Equals(value)) return;
                StringCollection[9] = value;
                this.RaisePropertyChanged(() => this.Value9);
            }
        }

        public T Value10
        {
            get
            {
                if (Number > 10) return StringCollection[10];
                return default(T);
            }
            set
            {
                if (Number <= 10) return;
                if (StringCollection[10] == null && value == null) return;
                if (StringCollection[10] != null && value != null && StringCollection[10].Equals(value)) return;
                StringCollection[10] = value;
                this.RaisePropertyChanged(() => this.Value10);
            }
        }


        public T Value11
        {
            get
            {
                if (Number > 11) return StringCollection[11];
                return default(T);
            }
            set
            {
                if (Number <= 11) return;
                if (StringCollection[11] == null && value == null) return;
                if (StringCollection[11] != null && value != null && StringCollection[11].Equals(value)) return;
                StringCollection[11] = value;
                this.RaisePropertyChanged(() => this.Value11);
            }
        }

        public T Value12
        {
            get
            {
                if (Number > 12) return StringCollection[12];
                return default(T);
            }
            set
            {
                if (Number <= 12) return;
                if (StringCollection[12] == null && value == null) return;
                if (StringCollection[12] != null && value != null && StringCollection[12].Equals(value)) return;
                StringCollection[12] = value;
                this.RaisePropertyChanged(() => this.Value12);
            }
        }

        public T Value13
        {
            get
            {
                if (Number > 13) return StringCollection[13];
                return default(T);
            }
            set
            {
                if (Number <= 13) return;
                if (StringCollection[13] == null && value == null) return;
                if (StringCollection[13] != null && value != null && StringCollection[13].Equals(value)) return;
                StringCollection[13] = value;
                this.RaisePropertyChanged(() => this.Value13);
            }
        }

        public T Value14
        {
            get
            {
                if (Number > 14) return StringCollection[14];
                return default(T);
            }
            set
            {
                if (Number <= 14) return;
                if (StringCollection[14] == null && value == null) return;
                if (StringCollection[14] != null && value != null && StringCollection[14].Equals(value)) return;
                StringCollection[14] = value;
                this.RaisePropertyChanged(() => this.Value14);
            }
        }

        public T Value15
        {
            get
            {
                if (Number > 15) return StringCollection[15];
                return default(T);
            }
            set
            {
                if (Number <= 15) return;
                if (StringCollection[15] == null && value == null) return;
                if (StringCollection[15] != null && value != null && StringCollection[15].Equals(value)) return;
                StringCollection[15] = value;
                this.RaisePropertyChanged(() => this.Value15);
            }
        }

        public T Value16
        {
            get
            {
                if (Number > 16) return StringCollection[16];
                return default(T);
            }
            set
            {
                if (Number <= 16) return;
                if (StringCollection[16] == null && value == null) return;
                if (StringCollection[16] != null && value != null && StringCollection[16].Equals(value)) return;
                StringCollection[16] = value;
                this.RaisePropertyChanged(() => this.Value16);
            }
        }

        public T Value17
        {
            get
            {
                if (Number > 17) return StringCollection[17];
                return default(T);
            }
            set
            {
                if (Number <= 17) return;
                if (StringCollection[17] == null && value == null) return;
                if (StringCollection[17] != null && value != null && StringCollection[17].Equals(value)) return;
                StringCollection[17] = value;
                this.RaisePropertyChanged(() => this.Value17);
            }
        }

        public T Value18
        {
            get
            {
                if (Number > 18) return StringCollection[18];
                return default(T);
            }
            set
            {
                if (Number <= 18) return;
                if (StringCollection[18] == null && value == null) return;
                if (StringCollection[18] != null && value != null && StringCollection[18].Equals(value)) return;
                StringCollection[18] = value;
                this.RaisePropertyChanged(() => this.Value18);
            }
        }

        public T Value19
        {
            get
            {
                if (Number > 19) return StringCollection[19];
                return default(T);
            }
            set
            {
                if (Number <= 19) return;
                if (StringCollection[19] == null && value == null) return;
                if (StringCollection[19] != null && value != null && StringCollection[19].Equals(value)) return;
                StringCollection[19] = value;
                this.RaisePropertyChanged(() => this.Value19);
            }
        }

        public T Value20
        {
            get
            {
                if (Number > 20) return StringCollection[20];
                return default(T);
            }
            set
            {
                if (Number <= 20) return;
                if (StringCollection[20] == null && value == null) return;
                if (StringCollection[20] != null && value != null && StringCollection[20].Equals(value)) return;
                StringCollection[20] = value;
                this.RaisePropertyChanged(() => this.Value20);
            }
        }

        public T Value21
        {
            get
            {
                if (Number > 21) return StringCollection[21];
                return default(T);
            }
            set
            {
                if (Number <= 21) return;
                if (StringCollection[21] == null && value == null) return;
                if (StringCollection[21] != null && value != null && StringCollection[21].Equals(value)) return;
                StringCollection[21] = value;
                this.RaisePropertyChanged(() => this.Value21);
            }
        }

        public T Value22
        {
            get
            {
                if (Number > 22) return StringCollection[22];
                return default(T);
            }
            set
            {
                if (Number <= 22) return;
                if (StringCollection[22] == null && value == null) return;
                if (StringCollection[22] != null && value != null && StringCollection[22].Equals(value)) return;
                StringCollection[22] = value;
                this.RaisePropertyChanged(() => this.Value22);
            }
        }

        public T Value23
        {
            get
            {
                if (Number > 23) return StringCollection[23];
                return default(T);
            }
            set
            {
                if (Number <= 23) return;
                if (StringCollection[23] == null && value == null) return;
                if (StringCollection[23] != null && value != null && StringCollection[23].Equals(value)) return;
                StringCollection[23] = value;
                this.RaisePropertyChanged(() => this.Value23);
            }
        }

        public T Value24
        {
            get
            {
                if (Number > 24) return StringCollection[24];
                return default(T);
            }
            set
            {
                if (Number <= 24) return;
                if (StringCollection[24] == null && value == null) return;
                if (StringCollection[24] != null && value != null && StringCollection[24].Equals(value)) return;
                StringCollection[24] = value;
                this.RaisePropertyChanged(() => this.Value24);
            }
        }

        public T Value25
        {
            get
            {
                if (Number > 25) return StringCollection[25];
                return default(T);
            }
            set
            {
                if (Number <= 25) return;
                if (StringCollection[25] == null && value == null) return;
                if (StringCollection[25] != null && value != null && StringCollection[25].Equals(value)) return;
                StringCollection[25] = value;
                this.RaisePropertyChanged(() => this.Value25);
            }
        }

        public T Value26
        {
            get
            {
                if (Number > 26) return StringCollection[26];
                return default(T);
            }
            set
            {
                if (Number <= 26) return;
                if (StringCollection[26] == null && value == null) return;
                if (StringCollection[26] != null && value != null && StringCollection[26].Equals(value)) return;
                StringCollection[26] = value;
                this.RaisePropertyChanged(() => this.Value26);
            }
        }

        public T Value27
        {
            get
            {
                if (Number > 27) return StringCollection[27];
                return default(T);
            }
            set
            {
                if (Number <= 27) return;
                if (StringCollection[27] == null && value == null) return;
                if (StringCollection[27] != null && value != null && StringCollection[27].Equals(value)) return;
                StringCollection[27] = value;
                this.RaisePropertyChanged(() => this.Value27);
            }
        }

        public T Value28
        {
            get
            {
                if (Number > 28) return StringCollection[28];
                return default(T);
            }
            set
            {
                if (Number <= 28) return;
                if (StringCollection[28] == null && value == null) return;
                if (StringCollection[28] != null && value != null && StringCollection[28].Equals(value)) return;
                StringCollection[28] = value;
                this.RaisePropertyChanged(() => this.Value28);
            }
        }

        public T Value29
        {
            get
            {
                if (Number > 29) return StringCollection[29];
                return default(T);
            }
            set
            {
                if (Number <= 29) return;
                if (StringCollection[29] == null && value == null) return;
                if (StringCollection[29] != null && value != null && StringCollection[29].Equals(value)) return;
                StringCollection[29] = value;
                this.RaisePropertyChanged(() => this.Value29);
            }
        }

        public T Value30
        {
            get
            {
                if (Number > 30) return StringCollection[30];
                return default(T);
            }
            set
            {
                if (Number <= 30) return;
                if (StringCollection[30] == null && value == null) return;
                if (StringCollection[30] != null && value != null && StringCollection[30].Equals(value)) return;
                StringCollection[30] = value;
                this.RaisePropertyChanged(() => this.Value30);
            }
        }

        #endregion

        #region 析构函数

         ~ObservableObjectCollection()
        {
            try
            {
                StringCollection = null;
            }
            catch (Exception ex)
            {

            }
        }


        public void CleanAttri()
        {
            try
            {
                StringCollection = null;
            }
            catch (Exception ex)
            {

            }
        }

        #endregion


    }

    /// <summary>
    /// 集合化属性参数 使用此方法时更新一个属性仅仅是更新此属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableObjectCollection : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        protected string  [] StringCollection;
        protected int[] IntCollection;
        protected int IntNumber;
        protected int StringNumber;

        /// <summary>
        /// 初始化属性 最多不得多于30个属性
        /// </summary>
        /// <param name="intNumber">不得多于30个属性</param>
        /// <param name="stringNumber">不得多于30个属性</param>
        public ObservableObjectCollection(int intNumber,int stringNumber)
        {
            if (intNumber >= 30) intNumber = 30;
            IntNumber = intNumber;
            StringNumber = stringNumber;
            StringCollection = new string[stringNumber];
            IntCollection = new int[intNumber];
        }

        #region  手动写入的属性 get  set

        private Visibility _isfather;
        public Visibility IsFather
        {
            get { return _isfather; }
            set
            {
                if (_isfather == value) return;
                _isfather = value;
                RaisePropertyChanged(() => IsFather);

            }
        }


        private int _rtuid;

        public int Rtuid
        {
            get { return _rtuid; }
            set
            {
                if (_rtuid == value) return;
                _rtuid = value;
                RaisePropertyChanged(() => Rtuid);

            }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (IsChecked == value) return;
                _isChecked = value;
                if (IsFather == Visibility.Hidden)
                {
                    foreach (var item in ChildTreeItems)
                    {
                        {
                            item.IsChecked = IsChecked;
                        }
                    }
                }
                RaisePropertyChanged(() => IsChecked);

            }
        }

        private ObservableCollection<ObservableObjectCollection> _childTreeItemsInfo;

        public ObservableCollection<ObservableObjectCollection> ChildTreeItems
        {
            get { return _childTreeItemsInfo ?? (_childTreeItemsInfo = new ObservableCollection<ObservableObjectCollection>()); }
        }

        public string ValueString0
        {
            get
            {
                if (StringNumber > 0) return StringCollection[0];
                return "";
            }
            set
            {
                if (StringNumber <= 0) return;
                if (StringCollection[0] == null && value == null) return;
                if (StringCollection[0] != null && value != null && StringCollection[0].Equals(value)) return;
                StringCollection[0] = value;
                this.RaisePropertyChanged(() => this.ValueString0);
            }
        }


        public string ValueString1
        {
            get
            {
                if (StringNumber > 1) return StringCollection[1];
                return "";
            }
            set
            {
                if (StringNumber <= 1) return;
                if (StringCollection[1] == null && value == null) return;
                if (StringCollection[1] != null && value != null && StringCollection[1].Equals(value)) return;
                StringCollection[1] = value;
                this.RaisePropertyChanged(() => this.ValueString1);
            }
        }


        public string ValueString2
        {
            get
            {
                if (StringNumber > 2) return StringCollection[2];
                return "";
            }
            set
            {
                if (StringNumber <= 2) return;
                if (StringCollection[2] == null && value == null) return;
                if (StringCollection[2] != null && value != null && StringCollection[2].Equals(value)) return;
                StringCollection[2] = value;
                this.RaisePropertyChanged(() => this.ValueString2);
            }
        }


        public string ValueString3
        {
            get
            {
                if (StringNumber > 3) return StringCollection[3];
                return "";
            }
            set
            {
                if (StringNumber <= 3) return;
                if (StringCollection[3] == null && value == null) return;
                if (StringCollection[3] != null && value != null && StringCollection[3].Equals(value)) return;
                StringCollection[3] = value;
                this.RaisePropertyChanged(() => this.ValueString3);
            }
        }

        public string ValueString4
        {
            get
            {
                if (StringNumber > 4) return StringCollection[4];
                return "";
            }
            set
            {
                if (StringNumber <= 4) return;
                if (StringCollection[4] == null && value == null) return;
                if (StringCollection[4] != null && value != null && StringCollection[4].Equals(value)) return;
                StringCollection[4] = value;
                this.RaisePropertyChanged(() => this.ValueString4);
            }
        }

        public string ValueString5
        {
            get
            {
                if (StringNumber > 5) return StringCollection[5];
                return "";
            }
            set
            {
                if (StringNumber <= 5) return;
                if (StringCollection[5] == null && value == null) return;
                if (StringCollection[5] != null && value != null && StringCollection[5].Equals(value)) return;
                StringCollection[5] = value;
                this.RaisePropertyChanged(() => this.ValueString5);
            }
        }

        public string ValueString6
        {
            get
            {
                if (StringNumber > 6) return StringCollection[6];
                return "";
            }
            set
            {
                if (StringNumber <= 6) return;
                if (StringCollection[6] == null && value == null) return;
                if (StringCollection[6] != null && value != null && StringCollection[6].Equals(value)) return;
                StringCollection[6] = value;
                this.RaisePropertyChanged(() => this.ValueString6);
            }
        }

        public string ValueString7
        {
            get
            {
                if (StringNumber > 7) return StringCollection[7];
                return "";
            }
            set
            {
                if (StringNumber <= 7) return;
                if (StringCollection[7] == null && value == null) return;
                if (StringCollection[7] != null && value != null && StringCollection[7].Equals(value)) return;
                StringCollection[7] = value;
                this.RaisePropertyChanged(() => this.ValueString7);
            }
        }

        public string ValueString8
        {
            get
            {
                if (StringNumber > 8) return StringCollection[8];
                return "";
            }
            set
            {
                if (StringNumber <= 8) return;
                if (StringCollection[8] == null && value == null) return;
                if (StringCollection[8] != null && value != null && StringCollection[8].Equals(value)) return;
                StringCollection[8] = value;
                this.RaisePropertyChanged(() => this.ValueString8);
            }
        }

        public string ValueString9
        {
            get
            {
                if (StringNumber > 9) return StringCollection[9];
                return "";
            }
            set
            {
                if (StringNumber <= 9) return;
                if (StringCollection[9] == null && value == null) return;
                if (StringCollection[9] != null && value != null && StringCollection[9].Equals(value)) return;
                StringCollection[9] = value;
                this.RaisePropertyChanged(() => this.ValueString9);
            }
        }

        public string ValueString10
        {
            get
            {
                if (StringNumber > 10) return StringCollection[10];
                return "";
            }
            set
            {
                if (StringNumber <= 10) return;
                if (StringCollection[10] == null && value == null) return;
                if (StringCollection[10] != null && value != null && StringCollection[10].Equals(value)) return;
                StringCollection[10] = value;
                this.RaisePropertyChanged(() => this.ValueString10);
            }
        }


        public string ValueString11
        {
            get
            {
                if (StringNumber > 11) return StringCollection[11];
                return "";
            }
            set
            {
                if (StringNumber <= 11) return;
                if (StringCollection[11] == null && value == null) return;
                if (StringCollection[11] != null && value != null && StringCollection[11].Equals(value)) return;
                StringCollection[11] = value;
                this.RaisePropertyChanged(() => this.ValueString11);
            }
        }

        public string ValueString12
        {
            get
            {
                if (StringNumber > 12) return StringCollection[12];
                return "";
            }
            set
            {
                if (StringNumber <= 12) return;
                if (StringCollection[12] == null && value == null) return;
                if (StringCollection[12] != null && value != null && StringCollection[12].Equals(value)) return;
                StringCollection[12] = value;
                this.RaisePropertyChanged(() => this.ValueString12);
            }
        }

        public string ValueString13
        {
            get
            {
                if (StringNumber > 13) return StringCollection[13];
                return "";
            }
            set
            {
                if (StringNumber <= 13) return;
                if (StringCollection[13] == null && value == null) return;
                if (StringCollection[13] != null && value != null && StringCollection[13].Equals(value)) return;
                StringCollection[13] = value;
                this.RaisePropertyChanged(() => this.ValueString13);
            }
        }

        public string ValueString14
        {
            get
            {
                if (StringNumber > 14) return StringCollection[14];
                return "";
            }
            set
            {
                if (StringNumber <= 14) return;
                if (StringCollection[14] == null && value == null) return;
                if (StringCollection[14] != null && value != null && StringCollection[14].Equals(value)) return;
                StringCollection[14] = value;
                this.RaisePropertyChanged(() => this.ValueString14);
            }
        }

        public string ValueString15
        {
            get
            {
                if (StringNumber > 15) return StringCollection[15];
                return "";
            }
            set
            {
                if (StringNumber <= 15) return;
                if (StringCollection[15] == null && value == null) return;
                if (StringCollection[15] != null && value != null && StringCollection[15].Equals(value)) return;
                StringCollection[15] = value;
                this.RaisePropertyChanged(() => this.ValueString15);
            }
        }

        public string ValueString16
        {
            get
            {
                if (StringNumber > 16) return StringCollection[16];
                return "";
            }
            set
            {
                if (StringNumber <= 16) return;
                if (StringCollection[16] == null && value == null) return;
                if (StringCollection[16] != null && value != null && StringCollection[16].Equals(value)) return;
                StringCollection[16] = value;
                this.RaisePropertyChanged(() => this.ValueString16);
            }
        }

        public string ValueString17
        {
            get
            {
                if (StringNumber > 17) return StringCollection[17];
                return "";
            }
            set
            {
                if (StringNumber <= 17) return;
                if (StringCollection[17] == null && value == null) return;
                if (StringCollection[17] != null && value != null && StringCollection[17].Equals(value)) return;
                StringCollection[17] = value;
                this.RaisePropertyChanged(() => this.ValueString17);
            }
        }

        public string ValueString18
        {
            get
            {
                if (StringNumber > 18) return StringCollection[18];
                return "";
            }
            set
            {
                if (StringNumber <= 18) return;
                if (StringCollection[18] == null && value == null) return;
                if (StringCollection[18] != null && value != null && StringCollection[18].Equals(value)) return;
                StringCollection[18] = value;
                this.RaisePropertyChanged(() => this.ValueString18);
            }
        }

        public string ValueString19
        {
            get
            {
                if (StringNumber > 19) return StringCollection[19];
                return "";
            }
            set
            {
                if (StringNumber <= 19) return;
                if (StringCollection[19] == null && value == null) return;
                if (StringCollection[19] != null && value != null && StringCollection[19].Equals(value)) return;
                StringCollection[19] = value;
                this.RaisePropertyChanged(() => this.ValueString19);
            }
        }

        public string ValueString20
        {
            get
            {
                if (StringNumber > 20) return StringCollection[20];
                return "";
            }
            set
            {
                if (StringNumber <= 20) return;
                if (StringCollection[20] == null && value == null) return;
                if (StringCollection[20] != null && value != null && StringCollection[20].Equals(value)) return;
                StringCollection[20] = value;
                this.RaisePropertyChanged(() => this.ValueString20);
            }
        }

        public string ValueString21
        {
            get
            {
                if (StringNumber > 21) return StringCollection[21];
                return "";
            }
            set
            {
                if (StringNumber <= 21) return;
                if (StringCollection[21] == null && value == null) return;
                if (StringCollection[21] != null && value != null && StringCollection[21].Equals(value)) return;
                StringCollection[21] = value;
                this.RaisePropertyChanged(() => this.ValueString21);
            }
        }

        public string ValueString22
        {
            get
            {
                if (StringNumber > 22) return StringCollection[22];
                return "";
            }
            set
            {
                if (StringNumber <= 22) return;
                if (StringCollection[22] == null && value == null) return;
                if (StringCollection[22] != null && value != null && StringCollection[22].Equals(value)) return;
                StringCollection[22] = value;
                this.RaisePropertyChanged(() => this.ValueString22);
            }
        }

        public string ValueString23
        {
            get
            {
                if (StringNumber > 23) return StringCollection[23];
                return "";
            }
            set
            {
                if (StringNumber <= 23) return;
                if (StringCollection[23] == null && value == null) return;
                if (StringCollection[23] != null && value != null && StringCollection[23].Equals(value)) return;
                StringCollection[23] = value;
                this.RaisePropertyChanged(() => this.ValueString23);
            }
        }

        public string ValueString24
        {
            get
            {
                if (StringNumber > 24) return StringCollection[24];
                return "";
            }
            set
            {
                if (StringNumber <= 24) return;
                if (StringCollection[24] == null && value == null) return;
                if (StringCollection[24] != null && value != null && StringCollection[24].Equals(value)) return;
                StringCollection[24] = value;
                this.RaisePropertyChanged(() => this.ValueString24);
            }
        }

        public string ValueString25
        {
            get
            {
                if (StringNumber > 25) return StringCollection[25];
                return "";
            }
            set
            {
                if (StringNumber <= 25) return;
                if (StringCollection[25] == null && value == null) return;
                if (StringCollection[25] != null && value != null && StringCollection[25].Equals(value)) return;
                StringCollection[25] = value;
                this.RaisePropertyChanged(() => this.ValueString25);
            }
        }

        public string ValueString26
        {
            get
            {
                if (StringNumber > 26) return StringCollection[26];
                return "";
            }
            set
            {
                if (StringNumber <= 26) return;
                if (StringCollection[26] == null && value == null) return;
                if (StringCollection[26] != null && value != null && StringCollection[26].Equals(value)) return;
                StringCollection[26] = value;
                this.RaisePropertyChanged(() => this.ValueString26);
            }
        }

        public string ValueString27
        {
            get
            {
                if (StringNumber > 27) return StringCollection[27];
                return "";
            }
            set
            {
                if (StringNumber <= 27) return;
                if (StringCollection[27] == null && value == null) return;
                if (StringCollection[27] != null && value != null && StringCollection[27].Equals(value)) return;
                StringCollection[27] = value;
                this.RaisePropertyChanged(() => this.ValueString27);
            }
        }

        public string ValueString28
        {
            get
            {
                if (StringNumber > 28) return StringCollection[28];
                return "";
            }
            set
            {
                if (StringNumber <= 28) return;
                if (StringCollection[28] == null && value == null) return;
                if (StringCollection[28] != null && value != null && StringCollection[28].Equals(value)) return;
                StringCollection[28] = value;
                this.RaisePropertyChanged(() => this.ValueString28);
            }
        }

        public string ValueString29
        {
            get
            {
                if (StringNumber > 29) return StringCollection[29];
                return "";
            }
            set
            {
                if (StringNumber <= 29) return;
                if (StringCollection[29] == null && value == null) return;
                if (StringCollection[29] != null && value != null && StringCollection[29].Equals(value)) return;
                StringCollection[29] = value;
                this.RaisePropertyChanged(() => this.ValueString29);
            }
        }

        public string ValueString30
        {
            get
            {
                if (StringNumber > 30) return StringCollection[30];
                return "";
            }
            set
            {
                if (StringNumber <= 30) return;
                if (StringCollection[30] == null && value == null) return;
                if (StringCollection[30] != null && value != null && StringCollection[30].Equals(value)) return;
                StringCollection[30] = value;
                this.RaisePropertyChanged(() => this.ValueString30);
            }
        }

        #endregion
     


        #region  手动写入的属性 get  set

        public int ValueInt0
        {
            get
            {
                if (IntNumber > 0) return IntCollection[0];
                return 0;
            }
            set
            {
                if (IntNumber <= 0) return;
                if (IntCollection[0] == null && value == null) return;
                if (IntCollection[0] != null && value != null && IntCollection[0].Equals(value)) return;
                IntCollection[0] = value;
                this.RaisePropertyChanged(() => this.ValueInt0);
            }
        }


        public int ValueInt1
        {
            get
            {
                if (IntNumber > 1) return IntCollection[1];
                return 0;
            }
            set
            {
                if (IntNumber <= 1) return;
                if (IntCollection[1] == null && value == null) return;
                if (IntCollection[1] != null && value != null && IntCollection[1].Equals(value)) return;
                IntCollection[1] = value;
                this.RaisePropertyChanged(() => this.ValueInt1);
            }
        }


        public int ValueInt2
        {
            get
            {
                if (IntNumber > 2) return IntCollection[2];
                return 0;
            }
            set
            {
                if (IntNumber <= 2) return;
                if (IntCollection[2] == null && value == null) return;
                if (IntCollection[2] != null && value != null && IntCollection[2].Equals(value)) return;
                IntCollection[2] = value;
                this.RaisePropertyChanged(() => this.ValueInt2);
            }
        }


        public int ValueInt3
        {
            get
            {
                if (IntNumber > 3) return IntCollection[3];
                return 0;
            }
            set
            {
                if (IntNumber <= 3) return;
                if (IntCollection[3] == null && value == null) return;
                if (IntCollection[3] != null && value != null && IntCollection[3].Equals(value)) return;
                IntCollection[3] = value;
                this.RaisePropertyChanged(() => this.ValueInt3);
            }
        }

        public int ValueInt4
        {
            get
            {
                if (IntNumber > 4) return IntCollection[4];
                return 0;
            }
            set
            {
                if (IntNumber <= 4) return;
                if (IntCollection[4] == null && value == null) return;
                if (IntCollection[4] != null && value != null && IntCollection[4].Equals(value)) return;
                IntCollection[4] = value;
                this.RaisePropertyChanged(() => this.ValueInt4);
            }
        }

        public int ValueInt5
        {
            get
            {
                if (IntNumber > 5) return IntCollection[5];
                return 0;
            }
            set
            {
                if (IntNumber <= 5) return;
                if (IntCollection[5] == null && value == null) return;
                if (IntCollection[5] != null && value != null && IntCollection[5].Equals(value)) return;
                IntCollection[5] = value;
                this.RaisePropertyChanged(() => this.ValueInt5);
            }
        }

        public int ValueInt6
        {
            get
            {
                if (IntNumber > 6) return IntCollection[6];
                return 0;
            }
            set
            {
                if (IntNumber <= 6) return;
                if (IntCollection[6] == null && value == null) return;
                if (IntCollection[6] != null && value != null && IntCollection[6].Equals(value)) return;
                IntCollection[6] = value;
                this.RaisePropertyChanged(() => this.ValueInt6);
            }
        }

        public int ValueInt7
        {
            get
            {
                if (IntNumber > 7) return IntCollection[7];
                return 0;
            }
            set
            {
                if (IntNumber <= 7) return;
                if (IntCollection[7] == null && value == null) return;
                if (IntCollection[7] != null && value != null && IntCollection[7].Equals(value)) return;
                IntCollection[7] = value;
                this.RaisePropertyChanged(() => this.ValueInt7);
            }
        }

        public int ValueInt8
        {
            get
            {
                if (IntNumber > 8) return IntCollection[8];
                return 0;
            }
            set
            {
                if (IntNumber <= 8) return;
                if (IntCollection[8] == null && value == null) return;
                if (IntCollection[8] != null && value != null && IntCollection[8].Equals(value)) return;
                IntCollection[8] = value;
                this.RaisePropertyChanged(() => this.ValueInt8);
            }
        }

        public int ValueInt9
        {
            get
            {
                if (IntNumber > 9) return IntCollection[9];
                return 0;
            }
            set
            {
                if (IntNumber <= 9) return;
                if (IntCollection[9] == null && value == null) return;
                if (IntCollection[9] != null && value != null && IntCollection[9].Equals(value)) return;
                IntCollection[9] = value;
                this.RaisePropertyChanged(() => this.ValueInt9);
            }
        }

        public int ValueInt10
        {
            get
            {
                if (IntNumber > 10) return IntCollection[10];
                return 0;
            }
            set
            {
                if (IntNumber <= 10) return;
                if (IntCollection[10] == null && value == null) return;
                if (IntCollection[10] != null && value != null && IntCollection[10].Equals(value)) return;
                IntCollection[10] = value;
                this.RaisePropertyChanged(() => this.ValueInt10);
            }
        }


        public int ValueInt11
        {
            get
            {
                if (IntNumber > 11) return IntCollection[11];
                return 0;
            }
            set
            {
                if (IntNumber <= 11) return;
                if (IntCollection[11] == null && value == null) return;
                if (IntCollection[11] != null && value != null && IntCollection[11].Equals(value)) return;
                IntCollection[11] = value;
                this.RaisePropertyChanged(() => this.ValueInt11);
            }
        }

        public int ValueInt12
        {
            get
            {
                if (IntNumber > 12) return IntCollection[12];
                return 0;
            }
            set
            {
                if (IntNumber <= 12) return;
                if (IntCollection[12] == null && value == null) return;
                if (IntCollection[12] != null && value != null && IntCollection[12].Equals(value)) return;
                IntCollection[12] = value;
                this.RaisePropertyChanged(() => this.ValueInt12);
            }
        }

        public int ValueInt13
        {
            get
            {
                if (IntNumber > 13) return IntCollection[13];
                return 0;
            }
            set
            {
                if (IntNumber <= 13) return;
                if (IntCollection[13] == null && value == null) return;
                if (IntCollection[13] != null && value != null && IntCollection[13].Equals(value)) return;
                IntCollection[13] = value;
                this.RaisePropertyChanged(() => this.ValueInt13);
            }
        }

        public int ValueInt14
        {
            get
            {
                if (IntNumber > 14) return IntCollection[14];
                return 0;
            }
            set
            {
                if (IntNumber <= 14) return;
                if (IntCollection[14] == null && value == null) return;
                if (IntCollection[14] != null && value != null && IntCollection[14].Equals(value)) return;
                IntCollection[14] = value;
                this.RaisePropertyChanged(() => this.ValueInt14);
            }
        }

        public int ValueInt15
        {
            get
            {
                if (IntNumber > 15) return IntCollection[15];
                return 0;
            }
            set
            {
                if (IntNumber <= 15) return;
                if (IntCollection[15] == null && value == null) return;
                if (IntCollection[15] != null && value != null && IntCollection[15].Equals(value)) return;
                IntCollection[15] = value;
                this.RaisePropertyChanged(() => this.ValueInt15);
            }
        }

        public int ValueInt16
        {
            get
            {
                if (IntNumber > 16) return IntCollection[16];
                return 0;
            }
            set
            {
                if (IntNumber <= 16) return;
                if (IntCollection[16] == null && value == null) return;
                if (IntCollection[16] != null && value != null && IntCollection[16].Equals(value)) return;
                IntCollection[16] = value;
                this.RaisePropertyChanged(() => this.ValueInt16);
            }
        }

        public int ValueInt17
        {
            get
            {
                if (IntNumber > 17) return IntCollection[17];
                return 0;
            }
            set
            {
                if (IntNumber <= 17) return;
                if (IntCollection[17] == null && value == null) return;
                if (IntCollection[17] != null && value != null && IntCollection[17].Equals(value)) return;
                IntCollection[17] = value;
                this.RaisePropertyChanged(() => this.ValueInt17);
            }
        }

        public int ValueInt18
        {
            get
            {
                if (IntNumber > 18) return IntCollection[18];
                return 0;
            }
            set
            {
                if (IntNumber <= 18) return;
                if (IntCollection[18] == null && value == null) return;
                if (IntCollection[18] != null && value != null && IntCollection[18].Equals(value)) return;
                IntCollection[18] = value;
                this.RaisePropertyChanged(() => this.ValueInt18);
            }
        }

        public int ValueInt19
        {
            get
            {
                if (IntNumber > 19) return IntCollection[19];
                return 0;
            }
            set
            {
                if (IntNumber <= 19) return;
                if (IntCollection[19] == null && value == null) return;
                if (IntCollection[19] != null && value != null && IntCollection[19].Equals(value)) return;
                IntCollection[19] = value;
                this.RaisePropertyChanged(() => this.ValueInt19);
            }
        }

        public int ValueInt20
        {
            get
            {
                if (IntNumber > 20) return IntCollection[20];
                return 0;
            }
            set
            {
                if (IntNumber <= 20) return;
                if (IntCollection[20] == null && value == null) return;
                if (IntCollection[20] != null && value != null && IntCollection[20].Equals(value)) return;
                IntCollection[20] = value;
                this.RaisePropertyChanged(() => this.ValueInt20);
            }
        }

        public int ValueInt21
        {
            get
            {
                if (IntNumber > 21) return IntCollection[21];
                return 0;
            }
            set
            {
                if (IntNumber <= 21) return;
                if (IntCollection[21] == null && value == null) return;
                if (IntCollection[21] != null && value != null && IntCollection[21].Equals(value)) return;
                IntCollection[21] = value;
                this.RaisePropertyChanged(() => this.ValueInt21);
            }
        }

        public int ValueInt22
        {
            get
            {
                if (IntNumber > 22) return IntCollection[22];
                return 0;
            }
            set
            {
                if (IntNumber <= 22) return;
                if (IntCollection[22] == null && value == null) return;
                if (IntCollection[22] != null && value != null && IntCollection[22].Equals(value)) return;
                IntCollection[22] = value;
                this.RaisePropertyChanged(() => this.ValueInt22);
            }
        }

        public int ValueInt23
        {
            get
            {
                if (IntNumber > 23) return IntCollection[23];
                return 0;
            }
            set
            {
                if (IntNumber <= 23) return;
                if (IntCollection[23] == null && value == null) return;
                if (IntCollection[23] != null && value != null && IntCollection[23].Equals(value)) return;
                IntCollection[23] = value;
                this.RaisePropertyChanged(() => this.ValueInt23);
            }
        }

        public int ValueInt24
        {
            get
            {
                if (IntNumber > 24) return IntCollection[24];
                return 0;
            }
            set
            {
                if (IntNumber <= 24) return;
                if (IntCollection[24] == null && value == null) return;
                if (IntCollection[24] != null && value != null && IntCollection[24].Equals(value)) return;
                IntCollection[24] = value;
                this.RaisePropertyChanged(() => this.ValueInt24);
            }
        }

        public int ValueInt25
        {
            get
            {
                if (IntNumber > 25) return IntCollection[25];
                return 0;
            }
            set
            {
                if (IntNumber <= 25) return;
                if (IntCollection[25] == null && value == null) return;
                if (IntCollection[25] != null && value != null && IntCollection[25].Equals(value)) return;
                IntCollection[25] = value;
                this.RaisePropertyChanged(() => this.ValueInt25);
            }
        }

        public int ValueInt26
        {
            get
            {
                if (IntNumber > 26) return IntCollection[26];
                return 0;
            }
            set
            {
                if (IntNumber <= 26) return;
                if (IntCollection[26] == null && value == null) return;
                if (IntCollection[26] != null && value != null && IntCollection[26].Equals(value)) return;
                IntCollection[26] = value;
                this.RaisePropertyChanged(() => this.ValueInt26);
            }
        }

        public int ValueInt27
        {
            get
            {
                if (IntNumber > 27) return IntCollection[27];
                return 0;
            }
            set
            {
                if (IntNumber <= 27) return;
                if (IntCollection[27] == null && value == null) return;
                if (IntCollection[27] != null && value != null && IntCollection[27].Equals(value)) return;
                IntCollection[27] = value;
                this.RaisePropertyChanged(() => this.ValueInt27);
            }
        }

        public int ValueInt28
        {
            get
            {
                if (IntNumber > 28) return IntCollection[28];
                return 0;
            }
            set
            {
                if (IntNumber <= 28) return;
                if (IntCollection[28] == null && value == null) return;
                if (IntCollection[28] != null && value != null && IntCollection[28].Equals(value)) return;
                IntCollection[28] = value;
                this.RaisePropertyChanged(() => this.ValueInt28);
            }
        }

        public int ValueInt29
        {
            get
            {
                if (IntNumber > 29) return IntCollection[29];
                return 0;
            }
            set
            {
                if (IntNumber <= 29) return;
                if (IntCollection[29] == null && value == null) return;
                if (IntCollection[29] != null && value != null && IntCollection[29].Equals(value)) return;
                IntCollection[29] = value;
                this.RaisePropertyChanged(() => this.ValueInt29);
            }
        }

        public int ValueInt30
        {
            get
            {
                if (IntNumber > 30) return IntCollection[30];
                return 0;
            }
            set
            {
                if (IntNumber <= 30) return;
                if (IntCollection[30] == null && value == null) return;
                if (IntCollection[30] != null && value != null && IntCollection[30].Equals(value)) return;
                IntCollection[30] = value;
                this.RaisePropertyChanged(() => this.ValueInt30);
            }
        }

        #endregion



        #region 析构函数

        ~ObservableObjectCollection()
        {
            try
            {
                StringCollection = null;
            }
            catch (Exception ex)
            {

            }
        }


        public void CleanAttri()
        {
            try
            {
                StringCollection = null;
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

   //#region  手动写入的属性 get  set

   //     public T Value0
   //     {
   //         get
   //         {
   //             if (Number > 0) return StringCollection[0];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 0) return;
   //             if (StringCollection[0] == null && value == null) return;
   //             if (StringCollection[0] != null && value != null && StringCollection[0].Equals(value)) return;
   //             StringCollection[0] = value;
   //             this.RaisePropertyChanged(() => this.Value0);
   //         }
   //     }


   //     public T Value1
   //     {
   //         get
   //         {
   //             if (Number > 1) return StringCollection[1];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 1) return;
   //             if (StringCollection[1] == null && value == null) return;
   //             if (StringCollection[1] != null && value != null && StringCollection[1].Equals(value)) return;
   //             StringCollection[1] = value;
   //             this.RaisePropertyChanged(() => this.Value1);
   //         }
   //     }


   //     public T Value2
   //     {
   //         get
   //         {
   //             if (Number > 2) return StringCollection[2];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 2) return;
   //             if (StringCollection[2] == null && value == null) return;
   //             if (StringCollection[2] != null && value != null && StringCollection[2].Equals(value)) return;
   //             StringCollection[2] = value;
   //             this.RaisePropertyChanged(() => this.Value2);
   //         }
   //     }


   //     public T Value3
   //     {
   //         get
   //         {
   //             if (Number > 3) return StringCollection[3];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 3) return;
   //             if (StringCollection[3] == null && value == null) return;
   //             if (StringCollection[3] != null && value != null && StringCollection[3].Equals(value)) return;
   //             StringCollection[3] = value;
   //             this.RaisePropertyChanged(() => this.Value3);
   //         }
   //     }

   //     public T Value4
   //     {
   //         get
   //         {
   //             if (Number > 4) return StringCollection[4];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 4) return;
   //             if (StringCollection[4] == null && value == null) return;
   //             if (StringCollection[4] != null && value != null && StringCollection[4].Equals(value)) return;
   //             StringCollection[4] = value;
   //             this.RaisePropertyChanged(() => this.Value4);
   //         }
   //     }

   //     public T Value5
   //     {
   //         get
   //         {
   //             if (Number > 5) return StringCollection[5];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 5) return;
   //             if (StringCollection[5] == null && value == null) return;
   //             if (StringCollection[5] != null && value != null && StringCollection[5].Equals(value)) return;
   //             StringCollection[5] = value;
   //             this.RaisePropertyChanged(() => this.Value5);
   //         }
   //     }

   //     public T Value6
   //     {
   //         get
   //         {
   //             if (Number > 6) return StringCollection[6];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 6) return;
   //             if (StringCollection[6] == null && value == null) return;
   //             if (StringCollection[6] != null && value != null && StringCollection[6].Equals(value)) return;
   //             StringCollection[6] = value;
   //             this.RaisePropertyChanged(() => this.Value6);
   //         }
   //     }

   //     public T Value7
   //     {
   //         get
   //         {
   //             if (Number > 7) return StringCollection[7];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 7) return;
   //             if (StringCollection[7] == null && value == null) return;
   //             if (StringCollection[7] != null && value != null && StringCollection[7].Equals(value)) return;
   //             StringCollection[7] = value;
   //             this.RaisePropertyChanged(() => this.Value7);
   //         }
   //     }

   //     public T Value8
   //     {
   //         get
   //         {
   //             if (Number > 8) return StringCollection[8];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 8) return;
   //             if (StringCollection[8] == null && value == null) return;
   //             if (StringCollection[8] != null && value != null && StringCollection[8].Equals(value)) return;
   //             StringCollection[8] = value;
   //             this.RaisePropertyChanged(() => this.Value8);
   //         }
   //     }

   //     public T Value9
   //     {
   //         get
   //         {
   //             if (Number > 9) return StringCollection[9];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 9) return;
   //             if (StringCollection[9] == null && value == null) return;
   //             if (StringCollection[9] != null && value != null && StringCollection[9].Equals(value)) return;
   //             StringCollection[9] = value;
   //             this.RaisePropertyChanged(() => this.Value9);
   //         }
   //     }

   //     public T Value10
   //     {
   //         get
   //         {
   //             if (Number > 10) return StringCollection[10];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 10) return;
   //             if (StringCollection[10] == null && value == null) return;
   //             if (StringCollection[10] != null && value != null && StringCollection[10].Equals(value)) return;
   //             StringCollection[10] = value;
   //             this.RaisePropertyChanged(() => this.Value10);
   //         }
   //     }


   //     public T Value11
   //     {
   //         get
   //         {
   //             if (Number > 11) return StringCollection[11];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 11) return;
   //             if (StringCollection[11] == null && value == null) return;
   //             if (StringCollection[11] != null && value != null && StringCollection[11].Equals(value)) return;
   //             StringCollection[11] = value;
   //             this.RaisePropertyChanged(() => this.Value11);
   //         }
   //     }

   //     public T Value12
   //     {
   //         get
   //         {
   //             if (Number > 12) return StringCollection[12];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 12) return;
   //             if (StringCollection[12] == null && value == null) return;
   //             if (StringCollection[12] != null && value != null && StringCollection[12].Equals(value)) return;
   //             StringCollection[12] = value;
   //             this.RaisePropertyChanged(() => this.Value12);
   //         }
   //     }

   //     public T Value13
   //     {
   //         get
   //         {
   //             if (Number > 13) return StringCollection[13];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 13) return;
   //             if (StringCollection[13] == null && value == null) return;
   //             if (StringCollection[13] != null && value != null && StringCollection[13].Equals(value)) return;
   //             StringCollection[13] = value;
   //             this.RaisePropertyChanged(() => this.Value13);
   //         }
   //     }

   //     public T Value14
   //     {
   //         get
   //         {
   //             if (Number > 14) return StringCollection[14];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 14) return;
   //             if (StringCollection[14] == null && value == null) return;
   //             if (StringCollection[14] != null && value != null && StringCollection[14].Equals(value)) return;
   //             StringCollection[14] = value;
   //             this.RaisePropertyChanged(() => this.Value14);
   //         }
   //     }

   //     public T Value15
   //     {
   //         get
   //         {
   //             if (Number > 15) return StringCollection[15];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 15) return;
   //             if (StringCollection[15] == null && value == null) return;
   //             if (StringCollection[15] != null && value != null && StringCollection[15].Equals(value)) return;
   //             StringCollection[15] = value;
   //             this.RaisePropertyChanged(() => this.Value15);
   //         }
   //     }

   //     public T Value16
   //     {
   //         get
   //         {
   //             if (Number > 16) return StringCollection[16];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 16) return;
   //             if (StringCollection[16] == null && value == null) return;
   //             if (StringCollection[16] != null && value != null && StringCollection[16].Equals(value)) return;
   //             StringCollection[16] = value;
   //             this.RaisePropertyChanged(() => this.Value16);
   //         }
   //     }

   //     public T Value17
   //     {
   //         get
   //         {
   //             if (Number > 17) return StringCollection[17];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 17) return;
   //             if (StringCollection[17] == null && value == null) return;
   //             if (StringCollection[17] != null && value != null && StringCollection[17].Equals(value)) return;
   //             StringCollection[17] = value;
   //             this.RaisePropertyChanged(() => this.Value17);
   //         }
   //     }

   //     public T Value18
   //     {
   //         get
   //         {
   //             if (Number > 18) return StringCollection[18];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 18) return;
   //             if (StringCollection[18] == null && value == null) return;
   //             if (StringCollection[18] != null && value != null && StringCollection[18].Equals(value)) return;
   //             StringCollection[18] = value;
   //             this.RaisePropertyChanged(() => this.Value18);
   //         }
   //     }

   //     public T Value19
   //     {
   //         get
   //         {
   //             if (Number > 19) return StringCollection[19];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 19) return;
   //             if (StringCollection[19] == null && value == null) return;
   //             if (StringCollection[19] != null && value != null && StringCollection[19].Equals(value)) return;
   //             StringCollection[19] = value;
   //             this.RaisePropertyChanged(() => this.Value19);
   //         }
   //     }

   //     public T Value20
   //     {
   //         get
   //         {
   //             if (Number > 20) return StringCollection[20];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 20) return;
   //             if (StringCollection[20] == null && value == null) return;
   //             if (StringCollection[20] != null && value != null && StringCollection[20].Equals(value)) return;
   //             StringCollection[20] = value;
   //             this.RaisePropertyChanged(() => this.Value20);
   //         }
   //     }

   //     public T Value21
   //     {
   //         get
   //         {
   //             if (Number > 21) return StringCollection[21];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 21) return;
   //             if (StringCollection[21] == null && value == null) return;
   //             if (StringCollection[21] != null && value != null && StringCollection[21].Equals(value)) return;
   //             StringCollection[21] = value;
   //             this.RaisePropertyChanged(() => this.Value21);
   //         }
   //     }

   //     public T Value22
   //     {
   //         get
   //         {
   //             if (Number > 22) return StringCollection[22];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 22) return;
   //             if (StringCollection[22] == null && value == null) return;
   //             if (StringCollection[22] != null && value != null && StringCollection[22].Equals(value)) return;
   //             StringCollection[22] = value;
   //             this.RaisePropertyChanged(() => this.Value22);
   //         }
   //     }

   //     public T Value23
   //     {
   //         get
   //         {
   //             if (Number > 23) return StringCollection[23];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 23) return;
   //             if (StringCollection[23] == null && value == null) return;
   //             if (StringCollection[23] != null && value != null && StringCollection[23].Equals(value)) return;
   //             StringCollection[23] = value;
   //             this.RaisePropertyChanged(() => this.Value23);
   //         }
   //     }

   //     public T Value24
   //     {
   //         get
   //         {
   //             if (Number > 24) return StringCollection[24];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 24) return;
   //             if (StringCollection[24] == null && value == null) return;
   //             if (StringCollection[24] != null && value != null && StringCollection[24].Equals(value)) return;
   //             StringCollection[24] = value;
   //             this.RaisePropertyChanged(() => this.Value24);
   //         }
   //     }

   //     public T Value25
   //     {
   //         get
   //         {
   //             if (Number > 25) return StringCollection[25];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 25) return;
   //             if (StringCollection[25] == null && value == null) return;
   //             if (StringCollection[25] != null && value != null && StringCollection[25].Equals(value)) return;
   //             StringCollection[25] = value;
   //             this.RaisePropertyChanged(() => this.Value25);
   //         }
   //     }

   //     public T Value26
   //     {
   //         get
   //         {
   //             if (Number > 26) return StringCollection[26];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 26) return;
   //             if (StringCollection[26] == null && value == null) return;
   //             if (StringCollection[26] != null && value != null && StringCollection[26].Equals(value)) return;
   //             StringCollection[26] = value;
   //             this.RaisePropertyChanged(() => this.Value26);
   //         }
   //     }

   //     public T Value27
   //     {
   //         get
   //         {
   //             if (Number > 27) return StringCollection[27];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 27) return;
   //             if (StringCollection[27] == null && value == null) return;
   //             if (StringCollection[27] != null && value != null && StringCollection[27].Equals(value)) return;
   //             StringCollection[27] = value;
   //             this.RaisePropertyChanged(() => this.Value27);
   //         }
   //     }

   //     public T Value28
   //     {
   //         get
   //         {
   //             if (Number > 28) return StringCollection[28];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 28) return;
   //             if (StringCollection[28] == null && value == null) return;
   //             if (StringCollection[28] != null && value != null && StringCollection[28].Equals(value)) return;
   //             StringCollection[28] = value;
   //             this.RaisePropertyChanged(() => this.Value28);
   //         }
   //     }

   //     public T Value29
   //     {
   //         get
   //         {
   //             if (Number > 29) return StringCollection[29];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 29) return;
   //             if (StringCollection[29] == null && value == null) return;
   //             if (StringCollection[29] != null && value != null && StringCollection[29].Equals(value)) return;
   //             StringCollection[29] = value;
   //             this.RaisePropertyChanged(() => this.Value29);
   //         }
   //     }

   //     public T Value30
   //     {
   //         get
   //         {
   //             if (Number > 30) return StringCollection[30];
   //             return default(T);
   //         }
   //         set
   //         {
   //             if (Number <= 30) return;
   //             if (StringCollection[30] == null && value == null) return;
   //             if (StringCollection[30] != null && value != null && StringCollection[30].Equals(value)) return;
   //             StringCollection[30] = value;
   //             this.RaisePropertyChanged(() => this.Value30);
   //         }
   //     }

   //     #endregion

    }

}
