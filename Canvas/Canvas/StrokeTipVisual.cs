using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace Canvas
{
    public class StrokeTipVisual : DrawingVisual
    {
        public StrokeTipVisual(SolidColorBrush brush, DrawingAttributes drawingAttributes)
        {
            this._brush = brush;
            this._drawingAttributes = drawingAttributes;
        }

        public void Redraw(StylusPointCollection stylusPoints)
        {
            Geometry geometry;
            Rect rect;
            StrokeRenderer.CalcGeometryAndBounds(StrokeNodeIterator.GetIterator(stylusPoints, this._drawingAttributes), this._drawingAttributes, false, out geometry, out rect);
            DrawingContext drawingContext = base.RenderOpen();
            try
            {
                drawingContext.DrawGeometry(this._brush, null, geometry);
            }
            finally
            {
                if (drawingContext != null)
                {
                    ((IDisposable)drawingContext).Dispose();
                }
            }
        }

        private readonly SolidColorBrush _brush;

        private readonly DrawingAttributes _drawingAttributes;
    }
}
