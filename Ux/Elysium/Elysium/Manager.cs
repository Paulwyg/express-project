using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Security;
#if NETFX45
using System.Threading.Tasks;
#endif
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

using Elysium.Extensions;

using JetBrains.Annotations;

namespace Elysium
{
    [PublicAPI]
    public static class Manager
    {
        #region  URIs

        private static readonly Uri GenericDictionaryUri = new Uri("/Elysium;component/Themes/Generic.xaml", UriKind.Relative);
        private static readonly Uri BrushesDictionaryUri = new Uri("/Elysium;component/Themes/Brushes.xaml", UriKind.Relative);
        private static readonly Uri LightBrushesDictionaryUri = new Uri("/Elysium;component/Themes/LightBrushes.xaml", UriKind.Relative);
        private static readonly Uri DarkBrushesDictionaryUri = new Uri("/Elysium;component/Themes/DarkBrushes.xaml", UriKind.Relative);
        private static readonly List<ResourceDictionary> RegisteredUri = new List<ResourceDictionary>();
        public static void AddResourceDictionary(ResourceDictionary rd)
        {
            if (RegisteredUri.Contains(rd)) return;
            RegisteredUri.Add(rd);
        }
        #endregion

        #region Constractors

        [SecurityCritical]
        static Manager()
        {
            Cache = new Collection<WeakReference>();
        }

        #endregion

        #region Default values

        [PublicAPI]
        public static Theme DefaultTheme
        {
            get { return Theme.Light; }
        }

        [PublicAPI]
        public static SolidColorBrush DefaultAccentBrush
        {
            get { return AccentBrushes.Blue; }
        }

        [PublicAPI]
        public static SolidColorBrush DefaultContrastBrush
        {
            get { return Brushes.White; }
        }

        #endregion

        #region Theme

        #region Control

        [PublicAPI]
        public static bool TryGetTheme([NotNull] this FrameworkElement reference, out Theme theme)
        {
            ValidationHelper.NotNull(reference, "reference");

            return GetThemeCore(reference, out theme);
        }

        [PublicAPI]
        public static Theme GetTheme([NotNull] this FrameworkElement reference)
        {
            Theme result;
            if (!GetThemeCore(reference, out result))
            {
                throw new InvalidStyleException();
            }
            return result;
        }

        private static bool GetThemeCore([NotNull] FrameworkElement reference, out Theme theme)
        {
            ValidationHelper.NotNull(reference, "reference");

            theme = default(Theme);

            Theme? currentTheme;
            var frameworkElement = reference;
            do
            {
                currentTheme = Parameters.Manager.GetTheme(frameworkElement);
                frameworkElement = LogicalTreeHelper.GetParent(frameworkElement) as FrameworkElement;
            }
            while (currentTheme == null && frameworkElement != null);

            if (currentTheme.HasValue)
            {
                theme = currentTheme.Value;
                return true;
            }
            return Application.Current != null && GetThemeCore(Application.Current, out theme);
        }

        #endregion

        #region Application

        [PublicAPI]
        public static bool TryGetTheme([NotNull] this Application reference, out Theme theme)
        {
            ValidationHelper.NotNull(reference, "reference");

            return GetThemeCore(reference, out theme);
        }

        [PublicAPI]
        public static Theme GetTheme([NotNull] this Application reference)
        {
            ValidationHelper.NotNull(reference, "reference");

            Theme result;
            if (!GetThemeCore(reference, out result))
            {
                throw new InvalidStyleException();
            }
            return result;
        }

        private static bool GetThemeCore([NotNull] Application reference, out Theme theme)
        {
            ValidationHelper.NotNull(reference, "reference");

            theme = default(Theme);

            ResourceDictionary genericDictionary;
            if (!GetGenericDictionary(reference.Resources, out genericDictionary))
            {
                return false;
            }

            var lightBrushesDictionaryExist = genericDictionary.MergedDictionaries.Any(dictionary => dictionary.Source == LightBrushesDictionaryUri);
            var darkBrushesDictionaryExist = genericDictionary.MergedDictionaries.Any(dictionary => dictionary.Source == DarkBrushesDictionaryUri);
            if (lightBrushesDictionaryExist && darkBrushesDictionaryExist)
            {
                return false;
            }

            if (lightBrushesDictionaryExist)
            {
                theme = Theme.Light;
                return true;
            }
            if (darkBrushesDictionaryExist)
            {
                theme = Theme.Dark;
                return true;
            }
            return false;
        }

