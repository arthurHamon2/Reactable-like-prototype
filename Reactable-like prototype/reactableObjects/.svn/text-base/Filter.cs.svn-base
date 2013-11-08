using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace WpfApplication2.reactableObjects
{
    class Filter : EffectFilter
    {
        /// <summary>
        /// Constructs an filter object.
        /// </summary>
        /// <param name="_canvas">canvas where the oscillator is on it</param>
        /// <param name="x">Coordonne x</param>
        /// <param name="y">Coordonne y</param>
        /// <param name="_width">The size of each object's side</param>
        public Filter(Canvas _canvas, int _x, int _y, double _width) : base(_canvas, _x, _y, _width)
        {
            geometricShape.Fill = Brushes.Orange;
            geometricShape.Stroke = Brushes.Black;

			// Initialization position left pointer
			pointerArcLeftMax = 44100;
			pointerArcLeftMin = 0;
			pointerPositionArcLeft = 0;
            drawSubtypeMenubButton();
        }
    }
}
