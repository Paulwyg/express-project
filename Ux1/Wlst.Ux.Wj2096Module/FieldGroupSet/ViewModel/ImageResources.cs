using System;
using System.Windows.Media.Imaging;

namespace Wlst.Ux.Wj2096Module.FieldGroupSet.Services
{
    public class ImageResources
    {
        #region tml tree node icon image

        /// <summary>
        /// Open NoErr 1
        /// </summary>
        private static BitmapImage TmlOpenWithNoErr =Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("3007");


        /// <summary>
        /// Open Err 2
        /// </summary>
        private static BitmapImage TmlOpenWithErr =Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("3008"); 

        /// <summary>
        /// Close NoErr 3
        /// </summary>
        private static BitmapImage TmlCloseWithNoErr =Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("3005");
        

        /// <summary>
        /// Close Err 4 
        /// </summary>
        private static BitmapImage TmlCloseWithErr =Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("3006");
          

        /// <summary>
        /// Stop 5 
        /// </summary>
        private static BitmapImage TmlStopUse =Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("3002"); 

        /// <summary>
        /// No Use 6
        /// </summary>
        private static BitmapImage TmlNotUse =Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("3001"); 

        /// <summary>
        /// <para>1 TmlOpenWithNoErr</para>
        /// <para>2 TmlOpenWithErr</para>
        /// <para>3 TmlCloseWithNoErr</para>
        /// <para>4 TmlCloseWithErr</para>
        /// <para>5 TmlStopUse</para>
        /// <para>6 TmlNotUse</para>
        /// </summary>
        /// <param name="state">
        /// <para>1 TmlOpenWithNoErr</para>
        /// <para>2 TmlOpenWithErr</para>
        /// <para>3 TmlCloseWithNoErr</para>
        /// <para>4 TmlCloseWithErr</para>
        /// <para>5 TmlStopUse</para>
        /// <para>6 TmlNotUse</para>
        /// </param>
        /// <returns>BitmapImage or Null</returns>
        public static BitmapImage GetTmlTreeIcon(int state)
        {
            switch (state)
            {
                case 1:
                    return TmlOpenWithNoErr;
                    break;
                case 2:
                    return TmlOpenWithErr;
                    break;
                case 3:
                    return TmlCloseWithNoErr;
                    break;
                case 4:
                    return TmlCloseWithErr;
                    break;
                case 5:
                    return TmlStopUse;
                    break;
                case 6:
                    return TmlNotUse;
                    break;
                default:
                    return null;
            }
            return null;
        }

        #endregion

        /// <summary>
        /// Open NoErr 1
        /// </summary>
        public static BitmapImage FieldIcon =Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage(2090);

        public static BitmapImage CtrlIcon =Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage(20902);


           
       
    }
}
