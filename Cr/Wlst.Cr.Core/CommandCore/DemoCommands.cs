using System;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel.Composition;

namespace Wlst.Cr.Core.CommandCore
{
    [Serializable]
    public static class DemoCommands
    {
        //private static CompositeCommand submitOrderCommand = new CompositeCommand(true);
        //private static CompositeCommand cancelOrderCommand = new CompositeCommand(true);
        private static CompositeCommand _submitAllOrdersCommand = new CompositeCommand();
        private static CompositeCommand _cancelAllOrdersCommand = new CompositeCommand();

        
        //public static CompositeCommand SubmitOrderCommand
        //{
        //    get { return submitOrderCommand; }
        //    set { submitOrderCommand = value; }
        //}

        //public static CompositeCommand CancelOrderCommand
        //{
        //    get { return cancelOrderCommand; }
        //    set { cancelOrderCommand = value; }
        //}

        public static CompositeCommand SubmitAllOrdersCommand
        {
            get { return _submitAllOrdersCommand; }
            set { _submitAllOrdersCommand = value; }
        }

        public static CompositeCommand CancelAllOrdersCommand
        {
            get { return _cancelAllOrdersCommand; }
            set { _cancelAllOrdersCommand = value; }
        }
    }

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DemoCommandProxy
    {
        //virtual public CompositeCommand SubmitOrderCommand
        //{
        //    get { return StockTraderRICommands.SubmitOrderCommand; }
        //}

        //virtual public CompositeCommand CancelOrderCommand
        //{
        //    get { return StockTraderRICommands.CancelOrderCommand; }
        //}

        virtual public CompositeCommand SubmitAllOrdersCommand
        {
            get { return DemoCommands.SubmitAllOrdersCommand; }
        }

        virtual public CompositeCommand CancelAllOrdersCommand
        {
            get { return DemoCommands.CancelAllOrdersCommand; }
        }
    }
}
