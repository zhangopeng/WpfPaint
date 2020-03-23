using System;
using System.Collections.Generic;
using System.Windows.Media;
namespace Canvas
{
    internal static class InkRenderVisual
    {
        public static List<DrawingVisual> GetInkRenderDryingDrawVisual(MouseRender.InkRenderInfo inkRender)
        {
            List<DrawingVisual> dryingDrawVisualFromStrokeVisualList;

                    dryingDrawVisualFromStrokeVisualList = InkRenderVisual.GetDryingDrawVisualFromStrokeVisualList(inkRender.StrokeVisualList);
                MouseRender.InkStrokeTipBuilder strokeTipBuilder = inkRender.StrokeTipBuilder;
                DrawingVisual drawingVisual = (strokeTipBuilder != null) ? ((MouseRender.IInkStrokeTipBuilder)strokeTipBuilder).GetBuildingStroke() : null;
                DrawingVisual drawingVisual2;

                    drawingVisual2 = drawingVisual;
                if ( drawingVisual2 != null)
                {
                    dryingDrawVisualFromStrokeVisualList.Add(drawingVisual2);
                }
            return dryingDrawVisualFromStrokeVisualList;
        }

        private static List<DrawingVisual> GetDryingDrawVisualFromStrokeVisualList(LinkedList<StrokeVisual> inkRenderStrokeVisualList)
        {
            List<DrawingVisual> list = new List<DrawingVisual>();
            foreach(var item in inkRenderStrokeVisualList)
            {
                list.Add(item);
            }
            return list;
        }
    }
}
