using System;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace Canvas
{
    public class StrokeVisual : DrawingVisual
    {
        public StrokeVisual(SolidColorBrush brush, DrawingAttributes drawingAttributes)
        {
            this._brush = brush;
            this._drawingAttributes = drawingAttributes;
        }

        public StrokeVisual(SolidColorBrush brush, DrawingAttributes drawingAttributes, StylusPointCollection stylusPointCollection) : this(brush, drawingAttributes)
        {
            this.Stroke = new Stroke(stylusPointCollection)
            {
                DrawingAttributes = this._drawingAttributes
            };
        }

        public Stroke Stroke { get; set; }

        public void Redraw()
        {
            using (DrawingContext drawingContext = base.RenderOpen())
            {
                Geometry geometry;
                Rect rect;
                StrokeRenderer.CalcGeometryAndBounds(StrokeNodeIterator.GetIterator(this.Stroke, this._drawingAttributes), this._drawingAttributes, false, out geometry, out rect);
                drawingContext.DrawGeometry(this._brush, null, geometry);
            }
        }

        private readonly SolidColorBrush _brush;

        private readonly DrawingAttributes _drawingAttributes;
    }
}
