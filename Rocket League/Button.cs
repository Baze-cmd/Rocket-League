using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket_League
{
    public class Button
    {
        public Vector2D position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public String text { get; set; }
        public bool isOn { get; set; }
        public Button(Vector2D pos, int W, int H, bool isON, String txt)
        {
            position = new Vector2D(pos);
            Width = W;
            Height = H;
            isOn = isON;
            text = txt;
        }
        public void draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.White), new Rectangle((int)position.X, (int)position.Y, Width, Height));
            g.DrawRectangle(new Pen(Color.Black), new Rectangle((int)position.X, (int)position.Y, Width, Height));
            Font f = new Font("Arial", 16);
            Brush b = new SolidBrush(Color.Black);
            g.DrawString(text, f, b, new Point((int)position.X + 10, (int)position.Y + 12));
            b.Dispose();
            f.Dispose();
        }

        public bool click(Point p)
        {
            if (position.X <= p.X && p.X <= position.X + Width && position.Y <= p.Y && p.Y <= position.Y + Height)
            {
                isOn = !isOn;
                return true;
            }
            return false;
        }
    }
}
