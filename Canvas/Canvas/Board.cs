using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input.StylusPlugIns;

namespace Canvas
{
    [DebuggerDisplay("Board [{Name,nq}] [{GetHashCode(),nq}]")]
    public sealed class Board : Control, ISavable<BoardSaveInfo>, IExtensible<Board>, INotifyPropertyChanged, ISavable
    {
        // Token: 0x06000185 RID: 389 RVA: 0x00005EC0 File Offset: 0x000040C0
        public static IUndoRedoProvider GetUndoRedoProvider([NotNull] DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (IUndoRedoProvider)element.GetValue(Board.UndoRedoProviderProperty);
        }

        // Token: 0x06000186 RID: 390 RVA: 0x00005EF0 File Offset: 0x000040F0
        public static void SetUndoRedoProvider([NotNull] DependencyObject element, IUndoRedoProvider value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.SetValue(Board.UndoRedoProviderProperty, value);
        }

        // Token: 0x17000030 RID: 48
        // (get) Token: 0x06000187 RID: 391 RVA: 0x00005F20 File Offset: 0x00004120
        // (set) Token: 0x06000188 RID: 392 RVA: 0x00005F3C File Offset: 0x0000413C
        public IUndoRedoProvider UndoRedoProvider
        {
            get
            {
                return (IUndoRedoProvider)base.GetValue(Board.UndoRedoProviderProperty);
            }
            set
            {
                base.SetValue(Board.UndoRedoProviderProperty, value);
            }
        }

        // Token: 0x06000189 RID: 393 RVA: 0x00005F58 File Offset: 0x00004158
        public static double GetContentScale([NotNull] DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (double)element.GetValue(Board.ContentScaleProperty);
        }

        // Token: 0x0600018A RID: 394 RVA: 0x00005F88 File Offset: 0x00004188
        public static void SetContentScale([NotNull] DependencyObject element, double value)
        {
            while (!false && element != null)
            {
                if (!false)
                {
                    if (5 != 0)
                    {
                        element.SetValue(Board.ContentScaleProperty, value);
                    }
                    if (!false)
                    {
                        return;
                    }
                    break;
                }
            }
            throw new ArgumentNullException("element");
        }

        // Token: 0x17000031 RID: 49
        // (get) Token: 0x0600018B RID: 395 RVA: 0x00005FD4 File Offset: 0x000041D4
        // (set) Token: 0x0600018C RID: 396 RVA: 0x00005FF0 File Offset: 0x000041F0
        public double ContentScale
        {
            get
            {
                return (double)base.GetValue(Board.ContentScaleProperty);
            }
            set
            {
                base.SetValue(Board.ContentScaleProperty, value);
            }
        }

        // Token: 0x0600018D RID: 397 RVA: 0x00006010 File Offset: 0x00004210
        public static Vector GetContentOffset([NotNull] DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (Vector)element.GetValue(Board.ContentOffsetProperty);
        }

        // Token: 0x0600018E RID: 398 RVA: 0x00006040 File Offset: 0x00004240
        public static void SetContentOffset([NotNull] DependencyObject element, Vector value)
        {
            while (!false && element != null)
            {
                if (!false)
                {
                    if (5 != 0)
                    {
                        element.SetValue(Board.ContentOffsetProperty, value);
                    }
                    if (!false)
                    {
                        return;
                    }
                    break;
                }
            }
            throw new ArgumentNullException("element");
        }

        // Token: 0x17000032 RID: 50
        // (get) Token: 0x0600018F RID: 399 RVA: 0x0000608C File Offset: 0x0000428C
        // (set) Token: 0x06000190 RID: 400 RVA: 0x000060A8 File Offset: 0x000042A8
        public Vector ContentOffset
        {
            get
            {
                return (Vector)base.GetValue(Board.ContentOffsetProperty);
            }
            set
            {
                base.SetValue(Board.ContentOffsetProperty, value);
            }
        }

        // Token: 0x06000191 RID: 401 RVA: 0x000060C8 File Offset: 0x000042C8
        public static FittingMode GetContentFitting([NotNull] DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (FittingMode)element.GetValue(Board.ContentFittingProperty);
        }

        // Token: 0x06000192 RID: 402 RVA: 0x000060F8 File Offset: 0x000042F8
        public static void SetContentFitting([NotNull] DependencyObject element, FittingMode value)
        {
            while (!false && element != null)
            {
                if (!false)
                {
                    if (5 != 0)
                    {
                        element.SetValue(Board.ContentFittingProperty, value);
                    }
                    if (!false)
                    {
                        return;
                    }
                    break;
                }
            }
            throw new ArgumentNullException("element");
        }

        // Token: 0x17000033 RID: 51
        // (get) Token: 0x06000193 RID: 403 RVA: 0x00006144 File Offset: 0x00004344
        // (set) Token: 0x06000194 RID: 404 RVA: 0x00006160 File Offset: 0x00004360
        public FittingMode ContentFitting
        {
            get
            {
                return (FittingMode)base.GetValue(Board.ContentFittingProperty);
            }
            set
            {
                base.SetValue(Board.ContentFittingProperty, value);
            }
        }

        // Token: 0x06000195 RID: 405 RVA: 0x00006180 File Offset: 0x00004380
        public static UIElement GetRenderSource([NotNull] UIElement element)
        {
            while (4 != 0 && 2 != 0)
            {
                if (element == null && 3 != 0)
                {
                    throw new ArgumentNullException("element");
                }
                if (7 != 0)
                {
                    UIElement result;
                    if ((result = (UIElement)element.GetValue(Board.RenderSourceProperty)) == null)
                    {
                        break;
                    }
                    return result;
                }
            }
            return element;
        }

        // Token: 0x17000034 RID: 52
        // (get) Token: 0x06000196 RID: 406 RVA: 0x000061D0 File Offset: 0x000043D0
        // (set) Token: 0x06000197 RID: 407 RVA: 0x000061EC File Offset: 0x000043EC
        public Size SlideSize
        {
            get
            {
                return (Size)base.GetValue(Board.SlideSizeProperty);
            }
            set
            {
                base.SetValue(Board.SlideSizeProperty, value);
            }
        }

        // Token: 0x06000198 RID: 408 RVA: 0x0000620C File Offset: 0x0000440C
        private static void SlideSizeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs o)
        {
            Size size = (Size)o.NewValue;
            Board board = (Board)d;
            board.Slides.ForEach(delegate (Slide s)
            {
                for (; ; )
                {
                    if (!false)
                    {
                        s.Width = size.Width;
                    }
                    if (!false)
                    {
                        if (5 != 0)
                        {
                            s.Height = size.Height;
                        }
                        if (!false)
                        {
                            break;
                        }
                    }
                }
            });
            ContentPresenter contentHost = board._contentHost;
            if (contentHost != null)
            {
                contentHost.InvalidateMeasure();
            }
            SlideViewbox slideViewbox = board._slideViewbox;
            if (slideViewbox == null)
            {
                return;
            }
            slideViewbox.InvalidateMeasure();
        }

