using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas
{
    public interface IInkSynchronizer
    {
        void AddRawStroke(MouseRender.StrokeInfo strokeInfo);
    }
}
