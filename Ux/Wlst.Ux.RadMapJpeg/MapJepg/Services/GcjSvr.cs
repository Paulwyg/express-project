//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Wlst.Ux.RadMapJpeg.MapJepg.Services
//{
//    /// <summary>
//    /// 火星坐标系 (GCJ-02)与百度坐标系 (BD-09) 转换帮助类
//    /// </summary>
//    public class BDGCJLatLonHelper
//    {
//        /*
//         *参考：
//         *BD09坐标系：即百度坐标系，GCJ02坐标系经加密后的坐标系。
//         */

//        #region 常量

//        private const double x_pi = 3.14159265358979324*3000.0/180.0;

//        #endregion

//        #region 将GCJ-02坐标转换成BD-09坐标

//        /// <summary>
//        /// 将GCJ-02坐标转换成BD-09坐标
//        /// </summary>
//        /// <param name="gcjPoint">GCJ-02坐标</param>
//        /// <returns>BD-09坐标</returns>
//        public static LatLngPoint GCJ02ToBD09(LatLngPoint gcjPoint)
//        {
//            LatLngPoint _bdPoint = new LatLngPoint();
//            double _x = gcjPoint.LonX, y = gcjPoint.LatY;
//            double _z = Math.Sqrt(_x*_x + y*y) + 0.00002*Math.Sin(y*x_pi);
//            double _theta = Math.Atan2(y, _x) + 0.000003*Math.Cos(_x*x_pi);
//            _bdPoint.LonX = _z*Math.Cos(_theta) + 0.0065;
//            _bdPoint.LatY = _z*Math.Cos(_theta) + 0.006;
//            return _bdPoint;
//        }

//        #endregion

//        #region 将BD-09坐标转换成GCJ-02坐标

//        /// <summary>
//        /// 将BD-09坐标转换成GCJ-02坐标
//        /// </summary>
//        /// <param name="bdPoint">BD-09坐标</param>
//        /// <returns>GCJ-02坐标</returns>
//        public static LatLngPoint BD09ToGCJ02(LatLngPoint bdPoint)
//        {
//            LatLngPoint _gcjPoint = new LatLngPoint();
//            double _x = bdPoint.LonX - 0.0065, _y = bdPoint.LatY - 0.006;
//            double _z = Math.Sqrt(_x*_x + _y*_y) - 0.00002*Math.Sin(_y*x_pi);
//            double _theta = Math.Atan2(_y, _x) - 0.000003*Math.Cos(_x*x_pi);
//            _gcjPoint.LonX = _z*Math.Cos(_theta);
//            _gcjPoint.LatY = _z*Math.Sin(_theta);
//            return _gcjPoint;
//        }

//        #endregion
//    }

//    /// <summary>
//    /// 地球坐标系 (WGS-84) 到火星坐标系 (GCJ-02) 的转换 帮助类 
//    /// </summary>
//    public class WGSGCJLatLonHelper
//    {
//        /*
//         *参考：
//         *1.http://blog.csdn.net/yorling/article/details/9175913
//         *2.http://kongxz.com/2013/10/wgs-cgj/
//         *3.https://on4wp7.codeplex.com/SourceControl/changeset/view/21483#EvilTransform.cs
//         *4.https://github.com/googollee/eviltransform
//         *5.https://github.com/shenqiliang/WGS2Mars
//         *
//         *WGS84:World Geodetic System 1984，是为GPS全球定位系统使用而建立的坐标系统。通过遍布世界的卫星观测站观测到
//         *的坐标建立，其初次WGS84的精度为1-2m，在1994年1月2号，通过10个观测站在GPS测量方法上改正，得到了WGS84
//         *（G730），G表示由GPS测量得到，730表示为GPS时间第730个周。1996年，National Imagery and Mapping Agency (NIMA) 
//         *为美国国防部 (U.S.Departemt of Defense, DoD)做了一个新的坐标系统。这样实现了新的WGS版本：WGS（G873）。其因为
//         *加入了USNO站和北京站的改正，其东部方向加入了31-39cm 的改正。所有的其他坐标都有在1分米之内的修正。
//         *
//         *GCJ-02:国家保密插件，也叫做加密插件或者加偏或者SM模组，其实就是对真实坐标系统进行人为的加偏处理，按照几行代码的算
//         *法，将真实的坐标加密成虚假的坐标，而这个加偏并不是线性的加偏，所以各地的偏移情况都会有所不同。而加密后的坐标也常
//         *被人称为火星坐标系统。GCJ-02 意味“国测局-2002”，也就是说，这是国家测绘局于2002年弄出的标准。
//         */

