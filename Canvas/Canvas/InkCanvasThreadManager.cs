using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace Canvas
{
    public class InkCanvasThreadManager : IInkCanvasThreadManager
    {
        public InkCanvasThreadManager(TestLable inkCanvas, DynamicRendererThreadManager dynamicRendererThreadManager)
        {
            if (inkCanvas == null)
            {
                throw new ArgumentNullException("inkCanvas");
            }
            if (dynamicRendererThreadManager == null)
            {
                throw new ArgumentNullException("dynamicRendererThreadManager");
            }
            InkCanvas = inkCanvas;
            DynamicRendererThreadManager = dynamicRendererThreadManager;
        }

        public void RunInTouchDispatcher(Action action)
        {
            this.InkDispatcher.InvokeAsync(delegate ()
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);

                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }

                }
            }, DispatcherPriority.Send);
        }

        public void RunInMainDispatcher(Action action)
        {
            this.MainDispatcher.InvokeAsync(delegate ()
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }
                }
            });
        }

        private Dispatcher InkDispatcher
        {
            get
            {
                return this.DynamicRendererThreadManager.InkingDispatcher;
            }
        }

        private Dispatcher MainDispatcher
        {
            get
            {
                return this.InkCanvas.Dispatcher;
            }
        }

        private DynamicRendererThreadManager DynamicRendererThreadManager { get; }

        private TestLable InkCanvas { get; }
    }
}
