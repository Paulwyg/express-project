namespace Wlst.Cr.Core.CoreInterface
{
   /// <summary>
   /// 任何界面需要执行导航前初始化，获取前初始化则必须实现的接口
   /// </summary>
   public  interface IINavOnLoad
   {
       /// <summary>
       /// 当导航到某页面的时候如果该页面实现了本接口则会在导航到本页面的时候立即执行本函数
       /// </summary>
       /// <param name="parsObjects"></param>
       void NavOnLoad(params object[] parsObjects);

   }
}
