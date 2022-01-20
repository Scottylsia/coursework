using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace coursework.Objects
{
    class Treatment : BaseObject
    {
        public Treatment(float x, float y, float angle) : base(x, y, angle)
        {
        }

        public override GraphicsPath GetGraphicsPath()
        {

            var path = base.GetGraphicsPath();
            var rec = new Rectangle(-10, -10, 20, 20);
            path.AddRectangle(rec);
            return path;
        }

        public override void Render(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.White), -10, -10, 20, 20);
            g.DrawRectangle(new Pen(Color.Black), -10, -10, 20, 20);
            g.FillRectangle(new SolidBrush(Color.Red), -2, -8, 4, 16);
            g.FillRectangle(new SolidBrush(Color.Red), -8, -2, 16, 4);
        }
    }
}