        // Token: 0x17000035 RID: 53
        // (get) Token: 0x06000199 RID: 409 RVA: 0x00006294 File Offset: 0x00004494
        public BoardSettingsProvider BoardSettings
        {
            get
            {
                return this.Settings();
            }
        }

        // Token: 0x17000036 RID: 54
        // (get) Token: 0x0600019A RID: 410 RVA: 0x000062A4 File Offset: 0x000044A4
        public SlideCollection Slides
        {
            get
            {
                return this._slideList;
            }
        }

        // Token: 0x17000037 RID: 55
        // (get) Token: 0x0600019B RID: 411 RVA: 0x000062B0 File Offset: 0x000044B0
        // (set) Token: 0x0600019C RID: 412 RVA: 0x000062BC File Offset: 0x000044BC
        public Slide CurrentSlide
        {
            get
            {
                return this._currentSlide;
            }
            private set
            {
                Board.<> c__DisplayClass38_0 CS$<> 8__locals1 = new Board.<> c__DisplayClass38_0();
                CS$<> 8__locals1.<> 4__this = this;
                CS$<> 8__locals1.value = value;
                CS$<> 8__locals1.oldSlide = this._currentSlide;
                CS$<> 8__locals1.newSlide = CS$<> 8__locals1.value;
                if (object.Equals(CS$<> 8__locals1.oldSlide, CS$<> 8__locals1.newSlide))
                {
                    return;
                }
                PaintAggregateExceptionBuilder.DoActionsWithException("切换页面时发生了异常，异常的详细信息如下：", new Action(CS$<> 8__locals1.< set_CurrentSlide > g__ReloadSlide | 0), new Action[]
                {
                    new Action(CS$<>8__locals1.<set_CurrentSlide>g__RaiseCurrentSlideChangingEvent|1),
                    new Action(CS$<>8__locals1.<set_CurrentSlide>g__CloseOldMode|2),
                    new Action(CS$<>8__locals1.<set_CurrentSlide>g__UpdateCurrentSlide|3),
                    new Action(this.UpdateCurrentSlideValue),
                    new Action(CS$<>8__locals1.<set_CurrentSlide>g__OpenMode|4),
                    new Action(CS$<>8__locals1.<set_CurrentSlide>g__RaiseCurrentSlideChangedEvent|5)
                });
            }
        }

