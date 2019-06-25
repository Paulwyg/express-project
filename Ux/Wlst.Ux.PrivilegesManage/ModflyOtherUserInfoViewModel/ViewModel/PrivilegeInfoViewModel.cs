namespace Wlst.Ux.PrivilegesManage.ModflyOtherUserInfoViewModel.ViewModel
{
    public class PrivilegeInfoViewModel : Cr.Core.CoreServices.ObservableObject
    {
        #region Attri privilege

        private int _id;

        /// <summary>
        /// Control Value
        /// </summary>
        public int Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                RaisePropertyChanged(() => Id);
            }
        }


        private string _description;

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        private bool _isCanRead;

        /// <summary>
        /// Can Read or Show
        /// </summary>
        public bool IsCanReadOrShow
        {
            get { return _isCanRead; }
            set
            {
                if (value == _isCanRead) return;
                if(!value)
                {
                    IsCanWriteOrOperator = false;
                }
                _isCanRead = value;
                RaisePropertyChanged(() => IsCanReadOrShow);
            }
        }

        private bool _isCanWriteOrOperator;

        /// <summary>
        /// Can Wirte or Operator
        /// </summary>
        public bool IsCanWriteOrOperator
        {
            get { return _isCanWriteOrOperator; }
            set
            {
                if (value == _isCanWriteOrOperator) return;
                _isCanWriteOrOperator = value;
                RaisePropertyChanged(() => IsCanWriteOrOperator);
                if (value)
                {
                    IsCanReadOrShow = true;
                }
            }
        }

        #endregion

        public PrivilegeInfoViewModel()
        {
            Id = 0;
            Description = "NotSet";
            IsCanReadOrShow = false;
            IsCanWriteOrOperator = false;
        }

        public PrivilegeInfoViewModel(int id, string description)
        {
            Id = id;
            Description = description;
            IsCanReadOrShow = false;
            IsCanWriteOrOperator = false;
        }

        public PrivilegeInfoViewModel(int id, string description, bool canReadOrShow, bool canWriteOrOperator)
        {
            Id = id;
            Description = description;
            IsCanReadOrShow = canReadOrShow;
            IsCanWriteOrOperator = canWriteOrOperator;
        }
    }
}
