using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using System.Windows.Threading;

namespace Canvas
{
    public class MouseRender : StylusPlugIn, MouseRender.IInkStrokeTipBuilderBuilder, IInkDryCanvas
    {
        private ContainerVisual _mainContainerVisual;
        private ContainerVisual _mainRawInkContainerVisual;
        private DynamicRendererHostVisual _rawInkHostVisual1;
        private static readonly SolidColorBrush DefaultFillSolidColorBrush = Brushes.Black;
        private readonly Dictionary<int, StrokeInfo> _strokeInfoList;
       // private InkBoardSettings _inkBoardSettings;
        public bool IsStarted { get; private set; }
        private DynamicRendererHostVisual _rawInkHostVisual2;
        private StylusPointCollectionInfo stylusPointCollectionInfo;
        private DrawingAttributes _drawingAttributes;
        private SolidColorBrush _fillSolidColorBrush;
        private StylusPointDescription StylusPointDescription { get; set; }
        private StrokeInfo MouseStrokeInfo { get; set; }
        public TestLable InkCanvas { get; set; }
        private IInkSynchronizer _inkSynchronizer;
        private DynamicRendererHostVisual _currentHostVisual;
        private EventHandler strokesCollected;
        internal event EventHandler StrokesCollected
        {
            add
            {
                EventHandler handler2;
                EventHandler fieldsChanged = this.strokesCollected;
                do
                {
                    handler2 = fieldsChanged;
                    EventHandler handler3 = (EventHandler)Delegate.Combine(handler2, value);
                    fieldsChanged = Interlocked.CompareExchange<EventHandler>(ref this.strokesCollected, handler3, handler2);
                }
                while (fieldsChanged != handler2);
            }
            remove
            {

                EventHandler handler2;
                EventHandler fieldsChanged = this.strokesCollected;
                do
                {
                    handler2 = fieldsChanged;
                    EventHandler handler3 = (EventHandler)Delegate.Remove(handler2, value);
                    fieldsChanged = Interlocked.CompareExchange<EventHandler>(ref this.strokesCollected, handler3, handler2);
                }
                while (fieldsChanged != handler2);
            }
        }

        protected InkRenderer Render { get; set; }

        public bool AllowsMouseInput { get; set; }

        private Dispatcher _applicationDispatcher;

        public Visual RootVisual
        {
            get
            {
                if (_mainContainerVisual == null)
                {
                    CreateInkingVisuals();
                }
                return _mainContainerVisual;
            }
        }

        public IInkSynchronizer InkSynchronizer
        {
            get
            {
                return this._inkSynchronizer;
            }
            set
            {
                this._inkSynchronizer = value;
            }
        }

        public MouseRender(TestLable inkCanvas)
        {
            AllowsMouseInput= true;
            this._fillSolidColorBrush = MouseRender.DefaultFillSolidColorBrush;
            this._strokeInfoList = new Dictionary<int, MouseRender.StrokeInfo>(50);
            this.InkCanvas = inkCanvas;
            this._drawingAttributes = new DrawingAttributes
            {
                FitToCurve = false,
                IgnorePressure = false,
                StylusTip = StylusTip.Ellipse,
                Color = Colors.Blue,
                Width = 3,
                Height = 3
            };
            this._fillSolidColorBrush = Brushes.Red ;
            //if (this._inkBoardSettings == null)
            //{
            //    InkBoardSettings inkBoardSettings = new InkBoardSettings
            //    {
            //        InkColor = MouseRender.DefaultFillSolidColorBrush.Color,
            //        InkThickness = 3.0
            //    };
            //    this.UpdateInkBoardSettingsInternal(inkBoardSettings);
            //}
        }
        //private void UpdateInkBoardSettingsInternal(InkBoardSettings inkBoardSettings)
        //{
        //    if (this._inkBoardSettings == null)
        //    {
        //        this._inkBoardSettings = new InkBoardSettings();
        //    }
        //    this._inkBoardSettings.InkColor = inkBoardSettings.InkColor;
        //    this._inkBoardSettings.InkThickness = inkBoardSettings.InkThickness * inkBoardSettings.InkScale;
        //    this._drawingAttributes = this.UpdateDrawingAttributes(this._inkBoardSettings);
        //    this._fillSolidColorBrush = this.UpdateInkColor(inkBoardSettings);
        //}
        public class StylusPointCollectionInfo
        {
            public StylusPointCollectionInfo(StrokeInfo strokeInfo, StylusPointCollection stylusPoints, int timestamp)
            {
                if (strokeInfo == null)
                {
                    throw new ArgumentNullException("strokeInfo");
                }
                if (stylusPoints == null)
                {
                    throw new ArgumentNullException("stylusPoints");
                }
                StrokeInfo = strokeInfo;
                StylusPlugInCollection = stylusPoints;
                Timestamp = timestamp;
            }
            public StrokeInfo StrokeInfo { get; }

