namespace TradeHub.Core.Model.Enums.Indices
{
    /// <summary>
    /// List of indices on the TSX
    /// </summary>
    public static class Tsx
    {
        // http://web.tmxmoney.com/indices.php

        public static string[] Main = {
            "^TSX", // S&P/TSX Composite Index
            "^15U", // S&P/TSX Composite Index (USD)
            "^15NT", // S&P/TSX Composite Index (Net TR)
            "^15NTU", // S&P/TSX Composite Index (USD Net TR)
            "^TX60", // S&P/TSX 60 Index (CAD)
            "^TX60U", // S&P/TSX 60 Index (USD)
            "^TX60UH", // S&P/TSX 60 Hedged Index (USD
            "^TX6UHN", // S&P/TSX 60 Hedged Index (USD Net TR)
            "^TXSI", // S&P/TSX 60 Shariah Index
            "^TXCA", // The S&P/TSX 60 Carbon Efficient Index
            "^TXCAU", // The S&P/TSX 60 Carbon Efficient Index (USD)
            "^TXCAN", // The S&P/TSX 60 Carbon Efficient Index (Net TR)
            "^TXCANU", // The S&P/TSX 60 Carbon Efficient Index (USD Net TR)
            "^TXFF", // The S&P/TSX 60 Fossil Fuel Free Index
            "^TXFFN", // The S&P/TSX 60 Fossil Fuel Free Index (Net TR)
            "^TXFFU", // The S&P/TSX 60 Fossil Fuel Free Index (USD)
            "^TXFFNU", // The S&P/TSX 60 Fossil Fuel Free Index (USD Net TR)
            "^TXFFC", // S&P/TSX 60 Fossil Fuel Free Carbon Efficient Index
            "^TXFCN", // S&P/TSX 60 Fossil Fuel Free Carbon Efficient Index (Net TR)
            "^TXFCU", // S&P/TSX 60 Fossil Fuel Free Carbon Efficient Index (USD)
            "^TXFCNU", // S&P/TSX 60 Fossil Fuel Free Carbon Efficient Index (USD Net TR)
            "^TIMM", // S&P/TSX Composite Metals and Mining Industry Index
            "^TX40", // S&P/TSX Completion Index
            "^TX20", // S&P/TSX SmallCap Index
            "^TXSC", // S&P/TSX SmallCap Select Index (CAD)
            "^RTCM", // S&P/TSX Income Trust Index
            "^TXEI", // S&P/TSX Composite High Dividend Index (CAD)
            "^TXEU", // S&P/TSX Composite High Dividend Index (USD)
            "^TXEN", // S&P/TSX Composite High Dividend Index (Net TR)
            "^TXBB", // S&P/TSX Composite Buyback Index (CAD)
            "^TXBBU", // S&P/TSX Composite Buyback Index (USD)
            "^TXBBN", // S&P/TSX Composite Buyback Index (CAD) (Net Total Return)
            "^TXBBNU", // S&P/TSX Composite Buyback Index (USD) (Net Total Return)
            "^TXHE", // S&P/TSX High Income Energy Index (CAD)
            "^TXHU", // S&P/TSX High Income Energy Index (USD)
            "^TXPR", // S&P/TSX Preferred Share Index
            "^STNC", // S&P/TSX North American Preferred Stock Index (Price Return) (CAD)
            "^STNU", // S&P/TSX North American Preferred Stock Index (Price Return) (USD)
            "^STNCH", // S&P/TSX North American Preferred Stock Index (Price Return) (CAD Hedged)
            "^STNCN", // S&P/TSX North American Preferred Stock Index (Net Total Return) (CAD)
            "^STNUN", // S&P/TSX North American Preferred Stock Index (Net Total Return) (USD)
            "^STNCHN", // S&P/TSX North American Preferred Stock Index (Net Total Return) (CAD Hedged)
            "^TXDV", // S&P/TSX Canadian Dividend Aristocrats Index
            "^TXDC", // S&P/TSX Composite Dividend Index
            "^TXDP", // S&P/TSX 60 Dividend Points Index
            "^TXDPA", // S&P/TSX 60 Dividend Points Index (Annual)
            "^TXCT", // S&P/TSX Renewable Energy and Clean Technology Index
            "^XCAN", // S&P/TSX Cannabis Index (CAD)
            "^XCANN", // S&P/TSX Cannabis Index (USD) NTR
            "^TTGD", // S&P/TSX Global Gold Index
            "^TXGU", // S&P/TSX Global Gold Index (USD)
            "^TXGM", // S&P/TSX Global Mining Index
            "^TXBM", // S&P/TSX Global Base Metals Index
            "^TXBMU", // S&P/TSX Global Base Metals Index (USD)
            "^MCAN", // S&P/TSX International Cannabis Index (CAD)
            "^SPTSEN", // S&P/TSX 60 Index NTR
            "^TXHB", // S&P/TSX Composite High Beta Index
            "^TXLV", // S&P/TSX Composite Low Volatility Index
            "^TXBA", // S&P/TSX Composite Index Banks (Industry Group)
        };

