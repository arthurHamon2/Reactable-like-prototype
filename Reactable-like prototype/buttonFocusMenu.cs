using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace WpfApplication2
{
    class buttonFocusMenu
    {
        private Path activeButtonSurface;
        private Rectangle button;
        private Canvas buttonCanvas;
        private Point buttonPoint;
        private Path buttonPath;

        public buttonFocusMenu(Canvas _buttonCanvas, Point _buttonPoint, Path _buttonPath,int _height, int _width, Brush colour)
        {
            button = new Rectangle();
            button.Height = _height;
            button.Width = _width;
            button.Fill = colour;
            button.Stroke = Brushes.AntiqueWhite;
            button.StrokeThickness = 2;
            button.RadiusX = 20; 
            button.RadiusY = 20;
        }

        public void setUpButton()
        {
            activeButtonSurface = new Path();
            //few math to calculate the menu position vertically
            double activeMenuSurfaceTop = buttonPoint.Y - button.Height;
            double activeMenuSurfaceLeft = buttonPoint.X - button.Width;
            double activeMenuSurfaceBottom = buttonPoint.Y + button.Height;
            double activeMenuSurfaceRight = buttonPoint.X + button.Width;

            Point pointActiveMenuSurfaceTopLeft = new System.Windows.Point(activeMenuSurfaceLeft, activeMenuSurfaceTop);
            Point pointActiveMenuSurfaceBottomRight = new System.Windows.Point(activeMenuSurfaceRight, activeMenuSurfaceBottom);
            activeButtonSurface.Data = new RectangleGeometry(new Rect(pointActiveMenuSurfaceTopLeft,
                                                                      pointActiveMenuSurfaceBottomRight), 2, 2);

        }

    }
}
