namespace TradeHub.Core.Math
{
    public static class MathHelper
    {
        /// <remarks>
        /// https://stackoverflow.com/questions/15623129/simple-linear-regression-for-data-set
        /// </remarks>
        public static void LinearRegression(decimal[] xVals, decimal[] yVals,
                                        int inclusiveStart, int exclusiveEnd,
                                        out decimal yintercept, out decimal slope)
        {
            decimal sumOfX = 0;
            decimal sumOfY = 0;
            decimal sumOfXSq = 0;
            decimal sumOfYSq = 0;
            decimal ssX = 0;
            decimal sumCodeviates = 0;
            decimal sCo = 0;
            decimal count = exclusiveEnd - inclusiveStart;

            for (int ctr = inclusiveStart; ctr < exclusiveEnd; ctr++)
            {
                var x = xVals[ctr];
                var y = yVals[ctr];
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }
            ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            var meanX = sumOfX / count;
            var meanY = sumOfY / count;
            yintercept = meanY - ((sCo / ssX) * meanX);
            slope = sCo / ssX;
        }
    }
}
