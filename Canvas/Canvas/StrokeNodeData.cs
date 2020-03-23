using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas
{
    internal struct StrokeNodeData
    {
        private static StrokeNodeData s_empty;

        private System.Windows.Point _position;

        private float _pressure;

        internal static StrokeNodeData Empty => s_empty;

        internal bool IsEmpty => DoubleUtil.AreClose(_pressure, s_empty._pressure);

        internal System.Windows.Point Position => _position;

        internal float PressureFactor => _pressure;

        internal StrokeNodeData(System.Windows.Point position)
        {
            _position = position;
            _pressure = 1f;
        }

        internal StrokeNodeData(System.Windows.Point position, float pressure)
        {
            _position = position;
            _pressure = pressure;
        }
    }
}
