using Prism.Regions;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace ADM_Scada.Cores.Regions
{
    public class StackPanelRegionAdapter : RegionAdapterBase<StackPanel>
    {
        public StackPanelRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {

        }
        protected override void Adapt(IRegion region, StackPanel regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (FrameworkElement view in e.NewItems)
                    {
                        regionTarget.Children.Add(view);
                    }
                }
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (FrameworkElement view in e.NewItems)
                    {
                        regionTarget.Children.Remove(view);
                    }
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}
