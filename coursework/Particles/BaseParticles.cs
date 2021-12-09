using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace coursework.Particles
{
    class BaseParticles
    {
        public int Radius; // радиус частицы
        public float X; // X координата положения частицы в пространстве
        public float Y; // Y координата положения частицы в пространстве
        public float Life;

        public float Direction; // направление движения
        public float Speed; // скорость перемещения
        public float SpeedX;
        public float SpeedY;

        public Action<BaseParticles, BaseParticles> OnRelife;
        public BaseParticles(float x, float y)
        {
            X = x;
            Y = y;
        }

        public virtual void Draw(Graphics g)
        {
            var b = new SolidBrush(Color.Black);

            g.FillEllipse(b, X - Radius, Y - Radius, Radius * 2, Radius * 2);


            b.Dispose();
        }

        public virtual void move()
        {
            var directionInRadians = Direction / 180 * Math.PI;
            X += (float)(Speed * Math.Cos(directionInRadians));
            Y -= (float)(Speed * Math.Sin(directionInRadians));
        }

        public virtual void gravityMove(Point gravityPoint )
        {

            float gX = gravityPoint.X - X;
            float gY = gravityPoint.Y - Y;

            // считаем квадрат расстояния между частицей и точкой r^2
            float r2 = gX * gX + gY * gY;
            float M = 5;
            // пересчитываем вектор скорости с учетом притяжения к точке
            SpeedX += (gX) * M / r2;
            SpeedY += (gY) * M / r2;

            // а это старый код, его не трогаем
            /*SpeedX += GravitationX;
            SpeedY += GravitationY;*/
            
            X += SpeedX;
            Y += SpeedY;
        }
        
/*        public virtual void relife()
        {
            if(this.OnRelife != null)
            {
                this.OnRelife(this, this);
            }
        }*/

    }
}
