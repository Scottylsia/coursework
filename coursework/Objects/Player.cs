using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace coursework.Objects
{

    class Player : BaseObject
    {
        public Action<Marker> OnMarkerOverlap;

        public Action<Enemy> OnEnemyOverlap;

        public float vX, vY;
        public Player(float x, float y, float angle) : base(x, y, angle)
        {

        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(
                new SolidBrush(Color.DeepPink),
                -size / 2, -size / 2,
                size, size
            );
            g.DrawEllipse(
                new Pen(Color.Black, 2),
                -size / 2, -size / 2,
                size, size
            );
            g.DrawLine(new Pen(Color.Black, 2), 0, 0, 25, 0);

        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-size/2, -size/2, size, size);
            return path;
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);

            if (obj is Marker)
            {
                OnMarkerOverlap(obj as Marker);
            }
            if(obj is Enemy)
            {
                OnEnemyOverlap(obj as Enemy);
            }
        }
    }
}
