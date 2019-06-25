using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.StateBarModule.FlashPlayer.Services
{
    public interface IIFlashPlayerView : Wlst.Cr.Core.CoreInterface.IINavOnLoad,
                                         Wlst.Cr.Core.CoreInterface.IIOnHideOrClose, Wlst.Cr.Core.CoreInterface.IITab
    {
        event EventHandler<PlayPahArgs> Play;
        event EventHandler StopPlay;
    }

    public class PlayPahArgs : EventArgs
    {
        public string Path;

        public PlayPahArgs(string path)
        {
            Path = path;
        }
    }
}