            public StylusPointCollection StylusPlugInCollection { get; }

            public int Timestamp { get; }
        }
        public class StrokeInfo
        {
            public StrokeInfo(int stylusDeviceId, DrawingAttributes drawingAttributes,SolidColorBrush fillSolidColorBrush, int startTime,InkRenderInfo inkRenderInfo)
            {
                if (drawingAttributes == null)
                {
                    throw new ArgumentNullException("drawingAttributes");
                }
                if (fillSolidColorBrush == null)
                {
                    throw new ArgumentNullException("fillSolidColorBrush");
                }
                if (inkRenderInfo == null)
                {
                    throw new ArgumentNullException("inkRenderInfo");
                }
                StylusDeviceId = stylusDeviceId;
                DrawingAttributes = drawingAttributes;
                FillSolidColorBrush= fillSolidColorBrush;
                StartTime= startTime;
                this.LastTime = startTime;
                this.StrokeNodeIterator = new StrokeNodeIterator(drawingAttributes);
                InkRenderInfo= inkRenderInfo;
            }

            public bool SeenUp { get; set; }

            public bool LostCapture { get; set; }

            public int StylusDeviceId { get; }

            public DrawingAttributes DrawingAttributes { get; }

            public SolidColorBrush FillSolidColorBrush { get; }

            public int StartTime { get; }

            public int LastTime { get; set; }

            internal StrokeNodeIterator StrokeNodeIterator { get; set; }

            public InkRenderInfo InkRenderInfo { get; }

            public bool IsTimestampWithin(int timestamp)
            {
                if (SeenUp)
                {
                    if (StartTime < LastTime)
                    {
                        if (timestamp >= StartTime)
                        {
                            return timestamp <= LastTime;
                        }
                        return false;
                    }
                    if (timestamp < StartTime)
                    {
                        return timestamp <= LastTime;
                    }
                    return true;
                }
                return true;
            }

            public bool IsTimestampAfter(int timestamp)
            {
                if (!SeenUp)
                {
                    if (LastTime >= StartTime)
                    {
                        if (timestamp >= LastTime)
                        {
                            return true;
                        }
                        if (LastTime > 0)
                        {
                            return timestamp < 0;
                        }
                        return false;
                    }
                    if (timestamp >= LastTime)
                    {
                        return timestamp <= StartTime;
                    }
                    return false;
                }
                return false;
            }
        }
        public class DynamicRendererHostVisual : HostVisual
        {
            private VisualTarget _visualTarget;

            private List<StrokeInfo> _strokeInfoList = new List<StrokeInfo>();

            internal bool InUse => _strokeInfoList.Count > 0;

            internal bool HasSingleReference => _strokeInfoList.Count == 1;

            internal VisualTarget VisualTarget
            {
                [SecuritySafeCritical]
                get
                {
                    if (_visualTarget == null)
                    {
                        _visualTarget = new VisualTarget(this);
                        _visualTarget.RootVisual = new ContainerVisual();
                    }
                    return _visualTarget;
                }
            }

            internal void AddStrokeInfoRef(StrokeInfo si)
            {
                _strokeInfoList.Add(si);
            }

