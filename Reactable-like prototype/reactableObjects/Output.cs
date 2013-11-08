using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApplication2.reactableObjects
{
    public class Output : ReactableObject
    {
        public Ellipse outputCircle;
        public const int windowCentreX = 650;
        public const int windowCentreY = 350;
        private const int height = 10;
        private const int width = 10;
		//public positionOutput;

        public Output(Canvas _canvas)
        {
            Canvas = _canvas;
			x = 650;
			y = 350;
            outputCircle = new Ellipse();
            outputCircle.Height = height;
            outputCircle.Width = width;
            outputCircle.Fill = Brushes.Black;
			Canvas.SetTop(outputCircle, y - height/2);
			Canvas.SetLeft(outputCircle, x - width/2);

			InputObject = new ReactableObject[1];
			InputObject[0] = null;

            Canvas.Children.Add(outputCircle);

        }
        
        public int getHeight()
        {
            return height;
        }
        public int getWidth()
        {
            return width;
        }

    }
}
