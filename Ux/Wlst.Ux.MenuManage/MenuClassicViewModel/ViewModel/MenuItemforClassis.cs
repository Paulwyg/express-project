using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.MenuManage.MenuClassicViewModel.ViewModel
{
    public class MenuItemforClassis : MenuItemBase
    {
        private bool _selected;

        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    this.RaisePropertyChanged(() => this.Selected);
                }
            }
        }


        //private Visibility _visi;

        //public Visibility Visi
        //{
        //    get { return _visi; }
        //    set
        //    {
        //        if (_visi != value)
        //        {
        //            _visi = value;
        //            this.RaisePropertyChanged(() => this.Visi);
        //        }
        //    }
        //}
    }
}