            internal void RemoveStrokeInfoRef(StrokeInfo si)
            {
                _strokeInfoList.Remove(si);
            }
        }
        public class InkRenderer
        {
            public void InitStrokeTipBuilder(StylusPointCollectionInfo stylusPointCollectionInfo)
            {
                StrokeInfo strokeInfo = stylusPointCollectionInfo.StrokeInfo;
                InkRenderInfo inkRenderInfo = strokeInfo.InkRenderInfo;
                InkStrokeTipBuilder inkStrokeTipBuilder = inkRenderInfo.InkStrokeTipBuilderBuilder.GetInkStrokeTipBuilder();
                inkStrokeTipBuilder.Brush = strokeInfo.FillSolidColorBrush;
                inkStrokeTipBuilder.DrawingAttributes = strokeInfo.DrawingAttributes;
                inkRenderInfo.StrokeTipBuilder = inkStrokeTipBuilder;
                inkRenderInfo.ContainerVisual.Children.Add(inkRenderInfo.StrokeTipBuilder.CreateStroke());
                this.RenderPackets(stylusPointCollectionInfo);
            }

            public void RenderPackets(StylusPointCollectionInfo stylusPointCollectionInfo)
            {
                StrokeInfo strokeInfo = stylusPointCollectionInfo.StrokeInfo;
                InkRenderInfo inkRenderInfo = strokeInfo.InkRenderInfo;
                foreach (var stylusPoint in stylusPointCollectionInfo.StylusPlugInCollection)
                {
                    StylusPointCollection stylusPointCollection = inkRenderInfo.StrokeTipBuilder.AddPoint(stylusPoint);
                    if (stylusPointCollection != null)
                    {
                        StrokeVisual strokeVisual = new StrokeVisual(strokeInfo.FillSolidColorBrush, strokeInfo.DrawingAttributes, stylusPointCollection);
                        inkRenderInfo.ContainerVisual.Children.Add(strokeVisual);
                        inkRenderInfo.StrokeVisualList.AddLast(strokeVisual);
                        strokeVisual.Redraw();
                    }
                }
               // inkRenderInfo.StrokeTipBuilder.Redraw();
            }

            public void CleanStrokeTipBuilder(StylusPointCollectionInfo stylusPointCollectionInfo)
            {
                this.RenderPackets(stylusPointCollectionInfo);
            }
        }
        public class InkRenderInfo
        {
            public InkRenderInfo(IInkStrokeTipBuilderBuilder inkStrokeTipBuilderBuilder, ContainerVisual containerVisual)
            {
                StrokeVisualList = new LinkedList<StrokeVisual>();
                if (inkStrokeTipBuilderBuilder == null)
                {
                    throw new ArgumentNullException("inkStrokeTipBuilderBuilder");
                }
                if (containerVisual == null)
                {
                    throw new ArgumentNullException("containerVisual");
                }
                InkStrokeTipBuilderBuilder = inkStrokeTipBuilderBuilder;
                ContainerVisual = containerVisual;
                this.Dispatcher = containerVisual.Dispatcher;
            }

            public ContainerVisual ContainerVisual { get; }

            public LinkedList<StrokeVisual> StrokeVisualList { get; }

            public InkStrokeTipBuilder StrokeTipBuilder
            {
                get
                {
                    return this._strokeTipBuilder;
                }
                set
                {
                    this._strokeTipBuilder = value;
                }
            }

            public IInkStrokeTipBuilderBuilder InkStrokeTipBuilderBuilder { get; }

            public Dispatcher Dispatcher { get; set; }

            private InkStrokeTipBuilder _strokeTipBuilder;
        }
        public class HardPenInkStrokeTipBuilder : InkStrokeTipBuilder
        {
            public override StylusPointCollection AddPoint(StylusPoint stylusPoint)
            {
                if (this._stylusPointQueue == null)
                {
                    this._stylusPointQueue = new StylusPointQueue(18);
                    this._stylusPointQueue.Enqueue(stylusPoint);
                    return null;
                }
                StylusPointCollection stylusPointCollection2;
                int num = this._stylusPointQueue.Count;

                if (num != 16)
                {
                    this._stylusPointQueue.Enqueue(stylusPoint);
                }
                stylusPointCollection2 = new StylusPointCollection(stylusPoint.Description);
                int num2 = 0;
                if (num < 8)
                {
                    stylusPointCollection2.Add(this._stylusPointQueue.Dequeue());
                    num2++;
                    num = num2;
                }
                stylusPointCollection2.Add(this._stylusPointQueue[0]);
                return stylusPointCollection2;
            }

