using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApplication2.reactableObjects
{
	class OscillatorController : Controller
	{
		/// <summary>
        /// Constructs an oscillator object.
        /// </summary>
        /// <param name="_canvas">canvas where the oscillator is on it</param>
        /// <param name="x">Coordonne x</param>
        /// <param name="y">Coordonne y</param>
        /// <param name="_width">The size of each object's side</param>
        public OscillatorController(Canvas _canvas, int _x, int _y, double _width) : base(_canvas, _x, _y, _width)
        {
            geometricShape.Fill = Brushes.Gold;
            geometricShape.Stroke = Brushes.Black;

			// Initialization position left pointer
			pointerArcLeftMax = 44100;
			pointerArcLeftMin = 0;
			pointerPositionArcLeft = 0;
        }
	}
}