//        #region 常量以及私有方法

//        private const double pi = 3.14159265358979324; //Math.PI;//;
//        private const double a = 6378245.0;
//        private const double ee = 0.00669342162296594323;

//        private static double TransformLat(double lonX, double latY)
//        {
//            double _ret = -100.0 + 2.0*lonX + 3.0*latY + 0.2*latY*latY + 0.1*lonX*latY + 0.2*Math.Sqrt(Math.Abs(lonX));
//            _ret += (20.0*Math.Sin(6.0*lonX*pi) + 20.0*Math.Sin(2.0*lonX*pi))*2.0/3.0;
//            _ret += (20.0*Math.Sin(latY*pi) + 40.0*Math.Sin(latY/3.0*pi))*2.0/3.0;
//            _ret += (160.0*Math.Sin(latY/12.0*pi) + 320*Math.Sin(latY*pi/30.0))*2.0/3.0;
//            return _ret;
//        }

//        private static double TransformLon(double lonX, double latY)
//        {
//            double _ret = 300.0 + lonX + 2.0*latY + 0.1*lonX*lonX + 0.1*lonX*latY + 0.1*Math.Sqrt(Math.Abs(lonX));
//            _ret += (20.0*Math.Sin(6.0*lonX*pi) + 20.0*Math.Sin(2.0*lonX*pi))*2.0/3.0;
//            _ret += (20.0*Math.Sin(lonX*pi) + 40.0*Math.Sin(lonX/3.0*pi))*2.0/3.0;
//            _ret += (150.0*Math.Sin(lonX/12.0*pi) + 300.0*Math.Sin(lonX/30.0*pi))*2.0/3.0;
//            return _ret;
//        }

//        private static LatLngPoint Transform(LatLngPoint point)
//        {
//            LatLngPoint _transPoint = new LatLngPoint();
//            double _lat = TransformLat(point.LonX - 105.0, point.LatY - 35.0);
//            double _lon = TransformLon(point.LonX - 105.0, point.LatY - 35.0);
//            double _radLat = point.LatY/180.0*pi;
//            double _magic = Math.Sin(_radLat);
//            _magic = 1 - ee*_magic*_magic;
//            double _sqrtMagic = Math.Sqrt(_magic);
//            _lat = (_lat*180.0)/((a*(1 - ee))/(_magic*_sqrtMagic)*pi);
//            _lon = (_lon*180.0)/(a/_sqrtMagic*Math.Cos(_radLat)*pi);
//            _transPoint.LatY = _lat;
//            _transPoint.LonX = _lon;

//            return _transPoint;
//        }

//        #endregion

//        #region 火星坐标转 (GCJ-02)地球坐标(WGS-84)

//        /// <summary>
//        /// 火星坐标转 (GCJ-02)地球坐标(WGS-84)
//        /// </summary>
//        /// <param name="gcjPoint">火星坐标转 (GCJ-02)</param>
//        /// <returns>地球坐标(WGS-84)</returns>
//        public static LatLngPoint GCJ02ToWGS84(LatLngPoint gcjPoint)
//        {
//            //if (MapHelper.OutOfChina(gcjPoint))
//            //{
//            //    return gcjPoint;
//            //}
//            LatLngPoint _transPoint = Transform(gcjPoint);
//            return new LatLngPoint(gcjPoint.LatY - _transPoint.LatY, gcjPoint.LonX - _transPoint.LonX);
//        }

//        #endregion

//        #region 地球坐标(WGS-84)转火星坐标 (GCJ-02)

//        /// <summary>
//        /// 地球坐标(WGS-84)转火星坐标 (GCJ-02)
//        /// </summary>
//        /// <param name="wgsPoint">地球坐标(WGS-84)</param>
//        /// <returns>火星坐标 (GCJ-02)</returns>
//        public static LatLngPoint WGS84ToGCJ02(LatLngPoint wgsPoint)
//        {
//            //if (MapHelper.OutOfChina(wgsPoint))
//            //{
//            //    return wgsPoint;
//            //}
//            LatLngPoint _transPoint = Transform(wgsPoint);
//            return new LatLngPoint(wgsPoint.LatY + _transPoint.LatY, wgsPoint.LonX + _transPoint.LonX);
//        }

//        #endregion

//    }


//    public class LatLngPoint
//    {
//        public double LonX;
//        public double LatY;

//        public LatLngPoint(double laty, double lonx)
//        {
//            LonX = lonx;
//            LatY = laty;
//        }

//        public LatLngPoint()
//        {

//        }
//    }


//}