            public override StylusPointCollection StrokeTipCollection
            {
                get
                {
                    if (this._stylusPointQueue == null || this._stylusPointQueue.Count == 0)
                    {
                        return null;
                    }
                    if (this._stylusPointCollection == null && 2 != 0)
                    {
                        this._stylusPointCollection = new StylusPointCollection(this._stylusPointQueue[0].Description, 18);
                    }
                    StylusPointCollection stylusPointCollection = this._stylusPointCollection;
                    this._stylusPointQueue.CopyToStylusPointCollection(stylusPointCollection);
                    return stylusPointCollection;
                }
            }

            public override void Redraw()
            {
                if (this._stylusPointQueue != null)
                {
                    if(this._stylusPointQueue.Count != 0)
                    {
                        base.Redraw();
                    }
                }
            }

            private StylusPointCollection _stylusPointCollection;

            private StylusPointQueue _stylusPointQueue;
        }
        public abstract class InkStrokeTipBuilder : IInkStrokeTipBuilder
        {
            public abstract StylusPointCollection AddPoint(StylusPoint stylusPoint);

            public abstract StylusPointCollection StrokeTipCollection { get; }

            protected StrokeTipVisual BuildingStroke { get; set; }

            public Visual CreateStroke()
            {
                this.BuildingStroke = new StrokeTipVisual(this.Brush, this.DrawingAttributes);
                return this.BuildingStroke;
            }

            public virtual void Redraw()
            {
                this.BuildingStroke.Redraw(this.StrokeTipCollection);
            }

            public DrawingAttributes DrawingAttributes { get; set; }

            public SolidColorBrush Brush { get; set; }

            public DrawingVisual GetBuildingStroke()
            {
                return this.BuildingStroke;
            }

            public const int RearPointCount = 8;
        }
        public interface IInkStrokeTipBuilder
        {
            DrawingVisual GetBuildingStroke();
        }

        public interface IInkStrokeTipBuilderBuilder
        {
            InkStrokeTipBuilder GetInkStrokeTipBuilder();
        }
        private void CreateInkingVisuals()
        {
            if (_mainContainerVisual == null)
            {
                _mainContainerVisual = new ContainerVisual();
                _mainRawInkContainerVisual = new ContainerVisual();
                _mainContainerVisual.Children.Add(_mainRawInkContainerVisual);
            }
            if (base.IsActiveForInput)
            {
                using (base.Element.Dispatcher.DisableProcessing())
                {
                    CreateRealTimeVisuals();
                }
            }
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            _applicationDispatcher = base.Element.Dispatcher;
            if (base.IsActiveForInput)
            {
                CreateRealTimeVisuals();
            }
        }
        //protected virtual SolidColorBrush UpdateInkColor(InkBoardSettings inkBoardSettings)
        //{
        //    SolidColorBrush solidColorBrush = new SolidColorBrush(inkBoardSettings.InkColor);
        //    solidColorBrush.Freeze();
        //    return solidColorBrush;
        //}
        //protected virtual DrawingAttributes UpdateDrawingAttributes(InkBoardSettings inkBoardSettings)
        //{
        //    return new DrawingAttributes
        //    {
        //        FitToCurve = false,
        //        IgnorePressure = false,
        //        StylusTip = StylusTip.Ellipse,
        //        Color = inkBoardSettings.InkColor,
        //        Width = inkBoardSettings.InkThickness,
        //        Height = inkBoardSettings.InkThickness
        //    };
        //}
        private void CreateRealTimeVisuals()
        {
            if (_mainContainerVisual != null && _rawInkHostVisual1 == null)
            {
                _rawInkHostVisual1 = new DynamicRendererHostVisual();
                _rawInkHostVisual2 = new DynamicRendererHostVisual();
                _currentHostVisual = null;
                _mainContainerVisual.Children.Add(_rawInkHostVisual1);
                _mainContainerVisual.Children.Add(_rawInkHostVisual2);
                //_renderingThread = DynamicRendererThreadManager.GetCurrentThreadInstance();
            }
        }
        #region override
        protected override void OnEnabledChanged()
        {
            base.OnEnabledChanged();
            if (!base.Enabled)
            {
               // AbortAllStrokes();
            }
        }

