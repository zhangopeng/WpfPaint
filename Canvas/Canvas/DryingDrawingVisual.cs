using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Canvas
{
    public class DryingDrawingVisual
    {
        public ContainerVisual ContainerVisual
        {
            get;
        }

        public List<DrawingVisual> DryingDrawingVisualList
        {
            get;
        }

        public DryingDrawingVisual([NotNull] ContainerVisual containerVisual, [NotNull] List<DrawingVisual> dryingDrawingVisualList)
        {
            if (containerVisual == null)
            {
                throw new ArgumentNullException("containerVisual");
            }
            if (dryingDrawingVisualList == null)
            {
                throw new ArgumentNullException("dryingDrawingVisualList");
            }
            ContainerVisual = containerVisual;
            DryingDrawingVisualList = dryingDrawingVisualList;
        }
    }
}
