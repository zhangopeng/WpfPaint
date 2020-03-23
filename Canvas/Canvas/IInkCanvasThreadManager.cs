using System;

namespace Canvas
{
    public interface IInkCanvasThreadManager
    {
        void RunInTouchDispatcher(Action action);

        void RunInMainDispatcher(Action action);
    }
}