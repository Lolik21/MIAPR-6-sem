using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Images
{
    public class Rectangle
    {
        public Point X { get; set; }
        public Point Y { get; set; }

        public bool IsInRectangle(Point  point)
        {
            if (point.X >= this.X.X && point.X <= this.Y.X )
            {
                if (point.Y >= this.X.Y && point.Y <= this.Y.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsOver(Rectangle rectangle)
        {
            if (IsInRectangle(rectangle.X) || IsInRectangle(rectangle.Y))
            {
                return true;
            }
            Point point1 = new Point { X = rectangle.Y.X, Y = rectangle.X.Y };
            Point point2 = new Point { X = rectangle.X.X, Y = rectangle.Y.Y };
            if (IsInRectangle(point1) || IsInRectangle(point2))
            {
                return true;
            }
            return false;
        }

        public bool Contains(Rectangle rectangle)
        {
            if (IsInRectangle(rectangle.X) || IsInRectangle(rectangle.Y))
            {
                return true;
            }
            return false;
        }
    }
}
