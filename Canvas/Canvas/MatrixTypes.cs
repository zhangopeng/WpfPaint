using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas
{
    [Flags]
    internal enum MatrixTypes
    {
        TRANSFORM_IS_IDENTITY = 0x0,
        TRANSFORM_IS_TRANSLATION = 0x1,
        TRANSFORM_IS_SCALING = 0x2,
        TRANSFORM_IS_UNKNOWN = 0x4
    }
}
