namespace Rocket_League
{
    //simple 2D vector class with the needed functionality
    public class Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }
        public Vector2D(Vector2D other)
        {
            this.X = other.X;
            this.Y = other.Y;
        }
        public void add(Vector2D other)
        {
            X += other.X;
            Y += other.Y;
        }
        public void sub(Vector2D other)
        {
            X -= other.X;
            Y -= other.Y;
        }
        public void mult(double m)
        {
            X *= m;
            Y *= m;
        }
        public double dot(Vector2D p)
        {
            return p.X * X + p.Y * Y;
        }
        public Vector2D getInverse()
        {
            return new Vector2D(-X, -Y);
        }
        public void normalize()
        {
            double mag = Math.Sqrt(X * X + Y * Y);
            if (mag == 0)
            {
                return;
            }
            X /= mag;
            Y /= mag;
        }
        public double mag()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        public void SetToNormal()
        {
            double temp = X;
            X = -Y;
            Y = temp;
        }
        public double distanceToLineSeg(Vector2D p1, Vector2D p2)
        {
            double px = p2.X - p1.X;
            double py = p2.Y - p1.Y;
            double temp = (px * px) + (py * py);
            double u = ((X - p1.X) * px + (Y - p1.Y) * py) / (temp);
            if (u > 1)
            {
                u = 1;
            }
            else if (u < 0)
            {
                u = 0;
            }
            double x = p1.X + u * px;
            double y = p1.Y + u * py;

            double dx = x - X;
            double dy = y - Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            return dist;
        }
        public void RotateAround(Vector2D center, double angle)
        {
            //Translate the vertex and central point to the origin
            double translatedX = X - center.X;
            double translatedY = Y - center.Y;
            //Perform the rotation
            double cosA = Math.Cos(angle);
            double sinA = Math.Sin(angle);
            double rotatedX = translatedX * cosA - translatedY * sinA;
            double rotatedY = translatedX * sinA + translatedY * cosA;
            //Apply the inverse translation
            X = rotatedX + center.X;
            Y = rotatedY + center.Y;
        }
        public void set(double x, double y)
        {
            X = x;
            Y = y;
        }
        public void set(Vector2D v)
        {
            X = v.X;
            Y = v.Y;
        }
        public void set(Point v)
        {
            X = v.X;
            Y = v.Y;
        }
        public void setX(double x)
        {
            X = x;
        }
        public void setY(double y)
        {
            Y = y;
        }
        public Vector2D neg()
        {
            return new Vector2D(-X, -Y);
        }
        public void invertY()
        {
            Y *= -1;
        }
        public void invertX()
        {
            X *= -1;
        }
        public void invert()
        {
            X *= -1;
            Y *= -1;
        }
        public bool Equals(Vector2D other)
        {
            return (X == other.X && Y == other.Y);
        }
    }
}