        #endregion

        #endregion

        #region Accent brush

        #region Control

        [PublicAPI]
        public static bool TryGetAccentBrush([NotNull] this FrameworkElement reference, out SolidColorBrush accentBrush)
        {
            ValidationHelper.NotNull(reference, "reference");
            Contract.Ensures(!Contract.Result<bool>() || !Equals(Contract.ValueAtReturn(out accentBrush), default(SolidColorBrush)));

            return GetAccentBrushCore(reference, out accentBrush);
        }

        [PublicAPI]
        public static SolidColorBrush GetAccentBrush([NotNull] this FrameworkElement reference)
        {
            ValidationHelper.NotNull(reference, "reference");
            Contract.Ensures(!Equals(Contract.Result<SolidColorBrush>(), default(SolidColorBrush)));

            SolidColorBrush result;
            if (!GetAccentBrushCore(reference, out result))
            {
                throw new InvalidStyleException();
            }
            return result;
        }

        [PublicAPI]
        private static bool GetAccentBrushCore([NotNull] FrameworkElement reference, out SolidColorBrush accentBrush)
        {
            ValidationHelper.NotNull(reference, "reference");
            Contract.Ensures(!Contract.Result<bool>() || !Equals(Contract.ValueAtReturn(out accentBrush), default(SolidColorBrush)));

            return GetBrush(reference, Parameters.Manager.GetAccentBrush, GetAccentBrushCore, out accentBrush);
        }

        #endregion

        #region Application

        [PublicAPI]
        public static bool TryGetAccentBrush([NotNull] this Application reference, out SolidColorBrush accentBrush)
        {
            ValidationHelper.NotNull(reference, "reference");
            Contract.Ensures(!Contract.Result<bool>() || !Equals(Contract.ValueAtReturn(out accentBrush), default(SolidColorBrush)));

            return GetAccentBrushCore(reference, out accentBrush);
        }

        [PublicAPI]
        public static SolidColorBrush GetAccentBrush([NotNull] this Application reference)
        {
            ValidationHelper.NotNull(reference, "reference");
            Contract.Ensures(!Equals(Contract.Result<SolidColorBrush>(), default(SolidColorBrush)));

            SolidColorBrush result;
            if (!GetAccentBrushCore(reference, out result))
            {
                throw new InvalidStyleException();
            }
            return result;
        }

        private static bool GetAccentBrushCore([NotNull] Application reference, out SolidColorBrush accentBrush)
        {
            ValidationHelper.NotNull(reference, "reference");
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out accentBrush) != null);

            accentBrush = null;

            ResourceDictionary genericDictionary;
            ResourceDictionary brushesDictionary;
            if (!GetGenericDictionary(reference.Resources, out genericDictionary) || !GetBrushesDictionary(genericDictionary, out brushesDictionary))
            {
                return false;
            }

            var result = brushesDictionary.SafeGet("AccentBrush", out accentBrush);