        public static string[] Capped = {
            "^T00C", // S&P/TSX Capped Composite Index
            "^TX6C", // S&P/TSX 60 Capped Index
            "^TTCD", // S&P/TSX Capped Consumer Discretionary Index
            "^TTCS", // S&P/TSX Capped Consumer Staples Index
            "^TTEN", // S&P/TSX Capped Energy Index
            "^TTFS", // S&P/TSX Capped Financial Index
            "^TTHC", // S&P/TSX Capped Health Care Index
            "^TTIN", // S&P/TSX Capped Industrials Index
            "^TTTK", // S&P/TSX Capped Information Technology Index
            "^TTMT", // S&P/TSX Capped Materials Index
            "^TTRE", // S&P/TSX Capped Real Estate Index
            "^TTTS", // S&P/TSX Capped Telecommunication Services Index
            "^TTUT", // S&P/TSX Capped Utilities Index
            "^RTRE", // S&P/TSX Capped REIT Index
            "^SPTRED", // S&P/TSX Capped REIT Income Index
        };

        public static string[] EqualWeight = {
            "^TXCE", // S&P/TSX Composite Equal Weight Index
            "^TXEW", // S&P/TSX 60 Equal Weight Index
            "^TXDE", // S&P/TSX Equal Weight Diversified Banks Index
            "^TXIE", // S&P/TSX Equal Weight Industrials Index
            "^TXBE", // S&P/TSX Equal Weight Global Base Metals Index
            "^TXGE", // S&P/TSX Equal Weight Global Gold Index
            "^TXOE", // S&P/TSX Equal Weight Oil & Gas Index
            "^TXBEH", // S&P/TSX Equal Weight Global Base Metals CAD Hedged Index
        };

        public static string[] Factor = {
            "^TXEV", // S&P/TSX Composite Enhanced Value Index
            "^TXEVLQ", // S&P/TSX Composite Enhanced Value - Lowest Quintile Index
            "^TXQ", // S&P/TSX Composite Quality Index
            "^TXQLQ", // S&P/TSX Composite Quality - Lowest Quintile Index
            "^TVXHQ", // S&P/TSX Composite Quality - Highest Quintile Index
            "^TXM", // S&P/TSX Composite Momentum Index
            "^TXMLQ", // S&P/TSX Composite Momentum - Lowest Quintile Index
            "^TXLVHD", // S&P/TSX Composite Low Volatility High Dividend Index
        };

        public static string[] Venture = {
            "^JX", // S&P/TSX Venture Composite Index
            "^JX10", // S&P/TSX Venture Energy (Sector) Index (CAD)
            "^JX15", // S&P/TSX Venture Materials (Sector) Index (CAD)
            "^JX20", // S&P/TSX Venture Industrials (Sector) Index (CAD)
            "^JX25", // S&P/TSX Venture Consumer Discretionary (Sector) Index (CAD)
            "^JX30", // S&P/TSX Venture Consumer Staples (Sector) Index (CAD)
            "^JX35", // S&P/TSX Venture Health Care (Sector) Index (CAD)
            "^JX40", // S&P/TSX Venture Financials (Sector) Index (CAD)
            "^JX45", // S&P/TSX Venture Information Technology (Sector) Index (CAD)
            "^JX60", // S&P/TSX Venture Real Estate (Sector) Index (CAD
            "^JX3520", // S&P/TSX Venture Pharmaceuticals, Biotechnology & Life Sciences (Industry Group) Index (CAD)
            "^JX4510 ", // S&P/TSX Venture Software & Services (Industry Group) Index (CAD)
            "^JX101020", // S&P/TSX Venture Oil, Gas & Consumable Fuels (Industry) Index (CAD)
            "^JX151040", // S&P/TSX Venture Metals & Mining (Industry) Index (CAD)
            "^JX10102020", // S&P/TSX Venture Oil & Gas Exploration & Production (Sub Industry) Index (CAD)
            "^JX15104020", // S&P/TSX Venture Diversified Metals & Mining (Sub Industry) Index (CAD)
            "^JX15104030 ", // S&P/TSX Venture Gold (Sub Industry) Index (CAD)
            "^JX15104040", // S&P/TSX Venture Precious Metals & Minerals (Sub Industry) Index (CAD)
        };
    }
}
