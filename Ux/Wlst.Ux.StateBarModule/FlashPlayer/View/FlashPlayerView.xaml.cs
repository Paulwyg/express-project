using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.StateBarModule.FlashPlayer.Services;

namespace Wlst.Ux.StateBarModule.FlashPlayer.View
{
    /// <summary>
    /// FlashPlayerView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(AttachNow = false, ID = Wlst.Ux.StateBarModule.Services.ViewIdAssign.FlashPlayerViewId,
        AttachRegion = Wlst.Ux.StateBarModule.Services.ViewIdAssign.FlashPlayerViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class FlashPlayerView : UserControl
    {
        private AxShockwaveFlashObjects.AxShockwaveFlash axShockwaveFlash;

        public FlashPlayerView()
        {
            InitializeComponent();

            //axShockwaveFlash = new AxShockwaveFlashObjects.AxShockwaveFlash();
            //windowsFormsHost1.Child = axShockwaveFlash;

          
        }

        private void Model_StopPlay(object sender, EventArgs e)
        {
            if (first)
            {
                axShockwaveFlash = new AxShockwaveFlashObjects.AxShockwaveFlash();
                windowsFormsHost1.Child = axShockwaveFlash;
                first = false;
            }

           //throw new NotImplementedException();
            this.Stop();
        }

        private void Model_Play(object sender, PlayPahArgs e)
        {

            if (first)
            {
                axShockwaveFlash = new AxShockwaveFlashObjects.AxShockwaveFlash();
                windowsFormsHost1.Child = axShockwaveFlash;
                first = false;
            }

            string swfPath = System.Environment.CurrentDirectory;
            swfPath += @"\FlashPlayer\xxx.swf";
            //throw new NotImplementedException();
            this.Show(swfPath);
        }


        [Import]
        public IIFlashPlayerView Model
        {
            get { return DataContext as IIFlashPlayerView; }
            set
            {
                DataContext = value;

               
                value.Play += new EventHandler<PlayPahArgs>(Model_Play);
                value.StopPlay += new EventHandler(Model_StopPlay);
            }
        }

        private bool first = true;
        private void Show(string swfPath)
        {
            try
            {
               
                //string swfPath = System.Environment.CurrentDirectory;
                //swfPath += @"\xxxx\xxx.swf";
                axShockwaveFlash.Movie = swfPath;
            }
            catch (Exception ex)
            {

            }
        }


        private void Stop()
        {
            axShockwaveFlash.Stop();

            try
            {
                axShockwaveFlash.Stop();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