            // Logic ensures that the brush is not null
            Contract.Assume(!result || accentBrush != null);
            return result;
        }

        #endregion

        #endregion

        #region Contrast brush

        #region Control

        [PublicAPI]
        public static bool TryGetContrastBrush([NotNull] this FrameworkElement reference, out SolidColorBrush contrastBrush)
        {
            ValidationHelper.NotNull(reference, "reference");
            Contract.Ensures(!Contract.Result<bool>() || !Equals(Contract.ValueAtReturn(out contrastBrush), default(SolidColorBrush)));

            return GetContrastBrushCore(reference, out contrastBrush);
        }

        [PublicAPI]
        public static SolidColorBrush GetContrastBrush([NotNull] this FrameworkElement reference)
        {
            ValidationHelper.NotNull(reference, "reference");
            Contract.Ensures(!Equals(Contract.Result<SolidColorBrush>(), default(SolidColorBrush)));

            SolidColorBrush result;
            if (!GetContrastBrushCore(reference, out result))
            {
                throw new InvalidStyleException();
            }
            return result;
        }

        [PublicAPI]
        private static bool GetContrastBrushCore([NotNull] FrameworkElement reference, out SolidColorBrush contrastBrush)
        {
            ValidationHelper.NotNull(reference, "reference");
            Contract.Ensures(!Contract.Result<bool>() || !Equals(Contract.ValueAtReturn(out contrastBrush), default(SolidColorBrush)));

            return GetBrush(reference, Parameters.Manager.GetContrastBrush, GetContrastBrushCore, out contrastBrush);
        }

        #endregion

        #region Application

        [PublicAPI]
        public static bool TryGetContrastBrush([NotNull] this Application reference, out SolidColorBrush contrastBrush)
        {
            ValidationHelper.NotNull(reference, "reference");
            Contract.Ensures(!Contract.Result<bool>() || !Equals(Contract.ValueAtReturn(out contrastBrush), default(SolidColorBrush)));

            return GetContrastBrushCore(reference, out contrastBrush);
        }

        [PublicAPI]
        public static SolidColorBrush GetContrastBrush([NotNull] this Application reference)
        {
            ValidationHelper.NotNull(reference, "reference");
            Contract.Ensures(!Equals(Contract.Result<SolidColorBrush>(), default(SolidColorBrush)));

            SolidColorBrush result;
            if (!GetContrastBrushCore(reference, out result))
            {
                throw new InvalidStyleException();
            }
            return result;
        }

        private static bool GetContrastBrushCore([NotNull] Application reference, out SolidColorBrush contrastBrush)
        {
            ValidationHelper.NotNull(reference, "reference");
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out contrastBrush) != null);

            contrastBrush = null;

            ResourceDictionary genericDictionary;
            ResourceDictionary brushesDictionary;
            if (!GetGenericDictionary(reference.Resources, out genericDictionary) || !GetBrushesDictionary(genericDictionary, out brushesDictionary))
            {
                return false;
            }

            var result = brushesDictionary.SafeGet("ContrastBrush", out contrastBrush);

            // Logic ensures that the brush is not null
            Contract.Assume(!result || contrastBrush != null);
            return result;
        }

        #endregion

        #endregion

        #region Public members

        [SecurityCritical]
        private static readonly Collection<WeakReference> Cache;

        #region Application

#if NETFX4
        [PublicAPI]
        public static DispatcherOperation BeginApply(this Application application)
        {
            return BeginApply(application, null, null, null);
        }
#elif NETFX45
        [PublicAPI]
        public static async Task ApplyAsync(this Application application)
        {
            await ApplyAsync(application, null, null, null);
        }
#endif

        [PublicAPI]
        public static void Apply(this Application application)
        {
            Apply(application, null, null, null);
        }

#if NETFX4
        [PublicAPI]
        public static DispatcherOperation BeginApply(this Application application, Theme? theme)
        {
            return BeginApply(application, theme, null, null);
        }
#elif NETFX45
        [PublicAPI]
        public static async Task ApplyAsync(this Application application, Theme? theme)
        {
            await ApplyAsync(application, theme, null, null);
        }
#endif

        [PublicAPI]
        public static void Apply(this Application application, Theme? theme)
        {
            Apply(application, theme, null, null);
        }

#if NETFX4
        [PublicAPI]
        public static DispatcherOperation BeginApply(this Application application, SolidColorBrush accentBrush, SolidColorBrush contrastBrush)
        {
            return BeginApply(application, null, accentBrush, contrastBrush);
        }
#elif NETFX45
        [PublicAPI]
        public static async Task ApplyAsync(this Application application, SolidColorBrush accentBrush, SolidColorBrush contrastBrush)
        {
            await ApplyAsync(application, null, accentBrush, contrastBrush);
        }
#endif

        [PublicAPI]
        public static void Apply(this Application application, SolidColorBrush accentBrush, SolidColorBrush contrastBrush)
        {
            Apply(application, null, accentBrush, contrastBrush);
        }

#if NETFX4
        [PublicAPI]
        public static DispatcherOperation BeginApply(this Application application, Theme? theme, SolidColorBrush accentBrush, SolidColorBrush contrastBrush)
        {
            ValidationHelper.NotNull(application, "application");

            return application.Dispatcher.BeginInvoke(() => ApplyInternal(application, theme, accentBrush, contrastBrush), DispatcherPriority.Render);
        }
#elif NETFX45
        [PublicAPI]
        public static async Task ApplyAsync(this Application application, Theme? theme, SolidColorBrush accentBrush, SolidColorBrush contrastBrush)
        {
            ValidationHelper.NotNull(application, "application");

            await application.Dispatcher.InvokeAsync(() => ApplyInternal(application, theme, accentBrush, contrastBrush), DispatcherPriority.Render);
        }