        protected override void OnIsActiveForInputChanged()
        {
            base.OnIsActiveForInputChanged();
        }

        protected override void OnRemoved()
        {
        }

        protected override void OnStylusDown(RawStylusInput rawStylusInput)
        {
            Down(rawStylusInput.StylusDeviceId, rawStylusInput.Timestamp, rawStylusInput.GetStylusPoints());
            rawStylusInput.NotifyWhenProcessed(rawStylusInput.StylusDeviceId);
            base.OnStylusDown(rawStylusInput);
        }

        protected override void OnStylusDownProcessed(object callbackData, bool targetVerified)
        {

        }

        protected override void OnStylusEnter(RawStylusInput rawStylusInput, bool confirmed)
        {

        }

        protected override void OnStylusLeave(RawStylusInput rawStylusInput, bool confirmed)
        {

        }

        protected override void OnStylusMove(RawStylusInput rawStylusInput)
        {
            int num = rawStylusInput.StylusDeviceId;
            if(!AllowsMouseInput)
            {
                return;
            }
            this.Move(rawStylusInput.StylusDeviceId, rawStylusInput.Timestamp, rawStylusInput.GetStylusPoints());
            base.OnStylusMove(rawStylusInput);
        }

        protected override void OnStylusMoveProcessed(object callbackData, bool targetVerified)
        {

        }

        protected override void OnStylusUp(RawStylusInput rawStylusInput)
        {

        }