        // Token: 0x17000038 RID: 56
        // (get) Token: 0x0600019D RID: 413 RVA: 0x000063DC File Offset: 0x000045DC
        // (set) Token: 0x0600019E RID: 414 RVA: 0x00006400 File Offset: 0x00004600
        public Mode Mode
        {
            get
            {
                Mode currentMode;
                if ((currentMode = this._currentMode) == null)
                {
                    currentMode = this._modeController.CurrentMode;
                }
                return currentMode;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Mode 不能被设置为 null，请使用 NoneMode 替代。");
                }
                if (!object.Equals(this._currentMode, value))
                {
                    if (base.IsLoaded)
                    {
                        this._modeController.CurrentMode = value;
                        this._currentMode = null;
                        return;
                    }
                    this._currentMode = value;
                    if (this._switchModeAction != null)
                    {
                        this._lazyBoardLoadedActionList.Remove(this._switchModeAction);
                    }
                    this._switchModeAction = delegate ()
                    {
                        for (; ; )
                        {
                            if (true && !false)
                            {
                                this._modeController.CurrentMode = value;
                            }
                            for (; ; )
                            {
                                this._currentMode = null;
                                if (false)
                                {
                                    break;
                                }
                                if (!false)
                                {
                                    return;
                                }
                            }
                        }
                    };
                    this._lazyBoardLoadedActionList.Add(this._switchModeAction);
                }
            }
        }

        // Token: 0x17000039 RID: 57
        // (get) Token: 0x0600019F RID: 415 RVA: 0x00006510 File Offset: 0x00004710
        public ModeController Controller
        {
            get
            {
                return this._modeController;
            }
        }

        // Token: 0x1700003A RID: 58
        // (get) Token: 0x060001A0 RID: 416 RVA: 0x0000651C File Offset: 0x0000471C
        public SlideViewbox Viewbox
        {
            get
            {
                return this._slideViewbox;
            }
        }

        // Token: 0x060001A1 RID: 417 RVA: 0x00006528 File Offset: 0x00004728
        static Board()
        {
            Board.SlidesSortingEvent = EventManager.RegisterRoutedEvent("SlidesSorting", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Board));
            Board.SlidesSortedEvent = EventManager.RegisterRoutedEvent("SlidesSorted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Board));
            Board.CurrentSlideChangingEvent = EventManager.RegisterRoutedEvent("CurrentSlideChanging", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Slide>), typeof(Board));
            Board.CurrentSlideChangedEvent = EventManager.RegisterRoutedEvent("CurrentSlideChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Slide>), typeof(Board));
            Board.SlidesInsertingEvent = EventManager.RegisterRoutedEvent("SlidesInserting", RoutingStrategy.Bubble, typeof(EventHandler<SlidesRoutedEventArgs>), typeof(Board));
            Board.SlidesInsertedEvent = EventManager.RegisterRoutedEvent("SlidesInserted", RoutingStrategy.Bubble, typeof(EventHandler<SlidesRoutedEventArgs>), typeof(Board));
            Board.SlidesDeletingEvent = EventManager.RegisterRoutedEvent("SlidesDeleting", RoutingStrategy.Bubble, typeof(EventHandler<SlidesRoutedEventArgs>), typeof(Board));
            Board.SlidesDeletedEvent = EventManager.RegisterRoutedEvent("SlidesDeleted", RoutingStrategy.Bubble, typeof(EventHandler<SlidesRoutedEventArgs>), typeof(Board));
            Board.BoardClearingEvent = EventManager.RegisterRoutedEvent("BoardClearing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Board));
            Board.BoardClearedEvent = EventManager.RegisterRoutedEvent("BoardCleared", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Board));
            Board.BoardResetingEvent = EventManager.RegisterRoutedEvent("BoardReseting", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Board));
            Board.BoardResetedEvent = EventManager.RegisterRoutedEvent("BoardReseted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Board));
            Board.ModeChangingEvent = EventManager.RegisterRoutedEvent("ModeChanging", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Mode>), typeof(Board));
            Board.ModeChangedEvent = EventManager.RegisterRoutedEvent("ModeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Mode>), typeof(Board));
            Board.ElementClipGeometryChangedEvent = EventManager.RegisterRoutedEvent("ElementClipGeometryChanged", RoutingStrategy.Bubble, typeof(EventHandler<ElementClipGeometryChangedEventArgs>), typeof(Board));
            Board.UndoRedoEnableChangedEvent = EventManager.RegisterRoutedEvent("UndoRedoEnableChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Board));
            Board.EraserSlippingClearEvent = EventManager.RegisterRoutedEvent("EraserSlippingClear", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(Board));
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Board), new FrameworkPropertyMetadata(typeof(Board)));
            Board.PaintInitializationManager.Initialzie();
        }

        // Token: 0x060001A2 RID: 418 RVA: 0x00006990 File Offset: 0x00004B90
        public Board() : this(Enumerable.Empty<Slide>())
        {
        }

        // Token: 0x060001A3 RID: 419 RVA: 0x000069A0 File Offset: 0x00004BA0
        internal Board(IEnumerable<Slide> slideList)
        {
            this.< Extensions > k__BackingField = new ExtensionCollection<Board>(this);
            List<Slide> slides = Board.CheckSlideListArgument(slideList);
            this._slideList = new SlideCollection(this, new Action<Slide>(base.AddLogicalChild), new Action<Slide>(base.RemoveLogicalChild));
            this._slideList.AddRange(slides);
            this.InitUndoRedo();
            this.InitSetting();
            this._modeController = new ModeController(() => this.CurrentSlide);
            ModeController.SetController(this, this._modeController);
            BoardFocusManager.Manage(this);
            this.RegisterSlidesInserted();
            base.StylusPlugIns.Add(new DynamicRenderer());
            this.Mode = this._modeController.CurrentMode;
            this.GoTo(0);
            Board.PaintInitializationManager.Initialzie(this);
            base.Loaded += delegate (object s, RoutedEventArgs e)
            {
                Action[] array = this._lazyBoardLoadedActionList.ToArray();
                this._lazyBoardLoadedActionList.Clear();
                List<Exception> list = new List<Exception>();
                Action[] array2 = array;
                int num = 0;
                int i;
                if (!false)
                {
                    i = num;
                }
                while (i < array2.Length)
                {
                    Action action = array2[i];
                    try
                    {
                        if (action != null)
                        {
                            action();
                        }
                    }
                    catch (Exception item)
                    {
                        list.Add(item);
                    }
                    i++;
                }
                if (list.Any<Exception>())
                {
                    ITracer tracer = this.GetSettings<TraceSettings>().Tracer;
                    if (tracer == null)
                    {
                        return;
                    }
                    tracer.Trace(new AggregateException("Board 初始化出现异常", list), null, new string[0]);
                }
            };
        }

        // Token: 0x060001A4 RID: 420 RVA: 0x00006A84 File Offset: 0x00004C84
        private static List<Slide> CheckSlideListArgument(IEnumerable<Slide> slideList)
        {
            List<Slide> list;
            if ((list = (slideList as List<Slide>)) == null)
            {
                if (!true)
                {
                    goto IL_20;
                }
                list = slideList.ToList<Slide>();
            }
            List<Slide> list2 = list;
        IL_20:
            int num;
            bool flag = (num = (list2.Any<Slide>() ? 1 : 0)) != 0;
            if (!false && !false)
            {
                if (!flag)
                {
                    if (!true)
                    {
                        goto IL_7B;
                    }
                    list2.Add(new Slide());
                }
                if (list2.Any((Slide slide) => slide == null))
                {
                    throw new ArgumentException("指定 Slide 集合中存在 null 值。", "slideList");
                }
            IL_7B:
                num = list2.Distinct<Slide>().Count<Slide>();
            }
            if (num != list2.Count)
            {
                throw new ArgumentException("指定 Slide 集合中存重复的对象实例。", "slideList");
            }
            return list2;
        }

        // Token: 0x060001A5 RID: 421 RVA: 0x00006B78 File Offset: 0x00004D78
        private void InitUndoRedo()
        {
            for (; ; )
            {
                if (true && !false)
                {
                    this.UndoRedoProvider = new BoardUndoRedoProvider(this);
                }
                for (; ; )
                {
                    this.UndoRedoProvider.UndoRedoEnableChanged += delegate (object sender, EventArgs args)
                    {
                        base.RaiseEvent(new RoutedEventArgs(Board.UndoRedoEnableChangedEvent, this));
                    };
                    if (false)
                    {
                        break;
                    }
                    if (!false)
                    {
                        return;
                    }
                }
            }
        }

        // Token: 0x060001A6 RID: 422 RVA: 0x00006BD4 File Offset: 0x00004DD4
        private void InitSetting()
        {
            BoardSettingsProvider boardSettingsProvider = new BoardSettingsProvider
            {
                Board = this
            };
            BoardSettingsProvider settings = boardSettingsProvider;
            if (2 != 0)
            {
                this.SetSettings(settings);
            }
        }

        // Token: 0x060001A7 RID: 423 RVA: 0x00006C10 File Offset: 0x00004E10
        private void RegisterSlidesInserted()
        {
            this.SlidesInserted += delegate (object s, SlidesRoutedEventArgs e)
            {
                if (-1 != 0)
                {
                    IEnumerator<Slide> enumerator = e.Slides.GetEnumerator();
                    IEnumerator<Slide> enumerator2;
                    if (4 != 0)
                    {
                        enumerator2 = enumerator;
                    }
                    try
                    {
                        for (; ; )
                        {
                            if (!enumerator2.MoveNext())
                            {
                                if (!false && !false)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                Slide slide = enumerator2.Current;
                                if (!false)
                                {
                                    ElementInitializeProvider.OnInsertSlide(this, slide);
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator2 != null)
                        {
                            enumerator2.Dispose();
                        }
                    }
                }
            };
        }

        // Token: 0x060001A8 RID: 424 RVA: 0x00006C30 File Offset: 0x00004E30
        public override void OnApplyTemplate()
        {
            if (-1 != 0)
            {
                this._slideViewbox = (SlideViewbox)base.GetTemplateChild("PART_SlideViewBox");
                if (!false)
                {
                    this._eventSource = (base.GetTemplateChild("PART_DecoratorSource") as BoardEventSource);
                    while (this._eventSource != null)
                    {
                        this._contentHost = (ContentPresenter)base.GetTemplateChild("PART_ContentHost");
                        this._modeController.EventSource = this._eventSource;
                        if (!false)
                        {
                            base.SetValue(Board.RenderSourcePropertyKey, this._eventSource);
                            goto IL_9B;
                        }
                    }
                    throw new ArgumentException("Board的样式需要包含 PART_DecoratorSource 同时要求这个类是 BoardEventSource 用于接受输入");
                }
            IL_9B:
                if (3 != 0)
                {
                    this.UpdateCurrentSlideValue();
                }
            }
            this.UpdateUtilitiesGrid();
        }

        // Token: 0x060001A9 RID: 425 RVA: 0x00006D2C File Offset: 0x00004F2C
        private void UpdateCurrentSlideValue()
        {
            for (; ; )
            {
                if (false)
                {
                    goto IL_20;
                }
                if (this._contentHost == null)
                {
                    goto IL_0C;
                }
                if (this._contentHost.GetBindingExpression(ContentPresenter.ContentProperty) == null)
                {
                    goto IL_20;
                }
            IL_36:
                if (7 != 0)
                {
                    return;
                }
                continue;
            IL_0C:
                if (!false)
                {
                    break;
                }
                goto IL_36;
            IL_20:
                Slide currentSlide = this.CurrentSlide;
                Slide content;
                if (7 != 0)
                {
                    content = currentSlide;
                }
                if (!false)
                {
                    this._contentHost.Content = content;
                    goto IL_36;
                }
                goto IL_0C;
            }
        }

        // Token: 0x060001AA RID: 426 RVA: 0x00006D9C File Offset: 0x00004F9C
        private void ReloadSlide(Slide slide)
        {
            if (!object.Equals(slide.RenderSize, default(Size)))
            {
                goto IL_A2;
            }
            double num;
            if (!false)
            {
                num = slide.Width;
                goto IL_38;
            }
            return;
            do
            {
            IL_5B:
                slide.Measure(new Size(slide.Width, slide.Height));
            }
            while (false);
            slide.Arrange(new Rect(0.0, 0.0, slide.Width, slide.Height));
            goto IL_A2;
        IL_38:
            if (num <= 0.0)
            {
                goto IL_A2;
            }
        IL_43:
            double num2 = num = slide.Height;
            if (5 == 0)
            {
                goto IL_38;
            }
            if (num2 > 0.0)
            {
                goto IL_5B;
            }
        IL_A2:
            if (false)
            {
                goto IL_43;
            }
            if (false)
            {
                goto IL_5B;
            }
            slide.ReloadElements(ReloadMode.Display);
        }

        // Token: 0x060001AB RID: 427 RVA: 0x00006EA0 File Offset: 0x000050A0
        private void UpdateUtilitiesGrid()
        {
            for (; ; )
            {
                Grid utilitiesGrid = this._utilitiesGrid;
                if (utilitiesGrid == null)
                {
                    if (2 == 0 || false)
                    {
                        continue;
                    }
                }
                else
                {
                    utilitiesGrid.Children.Clear();
                }
                while (!false)
                {
                    this._utilitiesGrid = (Grid)base.GetTemplateChild("PART_UtilitiesGrid");
                    if (8 != 0)
                    {
                        return;
                    }
                }
            }
        }

        // Token: 0x060001AC RID: 428 RVA: 0x00006F00 File Offset: 0x00005100
        public void AddBoardUtilitiesElement(UIElement element)
        {
            this._utilitiesGrid.Children.Add(element);
        }

        // Token: 0x060001AD RID: 429 RVA: 0x00006F24 File Offset: 0x00005124
        public void RemoveBoardUtilitiesElement(UIElement element)
        {
            this._utilitiesGrid.Children.Remove(element);
        }

        // Token: 0x060001AE RID: 430 RVA: 0x00006F48 File Offset: 0x00005148
        public bool ContainsBoardUtilitiesElement(UIElement element)
        {
            return this._utilitiesGrid.Children.Contains(element);
        }

        // Token: 0x060001AF RID: 431 RVA: 0x00006F6C File Offset: 0x0000516C
        public void SortSlides(IList<int> sortList)
        {
            do
            {
                base.RaiseEvent(new RoutedEventArgs(Board.SlidesSortingEvent, this._slideList.ToList<Slide>()));
            }
            while (7 == 0);
            if (!false)
            {
                this._slideList.Sort(sortList);
                base.RaiseEvent(new RoutedEventArgs(Board.SlidesSortedEvent, this._slideList.ToList<Slide>()));
            }
        }

        // Token: 0x060001B0 RID: 432 RVA: 0x00006FE8 File Offset: 0x000051E8
        public int GetSlideIndex(Slide slide)
        {
            if (slide == null)
            {
                throw new ArgumentNullException("slide cannot be null");
            }
            return this._slideList.IndexOf(slide);
        }

        // Token: 0x060001B1 RID: 433 RVA: 0x00007018 File Offset: 0x00005218
        public Slide GetSlideById(string id)
        {
            return this._slideList.FirstOrDefault((Slide x) => x.Id == id);
        }

        // Token: 0x060001B2 RID: 434 RVA: 0x00007068 File Offset: 0x00005268
        public void Insert(Slide slide)
        {
            this.Insert(new List<Slide>
            {
                slide
            });
        }

        // Token: 0x060001B3 RID: 435 RVA: 0x0000708C File Offset: 0x0000528C
        public void Insert(List<Slide> slides)
        {
            base.RaiseEvent(new SlidesRoutedEventArgs(Board.SlidesInsertingEvent, slides));
            this._slideList.Insert(slides);
            base.RaiseEvent(new SlidesRoutedEventArgs(Board.SlidesInsertedEvent, slides));
        }

        // Token: 0x060001B4 RID: 436 RVA: 0x000070EC File Offset: 0x000052EC
        public void InsertAt(Slide slide, int index)
        {
            do
            {
                if (true && !false)
                {
                    this.InsertAt(new List<Slide>
                    {
                        slide
                    }, new List<int>
                    {
                        index
                    });
                }
            }
            while (false || false);
        }

        // Token: 0x060001B5 RID: 437 RVA: 0x0000713C File Offset: 0x0000533C
        public void InsertAt(IList<Slide> slides, IList<int> indexs)
        {
            do
            {
                base.RaiseEvent(new SlidesRoutedEventArgs(Board.SlidesInsertingEvent, slides));
                this._slideList.InsertAt(slides, indexs);
                do
                {
                    if (!false)
                    {
                        base.RaiseEvent(new SlidesRoutedEventArgs(Board.SlidesInsertedEvent, slides));
                    }
                }
                while (false);
            }
            while (false);
        }

        // Token: 0x060001B6 RID: 438 RVA: 0x000071A8 File Offset: 0x000053A8
        public void Delete(Slide slide)
        {
            Ensure.ArgumentIsNotNull<Slide>(() => slide);
            this.Delete(new List<Slide>
            {
                slide
            }, null);
        }

        // Token: 0x060001B7 RID: 439 RVA: 0x00007230 File Offset: 0x00005430
        public void DeleteAt(int index)
        {
            this.DeleteAt(new int[]
            {
                index
            }, null);
        }

        // Token: 0x060001B8 RID: 440 RVA: 0x00007250 File Offset: 0x00005450
        public void Delete(IList<Slide> deleteSlides, string newSlideId = null)
        {
            Slide slide;
            Slide slide2;
            if (!false)
            {
                IList<Slide> slides = this.GetSlidesBySlideId(from tSlide in deleteSlides
                                                              select tSlide.Id);
                bool flag = slides.Contains(this.CurrentSlide);
                bool flag2;
                do
                {
                    slide = this.CurrentSlide;
                    if (!flag)
                    {
                        goto IL_111;
                    }
                    if (slides.Count != this._slideList.Count)
                    {
                        goto IL_B8;
                    }
                    flag2 = (flag = string.IsNullOrEmpty(newSlideId));
                }
                while (2 == 0);
                if (!flag2)
                {
                    goto IL_9C;
                }
                slide2 = new Slide();
                goto IL_A9;
            IL_B8:
                int indexCurrentSlide = this._slideList.IndexOf(this.CurrentSlide);
                slide = (this._slideList.Where(delegate (Slide s, int i)
                {
                    if (5 == 0)
                    {
                        goto IL_23;
                    }
                    int num = i;
                    int num3;
                    int num2 = num3 = indexCurrentSlide;
                    if (false)
                    {
                        goto IL_20;
                    }
                    if (i <= num2)
                    {
                        goto IL_23;
                    }
                IL_11:
                    int result = num = (slides.Contains(s) ? 1 : 0);
                    if (false)
                    {
                        return result != 0;
                    }
                    num3 = 0;
                IL_20:
                    return num == num3;
                IL_23:
                    if (5 == 0)
                    {
                        goto IL_11;
                    }
                    result = 0;
                    return result != 0;
                }).FirstOrDefault<Slide>() ?? this._slideList.Reverse<Slide>().Where(delegate (Slide s, int i)
                {
                    if (false)
                    {
                        goto IL_34;
                    }
                    if (false)
                    {
                        goto IL_22;
                    }
                    int num = i;
                    int num2 = this._slideList.Count;
                IL_16:
                    if (num <= num2 - indexCurrentSlide - 1)
                    {
                        goto IL_34;
                    }
                IL_22:
                    int num3 = num = (slides.Contains(s) ? 1 : 0);
                IL_2D:
                    int num4 = num2 = 0;
                    if (num4 == 0)
                    {
                        return num3 == num4;
                    }
                    goto IL_16;
                IL_34:
                    int num5 = num = (num3 = 0);
                    if (num5 == 0)
                    {
                        return num5 != 0;
                    }
                    goto IL_2D;
                }).FirstOrDefault<Slide>());
                goto IL_111;
            }
        IL_9C:
            slide2 = new Slide(newSlideId);
        IL_A9:
            slide = slide2;
            this._slideList.Insert(slide);
        IL_111:
            base.RaiseEvent(new SlidesRoutedEventArgs(Board.SlidesDeletingEvent, CS$<> 8__locals1.slides));
            this._slideList.Delete(CS$<> 8__locals1.slides, true);
            this.CurrentSlide = slide;
            base.RaiseEvent(new SlidesRoutedEventArgs(Board.SlidesDeletedEvent, CS$<> 8__locals1.slides));
        }

        // Token: 0x060001B9 RID: 441 RVA: 0x000073F8 File Offset: 0x000055F8
        public void DeleteAt(IEnumerable<int> indexs, string newSlideId = null)
        {
            for (; ; )
            {
                List<Slide> deleteSlides = (from index in indexs
                                            select this._slideList[index]).ToList<Slide>();
                if (!false)
                {
                    this.Delete(deleteSlides, newSlideId);
                    if (!false)
                    {
                        break;
                    }
                }
            }
        }

        // Token: 0x060001BA RID: 442 RVA: 0x00007450 File Offset: 0x00005650
        public void Clear(Slide newSlide = null)
        {
            if (newSlide == null)
            {
                newSlide = new Slide();
            }
            do
            {
                if (!false)
                {
                    base.RaiseEvent(new RoutedEventArgs(Board.BoardClearingEvent, this));
                    Slide currentSlide = this.CurrentSlide;
                    if (currentSlide != null)
                    {
                        currentSlide.Unselect();
                    }
                    double d2;
                    double d = d2 = newSlide.Width;
                    if (-1 == 0)
                    {
                        goto IL_6E;
                    }
                    if (!double.IsInfinity(d))
                    {
                        if (4 != 0)
                        {
                            if (3 == 0)
                            {
                                goto IL_75;
                            }
                            if (double.IsNaN(newSlide.Width))
                            {
                                goto IL_82;
                            }
                        }
                        d2 = newSlide.Height;
                        goto IL_6E;
                    }
                    goto IL_82;
                IL_A6:
                    Slide[] deleteSlides = this._slideList.ToArray<Slide>();
                    this.Insert(newSlide);
                    this.CurrentSlide = newSlide;
                    this.Delete(deleteSlides, null);
                    if (4 != 0)
                    {
                        this.UndoRedoProvider.ClearAll();
                        goto IL_D6;
                    }
                    goto IL_82;
                IL_75:
                    if (!double.IsNaN(newSlide.Height))
                    {
                        this.SlideSize = new Size(newSlide.Width, newSlide.Height);
                        goto IL_A6;
                    }
                IL_82:
                    base.ClearValue(Board.SlideSizeProperty);
                    goto IL_A6;
                IL_6E:
                    if (!double.IsInfinity(d2))
                    {
                        goto IL_75;
                    }
                    goto IL_82;
                }
            IL_D6:;
            }
            while (7 == 0);
            this._slideList.OnPropertyChanged("CurrentIndex");
            base.RaiseEvent(new RoutedEventArgs(Board.BoardClearedEvent, this));
        }

        // Token: 0x060001BB RID: 443 RVA: 0x000075A4 File Offset: 0x000057A4
        public void Reset(IEnumerable<Slide> slides)
        {
            base.RaiseEvent(new RoutedEventArgs(Board.BoardResetingEvent, this));
            this._slideList.Reset(slides);
            base.RaiseEvent(new RoutedEventArgs(Board.BoardResetedEvent, this));
        }

        // Token: 0x060001BC RID: 444 RVA: 0x00007604 File Offset: 0x00005804
        [Obsolete("请使用 Clear")]
        public void ClearWithNewSlide(Slide newSlide)
        {
            RoutedEventArgs e = new RoutedEventArgs(Board.BoardClearingEvent, this);
            if (8 != 0)
            {
                base.RaiseEvent(e);
            }
            Slide currentSlide = this.CurrentSlide;
            if (currentSlide != null)
            {
                currentSlide.Unselect();
            }
            if (-1 != 0)
            {
                bool flag2;
                bool flag = flag2 = double.IsInfinity(newSlide.Width);
                if (true)
                {
                    if (flag)
                    {
                        goto IL_75;
                    }
                    if (4 == 0)
                    {
                        goto IL_A5;
                    }
                    if (double.IsNaN(newSlide.Width))
                    {
                        goto IL_75;
                    }
                    flag2 = double.IsInfinity(newSlide.Height);
                }
                if (!flag2 && !double.IsNaN(newSlide.Height))
                {
                    this.SlideSize = new Size(newSlide.Width, newSlide.Height);
                    goto IL_99;
                }
            IL_75:
                base.ClearValue(Board.SlideSizeProperty);
            IL_99:
                Slide[] deleteSlides = this._slideList.ToArray<Slide>();
            IL_A5:
                this.Insert(newSlide);
                this.CurrentSlide = newSlide;
                this.Delete(deleteSlides, null);
                this.UndoRedoProvider.ClearAll();
                this._slideList.OnPropertyChanged("CurrentIndex");
            }
            if (!false)
            {
                base.RaiseEvent(new RoutedEventArgs(Board.BoardClearedEvent, this));
            }
        }

        // Token: 0x060001BD RID: 445 RVA: 0x00007748 File Offset: 0x00005948
        [Obsolete("请使用 GetSlideIndex")]
        public int IndexOf(Slide slide)
        {
            return this.GetSlideIndex(slide);
        }

        // Token: 0x060001BE RID: 446 RVA: 0x00007760 File Offset: 0x00005960
        public bool GoToNextSlide()
        {
            if (false)
            {
                goto IL_30;
            }
            int num2;
            if (!false)
            {
                int num = num2 = this._slideList.CurrentIndex;
                if (7 == 0)
                {
                    goto IL_31;
                }
                if (num != this._slideList.Count - 1)
                {
                    this.GoTo(this._slideList.CurrentIndex + 1);
                    goto IL_30;
                }
            }
            int result = 0;
            return result != 0;
        IL_30:
            num2 = 1;
        IL_31:
            int num3 = result = num2;
            if (num3 != 0)
            {
                return num3 != 0;
            }
            return result != 0;
        }

        // Token: 0x060001BF RID: 447 RVA: 0x000077CC File Offset: 0x000059CC
        public bool GoToPreviousSlide()
        {
            int num2;
            int num = num2 = this._slideList.CurrentIndex;
            int num4;
            do
            {
                if (8 != 0)
                {
                    if (num2 == 0)
                    {
                        num = 0;
                    }
                    else
                    {
                        this.GoTo(this._slideList.CurrentIndex - 1);
                        int num3 = num = 1;
                        if (num3 != 0)
                        {
                            num = num3;
                            num2 = num3;
                            if (num3 != 0)
                            {
                                return num3 != 0;
                            }
                            continue;
                        }
                    }
                }
                num4 = (num2 = (num = num));
            }
            while (num4 != 0);
            return num4 != 0;
        }

        // Token: 0x060001C0 RID: 448 RVA: 0x00007820 File Offset: 0x00005A20
        public void GoTo(int index)
        {
            if (!false)
            {
                if (!false)
                {
                    Board <> 4__this = this;
                    if (false)
                    {
                        return;
                    }
                    if (!false && this.IsSlideIndexValid(index))
                    {
                        if (!false)
                        {
                            Slide slide = this._slideList[index];
                            slide.WaitingForLoaded(delegate
                            {

                                <> 4__this.CurrentSlide = slide;
                            });
                            return;
                        }
                        return;
                    }
                }
                throw new ArgumentOutOfRangeException("index", "不包含指定页面。");
            }
        }

        // Token: 0x060001C1 RID: 449 RVA: 0x000078C0 File Offset: 0x00005AC0
        public void GoTo(Slide slide)
        {
            if (false)
            {
                goto IL_2A;
            }
            int num = this._slideList.IndexOf(slide);
            int num2;
            bool flag;
            do
            {
                if (7 != 0)
                {
                    num2 = num;
                }
                flag = ((num = (this.IsSlideIndexValid(num2) ? 1 : 0)) != 0);
            }
            while (false);
            if (flag)
            {
                goto IL_2A;
            }
        IL_1D:
            throw new ArgumentException("不包含指定页面。", "slide");
        IL_2A:
            if (!false && !false)
            {
                this.GoTo(num2);
                return;
            }
            goto IL_1D;
        }

        // Token: 0x060001C2 RID: 450 RVA: 0x0000792C File Offset: 0x00005B2C
        public Slide GetSlide(int index)
        {
            if (this.IsSlideIndexValid(index) && !false)
            {
                return this._slideList[index];
            }
            throw new ArgumentOutOfRangeException("index", "不包含指定页面。");
        }

        // Token: 0x060001C3 RID: 451 RVA: 0x00007980 File Offset: 0x00005B80
        private bool IsSlideIndexValid(int slideIndex)
        {
            int result;
            int num;
            int num3;
            do
            {
                if (true)
                {
                    result = slideIndex;
                    num = slideIndex;
                    if (3 == 0)
                    {
                        return result != 0;
                    }
                    int num2 = num3 = 0;
                    if (num2 != 0)
                    {
                        goto IL_1C;
                    }
                    if (slideIndex < num2)
                    {
                        goto IL_1F;
                    }
                }
            }
            while (false);
            num = slideIndex;
            num3 = this._slideList.Count;
        IL_1C:
            return num < num3;
        IL_1F:
            result = 0;
            return result != 0;
        }

        // Token: 0x060001C4 RID: 452 RVA: 0x000079B4 File Offset: 0x00005BB4
        private IList<Slide> GetSlidesBySlideId(IEnumerable<string> idList)
        {
            return (from id in idList
                    select this.Slides.FirstOrDefault((Slide tSlide) => tSlide.Id.Equals(id))).ToList<Slide>();
        }

        // Token: 0x14000032 RID: 50
        // (add) Token: 0x060001C5 RID: 453 RVA: 0x000079E0 File Offset: 0x00005BE0
        // (remove) Token: 0x060001C6 RID: 454 RVA: 0x00007A48 File Offset: 0x00005C48
        public event PropertyChangedEventHandler PropertyChanged
        {
            [CompilerGenerated]
            add
            {
                if (false)
                {
                    goto IL_38;
                }
                PropertyChangedEventHandler propertyChangedEventHandler;
                if (!false)
                {
                    propertyChangedEventHandler = this.PropertyChanged;
                }
            IL_0F:
                if (7 == 0)
                {
                    goto IL_34;
                }
                PropertyChangedEventHandler propertyChangedEventHandler2 = propertyChangedEventHandler;
                PropertyChangedEventHandler propertyChangedEventHandler3;
                if (6 != 0)
                {
                    propertyChangedEventHandler3 = propertyChangedEventHandler2;
                }
            IL_18:
                PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler3, value);
                propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, value2, propertyChangedEventHandler3);
            IL_34:
                if (propertyChangedEventHandler != propertyChangedEventHandler3)
                {
                    goto IL_0F;
                }
            IL_38:
                if (!false)
                {
                    return;
                }
                goto IL_18;
            }
            [CompilerGenerated]
            remove
            {
                if (false)
                {
                    goto IL_38;
                }
                PropertyChangedEventHandler propertyChangedEventHandler;
                if (!false)
                {
                    propertyChangedEventHandler = this.PropertyChanged;
                }
            IL_0F:
                if (7 == 0)
                {
                    goto IL_34;
                }
                PropertyChangedEventHandler propertyChangedEventHandler2 = propertyChangedEventHandler;
                PropertyChangedEventHandler propertyChangedEventHandler3;
                if (6 != 0)
                {
                    propertyChangedEventHandler3 = propertyChangedEventHandler2;
                }
            IL_18:
                PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler3, value);
                propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, value2, propertyChangedEventHandler3);
            IL_34:
                if (propertyChangedEventHandler != propertyChangedEventHandler3)
                {
                    goto IL_0F;
                }
            IL_38:
                if (!false)
                {
                    return;
                }
                goto IL_18;
            }
        }

        // Token: 0x060001C7 RID: 455 RVA: 0x00007AB0 File Offset: 0x00005CB0
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            for (; ; )
            {
                if (!false)
                {
                    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                    if (propertyChanged != null)
                    {
                        propertyChanged(this, new PropertyChangedEventArgs(propertyName));
                        goto IL_1D;
                    }
                }
                if (false)
                {
                    continue;
                }
                if (5 != 0)
                {
                    break;
                }
            IL_1D:
                if (!false)
                {
                    return;
                }
            }
        }

        // Token: 0x1700003B RID: 59
        // (get) Token: 0x060001C8 RID: 456 RVA: 0x00007AEC File Offset: 0x00005CEC
        protected override IEnumerator LogicalChildren
        {
            get
            {
                return this._slideList.GetEnumerator();
            }
        }

        // Token: 0x1700003C RID: 60
        // (get) Token: 0x060001C9 RID: 457 RVA: 0x00007B04 File Offset: 0x00005D04
        public ExtensionCollection<Board> Extensions { get; }

        // Token: 0x060001CA RID: 458 RVA: 0x00007B10 File Offset: 0x00005D10
        public StylusPlugInCollection GetStylusPlugins()
        {
            return base.StylusPlugIns;
        }

        // Token: 0x060001CB RID: 459 RVA: 0x00007B20 File Offset: 0x00005D20
        public BoardSaveInfo GetSaveInfo()
        {
            Board.<> c__DisplayClass104_0 CS$<> 8__locals1 = new Board.<> c__DisplayClass104_0();
            CS$<> 8__locals1.<> 4__this = this;
            Board.<> c__DisplayClass104_0 CS$<> 8__locals2 = CS$<> 8__locals1;
            BoardSaveInfo boardSaveInfo = new BoardSaveInfo();
            boardSaveInfo.SlideIds = (from s in this._slideList
                                      select s.Id).ToArray<string>();
            CS$<> 8__locals2.boardSaveInfo = boardSaveInfo;
            this.Extensions.OfType<IExtensionSavable<BoardSaveInfo>>().ForEach(delegate (IExtensionSavable<BoardSaveInfo> extension)
            {
                try
                {
                    if (2 != 0 && -1 != 0 && 4 != 0)
                    {
                        extension.GetSaveInfo(ref CS$<> 8__locals1.boardSaveInfo);
                    }
                }
                catch (Exception exception)
                {
                    ITracer tracer = CS$<> 8__locals1.<> 4__this.GetSettings<TraceSettings>().Tracer;
                    if (tracer != null)
                    {
                        tracer.Trace(exception, null, new string[0]);
                    }
                }
            });
            return CS$<> 8__locals1.boardSaveInfo;
        }

        // Token: 0x060001CC RID: 460 RVA: 0x00007BE4 File Offset: 0x00005DE4
        public void SetSaveInfo([NotNull] BoardSaveInfo saveInfo)
        {
            if (saveInfo == null)
            {
                throw new ArgumentNullException("saveInfo");
            }
            this.< SetSaveInfo > g__OffController | 105_1();
            Slide currentSlide = this.CurrentSlide;
            if (currentSlide != null)
            {
                currentSlide.Unselect();
            }
            this._slideList.Reset(from id in saveInfo.SlideIds
                                  select new Slide(id)
                                  {
                                      IsDataLoaded = false
                                  });
            try
            {
                InterfaceCreator.GetInterfaceInstance<ISaveInfoProvider>(new object[0]).CreateExtensions<Board>(saveInfo, this.Extensions);
            }
            catch (Exception exception)
            {
                ITracer tracer = this.GetSettings<TraceSettings>().Tracer;
                if (tracer != null)
                {
                    tracer.Trace(exception, null, new string[0]);
                }
            }
            if (this._slideList.Contains(this.CurrentSlide))
            {
                this.< SetSaveInfo > g__OnController | 105_2();
            }
        }

        // Token: 0x14000033 RID: 51
        // (add) Token: 0x060001CD RID: 461 RVA: 0x00007D08 File Offset: 0x00005F08
        // (remove) Token: 0x060001CE RID: 462 RVA: 0x00007D24 File Offset: 0x00005F24
        public event RoutedEventHandler SlidesSorting
        {
            add
            {
                base.AddHandler(Board.SlidesSortingEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.SlidesSortingEvent, value);
            }
        }

        // Token: 0x14000034 RID: 52
        // (add) Token: 0x060001CF RID: 463 RVA: 0x00007D40 File Offset: 0x00005F40
        // (remove) Token: 0x060001D0 RID: 464 RVA: 0x00007D5C File Offset: 0x00005F5C
        public event RoutedEventHandler SlidesSorted
        {
            add
            {
                base.AddHandler(Board.SlidesSortedEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.SlidesSortedEvent, value);
            }
        }

        // Token: 0x14000035 RID: 53
        // (add) Token: 0x060001D1 RID: 465 RVA: 0x00007D78 File Offset: 0x00005F78
        // (remove) Token: 0x060001D2 RID: 466 RVA: 0x00007D94 File Offset: 0x00005F94
        public event RoutedPropertyChangedEventHandler<Slide> CurrentSlideChanging
        {
            add
            {
                base.AddHandler(Board.CurrentSlideChangingEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.CurrentSlideChangingEvent, value);
            }
        }

        // Token: 0x14000036 RID: 54
        // (add) Token: 0x060001D3 RID: 467 RVA: 0x00007DB0 File Offset: 0x00005FB0
        // (remove) Token: 0x060001D4 RID: 468 RVA: 0x00007DCC File Offset: 0x00005FCC
        public event RoutedPropertyChangedEventHandler<Slide> CurrentSlideChanged
        {
            add
            {
                base.AddHandler(Board.CurrentSlideChangedEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.CurrentSlideChangedEvent, value);
            }
        }

        // Token: 0x14000037 RID: 55
        // (add) Token: 0x060001D5 RID: 469 RVA: 0x00007DE8 File Offset: 0x00005FE8
        // (remove) Token: 0x060001D6 RID: 470 RVA: 0x00007E04 File Offset: 0x00006004
        public event EventHandler<SlidesRoutedEventArgs> SlidesInserting
        {
            add
            {
                base.AddHandler(Board.SlidesInsertingEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.SlidesInsertingEvent, value);
            }
        }

        // Token: 0x14000038 RID: 56
        // (add) Token: 0x060001D7 RID: 471 RVA: 0x00007E20 File Offset: 0x00006020
        // (remove) Token: 0x060001D8 RID: 472 RVA: 0x00007E3C File Offset: 0x0000603C
        public event EventHandler<SlidesRoutedEventArgs> SlidesInserted
        {
            add
            {
                base.AddHandler(Board.SlidesInsertedEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.SlidesInsertedEvent, value);
            }
        }

        // Token: 0x14000039 RID: 57
        // (add) Token: 0x060001D9 RID: 473 RVA: 0x00007E58 File Offset: 0x00006058
        // (remove) Token: 0x060001DA RID: 474 RVA: 0x00007E74 File Offset: 0x00006074
        public event EventHandler<SlidesRoutedEventArgs> SlidesDeleting
        {
            add
            {
                base.AddHandler(Board.SlidesDeletingEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.SlidesDeletingEvent, value);
            }
        }

        // Token: 0x1400003A RID: 58
        // (add) Token: 0x060001DB RID: 475 RVA: 0x00007E90 File Offset: 0x00006090
        // (remove) Token: 0x060001DC RID: 476 RVA: 0x00007EAC File Offset: 0x000060AC
        public event EventHandler<SlidesRoutedEventArgs> SlidesDeleted
        {
            add
            {
                base.AddHandler(Board.SlidesDeletedEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.SlidesDeletedEvent, value);
            }
        }

        // Token: 0x1400003B RID: 59
        // (add) Token: 0x060001DD RID: 477 RVA: 0x00007EC8 File Offset: 0x000060C8
        // (remove) Token: 0x060001DE RID: 478 RVA: 0x00007EE4 File Offset: 0x000060E4
        public event RoutedEventHandler BoardClearing
        {
            add
            {
                base.AddHandler(Board.BoardClearingEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.BoardClearingEvent, value);
            }
        }

        // Token: 0x1400003C RID: 60
        // (add) Token: 0x060001DF RID: 479 RVA: 0x00007F00 File Offset: 0x00006100
        // (remove) Token: 0x060001E0 RID: 480 RVA: 0x00007F1C File Offset: 0x0000611C
        public event RoutedEventHandler BoardCleared
        {
            add
            {
                base.AddHandler(Board.BoardClearedEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.BoardClearedEvent, value);
            }
        }

        // Token: 0x1400003D RID: 61
        // (add) Token: 0x060001E1 RID: 481 RVA: 0x00007F38 File Offset: 0x00006138
        // (remove) Token: 0x060001E2 RID: 482 RVA: 0x00007F54 File Offset: 0x00006154
        public event RoutedEventHandler BoardReseting
        {
            add
            {
                base.AddHandler(Board.BoardResetingEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.BoardResetingEvent, value);
            }
        }

        // Token: 0x1400003E RID: 62
        // (add) Token: 0x060001E3 RID: 483 RVA: 0x00007F70 File Offset: 0x00006170
        // (remove) Token: 0x060001E4 RID: 484 RVA: 0x00007F8C File Offset: 0x0000618C
        public event RoutedEventHandler BoardReseted
        {
            add
            {
                base.AddHandler(Board.BoardResetedEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.BoardResetedEvent, value);
            }
        }

        // Token: 0x1400003F RID: 63
        // (add) Token: 0x060001E5 RID: 485 RVA: 0x00007FA8 File Offset: 0x000061A8
        // (remove) Token: 0x060001E6 RID: 486 RVA: 0x00007FC4 File Offset: 0x000061C4
        public event RoutedPropertyChangedEventHandler<Mode> ModeChanging
        {
            add
            {
                base.AddHandler(Board.ModeChangingEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.ModeChangingEvent, value);
            }
        }

        // Token: 0x14000040 RID: 64
        // (add) Token: 0x060001E7 RID: 487 RVA: 0x00007FE0 File Offset: 0x000061E0
        // (remove) Token: 0x060001E8 RID: 488 RVA: 0x00007FFC File Offset: 0x000061FC
        public event RoutedPropertyChangedEventHandler<Mode> ModeChanged
        {
            add
            {
                base.AddHandler(Board.ModeChangedEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.ModeChangedEvent, value);
            }
        }

        // Token: 0x14000041 RID: 65
        // (add) Token: 0x060001E9 RID: 489 RVA: 0x00008018 File Offset: 0x00006218
        // (remove) Token: 0x060001EA RID: 490 RVA: 0x00008034 File Offset: 0x00006234
        public event EventHandler<ElementClipGeometryChangedEventArgs> ElementClipGeometryChanged
        {
            add
            {
                base.AddHandler(Board.ElementClipGeometryChangedEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.ElementClipGeometryChangedEvent, value);
            }
        }

        // Token: 0x14000042 RID: 66
        // (add) Token: 0x060001EB RID: 491 RVA: 0x00008050 File Offset: 0x00006250
        // (remove) Token: 0x060001EC RID: 492 RVA: 0x0000806C File Offset: 0x0000626C
        public event RoutedEventHandler UndoRedoEnableChanged
        {
            add
            {
                base.AddHandler(Board.UndoRedoEnableChangedEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.UndoRedoEnableChangedEvent, value);
            }
        }

        // Token: 0x14000043 RID: 67
        // (add) Token: 0x060001ED RID: 493 RVA: 0x00008088 File Offset: 0x00006288
        // (remove) Token: 0x060001EE RID: 494 RVA: 0x000080A4 File Offset: 0x000062A4
        public event EventHandler<RoutedEventArgs> EraserSlippingClear
        {
            add
            {
                base.AddHandler(Board.EraserSlippingClearEvent, value);
            }
            remove
            {
                base.RemoveHandler(Board.EraserSlippingClearEvent, value);
            }
        }

        // Token: 0x0400003C RID: 60
        public static readonly DependencyProperty UndoRedoProviderProperty = DependencyProperty.RegisterAttached("UndoRedoProvider", typeof(IUndoRedoProvider), typeof(Board), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        // Token: 0x0400003D RID: 61
        public static readonly DependencyProperty ContentScaleProperty = DependencyProperty.RegisterAttached("ContentScale", typeof(double), typeof(Board), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.Inherits));

        // Token: 0x0400003E RID: 62
        public static readonly DependencyProperty ContentOffsetProperty = DependencyProperty.RegisterAttached("ContentOffset", typeof(Vector), typeof(Board), new FrameworkPropertyMetadata(new Vector(0.0, 0.0), FrameworkPropertyMetadataOptions.Inherits));

        // Token: 0x0400003F RID: 63
        public static readonly DependencyProperty ContentFittingProperty = DependencyProperty.RegisterAttached("ContentFitting", typeof(FittingMode), typeof(Board), new FrameworkPropertyMetadata(FittingMode.AutoFit, FrameworkPropertyMetadataOptions.Inherits));

        // Token: 0x04000040 RID: 64
        private static readonly DependencyPropertyKey RenderSourcePropertyKey = DependencyProperty.RegisterAttachedReadOnly("RenderSource", typeof(UIElement), typeof(Board), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        // Token: 0x04000041 RID: 65
        public static readonly DependencyProperty RenderSourceProperty = Board.RenderSourcePropertyKey.DependencyProperty;

        // Token: 0x04000042 RID: 66
        public static readonly DependencyProperty SlideSizeProperty = DependencyProperty.Register("SlideSize", typeof(Size), typeof(Board), new PropertyMetadata(new Size(1280.0, 720.0), new PropertyChangedCallback(Board.SlideSizeChangedCallback)));

        // Token: 0x04000043 RID: 67
        private Mode _currentMode;

        // Token: 0x04000044 RID: 68
        private static readonly PaintInitializationManager PaintInitializationManager = new PaintInitializationManager();

        // Token: 0x04000047 RID: 71
        private BoardEventSource _eventSource;

        // Token: 0x04000048 RID: 72
        private Slide _currentSlide;

        // Token: 0x04000049 RID: 73
        private readonly SlideCollection _slideList;

        // Token: 0x0400004A RID: 74
        private readonly ModeController _modeController;

        // Token: 0x0400004B RID: 75
        private SlideViewbox _slideViewbox;

        // Token: 0x0400004C RID: 76
        private ContentPresenter _contentHost;

        // Token: 0x0400004D RID: 77
        private Grid _utilitiesGrid;

        // Token: 0x0400004E RID: 78
        private readonly List<Action> _lazyBoardLoadedActionList = new List<Action>();

        // Token: 0x0400004F RID: 79
        private Action _switchModeAction;

        // Token: 0x04000050 RID: 80
        private const string InvalidPageExceptionString = "不包含指定页面。";
    }
}
