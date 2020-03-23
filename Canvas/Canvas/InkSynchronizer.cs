using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
namespace Canvas
{
    public class InkSynchronizer : IInkSynchronizer
    {
        internal InkSynchronizer(IInkDryCanvas dynamicRenderer)
        {
            this._dynamicRenderer = dynamicRenderer;
            dynamicRenderer.InkSynchronizer = this;
        }

        void IInkSynchronizer.AddRawStroke(MouseRender.StrokeInfo strokeInfo)
        {
            if (strokeInfo == null)
            {
                throw new ArgumentNullException("strokeInfo");
            }
            object lockObject = this._lockObject;
            bool flag = false;
            bool flag2;
            flag2 = flag;
            try
            {
                Monitor.Enter(lockObject, ref flag2);
                this._strokeInfoList.Add(strokeInfo);
            }
            finally
            {
                Monitor.Exit(lockObject);
            }

        }

        public List<InkSynchronizer.StrokeSynchronizer> BeginDry()
        {
            object lockObject = this._lockObject;
            List<MouseRender.StrokeInfo> list2;
            lock (lockObject)
            {
                List<MouseRender.StrokeInfo> list = this._strokeInfoList.ToList<MouseRender.StrokeInfo>();
                if (!false)
                {
                    list2 = list;
                }
                this._strokeInfoList.Clear();
            }
            List<InkSynchronizer.StrokeSynchronizer> list3 = new List<InkSynchronizer.StrokeSynchronizer>();
            foreach (MouseRender.StrokeInfo strokeInfo in list2)
            {
                if (strokeInfo != null)
                {
                    MouseRender.InkRenderInfo inkRenderInfo = strokeInfo.InkRenderInfo;
                    StylusPointCollection inkRenderInfoStylusPointList = this.GetInkRenderInfoStylusPointList(inkRenderInfo);
                    if (inkRenderInfoStylusPointList != null && inkRenderInfoStylusPointList.Any<StylusPoint>())
                    {
                        list3.Add(new InkSynchronizer.StrokeSynchronizer(inkRenderInfoStylusPointList, strokeInfo.StylusDeviceId, strokeInfo.DrawingAttributes)
                        {
                            LostCapture = strokeInfo.LostCapture
                        });
                    }
                    DryingDrawingVisual dryingDrawingVisual = new DryingDrawingVisual(inkRenderInfo.ContainerVisual, InkRenderVisual.GetInkRenderDryingDrawVisual(inkRenderInfo));
                    this._dynamicRenderer.InkCanvas.AddDryingDrawingVisualList(dryingDrawingVisual);
                }
            }
            return list3;
        }

        public void EndDry()
        {
            this._dynamicRenderer.InkCanvas.NotifyOnNextRenderComplete();
        }

        internal void Clean()
        {
            object lockObject = this._lockObject;
            lock (lockObject)
            {
                this._strokeInfoList.Clear();
            }
        }

        private StylusPointCollection GetInkRenderInfoStylusPointList(MouseRender.InkRenderInfo inkRender)
        {
            StylusPointCollection stylusPointCollection = this.GetStylusPointsFromStrokeVisualList(inkRender.StrokeVisualList);
            MouseRender.InkStrokeTipBuilder strokeTipBuilder = inkRender.StrokeTipBuilder;
            MouseRender.InkStrokeTipBuilder inkStrokeTipBuilder;
            if (true)
            {
                inkStrokeTipBuilder = strokeTipBuilder;
            }
            if (inkStrokeTipBuilder != null)
            {
                StylusPointCollection strokeTipCollection = inkStrokeTipBuilder.StrokeTipCollection;
                if (strokeTipCollection != null && strokeTipCollection.Any<StylusPoint>())
                {
                    if (2 != 0 && stylusPointCollection != null)
                    {
                        stylusPointCollection.Add(strokeTipCollection);
                    }
                    else
                    {
                        try
                        {
                            stylusPointCollection = this.CloneStylusPointCollection(strokeTipCollection);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            return stylusPointCollection;
        }

        private StylusPointCollection GetStylusPointsFromStrokeVisualList(LinkedList<StrokeVisual> strokeVisualList)
        {
            StylusPointCollection stylusPointCollection = null;
            foreach(var item in strokeVisualList)
            {
                var stylusPoints=item.Stroke.StylusPoints;
                if (stylusPointCollection == null)
                {
                    stylusPointCollection = new StylusPointCollection(stylusPoints.Description);
                }
                stylusPointCollection.Add(stylusPoints);
            }
            return stylusPointCollection;
        }

        private StylusPointCollection CloneStylusPointCollection(StylusPointCollection spc)
        {
            StylusPointCollection stylusPointCollection;
            stylusPointCollection = new StylusPointCollection(spc.Description, spc.Count);
            foreach(var item in spc)
                {
                    stylusPointCollection.Add(item);
                }
            return stylusPointCollection;
        }

        private readonly object _lockObject = new object();

        private readonly List<MouseRender.StrokeInfo> _strokeInfoList = new List<MouseRender.StrokeInfo>();

        private readonly List<DrawingVisual> _dryingDrawingVisualList = new List<DrawingVisual>();

        private readonly IInkDryCanvas _dynamicRenderer;

        public class StrokeSynchronizer
        {
            internal StrokeSynchronizer(StylusPointCollection stylusPointCollection, int stylusDeviceId, DrawingAttributes drawingAttributes)
            {
                StylusPointCollection = stylusPointCollection;
                StylusDeviceId = stylusDeviceId;
                DrawingAttributes = drawingAttributes;
            }

            public StylusPointCollection StylusPointCollection { get; }

            public int StylusDeviceId { get; }

            public DrawingAttributes DrawingAttributes { get; }

            public bool LostCapture { get; set; }
        }
    }
}
