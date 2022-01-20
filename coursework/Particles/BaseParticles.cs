using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace coursework.Particles
{
    public class BaseParticles
    {
        public float X;
        public float Y;

        public int Radius;

        public float SpeedX; // скорость перемещения
        public float SpeedY; // скорость перемещения

        public float Life;

        public float Speed;
        public float Direction;

        public static Random rnd = new Random();

        public BaseParticles(float x, float y)
        {
            X = x;
            Y = y;
        }

        public BaseParticles()
        {

        }

        public virtual void Draw(Graphics g)
        {

            var b = new SolidBrush(Color.Red);

            g.FillEllipse(b, X - Radius, Y - Radius, Radius * 2, Radius * 2);

            b.Dispose();
        }


    }
}
