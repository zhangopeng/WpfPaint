using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using System.Windows.Threading;

namespace Canvas
{
    public class TestLable : UIElement, IDisposable, IStylusPlugInSource, IInputElement
    {
        private readonly HostVisual _hostVisual;
        private readonly DispatcherRequiring _invokeStrokesCollected;
        public InkSynchronizer InkSynchronizer { get; private set; }
        private IStylusPlugInSource _eventSource;
        private readonly EventHandler _mainRenderComplete;
        private MouseRender _dynamicRenderer;
        private readonly List<DryingDrawingVisual> _dryingDrawingVisualList = new List<DryingDrawingVisual>();
        private bool _isStarted;
        public IStylusPlugInSource EventSource
        {
            get
            {
                IStylusPlugInSource result;
                if ((result = this._eventSource) == null)
                {
                    result = this;
                }
                return result;
            }
            set
            {
                if (this._isInit)
                {
                    throw new InvalidOperationException("禁止在初始化之后设置输入源");
                }
                IStylusPlugInSource eventSource = this._eventSource;
                if (eventSource == value)
                {
                    if (!false)
                    {
                        return;
                    }
                }
                else
                {
                    MouseRender dynamicRenderer = this.DynamicRenderer;
                    if (dynamicRenderer != null && eventSource != null)
                    {
                        eventSource.RemoveStylusPlugIns(dynamicRenderer);
                        value.AddStylusPlugIns(dynamicRenderer);
                    }
                }
                this._eventSource = value;
                if (value != null && value != this)
                {
                    this._needInput = false;
                }
            }
        }
        private EventHandler started;
        private event EventHandler Started
        {
            add
            {
                EventHandler handler2;
                EventHandler fieldsChanged = this.started;
                do
                {
                    handler2 = fieldsChanged;
                    EventHandler handler3 = (EventHandler)Delegate.Combine(handler2, value);
                    fieldsChanged = Interlocked.CompareExchange<EventHandler>(ref this.started, handler3, handler2);
                }
                while (fieldsChanged != handler2);
            }
            remove
            {

                EventHandler handler2;
                EventHandler fieldsChanged = this.started;
                do
                {
                    handler2 = fieldsChanged;
                    EventHandler handler3 = (EventHandler)Delegate.Remove(handler2, value);
                    fieldsChanged = Interlocked.CompareExchange<EventHandler>(ref this.started, handler3, handler2);
                }
                while (fieldsChanged != handler2);
            }
        }
        private EventHandler<MouseRender> dynamicRendererChanged;
        public event EventHandler<MouseRender> DynamicRendererChanged
        {
            add
            {
                EventHandler<MouseRender> handler2;
                EventHandler<MouseRender> fieldsChanged = this.dynamicRendererChanged;
                do
                {
                    handler2 = fieldsChanged;
                    EventHandler<MouseRender> handler3 = (EventHandler<MouseRender>)Delegate.Combine(handler2, value);
                    fieldsChanged = Interlocked.CompareExchange<EventHandler<MouseRender>>(ref this.dynamicRendererChanged, handler3, handler2);
                }
                while (fieldsChanged != handler2);
            }
            remove
            {
                EventHandler<MouseRender> handler2;
                EventHandler<MouseRender> fieldsChanged = this.dynamicRendererChanged;
                do
                {
                    handler2 = fieldsChanged;
                    EventHandler<MouseRender> handler3 = (EventHandler<MouseRender>)Delegate.Remove(handler2, value);
                    fieldsChanged = Interlocked.CompareExchange<EventHandler<MouseRender>>(ref this.dynamicRendererChanged, handler3, handler2);
                }
                while (fieldsChanged != handler2);
            }
        }
        private EventHandler<InkSynchronizer> strokesCollected;
        public event EventHandler<InkSynchronizer> StrokesCollected
        {
            add
            {
                EventHandler<InkSynchronizer> handler2;
                EventHandler<InkSynchronizer> fieldsChanged = this.strokesCollected;
                do
                {
                    handler2 = fieldsChanged;
                    EventHandler<InkSynchronizer> handler3 = (EventHandler<InkSynchronizer>)Delegate.Combine(handler2, value);
                    fieldsChanged = Interlocked.CompareExchange<EventHandler<InkSynchronizer>>(ref this.strokesCollected, handler3, handler2);
                }
                while (fieldsChanged != handler2);
            }
            remove
            {
                EventHandler<InkSynchronizer> handler2;
                EventHandler<InkSynchronizer> fieldsChanged = this.strokesCollected;
                do
                {
                    handler2 = fieldsChanged;
                    EventHandler<InkSynchronizer> handler3 = (EventHandler<InkSynchronizer>)Delegate.Remove(handler2, value);
                    fieldsChanged = Interlocked.CompareExchange<EventHandler<InkSynchronizer>>(ref this.strokesCollected, handler3, handler2);
                }
                while (fieldsChanged != handler2);
            }
        }
        public MouseRender DynamicRenderer
        {
            get
            {
                return this._dynamicRenderer;
            }
            set
            {
                if (this._dynamicRenderer != value)
                {
                    if (this._dynamicRenderer != null)
                    {
                        this._dynamicRenderer.Stop();
                        this.EventSource.RemoveStylusPlugIns(this._dynamicRenderer);
                        this._dynamicRenderer.StrokesCollected -= this.DynamicRenderer_StrokesCollected;
                    }
                    this._dynamicRenderer = value;
                    this.InkSynchronizer = new InkSynchronizer(value);
                    if (this._isInit)
                    {
                        this.InitRender();
                        this.Inited += this.InkCanvas_Inited;
                    }
                    if (this._isStarted)
                    {
                        this.EventSource.AddStylusPlugIns(value);
                    }
                    this._dynamicRenderer.StrokesCollected -= this.DynamicRenderer_StrokesCollected;
                    this._dynamicRenderer.StrokesCollected += this.DynamicRenderer_StrokesCollected;

                    EventHandler<MouseRender> dynamicRendererChanged = this.dynamicRendererChanged;
                    if (dynamicRendererChanged == null)
                    {
                        return;
                    }
                    dynamicRendererChanged(this, value);
                    return;
                }
            }
        }
        private bool _needInput = true;
        private DynamicRendererThreadManager DynamicRendererThreadManager { get; }
        private bool _isInit;
        private VisualTarget _visualTarget;
        private IInkCanvasThreadManager InkCanvasThreadManager { get; }
        private Rect _hitTestRect;
        public ContainerVisual InkFromMouseContainerVisual { get; private set; }
        public ContainerVisual InkFromTouchContainerVisual { get; private set; }
        InkPresenter ip;
        protected override int VisualChildrenCount
        {
            get { return 2; }
        }
        public void Init()
        {
            if (this._isInit)
            {
                throw new InvalidOperationException("已经初始化不能再次调用此函数");
            }
            this._isInit = true;
            this.DynamicRendererThreadManager.StartUp();
            base.AddVisualChild(this._hostVisual);
            this.RunInTouchDispatcher(delegate ()
            {
                this._visualTarget = new VisualTarget(this._hostVisual);
                ContainerVisual containerVisual = new ContainerVisual();
                this.InkFromTouchContainerVisual = containerVisual;
                this._visualTarget.RootVisual = containerVisual;
            });
            this.InkFromMouseContainerVisual = new ContainerVisual();
            base.AddVisualChild(this.InkFromMouseContainerVisual);
            if (this.DynamicRenderer == null)
            {
                this.DynamicRenderer = new MouseRender(this);
            }
            this.Start();
            EventHandler inited = this.inited;
            if (inited == null)
            {
                return;
            }
            inited(this, null);
        }
        private EventHandler inited;
        public event EventHandler Inited
        {
            add
            {
                EventHandler handler2;
                EventHandler fieldsChanged = this.inited;
                do
                {
                    handler2 = fieldsChanged;
                    EventHandler handler3 = (EventHandler)Delegate.Combine(handler2, value);
                    fieldsChanged = Interlocked.CompareExchange<EventHandler>(ref this.inited, handler3, handler2);
                }
                while (fieldsChanged != handler2);
            }
            remove
            {

                EventHandler handler2;
                EventHandler fieldsChanged = this.inited;
                do
                {
                    handler2 = fieldsChanged;
                    EventHandler handler3 = (EventHandler)Delegate.Remove(handler2, value);
                    fieldsChanged = Interlocked.CompareExchange<EventHandler>(ref this.inited, handler3, handler2);
                }
                while (fieldsChanged != handler2);
            }
        }
        public void RunInTouchDispatcher(Action action)
        {
            this.InkCanvasThreadManager.RunInTouchDispatcher(action);
        }
        public void RunInMouseDispatcher(Action<ContainerVisual> action)
        {
            ContainerVisual inkFromMouseContainerVisual = this.InkFromMouseContainerVisual;
            this.InkCanvasThreadManager.RunInMainDispatcher(delegate
            {
                action(inkFromMouseContainerVisual);
            });
        }
        public TestLable() : this(true)
        {
            //ip = new InkPresenter();
            //this.AddVisualChild(ip);
            //MouseRender renderer = new MouseRender(this);
            //ip.AttachVisuals(renderer.RootVisual, new DrawingAttributes());
            //renderer.Enabled = true;
            //this.StylusPlugIns.Add(renderer);
        }

