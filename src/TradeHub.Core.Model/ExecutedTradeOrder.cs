namespace TradeHub.Core.Model
{
    /// <summary>
    /// Represents the execution of a trade order.
    /// </summary>
    public class ExecutedTradeOrder
    {
        /// <summary>
        /// The trade order that was executed.
        /// </summary>
        public TradeOrder TradeOrder { get; set; }

        /// <summary>
        /// The amount paid in commission.
        /// </summary>
        public decimal CommissionPaid { get; set; }

        /// <summary>
        /// Whether the strategy triggered the trade (true) or the trade was scheduled in advance (false).
        /// </summary>
        /// <remarks>
        /// This is used to evaluate the strategy's perfomance. If the trade was scheduled by the user,
        /// it doesn't impact the performance because the strategy wasn't responsible for it.
        /// </remarks>
        public bool WasTriggeredByStrategy { get; set; }
    }
}
