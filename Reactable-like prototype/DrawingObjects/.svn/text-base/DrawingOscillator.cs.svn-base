using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication2.DrawingObjects
{
    public partial class DrawingOscillator:Window
    {
        private double xmin = 0;

            private double xmax = 6.5;
            private double ymin = -1.1;
            private double ymax = 1.1;
            private Polyline pl;
            private Canvas canvas;

            public DrawingOscillator(Canvas _canvas)
            {
              canvas= _canvas;
              AddChart(canvas);
             }

          private void AddChart(Canvas canvas)
            {
                // Draw sine curve:
               pl = new Polyline();
               pl.Stroke = Brushes.Black;
               for (int i = 0; i < 70; i++)
                {
                   double x = i/5.0;
                   double y = Math.Sin(x);
                     pl.Points.Add(CurvePoint(
                     new Point(x, y)));
                 }
             canvas.Children.Add(pl);
              // Draw cosine curve:
                 pl = new Polyline();
               pl.Stroke = Brushes.Black;
               pl.StrokeDashArray = new DoubleCollection(
               new double[] { 4, 3 });
                for (int i = 0; i < 70; i++)
                 {
                   double x = i / 5.0;
                   double y = Math.Cos(x);
                   pl.Points.Add(CurvePoint(
                   new Point(x, y)));
                  }
                   canvas.Children.Add(pl);
             }
        private Point CurvePoint(Point pt)
          {
             Point result = new Point();
             result.X = (pt.X - xmin) * canvas.Width / (xmax - xmin);
             result.Y = canvas.Height - (pt.Y - ymin) * canvas.Height 
             /  (ymax - ymin);
             return result;
        }
   }    
}

