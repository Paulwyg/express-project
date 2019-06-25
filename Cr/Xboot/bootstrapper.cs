using System;
using System.Windows;
using Microsoft.Practices.Prism.MefExtensions;
using System.ComponentModel.Composition.Hosting;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Events;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.PrismAdapter.RadDockBehaviors;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.Core.UtilityFunction;

namespace Xboot
{
    public class Bootstrapper : MefBootstrapper
    {
        private static AggregateCatalog _aggregateCatalog;

        protected override void ConfigureAggregateCatalog()
        {

            Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapFrame("123");
            Wlst.Cr.CoreOne.Services.ImageSourceHelper.MySelf.GetIconById(0);


       //     Wlst.Cr.Core.CoreServices.RegionManage.SetLoadInner();
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof (Bootstrapper).Assembly));
            //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof (CETC50_Core.CrCore).Assembly));
            ////this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(AutoPopulateExportedViewsBehavior).Assembly));

            
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(IEventAggregator).Assembly));

            var ff = ModuleManage.ModuleMangeInatance.FirstLoad();
            foreach (var f in ff)
            {
                if (f.AssemblyConfigInfo != null)
                {
                    if (f.Catalog != null)
                    {
                        this.AggregateCatalog.Catalogs.Add(f.Catalog);
                    }
                }
            }

            //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Infrastructure.InfrastructureModule).Assembly));

            //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Core_ModuleConfig.CoreModuleConfig).Assembly));
            //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Core_Menu.CoreMenu).Assembly));
            //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Base_MenuControl.BaseMenuControl).Assembly));
            //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Model_MenuManage.ModelMenuManage).Assembly));
            //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Core_Socket .CoreSocket ).Assembly));
            //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Login.LoginModule).Assembly));


            _aggregateCatalog = this.AggregateCatalog;
        }


        ///// <summary>
        ///// 为外部提供程序集装载功能
        ///// </summary>
        ///// <param name="composablePartCatalog"> </param>
        [Export("AddModule", typeof (Action<ComposablePartCatalog>))]
        public void AddModule(ComposablePartCatalog composablePartCatalog)
        {
            //var t = typeof (WJ3005Module.WJ3005Module).Assembly;

            try
            {
                if (!_aggregateCatalog.Catalogs.Contains(composablePartCatalog))
                {
                    //var tr = composablePartCatalog.Parts;
                    //foreach (var ff in composablePartCatalog.Parts)
                    //{
                    //    string varrr = ff.ToString();
                    //}
                    //var g = _mmoduleCatalog  ;

                    _aggregateCatalog.Catalogs.Add(composablePartCatalog);

                    //var f = _mmoduleCatalog;
                }
            }
            catch (Exception exception)
            {
                WriteLog.WriteLogError("Core AddModule Occer An Super Error:" + exception.ToString());
            }
        }

        [Export("RemoveModule", typeof (Action<ComposablePartCatalog>))]
        public void RemoveModule(ComposablePartCatalog composablePartCatalog)
        {
            try
            {
                if (_aggregateCatalog.Catalogs.Contains(composablePartCatalog))
                {
                   
                    _aggregateCatalog.Catalogs.Remove(composablePartCatalog);
                  
                    // composablePartCatalog.Dispose();
                    //  var ls = MenuComponentHolding._menuItems;
                }
            }
            catch (Exception exception)
            {
                WriteLog.WriteLogError("Core RemoveModule Occer An Super Error:" + exception.ToString());
            }
        }

        public override void Run(bool runWithDefaultConfiguration)
        {
            base.Run(runWithDefaultConfiguration);
            Wlst.Cr.Core.Behavior.AutoPopulateExportedViewsBehavior.OmOnImportsSatisfied();
        }


        protected override DependencyObject CreateShell()
        {
            //return   ServiceLocator.Current.GetAllInstances<MainWindow>(); 
            return this.Container.GetExportedValue<MainWindow>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (MainWindow) this.Shell;
            //Application.Current.MainWindow.Show();
        }

        private RegionAdapterMappings _mappings;

        protected override Microsoft.Practices.Prism.Regions.RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            //_mappings = base.ConfigureRegionAdapterMappings();
            //_mappings.RegisterMapping(typeof(LayoutAnchorable), new AnchorableRegionAdapter(ServiceLocator.Current.GetInstance<RegionBehaviorFactory>()));
            //_mappings.RegisterMapping(typeof(DockingManager), new DockingManagerRegionAdapter(ServiceLocator.Current.GetInstance<RegionBehaviorFactory>()));

            //return _mappings;


            _mappings = base.ConfigureRegionAdapterMappings();
            if (_mappings != null)
            {
                // _mappings.RegisterMapping(typeof(LayoutAnchorable), new AnchorableRegionAdapter(ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>()));
                ////_mappings.RegisterMapping(typeof(DockingManager), new DockingManagerRegionAdapter(ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>()));
                // _mappings.RegisterMapping(typeof(LayoutDocumentPane), new LayoutDocumentRegionAdapter(ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>()));
                // _mappings.RegisterMapping(typeof(LayoutAnchorablePane), new LayoutAnchorableRegionAdapter(ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>()));
                _mappings.RegisterMapping(typeof (RadPaneGroup),
                                          new RadPaneGroupRegionAdapter(
                                              ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>()));
            }

            return _mappings;
        }

        protected override Microsoft.Practices.Prism.Regions.IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var factory = base.ConfigureDefaultRegionBehaviors();
            factory.AddIfMissing("AutoPopulateExportedViewsBehavior", typeof(AutoPopulateExportedViewsBehavior));

            return factory;
        }


    
        /// <summary>
        /// Creates the <see cref="IModuleCatalog"/> used by Prism.
        /// </summary>
        /// <remarks>
        /// The base implementation returns a new ModuleCatalog.
        /// </remarks>
        /// <returns>
        /// A ConfigurationModuleCatalog.
        /// </returns>
        protected override IModuleCatalog CreateModuleCatalog()
        {
           
            
            // When using MEF, the existing Prism ModuleCatalog is still the place to configure modules via configuration files.
            return new ConfigurationModuleCatalog();
        }

    }
}