        public TestLable(bool withInit = true) : this(withInit, null)
        {
        }
        private void InkCanvas_Inited(object sender, EventArgs e)
        {
            this.Inited -= this.InkCanvas_Inited;
            this.InitRender();
        }
        private void InitRender()
        {
            this.RunInTouchDispatcher(delegate ()
            {
                this.DynamicRenderer.Init();
                if (this._isStarted)
                {
                    this.DynamicRenderer.Start();
                }
            });
        }
        public void Start()
        {
            if (this._isStarted)
            {
                return;
            }
            IStylusPlugInSource eventSource = this.EventSource;
            if (!eventSource.ContainsStylusPlugIns(this.DynamicRenderer))
            {
                eventSource.AddStylusPlugIns(this.DynamicRenderer);
            }
            eventSource.StylusUp += this.Source_StylusUp;
            eventSource.MouseUp += this.Source_MouseUp;
            this.DynamicRenderer.Start();
            base.IsHitTestVisible = this._needInput;
            this._isStarted = true;
            EventHandler started = this.started;
            if (started == null)
            {
                return;
            }
            started(this, null);
        }
        private void DynamicRenderer_StrokesCollected(object sender, EventArgs e)
        {
            base.Dispatcher.InvokeAsync(new Action(this.OnStrokesCollected));
        }

