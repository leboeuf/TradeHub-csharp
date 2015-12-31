namespace TradeHub.Charts.Modules
{
    /// <summary>
    /// A StaticChartModule that represents volume as an histogram.
    /// </summary>
    public class VolumeStaticChartModule : StaticChartModule
    {
        public VolumeStaticChartModule(StaticChart parent)
        {
            this.parent = parent;
            Height = 100;
        }
    }
}
