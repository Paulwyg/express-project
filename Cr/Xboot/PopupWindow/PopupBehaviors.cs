using System.Windows;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel;

namespace Xboot.PopupWindow
{
    public static class PopupBehaviors
    {
        /// <summary>
        /// The name of the Popup <see cref="IRegion"/>.
        /// </summary>
        public static readonly DependencyProperty CreatePopupRegionWithNameProperty =
            DependencyProperty.RegisterAttached("CreatePopupRegionWithName", typeof(string), typeof(PopupBehaviors), new PropertyMetadata(CreatePopupRegionWithNamePropertyChanged));

        /// <summary>
        /// Gets the name of the Popup <see cref="IRegion"/>.
        /// </summary>
        /// <param name="owner">Owner of the Popup.</param>
        /// <returns>The name of the Popup <see cref="IRegion"/>.</returns>
        public static string GetCreatePopupRegionWithName(DependencyObject owner)
        {
            return owner.GetValue(CreatePopupRegionWithNameProperty) as string;
        }

        /// <summary>
        /// Sets the name of the Popup <see cref="IRegion"/>.
        /// </summary>
        /// <param name="owner">Owner of the Popup.</param>
        /// <param name="value">Name of the Popup <see cref="IRegion"/>.</param>
        public static void SetCreatePopupRegionWithName(DependencyObject owner, string value)
        {
            owner.SetValue(CreatePopupRegionWithNameProperty, value);
        }



        /// <summary>
        /// Creates a new <see cref="IRegion"/> and registers it in the default <see cref="IRegionManager"/>
        /// attaching to it a <see cref="DialogActivationBehavior"/> behavior.
        /// </summary>
        /// <param name="owner">The owner of the Popup.</param>
        /// <param name="regionName">The name of the <see cref="IRegion"/>.</param>
        /// <remarks>
        /// This method would typically not be called directly, instead the behavior 
        /// should be set through the Attached Property <see cref="CreatePopupRegionWithNameProperty"/>.
        /// </remarks>
        public static void RegisterNewPopupRegion(DependencyObject owner, string regionName)
        {
            // Creates a new region and registers it in the default region manager.
            // Another option if you need the complete infrastructure with the default region behaviors
            // is to extend DelayedRegionCreationBehavior overriding the CreateRegion method and create an 
            // instance of it that will be in charge of registering the Region once a RegionManager is
            // set as an attached property in the Visual Tree.
            IRegionManager regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
            if (regionManager != null)
            {
                IRegion region = new AllActiveRegion();
                DialogActivationBehavior behavior;
                behavior = new DialogActivationBehavior();
                behavior.HostControl = owner;

                region.Behaviors.Add(DialogActivationBehavior.BehaviorKey, behavior);
                regionManager.Regions.Add(regionName, region);
            }
        }

        private static void CreatePopupRegionWithNamePropertyChanged(DependencyObject hostControl, DependencyPropertyChangedEventArgs e)
        {
            if (IsInDesignMode(hostControl))
            {
                return;
            }

            RegisterNewPopupRegion(hostControl, e.NewValue as string);
        }

        private static bool IsInDesignMode(DependencyObject element)
        {
            // Due to a known issue in Cider, GetIsInDesignMode attached property value is not enough to know if it's in design mode.
            return DesignerProperties.GetIsInDesignMode(element) || Application.Current == null
                   || Application.Current.GetType() == typeof(Application);
        }
    }
}
