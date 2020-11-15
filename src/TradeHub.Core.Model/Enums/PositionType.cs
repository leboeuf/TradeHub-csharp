namespace TradeHub.Core.Model.Enums
{
    /// <summary>
    /// The type of a trading position.
    /// </summary>
    public enum PositionType
    {
        /// <summary>
        /// Represents the buying of a financial instrument with the expectation that the asset will rise in value.
        /// </summary>
        Long = 1,

        /// <summary>
        /// Represents the sale of a borrowed financial instrument with the expectation that the asset will fall in value.
        /// </summary>
        Short = 2
    }
}
