using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas
{
    internal interface IInkDryCanvas
    {
        IInkSynchronizer InkSynchronizer { get; set; }

        TestLable InkCanvas { get; }
    }
}
