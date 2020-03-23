using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace Canvas
{
    public sealed class EllipseStylusShape : StylusShape
    {
        public EllipseStylusShape(double width, double height)
            : this(width, height, 0.0)
        {
        }

        public EllipseStylusShape(double width, double height, double rotation)
            : base(StylusTip.Ellipse, width, height, rotation)
        {
        }
    }

}
