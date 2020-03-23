using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace Canvas
{
    public sealed class RectangleStylusShape : StylusShape
    {
        public RectangleStylusShape(double width, double height)
            : this(width, height, 0.0)
        {
        }

        public RectangleStylusShape(double width, double height, double rotation)
            : base(StylusTip.Rectangle, width, height, rotation)
        {
        }
    }

}
