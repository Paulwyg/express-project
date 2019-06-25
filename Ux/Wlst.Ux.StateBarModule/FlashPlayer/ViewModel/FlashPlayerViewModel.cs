using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Ux.StateBarModule.FlashPlayer.Services;

namespace Wlst.Ux.StateBarModule.FlashPlayer.ViewModel
{
    [Export(typeof (IIFlashPlayerView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class FlashPlayerViewModel : IIFlashPlayerView
    {
        #region IITab

        public string Title
        {
            get { return "动画"; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

        public FlashPlayerViewModel()
        {

        }

        private int rtuid = 0;

        public void NavOnLoad(params object[] parsObjects)
        {

            try
            {
                rtuid = Convert.ToInt32(parsObjects[0]);
                if (Play != null) Play(this, new PlayPahArgs(rtuid + ""));

            }
            catch (Exception ex)
            {

            }
        }

        public void OnUserHideOrClosing()
        {

            if (StopPlay != null) StopPlay(this,EventArgs .Empty );
        }

        public event EventHandler<PlayPahArgs> Play;
        public event EventHandler StopPlay;
    }
}
