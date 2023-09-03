using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rocket_League
{
    public class World
    {
        public static double xPositionPercentage = 0.16;
        public static double yPositionPercentage = 0.1;
        public static double widthPercentage = 0.666;
        public static double heightPercentage = 0.6;
        public static double goalPercentage = 0.35;
        public static double gravityStrength = 0.002;
        public static int ballRadius = 29;
        public Ball ball { get; set; }
        public Car car { get; set; }
        public Vector2D gravity { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int goalTop { get; set; }
        public int goalBottom { get; set; }
        public int goalSize { get; set; }
        public int numOfPoints { get; set; }
        public List<List<Vector2D>> curves { get; set; }
        public int curveRadius { get; set; }
        public int blueScore { get; set; }
        public int orangeScore { get; set; }
        public bool isPaused { get; set; }
        public Vector2D inputCenter { get; set; }
        public int inputMaxDistance { get; set; }
        public Vector2D input { get; set; }
        public int inputRadius { get; set; }
        public int yInputOffset { get; set; }
        public Vector2D readRange { get; set; }
        public int rangeRadius { get; set; }
        public int numOfCarPoints { get; set; }
        public List<Vector2D> roof { get; set; }
        public List<Vector2D> undercarriage { get; set; }
        public List<Vector2D> back { get; set; }
        public List<Vector2D> front { get; set; }
        public double carMaxDistAndBallRadius { get; set; }
        public Image mapImg { get; set; }
        public bool highFPSMode { get; set; }
        public World(int X, int Y, int W, int H, bool isPaused)
        {
            this.X = X;
            this.Y = Y;
            this.Width = W;
            this.Height = H;
            goalSize = (int)(Height * goalPercentage);
            goalTop = Y + Height / 2 - goalSize / 2;
            goalBottom = Y + Height / 2 + goalSize / 2;
            numOfPoints = 10;
            curves = new List<List<Vector2D>>();
            curveRadius = (int)(H / 7.8);
            for (int i = 0; i < 4; i++)
            {
                List<Vector2D> c = new List<Vector2D>();
                double theta = 0;
                double deltaTheta = 0;
                Vector2D center = new Vector2D(Width / 2, Height / 2);
                if (i == 0)
                {
                    theta = Math.PI / 2;
                    deltaTheta = Math.PI / (2 * (numOfPoints - 1));
                    center = new Vector2D(X + curveRadius, Y + Height - curveRadius);
                }
                if (i == 1)
                {
                    theta = 0;
                    deltaTheta = Math.PI / (2 * (numOfPoints - 1));
                    center = new Vector2D(X + Width - curveRadius, Y + Height - curveRadius);
                }
                if (i == 2)
                {
                    theta = -Math.PI / 2;
                    deltaTheta = Math.PI / (2 * (numOfPoints - 1));
                    center = new Vector2D(X + Width - curveRadius, Y + curveRadius);
                }
                if (i == 3)
                {
                    theta = Math.PI;
                    deltaTheta = Math.PI / (2 * (numOfPoints - 1));
                    center = new Vector2D(X + curveRadius, Y + curveRadius);
                }
                for (int k = 0; k < numOfPoints; k++)
                {
                    Vector2D v = new Vector2D(center.X + Math.Cos(theta) * curveRadius, center.Y + Math.Sin(theta) * curveRadius);
                    c.Add(v);
                    theta += deltaTheta;
                }
                curves.Add(c);
            }
            Vector2D first = new Vector2D(X, Y);
            Vector2D second = new Vector2D(X + Width, Y + Height);
            ball = new Ball(first, second, ballRadius, goalSize, goalTop, goalBottom, curves, numOfPoints);
            car = new Car(new Vector2D(X + Width * 0.25, Y + Height * 0.95), new Vector2D(0, 0), 56, 28);
            gravity = new Vector2D(0, gravityStrength);
            blueScore = orangeScore = 0;
            this.isPaused = isPaused;
            inputMaxDistance = 50;
            inputRadius = 25;
            yInputOffset = 40;
            inputCenter = new Vector2D(X + Width / 2, Y + Height + inputMaxDistance + inputRadius + yInputOffset);
            input = new Vector2D(inputCenter.X, inputCenter.Y);
            readRange = new Vector2D(inputCenter.X, inputCenter.Y);
            rangeRadius = 90;
            numOfCarPoints = 16;
            roof = new List<Vector2D>(numOfCarPoints);
            undercarriage = new List<Vector2D>(numOfCarPoints);
            back = new List<Vector2D>(numOfCarPoints / 2);
            front = new List<Vector2D>(numOfCarPoints / 2);
            for (int i = 0; i < numOfCarPoints; i++)
            {
                roof.Add(new Vector2D(-1, -1));
                undercarriage.Add(new Vector2D(-1, -1));
            }
            for (int i = 0; i < numOfCarPoints / 2; i++)
            {
                back.Add(new Vector2D(-1, -1));
                front.Add(new Vector2D(-1, -1));
            }
            carMaxDistAndBallRadius = ball.radius + Math.Sqrt((car.width / 2) * (car.width / 2) + (car.height / 2) * (car.height / 2));
            mapImg = Image.FromFile("../../../assets/map.jpg");
            highFPSMode = false;
        }
        public void draw(Graphics g)
        {
            Pen pen = new Pen(Color.Black, 4);
            if (highFPSMode)
            {
                //draw world borders
                g.DrawLine(pen, new Point(X, Y + Height), new Point(X + Width, Y + Height));
                g.DrawLine(pen, new Point(X, Y), new Point(X + Width, Y));
                g.DrawLine(pen, new Point(X, Y), new Point(X, Y + Height));
                g.DrawLine(pen, new Point(X + Width, Y), new Point(X + Width, Y + Height));

                //draw the goals of the world
                pen = new Pen(Color.FromArgb(240, 240, 240), 4);
                g.DrawLine(pen, new Point(X, goalTop), new Point(X, goalBottom));
                g.DrawLine(pen, new Point(X + Width, goalTop), new Point(X + Width, goalBottom));
                pen = new Pen(Color.FromArgb(0, 0, 240), 4);
                g.DrawLine(pen, new Point(X - goalSize, goalTop), new Point(X - goalSize, goalBottom));
                g.DrawLine(pen, new Point(X - goalSize, goalTop), new Point(X, goalTop));
                g.DrawLine(pen, new Point(X - goalSize, goalBottom), new Point(X, goalBottom));
                pen = new Pen(Color.FromArgb(255, 150, 0), 4);
                g.DrawLine(pen, new Point(X + Width, goalTop), new Point(X + Width + goalSize, goalTop));
                g.DrawLine(pen, new Point(X + Width, goalBottom), new Point(X + Width + goalSize, goalBottom));
                g.DrawLine(pen, new Point(X + Width + goalSize, goalTop), new Point(X + Width + goalSize, goalBottom));

                //draw corners
                pen = new Pen(Color.Black, 4);
                for (int i = 0; i < 4; i++)
                {
                    for (int k = 0; k < numOfPoints; k++)
                    {
                        if (k == numOfPoints - 1)
                        {
                            Point p3 = new Point((int)curves[i][k - 1].X, (int)curves[i][k - 1].Y);
                            Point p4 = new Point((int)curves[i][k].X, (int)curves[i][k].Y);
                            g.DrawLine(pen, p3, p4);
                            break;
                        }
                        Point p1 = new Point((int)curves[i][k].X, (int)curves[i][k].Y);
                        Point p2 = new Point((int)curves[i][k + 1].X, (int)curves[i][k + 1].Y);
                        g.DrawLine(pen, p1, p2);
                    }
                }
            }
            else
            {
                g.DrawImage(mapImg, new Point(X - 80, Y - 15));
            }
            //draw analog stick range
            pen = new Pen(Color.Black, 2);
            g.DrawEllipse(pen, new RectangleF((int)inputCenter.X - inputMaxDistance, (int)inputCenter.Y - inputMaxDistance, 2 * inputMaxDistance, 2 * inputMaxDistance));
            g.DrawEllipse(pen, new RectangleF((int)inputCenter.X - rangeRadius, (int)inputCenter.Y - rangeRadius, 2 * rangeRadius, 2 * rangeRadius));
            //draw analog stick
            Brush brush = new SolidBrush(Color.FromArgb(66, 245, 221));
            g.FillEllipse(brush, new Rectangle((int)input.X - inputRadius, (int)input.Y - inputRadius, 2 * inputRadius, 2 * inputRadius));
            g.DrawLine(pen, (float)inputCenter.X, (float)inputCenter.Y, (float)input.X, (float)input.Y);
            brush.Dispose();
            pen.Dispose();
            ball.draw(g, highFPSMode);
            car.draw(g, highFPSMode);
            Font f = new Font("Arial", 16);
            Brush b = new SolidBrush(Color.Black);
            g.DrawString(getInfo() + score(), f, b, new Point(10, 10));
            b.Dispose();
            f.Dispose();
        }
        public void update()
        {
            if (isPaused) return;
            int score = ball.update(gravity);
            if (score == 1) orangeScore++;
            if (score == -1) blueScore++;
            if (score != 0) resetKickoff();
            Vector2D carGravity = new Vector2D(gravity);
            carGravity.mult(0.5);
            car.update(carGravity, X, Y, Width, Height, goalSize, goalTop, goalBottom, curves, numOfPoints);
            CalculateCarPoints();
            int pointOfCollision = CheckCollision();
            if (pointOfCollision == -1) return;
            Vector2D n = new Vector2D(0, 0);
            //front
            if (pointOfCollision == 2)
            {
                n.set(car.vertices[2].X - car.vertices[1].X, car.vertices[2].Y - car.vertices[1].Y);
            }
            //roof
            if (pointOfCollision == 1)
            {
                n.set(car.vertices[2].X - car.vertices[3].X, car.vertices[2].Y - car.vertices[3].Y);
            }
            //back
            if (pointOfCollision == 0)
            {
                n.set(car.vertices[3].X - car.vertices[0].X, car.vertices[3].Y - car.vertices[0].Y);
            }
            //undercarriage
            if (pointOfCollision == 3)
            {
                n.set(car.vertices[1].X - car.vertices[0].X, car.vertices[1].Y - car.vertices[0].Y);
            }
            n.normalize();
            n.SetToNormal();
            n.mult(2 * ball.velocity.dot(n));
            ball.velocity.set(ball.velocity.X - n.X, ball.velocity.Y - n.Y);
            Vector2D additionalVel = new Vector2D(car.velocity);
            additionalVel.normalize();
            additionalVel.mult(0.5);
            ball.velocity.add(additionalVel);
            Vector2D offset = new Vector2D(ball.position.X - car.position.X, ball.position.Y - car.position.Y);
            offset.normalize();
            offset.mult(0.001);
            double dist = Math.Sqrt((ball.position.X - car.position.X) * (ball.position.X - car.position.X) + (ball.position.Y - car.position.Y) * (ball.position.Y - car.position.Y));
            while (dist < carMaxDistAndBallRadius)
            {
                ball.position.add(offset);
                dist = Math.Sqrt((ball.position.X - car.position.X) * (ball.position.X - car.position.X) + (ball.position.Y - car.position.Y) * (ball.position.Y - car.position.Y));
            }
            /*
            //front
            if(0<pointOfCollision && pointOfCollision < numOfCarPoints*0.5)
            {
                Vector2D l = new Vector2D(ball.velocity.X - car.velocity.X, ball.velocity.Y - car.velocity.Y);
                l.normalize();
                ball.launch(l);
            }
            //roof
            if (numOfCarPoints * 0.5 < pointOfCollision && pointOfCollision < numOfCarPoints * 1.5)
            {
                Vector2D l = new Vector2D(ball.velocity.X - car.velocity.X, ball.velocity.Y - car.velocity.Y);
                l.normalize();
                ball.launch(l);
            }
            //back
            if (numOfCarPoints * 1.5 < pointOfCollision && pointOfCollision < numOfCarPoints * 2.5)
            {
                Vector2D l = new Vector2D(ball.velocity.X - car.velocity.X, ball.velocity.Y - car.velocity.Y);
                l.normalize();
                ball.launch(l);
            }
            //undercarriage
            if (numOfCarPoints * 1.5 < pointOfCollision && pointOfCollision < numOfCarPoints * 2.5)
            {
                Vector2D l = new Vector2D(ball.velocity.X - car.velocity.X, ball.velocity.Y - car.velocity.Y);
                l.normalize();
                ball.launch(l);
            }
            */
        }
        //0 - numOfCarPoints*0.5 (collision with front)
        //numOfCarPoints*0.5 - numOfCarPoints*1.5 (collision with roof)
        //numOfCarPoints*1.5 - numOfCarPoints*2 (collision with back)
        //numOfCarPoints*2 - numOfCarPoints*3 (collision with undercarriage)
        private int CheckCollision()
        {
            //exit if ball is nowhere near the car
            double dist = Math.Sqrt((ball.position.X - car.position.X) * (ball.position.X - car.position.X) + (ball.position.Y - car.position.Y) * (ball.position.Y - car.position.Y));
            if (dist > carMaxDistAndBallRadius) return -1;
            //check collision with the front(return 2)
            for (int i = 0; i < numOfCarPoints / 2; i++)
            {
                dist = Math.Sqrt((ball.position.X - front[i].X) * (ball.position.X - front[i].X) + (ball.position.Y - front[i].Y) * (ball.position.Y - front[i].Y));
                if (dist < ball.radius)
                {
                    return 2;
                }
            }
            //check collision with the roof(return 1)
            for (int i = 0; i < numOfCarPoints; i++)
            {
                dist = Math.Sqrt((ball.position.X - roof[i].X) * (ball.position.X - roof[i].X) + (ball.position.Y - roof[i].Y) * (ball.position.Y - roof[i].Y));
                if (dist < ball.radius)
                {
                    return 1;
                }
            }
            //check collision with the back(return 0)
            for (int i = 0; i < numOfCarPoints / 2; i++)
            {
                dist = Math.Sqrt((ball.position.X - back[i].X) * (ball.position.X - back[i].X) + (ball.position.Y - back[i].Y) * (ball.position.Y - back[i].Y));
                if (dist < ball.radius)
                {
                    return 0;
                }
            }
            //check collision with the undercarriage(return 3)
            for (int i = 0; i < numOfCarPoints; i++)
            {
                dist = Math.Sqrt((ball.position.X - undercarriage[i].X) * (ball.position.X - undercarriage[i].X) + (ball.position.Y - undercarriage[i].Y) * (ball.position.Y - undercarriage[i].Y));
                if (dist < ball.radius)
                {
                    return 3;
                }
            }
            return -1;
        }
        private void CalculateCarPoints()
        {
            //calculate all the points that make up the car
            int numOfHorizontalPoints = numOfCarPoints;
            int numOfVerticalPoints = numOfCarPoints / 2;
            double deltaX = (car.vertices[2].X - car.vertices[3].X) / numOfHorizontalPoints;
            double deltaY = (car.vertices[2].Y - car.vertices[3].Y) / numOfHorizontalPoints;
            for (int i = 0; i < numOfHorizontalPoints; i++)
            {
                roof[i].set(car.vertices[3].X + deltaX * i, car.vertices[3].Y + deltaY * i);
                undercarriage[i].set(car.vertices[1].X - deltaX * i, car.vertices[1].Y - deltaY * i);
            }
            deltaX = (car.vertices[2].X - car.vertices[1].X) / numOfVerticalPoints;
            deltaY = (car.vertices[2].Y - car.vertices[1].Y) / numOfVerticalPoints;
            for (int i = 0; i < numOfVerticalPoints; i++)
            {
                front[i].set(car.vertices[2].X - deltaX * i, car.vertices[2].Y - deltaY * i);
                back[i].set(car.vertices[0].X + deltaX * i, car.vertices[0].Y + deltaY * i);
            }
        }

        public void Input(Point p)
        {
            double dist = Math.Sqrt((p.X - inputCenter.X) * (p.X - inputCenter.X) +
                             (p.Y - inputCenter.Y) * (p.Y - inputCenter.Y));
            if (dist > rangeRadius)
            {
                input.set(inputCenter);
                car.SetInput(new Vector2D((input.X - inputCenter.X) / inputMaxDistance,
                                      (input.Y - inputCenter.Y) / inputMaxDistance));
                return;
            }
            input.set(p);
            dist = Math.Sqrt((input.X - inputCenter.X) * (input.X - inputCenter.X) +
                             (input.Y - inputCenter.Y) * (input.Y - inputCenter.Y));
            if (dist > inputMaxDistance)
            {
                double ratio = inputMaxDistance / dist;
                input.set(inputCenter.X + (input.X - inputCenter.X) * ratio,
                          inputCenter.Y + (input.Y - inputCenter.Y) * ratio);

                //alternative way and more complicated
                //double angle = Math.Atan2(p.Y - inputCenter.Y, p.X-inputCenter.X);
                //input.set(inputCenter.X + inputMaxDistance*Math.Cos(angle), inputCenter.Y+ inputMaxDistance*Math.Sin(angle));
            }
            car.SetInput(new Vector2D((input.X - inputCenter.X) / inputMaxDistance,
                                      (input.Y - inputCenter.Y) / inputMaxDistance));
        }

        public void resetKickoff()
        {
            ball.reset(X, Y, Width, Height);
            car.reset(X, Y, Width, Height);
        }

        public String score()
        {
            return $"{blueScore} - {orangeScore}";
        }
        public void switchPause()
        {
            isPaused = !isPaused;
        }
        public bool paused()
        {
            return isPaused;
        }

        public String getInfo()
        {
            return $"BOOST:{100 * car.boostAmount / Car.maxBoost}                                       ";// + ball.info() + car.info();
        }
        public static double Sqrt(double x)
        {
            double i = 0;
            double x1 = 0.0F;
            double x2 = 0.0F;


            if (x == 0F) return 0F;
            while ((i * i) <= x)
                i += 0.1F;

            x1 = i;

            for (int j = 0; j < 10; j++)
            {
                x2 = x;
                x2 /= x1;
                x2 += x1;
                x2 /= 2;
                x1 = x2;
            }

            return x2;

        }
    }
}
