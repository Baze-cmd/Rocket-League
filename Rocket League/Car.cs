using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Rocket_League
{
    public class Car
    {
        public static double coefficientOfFrictionWhileRolling = 0.004;
        public static double minimumRollingVelocityThreshold = 0.005;
        public static double maxVelocity = 1.1;
        public static double maxThrottleVelocity = 0.7;
        public static double boostStrength = 0.0075;
        public static double jumpStrength = 0.6;
        public static int maxJumpTimeCycles = 500;
        public static double throttleStrength = 0.01;
        public static int maxBoost = 512;
        public static double dodgeDeadzone = 0.25;
        public static int animationTime = 192;
        public Vector2D position { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public List<Vector2D> vertices { get; set; }
        public Vector2D velocity { get; set; }
        public Vector2D prevVelocity { get; set; }
        public double boost { get; set; }
        public bool isOnGround { get; set; }
        public bool hitCeiling { get; set; }
        public Vector2D input { get; set; }
        public bool isBoosting { get; set; }
        public int jumpingTimer { get; set; }
        public bool hasFlip { get; set; }
        public double angle { get; set; }
        public int boostAmount { get; set; }
        public int flipAnimationTimer { get; set; }
        public int directionOfFlip { get; set; }
        public double distanceToWall { get; set; }
        //public List<Image> carImages { get; set; }
        public Car(Vector2D position, Vector2D velocity, int width, int height)
        {
            //position is the center of the car while vertices are the outlines
            this.position = position;
            this.isOnGround = false;
            this.hitCeiling = false;
            this.velocity = velocity;
            this.prevVelocity = velocity;
            this.width = width;
            this.height = height;
            this.vertices = new List<Vector2D>();
            //[0]-bottom left, [1]-bottom right, [2]-top right, [3]-top left
            vertices.Add(new Vector2D(position.X - width / 2, position.Y + height / 2));
            vertices.Add(new Vector2D(position.X + width / 2, position.Y + height / 2));
            vertices.Add(new Vector2D(position.X + width / 2, position.Y - height / 2));
            vertices.Add(new Vector2D(position.X - width / 2, position.Y - height / 2));
            //input vector from the analog stick
            this.input = new Vector2D(0, 0);
            //rotate the car a little bit at the start
            for (int i = 0; i < vertices.Count; i++) vertices[i].RotateAround(position, Math.PI / 2 - 0.1);
            this.isBoosting = false;
            jumpingTimer = 0;
            hasFlip = true;
            angle = 0;
            boostAmount = maxBoost;
            flipAnimationTimer = 0;
            // -1 left, 0 no or null, 1 right
            directionOfFlip = 0;
            distanceToWall = 100;
            //carImages = new List<Image>(17);
            //carImages.Add(Image.FromFile("../../assets/80.png"));
            //carImages.Add(Image.FromFile("../../assets/70.png"));
            //carImages.Add(Image.FromFile("../../assets/60.png"));
            //carImages.Add(Image.FromFile("../../assets/50.png"));
            //carImages.Add(Image.FromFile("../../assets/40.png"));
            //carImages.Add(Image.FromFile("../../assets/30.png"));
            //carImages.Add(Image.FromFile("../../assets/20.png"));
            //carImages.Add(Image.FromFile("../../assets/10.png"));
            //carImages.Add(Image.FromFile("../../assets/0.png"));
            //carImages.Add(Image.FromFile("../../assets/-10.png"));
            //carImages.Add(Image.FromFile("../../assets/-20.png"));
            //carImages.Add(Image.FromFile("../../assets/-30.png"));
            //carImages.Add(Image.FromFile("../../assets/-40.png"));
            //carImages.Add(Image.FromFile("../../assets/-50.png"));
            //carImages.Add(Image.FromFile("../../assets/-60.png"));
            //carImages.Add(Image.FromFile("../../assets/-70.png"));
            //carImages.Add(Image.FromFile("../../assets/-80.png"));
        }
        public void draw(Graphics g, bool highFPSMode)
        {

            if (highFPSMode)
            {
                Point[] points = new Point[4];
                for (int i = 0; i < vertices.Count; i++) points[i] = new Point((int)vertices[i].X, (int)vertices[i].Y);
                //Pen pen = new Pen(Color.Blue, 2);
                //g.DrawPolygon(pen, points);
                if (hasFlip)
                {
                    g.FillPolygon(new SolidBrush(Color.Black), points);
                }
                //draw car body
                Pen pen = new Pen(Color.Gold, 3);
                g.DrawLine(pen, points[0], points[1]);
                g.DrawLine(pen, points[1], points[2]);
                g.DrawLine(pen, points[2], points[3]);
                g.DrawLine(pen, points[3], points[0]);
                //g.DrawLine(pen, new Point(0, (int)(a + b - height/2)), new Point(1000, (int)(a + b - height/2)));
                pen.Dispose();
            }
            else
            {
                Point[] points = new Point[4];
                for (int i = 0; i < vertices.Count; i++) points[i] = new Point((int)vertices[i].X, (int)vertices[i].Y);
                //Pen pen = new Pen(Color.Blue, 2);
                //g.DrawPolygon(pen, points);
                if (hasFlip)
                {
                    g.FillPolygon(new SolidBrush(Color.Black), points);
                }
                //draw car body
                Pen pen = new Pen(Color.Gold, 3);
                g.DrawLine(pen, points[0], points[1]);
                g.DrawLine(pen, points[1], points[2]);
                g.DrawLine(pen, points[2], points[3]);
                g.DrawLine(pen, points[3], points[0]);
                //g.DrawLine(pen, new Point(0, (int)(a + b - height/2)), new Point(1000, (int)(a + b - height/2)));
                pen.Dispose();
                //when i eventually add images
                /*
                int ind = 0;
                if(angle < -Math.PI / 2 || angle > Math.PI / 2)
                {

                }
                else
                {
                    double index = angle + Math.PI / 2;
                    index /= Math.PI;
                    index *= 16;
                    index = Math.Floor(index);
                    ind = (int)index;
                }
                if (0 <= ind && ind <= 16)
                    g.DrawImage(carImages[ind], (float)position.X-300,(float)position.Y-200);
                */
            }
        }
        public void update(Vector2D gravity, double sX, double sY, double worldWidth, double worldHeight, int gs, int gt, int gb, List<List<Vector2D>> curves, int numOfPoints)
        {
            //calculate if the car is on the ground
            isOnGround = (position.Y > sY + worldHeight - width);
            //calculate the angle of the car
            angle = Math.Atan2(vertices[0].Y - vertices[1].Y, vertices[0].X - vertices[1].X);
            if (isBoosting && boostAmount > 0)
            {
                //if the user is boosting calculate the force and apply it
                Vector2D acc = new Vector2D(vertices[1].X - vertices[0].X, vertices[1].Y - vertices[0].Y);
                acc.normalize();
                acc.mult(boostStrength);
                velocity.add(acc);
                boostAmount--;
            }
            //add throttle if the car is on the ground and the user inputs
            if (isOnGround && !isBoosting && input.X != 0 && (angle < Math.PI / 12 || angle > (Math.PI - Math.PI / 12)))
            {
                velocity.setX(lerp(input.X, velocity.X, throttleStrength));
            }
            //cap the velocity
            if (!isBoosting && velocity.mag() > maxThrottleVelocity)
            {
                velocity.normalize();
                velocity.mult(maxThrottleVelocity);
            }
            if (velocity.mag() > maxVelocity)
            {
                velocity.normalize();
                velocity.mult(maxVelocity);
            }
            //add velocity to the position and to all the other vertices
            addVel(velocity);
            velocity.add(gravity);
            bool hasPassedBottomBorder = false;
            double dist = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].Y > sY + worldHeight)
                {
                    dist = Math.Abs(vertices[i].Y - sY - worldHeight);
                    hasPassedBottomBorder = true;
                    velocity.setY(0);
                    break;
                }
            }
            if (hasPassedBottomBorder)
            {
                position.Y -= dist;
                for (int i = 0; i < vertices.Count; i++)
                {
                    vertices[i].Y -= dist;
                }
            }
            //if you have started the first jump, addvance the jumpTimer
            if (jumpingTimer != 0) jumpingTimer++;
            //cap the jumpTimer so that it doesnt go crazy
            if (jumpingTimer > maxJumpTimeCycles)
            {
                jumpingTimer = 0;
                hasFlip = false;
            }
            //streighten the car when its near the ground
            if (position.Y > sY + worldHeight - width && position.Y - velocity.Y < sY + worldHeight - width)
            {
                AlignVertices(position, angle);
            }
            //add roll friction
            if (isOnGround && !(Math.Abs(velocity.X) < 0.005))
            {
                ApplyRollFriction();
            }
            if (flipAnimationTimer > 0)
            {
                double deltaTheta = 2 * Math.PI / animationTime;
                if (directionOfFlip == -1) deltaTheta *= -1;
                for (int i = 0; i < vertices.Count; i++)
                {
                    vertices[i].RotateAround(position, deltaTheta);
                }
                flipAnimationTimer--;
            }
            else
            {
                Rotate(sY + worldHeight);
            }
            //check if it has passed through the world border and then readjust
            bool HPBB = false;
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].Y > sY + worldHeight)
                {
                    dist = Math.Abs(vertices[i].Y - sY - worldHeight);
                    velocity.setY(0);
                    HPBB = true;
                }
            }
            if (HPBB)
            {
                position.Y -= dist;
                for (int i = 0; i < vertices.Count; i++)
                {
                    vertices[i].Y -= dist;
                }
            }
            //calculate if "all 4 weels have touched the ground" and give ability back if so
            dist = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                dist = Math.Max(dist, Math.Abs(vertices[i].Y - sY - worldHeight));
            }
            if (dist < 1.5 * height)
            {
                hasFlip = true;
            }
            //flip the car based on the input from the user
            bool hasFlippedHorizontally = false;
            if (isOnGround && vertices[0].Y == vertices[1].Y)
            {
                if (input.X < 0 && vertices[0].X < vertices[1].X)
                {
                    flipCarHorizontally();
                    hasFlippedHorizontally = true;
                }
                if (input.X > 0 && vertices[0].X > vertices[1].X)
                {
                    flipCarHorizontally();
                    hasFlippedHorizontally = true;
                }
            }
            if (input.X < 0 && flipAnimationTimer == 0)
            {
                if (vertices[1].X < vertices[0].X)
                {
                    if (!hasFlippedHorizontally)
                        flipCarVertically();
                }
            }
            if (input.X == 0 && input.Y == 0 && velocity.X < 0 && vertices[0].X < vertices[1].X)
            {
                flipCarHorizontally();
            }

            bool hasHitWall = false;
            //check if it has hit the left wall and readjust
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].X < sX)
                {
                    dist = Math.Abs(vertices[i].X - sX);
                    velocity.setX(0);
                    position.X += dist;
                    for (int k = 0; k < vertices.Count; k++) vertices[k].X += dist;
                    hasHitWall = true;
                    break;
                }
            }
            //check if it has hit the right wall and readjust
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].X > sX + worldWidth)
                {
                    dist = Math.Abs(vertices[i].X - sX - worldWidth);
                    velocity.setX(0);
                    position.X -= dist;
                    for (int k = 0; k < vertices.Count; k++) vertices[k].X -= dist;
                    hasHitWall = true;
                    break;
                }
            }
            //check if it has hit the ceiling and align
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].Y < sY)
                {
                    AlignVertices(position, angle);
                    if (input.X < 0) flipCarHorizontally();
                    dist = Math.Abs(position.Y - sY);
                    if (dist < height / 2)
                    {
                        position.Y += dist;
                        for (int k = 0; k < vertices.Count; k++) vertices[k].Y += dist;
                        velocity.setY(0);
                        hasFlip = true;
                    }
                    break;
                }
            }
            //check the distance to the walls and give flip if its close enough
            distanceToWall = Math.Abs(position.X - sX);
            distanceToWall = Math.Min(distanceToWall, Math.Abs(position.X - sX - worldWidth));
            bool isInNetRange = (sY + worldHeight / 2 - gs / 2 < position.Y && position.Y < sY + worldHeight / 2 + gs / 2);
            if (distanceToWall < height * 0.75 && input.Y < 0 && !isInNetRange)
            {
                hasFlip = true;
            }
            //repel the car if it goes in the net
            if (hasHitWall && isInNetRange)
            {
                if (input.X < 0) velocity.setX(velocity.X + 0.2);
                else velocity.setX(velocity.X - 0.2);
            }
            //regenerate the boost if its touching a surface
            if ((isOnGround && boostAmount < maxBoost) || (Math.Abs(position.Y - sY) < height) || (distanceToWall < height * 0.75 && !isInNetRange))
            {
                boostAmount++;
            }
            //cap the boost
            boostAmount = Math.Min(boostAmount, maxBoost);
        }
        private void flipCarVertically()
        {
            Vector2D temp = new Vector2D(vertices[2]);
            vertices[2].set(vertices[1]);
            vertices[1].set(temp);
            temp.set(vertices[3]);
            vertices[3].set(vertices[0]);
            vertices[0].set(temp);
        }
        private void flipCarHorizontally()
        {
            Vector2D temp = new Vector2D(vertices[2]);
            vertices[2].set(vertices[3]);
            vertices[3].set(temp);
            temp.set(vertices[1]);
            vertices[1].set(vertices[0]);
            vertices[0].set(temp);
        }
        private void addVel(Vector2D v)
        {
            position.add(v);
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].add(v);
            }
        }
        private void ApplyRollFriction()
        {
            //calculates the rolling friction(different from bounce friction)
            Vector2D rollFriction = velocity.getInverse();
            rollFriction.setY(0);
            //rollFriction.normalize();
            rollFriction.mult(coefficientOfFrictionWhileRolling);
            velocity.add(rollFriction);
            //sets the horizontal car speed to 0 when it reaches a certain low speed
            velocity.X = (Math.Abs(velocity.X) <= minimumRollingVelocityThreshold) ? 0 : velocity.X;
        }
        private void Rotate(double eY)
        {
            //calculate the angle of the input
            double theta = Math.Atan2(input.Y, input.X);
            //dont allow the car to rotate in the ground
            if (theta > 0 && isOnGround)
            {
                AlignVertices(position, angle);
                return;
            }
            double dist = 1000;
            for (int i = 0; i < vertices.Count; i++)
            {
                dist = Math.Min(dist, Math.Abs(vertices[i].Y - eY));
            }
            //if its not boosing and on the ground and rotates upward calculate the height
            if (theta < 0 && dist < 2 && Math.Abs(velocity.Y) < 0.01 && !isBoosting && !(1 <= jumpingTimer && jumpingTimer < maxJumpTimeCycles - 1))
            {
                double elevation;
                if (0 < Math.Abs(theta) && Math.Abs(theta) < Math.PI / 2)
                    elevation = mapValue(Math.Abs(theta), 0, Math.PI / 2, eY - height / 2, eY - width / 2);
                else
                    elevation = mapValue(Math.Abs(theta), Math.PI, Math.PI / 2, eY - height / 2, eY - width / 2);
                position.set(position.X, elevation);
            }
            //level the car
            vertices[0].set(position.X - width / 2, position.Y + height / 2);
            vertices[1].set(position.X + width / 2, position.Y + height / 2);
            vertices[2].set(position.X + width / 2, position.Y - height / 2);
            vertices[3].set(position.X - width / 2, position.Y - height / 2);
            //rotate normally as in freefall
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].RotateAround(position, theta);
            }
        }
        private void AlignVertices(Vector2D pos, double a)
        {
            //level the car
            vertices[0].set(pos.X - width / 2, pos.Y + height / 2);
            vertices[1].set(pos.X + width / 2, pos.Y + height / 2);
            vertices[2].set(pos.X + width / 2, pos.Y - height / 2);
            vertices[3].set(pos.X - width / 2, pos.Y - height / 2);
            if (0 < a && a < Math.PI / 2)
            {
                flipCarHorizontally();
            }
        }
        public bool IsAligned()
        {
            return vertices[0].Y == vertices[1].Y;
        }
        public void SetInput(Vector2D i)
        {
            input.set(i);
        }
        public void StartBoost()
        {
            isBoosting = true;
        }
        public void StopBoost()
        {
            isBoosting = false;
        }
        public void Jump()
        {
            //you can not jump if the timer excedes maxJumpTime
            if (jumpingTimer >= maxJumpTimeCycles || !hasFlip)
            {
                return;
            }
            //if you are on a surface and its the first jump, start the timer
            if (hasFlip && jumpingTimer <= maxJumpTimeCycles)
            {
                jumpingTimer = 1;
            }
            Vector2D temp = new Vector2D(0, 0);
            bool flippedToRotate = input.mag() > dodgeDeadzone;
            if (flippedToRotate && !isOnGround && distanceToWall > height * 0.75)
                temp = new Vector2D(vertices[1].X - vertices[0].X, vertices[1].Y - vertices[0].Y);
            else
                temp = new Vector2D(vertices[2].X - vertices[1].X, vertices[2].Y - vertices[1].Y);
            //SoundPlayer simpleSound = new SoundPlayer(@"C:\Users\Baze\Desktop\pop-1-35897.wav");
            //simpleSound.Play();
            temp.normalize();
            temp.mult(jumpStrength);
            if (velocity.Y > 0 && input.Y < 0)
                velocity.mult(0);
            velocity.add(temp);
            if (flippedToRotate && !isOnGround && distanceToWall > height * 0.75)
            {
                flipAnimationTimer = animationTime;
                if (input.X < 0) directionOfFlip = -1;
                else directionOfFlip = 1;
            }
            if (!isOnGround)
            {
                jumpingTimer = maxJumpTimeCycles;
            }
        }
        private void CheckFloor(double gravity, double sY, double worldHeight)
        {
            bool oneIsHeigher = false;
            for (int i = 0; i < vertices.Count && isOnGround; i++)
            {
                if (vertices[i].Y < sY + worldHeight)
                {
                    oneIsHeigher = true;
                    break;
                }
            }
            if (oneIsHeigher)
            {
                position.Y += gravity;
                for (int i = 0; i < vertices.Count; i++)
                {
                    vertices[i].Y += gravity;
                }
            }
            bool oneIsLower = false;
            for (int i = 0; i < vertices.Count && isOnGround; i++)
            {
                if (vertices[i].Y < sY + worldHeight)
                {
                    oneIsLower = true;
                    break;
                }
            }
            if (oneIsLower)
            {
                position.Y -= gravity;
                for (int i = 0; i < vertices.Count; i++)
                {
                    vertices[i].Y -= gravity;
                }
            }
        }
        public void reset(double sX, double sY, double W, double H)
        {
            velocity.mult(0);
            position.set(sX + 0.25 * W, sY + 0.95 * H);
            AlignVertices(position, Math.PI);
        }
        public double lerp(double targetValue, double initialValue, double targetFactor)
        {
            return initialValue + targetFactor * (targetValue - initialValue);
        }
        public double mapValue(double input, double inputRangeStart, double inputRangeEnd, double outputRangeStart, double outputRangeEnd)
        {
            double inputPercentage = (input - inputRangeStart) / (inputRangeEnd - inputRangeStart);
            double outputValue = outputRangeStart + inputPercentage * (outputRangeEnd - outputRangeStart);
            return outputValue;
        }
        static int MapRealToInteger(double realNumber)
        {


            // Scale the real number to the range 0 to 1
            double scaledNumber = (realNumber + Math.PI / 2) / Math.PI;

            // Map the scaled number to the range 0 to 16
            int mappedInteger = (int)(scaledNumber * 16);

            return mappedInteger;
        }

        //for debugging purpose
        public String info()
        {
            return $"{angle:0.00}        BOOST:{boostAmount};pos: {position.X:0.000}, {position.Y:0.000}; vel: {velocity.X:0.000}, {velocity.Y:0.000};";
        }
    }
}
