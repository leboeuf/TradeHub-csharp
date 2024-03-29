﻿using System;

namespace TradeHub.Core.Model
{
    /// <summary>
    /// Represent the value of a marketable asset at a certain point in time.
    /// </summary>
    public class Tick
    {
        public DateTime Timestamp { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public int Volume { get; set; }

        /// <remarks>
        /// When a Tick is created with HistoricalFrequency.DividendOnly,
        /// only the Dividend and Timestamp fields will be populated.
        /// </remarks>
        public decimal? Dividend { get; set; }
    }
}
