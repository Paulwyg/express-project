using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;

namespace Wlst.Ux.EquipmentInfo.DailyStatistics.TerminalViewModel.ViewModel
{
	public class ScatterPointTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			//ScatterDataPoint dataPoint = (ScatterDataPoint)item;
			var series = container as PointSeries;
			var chart = series.GetVisualParent<RadCartesianChart>();
				return chart.Resources["ellipseTemplate"] as DataTemplate;

		}
	}
}