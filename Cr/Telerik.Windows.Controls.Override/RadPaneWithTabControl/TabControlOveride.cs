using System;
using System.Windows.Controls;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;


namespace Telerik.Windows.Controls.Override.RadPaneWithTabControl
{
    public class TabControlOveride : System.Windows.Controls.TabControl
    {
        public TabControlOveride()
            : base()
        {

        }

        /// <summary>
        /// 当内容发生变化的时候呈现
        /// </summary>
        public event EventHandler OnItemsChangeded;


        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (OnItemsChangeded != null)
            {
                this.OnItemsChangeded(this, e);
            }
        }


        public event EventHandler Pined;



        
        #region pin

        private ICommand _pin;

        public ICommand Pin
        {
            get
            {
                if (_pin == null) _pin = new RelayCommandtmp(Pinframe, CanPinframe);
                return _pin;

            }
        }


        private void Pinframe()
        {
            try
            {
                if (Pined != null)
                    this.Pined(this, EventArgs.Empty);
            }
            catch (Exception)
            {
                //throw;
            }
        }

        bool CanPinframe()
        {
            if (Pined == null) return false;
            return true;
        }

        #endregion


        #region close

        private ICommand _close;

        public ICommand Close
        {
            get
            {
                if (_close == null) _close = new RelayCommandtmp(Closeframe, CanCloseframe);
                return _close;

            }
        }

        private void Closeframe()
        {
            try
            {
                this.Items.Remove(this.SelectedItem);
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private bool CanCloseframe()
        {
            if (this.SelectedItem == null) return false;
            var gg = this.SelectedItem as RadPane;
            if (gg != null)
            {
                return gg.CanUserClose;
            }
            var ff = this.SelectedItem as UserControl;
            if (ff != null)
            {
                var ffdc = ff.DataContext as IITab;
                if (ffdc != null)
                {
                    return ffdc.CanClose;
                }
            }
            return true;
        }

        #endregion


    }
}
