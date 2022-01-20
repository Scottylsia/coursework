using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Drawing.Drawing2D;

namespace coursework.Particles
{
    class Bullet : BaseParticles
    {
        public float lifeDistance = 500;
        public float startX;
        public float startY;
        public Bullet(float x, float y, float Direction) :  base(x, y)
        {
            startX = x;
            startY = y;
            Speed = 2;
            this.Direction = Direction;
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.DarkRed),
                X - 3, Y - 3, 6, 6);
        }
        public void shot()
        {
            var directionInRadians = Direction / 180 * Math.PI;
            X += (float)(Speed * Math.Cos(directionInRadians));
            Y += (float)(Speed * Math.Sin(directionInRadians));
        }

        public bool alive()
        {
            float dx = X - startX;
            float dy = Y - startY;

            float length = MathF.Sqrt(dx * dx + dy * dy);

            if (length < lifeDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public GraphicsPath GetGraphicsPathBullet()
        {
            var path = new GraphicsPath();
            path.AddRectangle(new Rectangle(-15, -2, 15, 4));
            return path;
        }

        public Matrix GetTransform()
        {
            var matrix = new Matrix();
            matrix.Translate(X, Y);
            matrix.Rotate(Direction);

            return matrix;
        }
    }
}
