namespace TradeHub.Core.Model.Enums
{
    /// <summary>
    /// Represents the frequency of historical ticks for a collection.
    /// </summary>
    public enum HistoricalFrequency
    {
        /// <summary>
        /// Each tick represents one day.
        /// </summary>
        Daily = 'd',

        /// <summary>
        /// Each tick represents one week.
        /// </summary>
        Weekly = 'w',

        /// <summary>
        /// Each tick represents one month.
        /// </summary>
        Monthly = 'm',

        /// <summary>
        /// Each tick is a dividend payout.
        /// </summary>
        DividendsOnly = 'v'
    }
}