#endif

        [PublicAPI]
        public static void Apply(this Application application, Theme? theme, SolidColorBrush accentBrush, SolidColorBrush contrastBrush)
        {
            ValidationHelper.NotNull(application, "application");

            application.Dispatcher.Invoke(() => ApplyInternal(application, theme, accentBrush, contrastBrush), DispatcherPriority.Render);
        }

        [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect", Justification = "We need to garbage collection for correct use of cache")]
        [SecuritySafeCritical]
        private static void ApplyInternal(Application application, Theme? theme, SolidColorBrush accentBrush, SolidColorBrush contrastBrush)
        {
            ValidationHelper.NotNull(application, "application");

            Theme applicationTheme;
            SolidColorBrush applicationAcentBrush;
            SolidColorBrush applicationContrastBrush;

            ApplyCore(application.Resources,
                      theme ?? (application.TryGetTheme(out applicationTheme) ? (Theme?)applicationTheme : DefaultTheme),
                      accentBrush ?? (application.TryGetAccentBrush(out applicationAcentBrush) ? applicationAcentBrush : DefaultAccentBrush),
                      contrastBrush ?? (application.TryGetContrastBrush(out applicationContrastBrush) ? applicationContrastBrush : DefaultContrastBrush));

            GC.Collect();
            Cache.Collect();
            foreach (var control in Cache.Where(reference => reference.Target != null).Select(reference => (FrameworkElement)reference.Target))
            {
                // All members of cache is not null and can be casted to FrameworkElement type
                Contract.Assume(control != null);
                Apply(control);
            }

            try
            {
                foreach (var f in RegisteredUri)
                {
                    Application.Current.Resources.MergedDictionaries.Add(f);
                }
            }
            catch (Exception ex)
            {

            }
        }

#if NETFX4
        [PublicAPI]
        public static DispatcherOperation BeginRemove(this Application application)
        {
            ValidationHelper.NotNull(application, "application");

            return application.Dispatcher.BeginInvoke(() => RemoveInternal(application), DispatcherPriority.Render);
        }
#elif NETFX45
        [PublicAPI]
        public static async Task RemoveAsync(this Application application)
        {
            ValidationHelper.NotNull(application, "application");

            await application.Dispatcher.InvokeAsync(() => RemoveInternal(application), DispatcherPriority.Render);
        }
#endif

        [PublicAPI]
        public static void Remove(this Application application)
        {
            ValidationHelper.NotNull(application, "application");

            application.Dispatcher.Invoke(() => RemoveInternal(application), DispatcherPriority.Render);
        }

        [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect", Justification = "We need to garbage collection for correct use of cache")]
        [SecuritySafeCritical]
        private static void RemoveInternal(this Application application)
        {
            ValidationHelper.NotNull(application, "application");

            RemoveCore(application.Resources);

            GC.Collect();
            Cache.Collect();
            foreach (var control in Cache.Where(reference => reference.Target != null).Select(reference => (FrameworkElement)reference.Target))
            {
                // All members of cache is not null and can be casted to FrameworkElement type
                Contract.Assume(control != null);
                Apply(control);
            }
        }

        #endregion

        #region Control

        [PublicAPI]
        internal static void Apply(this FrameworkElement control)
        {
            Apply(control, null, null, null);
        }

        [PublicAPI]
        internal static void Apply(this FrameworkElement control, Theme? theme, SolidColorBrush accentBrush, SolidColorBrush contrastBrush)
        {
            ValidationHelper.NotNull(control, "control");

            control.Dispatcher.Invoke(() => ApplyInternal(control, theme, accentBrush, contrastBrush), DispatcherPriority.Render);
        }

        [SecuritySafeCritical]
        private static void ApplyInternal(this FrameworkElement control, Theme? theme, SolidColorBrush accentBrush, SolidColorBrush contrastBrush)
        {
            ValidationHelper.NotNull(control, "control");

            Cache.SafeSet(control);

            Theme controlTheme;
            SolidColorBrush controlAcentBrush;
            SolidColorBrush controlContrastBrush;

            ApplyCore(control.Resources,
                      theme ?? (control.TryGetTheme(out controlTheme) ? (Theme?)controlTheme : DefaultTheme),
                      accentBrush ?? (control.TryGetAccentBrush(out controlAcentBrush) ? controlAcentBrush : DefaultAccentBrush),
                      contrastBrush ?? (control.TryGetContrastBrush(out controlContrastBrush) ? controlContrastBrush : DefaultContrastBrush));
        }

        [PublicAPI]
        internal static void Remove(this FrameworkElement control)
        {
            ValidationHelper.NotNull(control, "control");

            control.Dispatcher.Invoke(() => RemoveInternal(control), DispatcherPriority.Render);
        }

        [SecuritySafeCritical]
        private static void RemoveInternal(this FrameworkElement control)
        {
            ValidationHelper.NotNull(control, "control");

            RemoveCore(control.Resources);
            Cache.SafeRemove(control);
        }

        #endregion

        #endregion

        #region Private members

        [SecurityCritical]
        internal static void ApplyCore(ResourceDictionary resources, Theme? theme, SolidColorBrush accentBrush, SolidColorBrush contrastBrush)
        {
            ValidationHelper.NotNull(resources, "resources");

            // Resource dictionaries
            var genericDictionary = new ResourceDictionary { Source = GenericDictionaryUri };
            var brushesDictionary = new ResourceDictionary { Source = BrushesDictionaryUri };
            var lightBrushesDictionary = new ResourceDictionary { Source = LightBrushesDictionaryUri };
            var darkBrushesDictionary = new ResourceDictionary { Source = DarkBrushesDictionaryUri };

            // Switch theme
            switch (theme)
            {
                case null:
                    RemoveDictionaries(genericDictionary, LightBrushesDictionaryUri);
                    RemoveDictionaries(genericDictionary, DarkBrushesDictionaryUri);
                    break;
                case Theme.Light:
                    ReplaceDictionary(genericDictionary, lightBrushesDictionary, LightBrushesDictionaryUri);
                    RemoveDictionaries(genericDictionary, DarkBrushesDictionaryUri);
                    break;
                case Theme.Dark:
                    ReplaceDictionary(genericDictionary, darkBrushesDictionary, DarkBrushesDictionaryUri);
                    RemoveDictionaries(genericDictionary, LightBrushesDictionaryUri);
                    break;
            }

            if (accentBrush != null)
            {
                // Must be frozen
                var accentBrushFrozen = FreezableExtensions.TryFreeze(accentBrush);
                brushesDictionary.SafeSet("AccentBrush", accentBrushFrozen);
            }
            else
            {
                brushesDictionary.SafeRemove("AccentBrush");
            }

            if (contrastBrush != null)
            {
                // Must be frozen
                var contrastBrushFrozen = FreezableExtensions.TryFreeze(contrastBrush);
                brushesDictionary.SafeSet("ContrastBrush", contrastBrushFrozen);

                // Must be frozen
                var semitransparentContrastBrush = contrastBrush.Clone();
                
                // NOTE: Lack of contracts: Clone() must ensure non-null return value
                Contract.Assume(semitransparentContrastBrush != null);
                semitransparentContrastBrush.Opacity = 1d / 8d;
                var semitransparentContrastBrushFrozen = FreezableExtensions.TryFreeze(semitransparentContrastBrush);
                brushesDictionary.SafeSet("SemitransparentContrastBrush", semitransparentContrastBrushFrozen);
            }
            else
            {
                brushesDictionary.SafeRemove("ContrastBrush");
                brushesDictionary.SafeRemove("SemitransparentContrastBrush");
            }

            // Bug in WPF 4: http://connect.microsoft.com/VisualStudio/feedback/details/555322/global-wpf-styles-are-not-shown-when-using-2-levels-of-references
            if (resources.Keys.Count == 0)
            {
                resources.Add(typeof(Window), new Style(typeof(Window)));
            }

            // Replace Brushes.xaml
            ReplaceDictionary(genericDictionary, brushesDictionary, BrushesDictionaryUri);

            // Replace Generic.xaml
            ReplaceDictionary(resources, genericDictionary, GenericDictionaryUri);

            OnThemeChanged();
        }

        [SecurityCritical]
        internal static void RemoveCore(ResourceDictionary resources)
        {
            ValidationHelper.NotNull(resources, "resources");

            ResourceDictionary genericDictionary;
            if (!GetGenericDictionary(resources, out genericDictionary))
            {
                return;
            }

            // Remove Generic.xaml
            resources.MergedDictionaries.Remove(genericDictionary);

            OnThemeChanged();
        }

        #region Helpers

        private delegate bool GetBrushFunc(Application reference, out SolidColorBrush result);

        private static bool GetBrush([NotNull] FrameworkElement reference,
                                     [NotNull] Func<FrameworkElement, SolidColorBrush> managerFunction,
                                     [NotNull] GetBrushFunc itselfFunction,
                                     out SolidColorBrush brush)
        {
            ValidationHelper.NotNull(reference, "reference");
            ValidationHelper.NotNull(managerFunction, "managerFunction");
            ValidationHelper.NotNull(itselfFunction, "itselfFunction");
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out brush) != null);

            brush = null;

            SolidColorBrush currentResult;
            var frameworkElement = reference;
            do
            {
                currentResult = managerFunction(frameworkElement);
                frameworkElement = LogicalTreeHelper.GetParent(frameworkElement) as FrameworkElement;
            }
            while (currentResult == null && frameworkElement != null);

            if (currentResult != null)
            {
                brush = currentResult;
                return true;
            }

            var result = Application.Current != null && itselfFunction(Application.Current, out brush);

            // Can't be proven
            Contract.Assume(!result || brush != null);
            return result;
        }

        private static bool GetGenericDictionary([NotNull] ResourceDictionary resources, out ResourceDictionary result)
        {
            ValidationHelper.NotNull(resources, "resources");
            Contract.Ensures(!Contract.Result<bool>() || !Equals(Contract.ValueAtReturn(out result), default(ResourceDictionary)));

            return GetDictionary(resources, GenericDictionaryUri, out result);
        }

        private static bool GetBrushesDictionary([NotNull] ResourceDictionary genericDictionary, out ResourceDictionary result)
        {
            ValidationHelper.NotNull(genericDictionary, "genericDictionary");
            Contract.Ensures(!Contract.Result<bool>() || !Equals(Contract.ValueAtReturn(out result), default(ResourceDictionary)));

            return GetDictionary(genericDictionary, BrushesDictionaryUri, out result);
        }

        private static bool GetDictionary([NotNull] ResourceDictionary dictionary, Uri uri, out ResourceDictionary result)
        {
            ValidationHelper.NotNull(dictionary, "dictionary");
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out result) != null);

            result = null;
            var dictionaries = dictionary.MergedDictionaries.Where(d => d.Source == uri).ToList();
            if (dictionaries.Count != 1)
            {
                return false;
            }
            result = dictionaries[0];

            // NOTE: Lack of contracts: MergedDictionaries must ensure non-null value of any item
            Contract.Assume(result != null);
            return true;
        }

        private static void ReplaceDictionary(ResourceDictionary resources, ResourceDictionary dictionary, Uri uri)
        {
            // Remove previous dictionaries
            var dictionaries = resources.MergedDictionaries.Where(d => d.Source == uri).ToList();
            var lastIndex = dictionaries.Select(d => resources.MergedDictionaries.IndexOf(d)).Concat(new[] { 0 }).Max();
            foreach (var d in dictionaries)
            {
                resources.MergedDictionaries.Remove(d);
            }

            // Add new dictionary
            if (dictionary != null)
            {
                try
                {
                    resources.MergedDictionaries.Insert(lastIndex, dictionary);
                }
                catch (Exception ex)
                {
                    var str = ex.ToString();
                }
            }
        }

        private static void RemoveDictionaries(ResourceDictionary resources, Uri uri)
        {
            // Remove, if included
            var dictionaries = resources.MergedDictionaries.Where(d => d.Source == uri).ToList();
            foreach (var d in dictionaries)
            {
                resources.MergedDictionaries.Remove(d);
            }
        }

        #endregion

        [SecurityCritical]
        private static void OnThemeChanged()
        {
            var systemColors = typeof(SystemColors);
            var invalidateColors = systemColors.GetMethod("InvalidateCache", BindingFlags.Static | BindingFlags.NonPublic);
            if (invalidateColors != null)
            {
                invalidateColors.Invoke(null, null);
            }

            var systemParameters = typeof(SystemParameters);
            var invalidateParameters = systemParameters.GetMethod("InvalidateCache", BindingFlags.Static | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            if (invalidateParameters != null)
            {
                invalidateParameters.Invoke(null, null);
            }

            var presentationFramework = Assembly.GetAssembly(typeof(Window));
            if (presentationFramework != null)
            {
                var systemResources = presentationFramework.GetType("System.Windows.SystemResources");

                if (systemResources != null)
                {
                    var onThemeChanged = systemResources.GetMethod("OnThemeChanged", BindingFlags.Static | BindingFlags.NonPublic);
                    if (onThemeChanged != null)
                    {
                        onThemeChanged.Invoke(null, null);
                    }

                    var invalidateResources = systemResources.GetMethod("InvalidateResources", BindingFlags.Static | BindingFlags.NonPublic);
                    if (invalidateResources != null)
                    {
                        invalidateResources.Invoke(null, new object[] { false });
                    }
                }
            }
        }

        #endregion
    }
}