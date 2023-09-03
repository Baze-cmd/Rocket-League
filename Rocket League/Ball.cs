using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket_League
{
    public class Ball
    {
        //Random rand = new Random();
        public static double coefficientOfFrictionWhenBouncing = 0.4;
        public static double coefficientOfFrictionWhileRolling = 0.001;
        public static double groundProximityThreshold = 0.5;
        public static double minimumVerticalVelocityThreshold = 0.01;
        public static double minimumRollingVelocityThreshold = 0.005;
        public static double minimumHorizontalBounceFriction = 0.02;
        public Vector2D position { get; set; }
        public Vector2D oldPos { get; set; }
        public Vector2D velocity { get; set; }
        public int radius { get; set; }
        public bool isOnGround { get; set; }
        public Vector2D worldStart { get; set; }
        public Vector2D worldEnd { get; set; }
        public int numOfPoints { get; set; }
        public List<List<Vector2D>> curves { get; set; }
        public int goalTop { get; set; }
        public int goalBottom { get; set; }
        public int goalSize { get; set; }
        public Image ballImg { get; set; }
        public int cycles { get; set; }
        public int cycleMin { get; set; }
        public Ball(Vector2D s, Vector2D e, int rad, int gs, int gt, int gb, List<List<Vector2D>> curves, int numOfPoints)
        {
            worldStart = new Vector2D(s.X, s.Y);
            worldEnd = new Vector2D(e.X, e.Y);
            double xStart = (worldStart.X + worldEnd.X) * 0.5;
            double yStart = (worldStart.Y + worldEnd.Y) * 0.5;
            position = new Vector2D(xStart - radius, yStart - radius);
            oldPos = new Vector2D(xStart - radius, yStart - radius);
            velocity = new Vector2D(0, 0.5);
            radius = rad;
            goalSize = gs;
            goalTop = gt;
            goalBottom = gb;
            this.curves = curves;
            this.numOfPoints = numOfPoints;
            ballImg = Image.FromFile("../../../assets/ball.png");
            cycles = 0;
            cycleMin = 75;
        }
        public void draw(Graphics g, bool highFPSMode)
        {
            if (highFPSMode)
            {
                Brush b = new SolidBrush(Color.Red);
                g.FillEllipse(b, (int)(position.X - radius), (int)(position.Y - radius), 2 * radius, 2 * radius);
                b.Dispose();
            }
            else
            {
                //draws the ball at its position with its radius
                if (1 < position.X && position.X < 2000 && 1 < position.Y && position.Y < 2000)
                    g.DrawImage(ballImg, new Rectangle((int)position.X - radius, (int)position.Y - radius, 2 * radius, 2 * radius));
            }
        }

        //returns 0 if no one scored, -1 if blue scored, 1 if orange scored
        public int update(Vector2D gravity)
        {
            cycles++;
            if (cycles > 10000) cycles = cycleMin;
            //save previous position
            oldPos.set(position);
            //update position and velocity
            position.add(velocity);
            velocity.add(gravity);

            //checks if the ball is on the ground or not
            isOnGround = (Math.Abs(position.Y - worldEnd.Y) - radius < groundProximityThreshold && Math.Abs(velocity.Y) < minimumVerticalVelocityThreshold);

            //find in which quadrant of the world the ball is in
            //0-bottom left, 1-bottom right, 2-top right, 3-top left
            int quadrant = 0;
            if ((position.X < (worldStart.X + worldEnd.X) / 2) && (position.Y > (worldStart.Y + worldEnd.Y) / 2))
                quadrant = 0;
            if ((position.X > (worldStart.X + worldEnd.X) / 2) && (position.Y > (worldStart.Y + worldEnd.Y) / 2))
                quadrant = 1;
            if ((position.X > (worldStart.X + worldEnd.X) / 2) && (position.Y < (worldStart.Y + worldEnd.Y) / 2))
                quadrant = 2;
            if ((position.X < (worldStart.X + worldEnd.X) / 2) && (position.Y < (worldStart.Y + worldEnd.Y) / 2))
                quadrant = 3;

            //flag to not reset y velocity later
            bool hitCurve = false;
            //checks collision with the curves and mirrors velocity if so
            for (int i = 0; i < numOfPoints - 1; i++)
            {
                //checks if there is a collision with that specific line segment
                if (Math.Abs(position.distanceToLineSeg(curves[quadrant][i], curves[quadrant][i + 1])) < radius)
                {
                    //math that reflects the balls velocity
                    Vector2D n = new Vector2D(curves[quadrant][i + 1].X - curves[quadrant][i].X, curves[quadrant][i + 1].Y - curves[quadrant][i].Y);
                    n.normalize();
                    n.SetToNormal();
                    n.mult(2 * velocity.dot(n));
                    velocity.set(velocity.X - n.X, velocity.Y - n.Y);
                    if (!(velocity.mag() < 0.02))
                    {
                        position.set(oldPos);
                    }
                    hitCurve = true;
                    if (cycles > cycleMin)
                    {
                        applyBounceFriction(coefficientOfFrictionWhenBouncing / 2);
                        cycles = 0;
                    }
                    break;
                }
            }

            //checks if the ball has hit the ceiling
            if (hitCeiling())
            {
                //adjust the position to be in the world and invert the appropriate velocity
                position.setY(worldStart.Y + radius);
                velocity.invertY();
            }
            //checks if the ball has hit the left wall
            if (hitLeftWall())
            {
                //check if entered the goal
                if (enterGoal())
                {
                    //check if hit top post
                    if (hitTopPost())
                    {
                        position.setY(goalTop + radius);
                        velocity.invertY();
                    }
                    //check if hit bottom post
                    if (hitBottomPost() && velocity.Y > 0)
                    {

                        velocity.setX(velocity.X + 0.01);
                        position.setY(goalBottom - radius);
                        velocity.invertY();
                        applyBounceFriction(coefficientOfFrictionWhenBouncing);
                    }
                    //check if hit back post
                    if (hitLeftPost())
                    {
                        position.setX(worldStart.X - goalSize + radius);
                        velocity.invertX();
                    }
                }
                else
                {
                    //adjust the position to be within the world and invert the appropriate velocity
                    position.setX(worldStart.X + radius);
                    velocity.invertX();
                }
            }
            //checks if the ball has hit the right wall
            if (hitRightWall())
            {
                //check if enter goal
                if (enterGoal())
                {
                    //check if hit top post
                    if (hitTopPost())
                    {
                        position.setY(goalTop + radius);
                        velocity.invertY();
                    }
                    //check if hit bottom post
                    if (hitBottomPost() && velocity.Y > 0)
                    {
                        velocity.setX(velocity.X - 0.01);
                        position.setY(goalBottom - radius);
                        velocity.invertY();
                        applyBounceFriction(coefficientOfFrictionWhenBouncing);
                    }
                    //check if hit back post
                    if (hitRightPost())
                    {
                        position.setX(worldEnd.X + goalSize - radius);
                        velocity.invertX();
                    }
                }
                else
                {
                    //adjust the position to be in the world and invert the appropriate velocity
                    position.setX(worldEnd.X - radius);
                    velocity.invertX();
                }
            }
            //checks if the ball has hit the floor
            if (hitFloor())
            {
                //adjust the position to be in the world and invert the appropriate velocity
                position.setY(worldEnd.Y - radius);
                velocity.invertY();
                applyBounceFriction(coefficientOfFrictionWhenBouncing);
            }
            //checks if the ball is on the floor
            if (isOnGround && !hitCurve)
            {
                //stops the vertical velocity so that it doesnt go on 'forever' and adjusts
                //the position to be on the floor exactly
                ApplyRollFriction();
                velocity.setY(0);
                position.setY(worldEnd.Y - radius);
            }
            //return whoever has scored (-1/0/1)
            return (position.X > worldEnd.X + goalSize * 0.5) ? -1 : ((position.X < worldStart.X - goalSize * 0.5) ? 1 : 0);
        }

        private void applyBounceFriction(double K)
        {
            //calculates the friction force on bounce
            Vector2D bounceFriction = velocity.getInverse();
            bounceFriction.mult(K);
            //calculate the horizontal friction on bounce
            //friction always acts oposite of the balls motion so it slows it down
            //it is 0 if the ball's horizontal speed is close to 0
            double c = Math.Abs(velocity.X) < minimumHorizontalBounceFriction ? 0 : minimumHorizontalBounceFriction;
            bounceFriction.X = bounceFriction.X > 0 ? c : -c;
            velocity.add(bounceFriction);
        }
        private void ApplyRollFriction()
        {
            //calculates the rolling friction(different from bounce friction)
            Vector2D rollFriction = velocity.getInverse();
            rollFriction.mult(coefficientOfFrictionWhileRolling);
            velocity.add(rollFriction);
            //sets the horizontal ball speed to 0 when it reaches a certain low speed
            velocity.X = (Math.Abs(velocity.X) <= minimumRollingVelocityThreshold) ? 0 : velocity.X;
        }
        public void reset(double sX, double sY, double W, double H)
        {
            double xStart = sX + W / 2;
            double yStart = sY + H / 2;
            position = new Vector2D(xStart, yStart);
            velocity = new Vector2D(0, 0.5);
        }
        //public void setPos(Point l)
        //{
        //    position.X = l.X;
        //    position.Y = l.Y;
        //}
        public void launch(Vector2D direction)
        {
            velocity.set(direction);
        }
        private bool hitCeiling()
        {
            return position.Y < worldStart.Y + radius;
        }
        private bool hitLeftWall()
        {
            return position.X < worldStart.X + radius;
        }
        private bool hitRightWall()
        {
            return position.X > worldEnd.X - radius;
        }
        private bool hitFloor()
        {
            return position.Y > worldEnd.Y - radius;
        }
        private bool enterGoal()
        {
            return goalTop < position.Y && position.Y < goalBottom;
        }
        private bool hitTopPost()
        {
            return position.Y < goalTop + radius;
        }
        private bool hitBottomPost()
        {
            return position.Y > goalBottom - radius;
        }
        private bool hitLeftPost()
        {
            return position.X < worldStart.X - goalSize + radius;
        }
        private bool hitRightPost()
        {
            return position.X > worldEnd.X + goalSize - radius;
        }

        //for debugging purpose
        public String info()
        {
            return $"pos: {position.X:0.000}, {position.Y:0.000}; vel: {velocity.X:0.000}, {velocity.Y:0.000};";
        }
    }
}
