using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

using Elysium.Controls.Primitives;
using Elysium.Extensions;

using JetBrains.Annotations;

using Microsoft.Expression.Shapes;

namespace Elysium.Controls
{
    [PublicAPI]
    [TemplatePart(Name = ArcName, Type = typeof(Arc))]
    [TemplatePart(Name = BusyBarName, Type = typeof(Canvas))]
    public class ProgressRing : ProgressBase
    {
        private const string ArcName = "PART_Arc";
        private const string BusyBarName = "PART_BusyBar";

        [SecurityCritical]
        private Arc _arc;
        private Canvas _busyBar;

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "We need to use static constructor for custom actions during dependency properties initialization")]
        static ProgressRing()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing), new FrameworkPropertyMetadata(typeof(ProgressRing)));
        }

        [PublicAPI]
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.RegisterAttached("Angle", typeof(double), typeof(ProgressRing),
                                                new FrameworkPropertyMetadata(-1d, FrameworkPropertyMetadataOptions.AffectsArrange));

        [PublicAPI]
        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        public static double GetAngle(UIElement obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return BoxingHelper<double>.Unbox(obj.GetValue(AngleProperty));
        }

        [PublicAPI]
        public static void SetAngle(UIElement obj, double value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(AngleProperty, value);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var desiredSize = base.MeasureOverride(constraint);

            // NOTE: Lack of contracts: MeasureOverride must ensure size with non-negative width and height
            Contract.Assume(desiredSize.Width >= 0d);
            Contract.Assume(desiredSize.Height >= 0d);
            var sizeValue = Math.Min(desiredSize.Width, desiredSize.Height);

            desiredSize.Width = sizeValue;
            desiredSize.Height = sizeValue;
            return desiredSize;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            // NOTE: Lack of contracts: ArrangeOverride must require non-negative width and height of arrangeBouns parameters
            if (!(arrangeBounds.Width >= 0d && arrangeBounds.Height >= 0d))
            {
                throw new ArgumentOutOfRangeException("arrangeBounds");
            }
            Contract.EndContractBlock();

            var sizeValue = Math.Min(arrangeBounds.Width, arrangeBounds.Height);

            // NOTE: Lack of contracts: Math.Min doesn't contains ensures
            Contract.Assume(sizeValue >= 0);
            return base.ArrangeOverride(new Size(sizeValue, sizeValue));
        }

        [SecurityCritical]
        internal override void OnApplyTemplateInternal()
        {
            if (Template != null)
            {
                _arc = Template.FindName(ArcName, this) as Arc;
                if (_arc == null)
                {
                    Trace.TraceWarning(ArcName + " not found.");
                }

                // NOTE: Lack of contracts: FindName must be marked as pure method
                Contract.Assume(Template != null);
                _busyBar = Template.FindName(BusyBarName, this) as Canvas;
                if (_busyBar == null)
                {
                    Trace.TraceWarning(BusyBarName + " not found.");
                }
            }

            base.OnApplyTemplateInternal();
        }

        [SecuritySafeCritical]
        protected override void OnAnimationsUpdating(RoutedEventArgs e)
        {
            base.OnAnimationsUpdating(e);

            OnAnimationsUpdatingInternal();
        }

        [SecurityCritical]
        private void OnAnimationsUpdatingInternal()
        {
            UpdateIndeterminateAnimation();

            UpdateBusyAnimation();
        }

        [SecurityCritical]
        private void UpdateIndeterminateAnimation()
        {
            if ((IndeterminateAnimation == null || (IndeterminateAnimation != null && IndeterminateAnimation.Name == DefaultIndeterminateAnimationName)) && Track != null && _arc != null)
            {
                if (IndeterminateAnimation != null && IsIndeterminateAnimationRunning)
                {
                    IsIndeterminateAnimationRunning = false;
                    IndeterminateAnimation.Stop(this);
                    IndeterminateAnimation.Remove(this);
                }

                IndeterminateAnimation = new Storyboard { Name = DefaultIndeterminateAnimationName, RepeatBehavior = RepeatBehavior.Forever };

                var trackSize = Math.Min(Track.ActualWidth, Track.ActualHeight);

                var time = Math.Sqrt(trackSize * Math.PI) / 10;

                var startAngleSetValueAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0d)));

                Storyboard.SetTarget(startAngleSetValueAnimation, _arc);
                Storyboard.SetTargetProperty(startAngleSetValueAnimation, new PropertyPath(Arc.StartAngleProperty));

                var endAngleSetValueAnimation = new DoubleAnimation(-270, new Duration(TimeSpan.FromSeconds(0d)));

                Storyboard.SetTarget(endAngleSetValueAnimation, _arc);
                Storyboard.SetTargetProperty(endAngleSetValueAnimation, new PropertyPath(Arc.EndAngleProperty));

                var startAngleAnimation = new DoubleAnimationUsingKeyFrames();

                // NOTE: Lack of contracts: DoubleAnimationUsingKeyFrames.KeyFrames is always have collection instance
                Contract.Assume(startAngleAnimation.KeyFrames != null);
                startAngleAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(360, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(time))));

                Storyboard.SetTarget(startAngleAnimation, _arc);
                Storyboard.SetTargetProperty(startAngleAnimation, new PropertyPath(Arc.StartAngleProperty));

                var endAngleAnimation = new DoubleAnimationUsingKeyFrames();

                // NOTE: Lack of contracts: DoubleAnimationUsingKeyFrames.KeyFrames is always have collection instance
                Contract.Assume(endAngleAnimation.KeyFrames != null);
                endAngleAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(90, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(time))));

                Storyboard.SetTarget(endAngleAnimation, _arc);
                Storyboard.SetTargetProperty(endAngleAnimation, new PropertyPath(Arc.EndAngleProperty));

                // NOTE: Bug in Code Contracts static checker: IndeterminateAnimation can't be null
                // NOTE: Lack of contracts: IndeterminateAnimation.Children is always have collection instance
                Contract.Assume(IndeterminateAnimation != null);
                Contract.Assume(IndeterminateAnimation.Children != null);

                IndeterminateAnimation.Children.Add(startAngleSetValueAnimation);
                IndeterminateAnimation.Children.Add(endAngleSetValueAnimation);
                IndeterminateAnimation.Children.Add(startAngleAnimation);
                IndeterminateAnimation.Children.Add(endAngleAnimation);

                if (IndeterminateAnimation.CanFreeze)
                {
                    IndeterminateAnimation.Freeze();
                }

                if (State == ProgressState.Indeterminate && IsEnabled)
                {
                    IndeterminateAnimation.Begin(this, Template, true);
                    IsIndeterminateAnimationRunning = true;
                }
            }
        }

        [SecurityCritical]
        private void UpdateBusyAnimation()
        {
            if ((BusyAnimation == null || (BusyAnimation != null && BusyAnimation.Name == DefaultBusyAnimationName)) && Track != null && _busyBar != null)
            {
                if (BusyAnimation != null && IsBusyAnimationRunning)
                {
                    IsBusyAnimationRunning = false;
                    BusyAnimation.Stop(this);
                    BusyAnimation.Remove(this);
                }

                // NOTE: Lack of contracts: Children always have collection instance
                Contract.Assume(_busyBar.Children != null);

                BusyAnimation = new Storyboard { Name = DefaultBusyAnimationName, RepeatBehavior = RepeatBehavior.Forever };

                var firstCycleAnimations = new Collection<DoubleAnimation>();
                var secondCycleAnimations = new Collection<DoubleAnimation>();

                const double time = 0.25;
                const double durationTime = time * 2;
                const double beginTimeIncrement = time / 2;
                const double shortPauseTime = time;
                const double longPauseTime = time * 1.5;
                var partMotionTime = (_busyBar.Children.Count - 1) * beginTimeIncrement + durationTime + shortPauseTime;

                var length = Math.Min(Track.ActualWidth, Track.ActualHeight) * Math.PI;

                for (var i = 0; i < _busyBar.Children.Count; i++)
                {
                    var element = (FrameworkElement)_busyBar.Children[_busyBar.Children.Count - i - 1];
                    if (element != null)
                    {
                        var elementLength = Math.Max(element.Width, element.Height);

                        var index = (_busyBar.Children.Count - 1) / 2 - i;

                        var endPosition = length / 2 + index * (elementLength * 2);
                        var endAngle = endPosition / length * 360d;

                        var duration = new Duration(TimeSpan.FromSeconds(durationTime));

                        var firstCycleAnimation =
                            new DoubleAnimation(0d, endAngle, duration) { BeginTime = TimeSpan.FromSeconds(i * beginTimeIncrement) };
                        Storyboard.SetTarget(firstCycleAnimation, element);
                        Storyboard.SetTargetProperty(firstCycleAnimation, new PropertyPath(AngleProperty));

                        var secondCycleAnimation =
                            new DoubleAnimation(0d, endAngle, duration)
                                { BeginTime = TimeSpan.FromSeconds(partMotionTime + durationTime + i * beginTimeIncrement) };
                        Storyboard.SetTarget(secondCycleAnimation, element);
                        Storyboard.SetTargetProperty(secondCycleAnimation, new PropertyPath(AngleProperty));

                        firstCycleAnimations.Add(firstCycleAnimation);
                        secondCycleAnimations.Add(secondCycleAnimation);
                    }
                }

                for (var i = 0; i < _busyBar.Children.Count; i++)
                {
                    var element = (FrameworkElement)_busyBar.Children[_busyBar.Children.Count - i - 1];
                    if (element != null)
                    {
                        var duration = new Duration(TimeSpan.FromSeconds(durationTime));

                        var firstCycleAnimation =
                            new DoubleAnimation(360d, duration) { BeginTime = TimeSpan.FromSeconds(partMotionTime + i * beginTimeIncrement) };
                        Storyboard.SetTarget(firstCycleAnimation, element);
                        Storyboard.SetTargetProperty(firstCycleAnimation, new PropertyPath(AngleProperty));

                        var secondCycleAnimation =
                            new DoubleAnimation(360d, duration)
                                { BeginTime = TimeSpan.FromSeconds(partMotionTime * 2 + durationTime + i * beginTimeIncrement) };
                        Storyboard.SetTarget(secondCycleAnimation, element);
                        Storyboard.SetTargetProperty(secondCycleAnimation, new PropertyPath(AngleProperty));

                        var moveAnimation =
                            new DoubleAnimation(-1.0, new Duration(TimeSpan.FromSeconds(0)))
                                { BeginTime = TimeSpan.FromSeconds(partMotionTime * 2 + durationTime * 2 + i * beginTimeIncrement) };
                        Storyboard.SetTarget(moveAnimation, element);
                        Storyboard.SetTargetProperty(moveAnimation, new PropertyPath(AngleProperty));

                        firstCycleAnimations.Add(firstCycleAnimation);
                        secondCycleAnimations.Add(secondCycleAnimation);
                        secondCycleAnimations.Add(moveAnimation);
                    }
                }

                BusyAnimation.Duration = new Duration(TimeSpan.FromSeconds(longPauseTime + partMotionTime * 3 + shortPauseTime * 2 + durationTime));

                // NOTE: Lack of contracts: Children always have collection instance
                Contract.Assume(BusyAnimation.Children != null);
                foreach (var animation in firstCycleAnimations)
                {
                    BusyAnimation.Children.Add(animation);
                }

                foreach (var animation in secondCycleAnimations)
                {
                    BusyAnimation.Children.Add(animation);
                }

                if (BusyAnimation.CanFreeze)
                {
                    BusyAnimation.Freeze();
                }

                if (State == ProgressState.Busy && IsEnabled)
                {
                    BusyAnimation.Begin(this, Template, true);
                    IsBusyAnimationRunning = true;
                }
            }
        }
    }
}
