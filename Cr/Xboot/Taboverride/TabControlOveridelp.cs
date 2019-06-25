using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xboot.Taboverride
{
    public class TabControlOveridelp : System.Windows.Controls.TabControl
    {
        public TabControlOveridelp()
            : base()
        {

        }

        /// <summary>
        /// 当内容发生变化的时候呈现
        /// </summary>
        public event EventHandler OnItemsChangeded;

        private int ItemsCount = 0;
        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            base.OnItemsChanged(e);
            if (OnItemsChangeded != null && this.Items.Count != ItemsCount)
            {
                ItemsCount = this.Items.Count;
                this.OnItemsChangeded(this, e);
            }

            //   this.SelectedIndex = 1;

        }


    }
}