        protected override void OnStylusUpProcessed(object callbackData, bool targetVerified)
        {

        }
        #endregion override
        #region method
        public virtual void Start()
        {
        }
        public void Down(int stylusDeviceId, int timestamp, StylusPointCollection stylusPointCollection)
        {
            if (IsMouse(stylusDeviceId))
            {
                this.MouseDown(timestamp, stylusPointCollection);
                return;
            }
            StrokeInfo strokeInfo;
            if (!this._strokeInfoList.TryGetValue(stylusDeviceId, out strokeInfo))
            {
                strokeInfo = new StrokeInfo(stylusDeviceId, _drawingAttributes, _fillSolidColorBrush, timestamp, new InkRenderInfo(this, this.GetContainerVisual(stylusDeviceId)));
                this._strokeInfoList[stylusDeviceId] = strokeInfo;
                StylusPointCollectionInfo stylusPointCollectionInfo = new StylusPointCollectionInfo(strokeInfo, stylusPointCollection, timestamp);
                this.Run(strokeInfo, delegate
                {
                    OnDown(stylusPointCollectionInfo);
                });
                return;
            }
            if (strokeInfo.SeenUp)
            {
                return;
            }
            Console.WriteLine(string.Format("在按下的时候找到两个 stylusDeviceId {0} 当前一共有 {1} 个点按下", stylusDeviceId, this._strokeInfoList.Count), new string[0]);
        }
        private static bool IsMouse(int id)
        {
            return id == -1 || id == 0;
        }
        public void Move(int stylusDeviceId, int timestamp, StylusPointCollection stylusPointCollection)
        {
            if (IsMouse(stylusDeviceId))
            {
                MouseMove(timestamp, stylusPointCollection);
                return;
            }
            StrokeInfo strokeInfo;
            if (_strokeInfoList.TryGetValue(stylusDeviceId, out strokeInfo))
            {
                if (strokeInfo == null)
                {
                    throw new ArgumentNullException("在移动过程拿到的 StrokeInfo 是空");
                }
                if (strokeInfo.IsTimestampAfter(timestamp))
                {
                   stylusPointCollectionInfo = new StylusPointCollectionInfo(strokeInfo, stylusPointCollection, timestamp);
                    this.Run(strokeInfo, delegate
                    {
                        OnMove(stylusPointCollectionInfo);
                    });
                    strokeInfo.LastTime = timestamp;
                }
            }
        }
        private void Run(StrokeInfo strokeInfo, Action action)
        {
            strokeInfo.InkRenderInfo.Dispatcher.InvokeAsync(delegate ()
            {
                try
                {
                    action();
                }
                catch (Exception)
                {

                }
            }, DispatcherPriority.Send);
        }
        private void MouseMove(int timestamp, StylusPointCollection stylusPointCollection)
        {
            StrokeInfo mouseStrokeInfo = MouseStrokeInfo;
            if (mouseStrokeInfo != null)
            {
                stylusPointCollection = this.SetStylusPointDescription(stylusPointCollection);
                StylusPointCollectionInfo stylusPointCollectionInfo = new StylusPointCollectionInfo(mouseStrokeInfo, stylusPointCollection, timestamp);
                this.OnMove(stylusPointCollectionInfo);
            }
        }
        protected virtual void OnMove(StylusPointCollectionInfo stylusPointCollectionInfo)
        {
            this.Render.RenderPackets(stylusPointCollectionInfo);
        }
        private StylusPointCollection SetStylusPointDescription(StylusPointCollection stylusPointCollection)
        {
            StylusPointDescription stylusPointDescription;
            stylusPointDescription =this.StylusPointDescription;
            StylusPointCollection stylusPointCollection2;
            stylusPointCollection2 = new StylusPointCollection(stylusPointDescription, stylusPointCollection.Count);
            if (stylusPointDescription != null)
            {
                foreach(var stylusPoint in stylusPointCollection)
                {
                    var item = new StylusPoint(stylusPoint.X, stylusPoint.Y, stylusPoint.PressureFactor, stylusPointDescription, new int[StylusPointDescription.PropertyCount-3]);
                    stylusPointCollection2.Add(item);
                }
            }
            return stylusPointCollection2;
        }
        protected virtual ContainerVisual GetContainerVisual(int stylusDeviceId)
        {
                int id = stylusDeviceId;
                if (IsMouse(id))
                {
                    return this.InkCanvas.InkFromMouseContainerVisual;
                }
                else
                {
                    return this.InkCanvas.InkFromTouchContainerVisual;
                }
        }
        private void MouseDown(int timestamp, StylusPointCollection stylusPointCollection)
        {
            if (this.MouseStrokeInfo != null)
            {
                this.OnUp(new StylusPointCollectionInfo(this.MouseStrokeInfo, new StylusPointCollection(this.StylusPointDescription), timestamp));
            }
            this.StylusPointDescription = stylusPointCollection.Description;
            stylusPointCollection = this.SetStylusPointDescription(stylusPointCollection);
            StrokeInfo strokeInfo;
            strokeInfo = new StrokeInfo(-1, this._drawingAttributes, this._fillSolidColorBrush, timestamp, new InkRenderInfo(this, this.GetContainerVisual(0)));
            StylusPointCollectionInfo stylusPointCollectionInfo = new StylusPointCollectionInfo(strokeInfo, stylusPointCollection, timestamp);
            this.OnDown(stylusPointCollectionInfo);
            this.MouseStrokeInfo = strokeInfo;
        }
        protected virtual void OnUp(StylusPointCollectionInfo stylusPointCollectionInfo)
        {
            this.Render.RenderPackets(stylusPointCollectionInfo);
        }
        protected virtual void OnDown(StylusPointCollectionInfo stylusPointCollectionInfo)
        {
            this.Render.InitStrokeTipBuilder(stylusPointCollectionInfo);
        }
        public virtual InkStrokeTipBuilder GetInkStrokeTipBuilder()
        {
            return new HardPenInkStrokeTipBuilder();
        }
        public virtual void Stop()
        {
            this.Rest();
        }
        public void Rest()
        {

                Dictionary<int,StrokeInfo> strokeInfoList = this._strokeInfoList;
                 strokeInfoList.Clear();
                this.InkCanvas.RunInTouchDispatcher(() =>
                {
                    ContainerVisual visual = new ContainerVisual();
                    visual.Children.Clear();
                });
            this.InkCanvas.RunInMouseDispatcher((ContainerVisual visual) =>
            {
                visual.Children.Clear();
            });
        }
        public void Init()
        {
            this.Render = new MouseRender.InkRenderer();
            this.OnInit();
            bool isStarted = true;
            this.IsStarted = isStarted;
        }
        protected virtual void OnInit()
        {
        }
        #endregion
    }
}
