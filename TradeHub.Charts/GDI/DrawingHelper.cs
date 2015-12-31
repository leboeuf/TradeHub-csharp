using System;
using System.Drawing;

namespace TradeHub.Charts.GDI
{
    /// <summary>
    /// Contains wrappers around GDI drawing functions to allow debugging.
    /// </summary>
    /// <remarks>
    /// This static helper is not thread-safe because it uses a public Bitmap instance that could be overwritten whenever more than one chart is generated at a time.
    /// </remarks>
    public static class DrawingHelper
    {
        /// <summary>
        /// When set to true, will output an image file after each method call.
        /// </summary>
        private const bool DEBUG = false;

        private const string DEBUG_FOLDER = @"C:\TradeHub.Charts.Debug\"; // Path MUST exist
        private static int debugIteration = 0;

        public static int Width;
        public static int Height;
        public static Bitmap BitmapToDebug;

        public static void DrawRectangle(Graphics g, Pen pen, float p1, float p2, float p3, float p4)
        {
            g.DrawRectangle(pen, p1, p2, p3, p4);
            DebugGraphics();
        }

        public static void DrawLine(Graphics g, Pen pen, float x1, float y1, float x2, float y2)
        {
            g.DrawLine(pen, x1, y1, x2, y2);
            DebugGraphics();
        }

        public static void DrawString(Graphics g, string label, Font f, Brush brush, PointF point)
        {
            g.DrawString(label, f, brush, point);
            DebugGraphics();
        }

        private static void DebugGraphics()
        {
            if (DEBUG)
            {
                BitmapToDebug.Save(String.Format("{0}{1}{2}", DEBUG_FOLDER, debugIteration++, ".png"), System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