        protected virtual void OnStrokesCollected()
        {
            this._invokeStrokesCollected.Require();
        }

        public void Stop()
        {
            if (this._isStarted)
            {
                this.DynamicRenderer.Stop();
                IStylusPlugInSource eventSource = this.EventSource;
                eventSource.StylusUp -= this.Source_StylusUp;
                eventSource.MouseUp -= this.Source_MouseUp;
                eventSource.RemoveStylusPlugIns(this.DynamicRenderer);
            }
            base.IsHitTestVisible = false;
            this.InkSynchronizer.Clean();
            this._isStarted = false;
        }
        private void Source_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void Source_StylusUp(object sender, StylusEventArgs e)
        {
        }
        public void Close()
        {
            this.Stop();
            this.DynamicRendererThreadManager.Close();
        }
        internal TestLable(bool withInit, IInkCanvasThreadManager inkCanvasThreadManager)
        {
            DynamicRendererThreadManager = new DynamicRendererThreadManager();
            this._hostVisual = new HostVisual();
            this._hitTestRect = Rect.Empty;
            if (inkCanvasThreadManager == null)
            {
                inkCanvasThreadManager = new InkCanvasThreadManager(this, this.DynamicRendererThreadManager);
            }
            InkCanvasThreadManager = inkCanvasThreadManager;
            if (withInit)
            {
                this.Init();
            }
            this._mainRenderComplete = new EventHandler(this.InternalRenderComplete);
            this._invokeStrokesCollected = new DispatcherRequiring((()=>
            {
                        EventHandler<InkSynchronizer> strokesCollected = this.strokesCollected;
                        if (strokesCollected != null)
                        {
                            strokesCollected(this, this.InkSynchronizer);
                        }
            }), DispatcherPriority.Background);
        }
        private void InternalRenderComplete(object sender, EventArgs e)
        {
            this.DynamicRendererThreadManager.MainMediaContextRenderComplete -= this._mainRenderComplete;
                List<DryingDrawingVisual> dryingDrawingVisualList = this._dryingDrawingVisualList;
                bool flag = false;
                List<DryingDrawingVisual> source;
                try
                {
                    Monitor.Enter(dryingDrawingVisualList, ref flag);
                    source = this._dryingDrawingVisualList.ToList<DryingDrawingVisual>();
                    this._dryingDrawingVisualList.Clear();
                }
                finally
                {
                    Monitor.Exit(dryingDrawingVisualList);
                }
                var group= (from temp in source group temp by temp.ContainerVisual);
                foreach(var dryingDrawingVisual in group)
                {
                    ContainerVisual containerVisual = dryingDrawingVisual.Key;
                    if (!containerVisual.CheckAccess())
                    {
                        containerVisual.Dispatcher.InvokeAsync(delegate ()
                        {
                            TestLable.RemoveDrawingVisual(dryingDrawingVisual, containerVisual);
                        }, DispatcherPriority.Background);
                        continue;
                    }
                    TestLable.RemoveDrawingVisual(dryingDrawingVisual, containerVisual);
                }
        }
        private static void RemoveDrawingVisual(IEnumerable<DryingDrawingVisual> dryingDrawingVisualList, ContainerVisual containerVisual)
        {

                foreach (DryingDrawingVisual dryingDrawingVisual in dryingDrawingVisualList)
                {
                foreach (var visual in dryingDrawingVisual.DryingDrawingVisualList)
                {
                    containerVisual.Children.Remove(visual);
                }
                }
        }
        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            if (_needInput)
            {
                var flag = _hitTestRect.Contains(hitTestParameters.HitPoint);
                if (flag)
                    return new PointHitTestResult(this, hitTestParameters.HitPoint);
                else
                    return base.HitTestCore(hitTestParameters);
            }
            return base.HitTestCore(hitTestParameters);
        }
        protected override void ArrangeCore(Rect finalRect)
        {
            base.RenderSize = finalRect.Size;
            this._hitTestRect = finalRect;
        }
        protected override Size MeasureCore(Size availableSize)
        {
            double width = availableSize.Width;
            double height = availableSize.Height;
            bool flag = double.IsInfinity(availableSize.Height);
            if (flag)
            {
                height = 1080.0;
            }
            bool flag1 = double.IsInfinity(availableSize.Width);
            if (flag1)
            {
                width = 1920.0;
            }
            return new Size(width, height);
        }
        protected override Visual GetVisualChild(int index)
        {
            if (index == 0)
            {
                return this._hostVisual;
            }
            return this.InkFromMouseContainerVisual;
        }
        public void AddDryingDrawingVisualList(DryingDrawingVisual dryingDrawingVisual)
        {
            List<DryingDrawingVisual> dryingDrawingVisualList;
            dryingDrawingVisualList = this._dryingDrawingVisualList;
            bool flag = false;
            try
            {
                        Monitor.Enter(dryingDrawingVisualList, ref flag);
                    this._dryingDrawingVisualList.Add(dryingDrawingVisual);
            }
            finally
            {
                if (flag)
                {
                    Monitor.Exit(dryingDrawingVisualList);
                }
            }
        }
        public void NotifyOnNextRenderComplete()
        {
            this.DynamicRendererThreadManager.MainMediaContextRenderComplete += this._mainRenderComplete;
        }
        public void AddStylusPlugIns(StylusPlugIn stylusPlugIns)
        {
            base.StylusPlugIns.Add(stylusPlugIns);
        }

        public void RemoveStylusPlugIns(StylusPlugIn stylusPlugIns)
        {
            base.StylusPlugIns.Remove(stylusPlugIns);
        }

        public bool ContainsStylusPlugIns(StylusPlugIn stylusPlugIns)
        {
            return base.StylusPlugIns.Contains(stylusPlugIns);
        }

        public void Dispose()
        {
            this.RunInTouchDispatcher(delegate ()
            {
                VisualTarget visualTarget = this._visualTarget;
                if (visualTarget == null)
                {
                    return;
                }
                visualTarget.Dispose();
            });
            this.Close();
        }
    }
}
