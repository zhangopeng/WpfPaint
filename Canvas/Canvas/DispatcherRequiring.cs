using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Canvas
{
    public class DispatcherRequiring : DispatcherObject
    {
        private bool _isAction;

        private readonly Action _action;

        private readonly DispatcherPriority _dispatcherPriority = DispatcherPriority.Normal;

        public DispatcherRequiring(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            else
            {
                this._action = action;
            }
        }

        public DispatcherRequiring(Action action, DispatcherPriority priority)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            else
            {
                this._action = action;
            }
            this._dispatcherPriority = priority;
        }

        public void Require()
        {
            if (!this._isAction)
            {
                this._isAction = true;
                base.Dispatcher.InvokeAsync(_action, this._dispatcherPriority);
            }
        }

        public void Invoke(bool withRequire = false)
        {
            this._isAction = withRequire;
            Check();
        }

        public void Cancel()
        {
            this._isAction = false;
        }

        private void Check()
        {
            if (this._isAction)

            {
                try
                {
                    this._action();
                }
                finally
                {
                    this._isAction = false;
                }
            }
        }
    }
}
