//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Windows;

//namespace Wlst.Cr.CoreOne.Services
//{

//    /// <summary>
//    /// 异步添加数据
//    /// </summary>
//    public class AsynObservableCollectionAdd
//    {


//        private delegate void CollectionInsertItem<T>(ObservableCollection<T> t, T item, int index);

//        private delegate void CollectionInsertItems<T>(ObservableCollection<T> t, List<T> itemd);


//        /// <summary>
//        /// 添加
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="collection"></param>
//        /// <param name="item"></param>
//        /// <param name="index"> 为0或不写入则为add</param>
//        public static void Insert<T>(ObservableCollection<T> collection, T item, int index = 0)
//        {
//            Application.Current.Dispatcher.Invoke(
//                System.Windows.Threading.DispatcherPriority.Normal, new CollectionInsertItem<T>(InsertInstances),
//                collection, item, index);
//        }

//        private static void InsertInstances<T>(ObservableCollection<T> collection, T item, int index)
//        {
//            if (collection == null) collection = new ObservableCollection<T>();
//            if (collection.Count == 0)
//            {
//                collection.Add(item);
//                return;
//            }
//            if (collection.Count <= index)
//            {
//                collection.Add(item);
//                return;
//            }
//            collection.Insert(index, item);

//        }






        

//        /// <summary>
//        /// 添加
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="collection"></param>
//        /// <param name="items"></param>
//        public static void Inserts<T>(ObservableCollection<T> collection, List<T> items)
//        {
            
//            Application.Current.Dispatcher.Invoke(
//                System.Windows.Threading.DispatcherPriority.Normal, new CollectionInsertItems<T>(InsertsInstances),
//                collection, items);
//        }

//        private static void InsertsInstances<T>(ObservableCollection<T> collection, List<T> item)
//        {
//            if (collection == null) collection = new ObservableCollection<T>();
//            foreach (var t in item)
//                collection.Add(t);
//        }
//    }
//}
