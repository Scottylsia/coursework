using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using coursework.Particles;


namespace coursework.Objects
{
    public class Enemy : BaseObject
    {
        public float Mx;
        public float My;
        bool findEnemy = false;
        List<ParticleOfFire> particleOfFires = new List<ParticleOfFire>();
        Point gravityPoint;
        ParticleOfFire pof;
        public Enemy(float x, float y, float angle):base(x,y,angle)
        {
           
            Mx = x;
            My = y;

            gravityPoint = new Point((int)x, (int)y+10);

            Random rnd = new Random();
            for (var i = 0; i < 50; i++)
            {
                particleOfFires.Add(new ParticleOfFire(x + 4 - rnd.Next(8), y + 4 - rnd.Next(8)));
                particleOfFires[i].onRelifefire += (rf) =>
                 {
                     particleOfFires.Remove(rf);
                     particleOfFires.Add(new ParticleOfFire(x + 4 - rnd.Next(8), y + 4 - rnd.Next(8)));
                 };
            }



/*            pof.onRelifefire += (rf) =>
            {
                particleOfFires.Remove(rf);
                particleOfFires.Add(new ParticleOfFire(x + 4 - rnd.Next(8), y + 4 - rnd.Next(8)));
            };*/


        }

        public override void Render(Graphics g)
        {

              
            g.FillEllipse(
                new SolidBrush(Color.DarkRed),
                -size / 2, -size / 2,
                size, size
            );
            g.DrawEllipse(
                new Pen(Color.Black, 2),
                -size / 2, -size / 2,
                size, size
            );

        }

        public override void renderParticles(Graphics g)
        {
            foreach (var gp in particleOfFires)
            {
                gp.Life--;
                if(gp.Life<0)
                {
                    Random rnd = new Random();
                    gp.X = x + 4 - rnd.Next(8);
                    gp.Y = y + 4 - rnd.Next(8);
                    gp.Life = 10 + rnd.Next(20);
                    gp.Radius = 1 + rnd.Next() % 5;
                    gp.SpeedY = 2 - rnd.Next() % 4;
                    gp.SpeedX = 2 - rnd.Next() % 4;
                }

                gp.gravityMove(gravityPoint);

                gp.Draw(g);
            }
        }


        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-size / 2, -size / 2, size, size);
            return path;
        }

        public GraphicsPath GetGraphicPathMarker()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-3, -3, 3, 3);
            return path;
        }

        public Matrix GetTransformMarker()
        {
            var matrix = new Matrix();
            matrix.Translate(Mx, My);
            matrix.Rotate(angle);

            return matrix;
        }
        public override void artificialIntelligence(BaseObject obj,Graphics g)
        {
           if(obj is Player)
            {
                float dx = obj.x - x;
                float dy = obj.y - y;

                float length = MathF.Sqrt(dx * dx + dy * dy);

                if(length <200)
                {
                    Mx = obj.x;
                    My = obj.y;
                    findEnemy = true;
                }
                else
                {
                    findEnemy = false;
                }
            }
           var path1 = GetGraphicsPath();
            var path2 = GetGraphicPathMarker();

            // применяем к объектам матрицы трансформации
            path1.Transform(GetTransform());
            path2.Transform(GetTransformMarker());

            // используем класс Region, который позволяет определить 
            // пересечение объектов в данном графическом контексте
            var region = new Region(path1);
            region.Intersect(path2); // пересекаем формы
            if(!region.IsEmpty(g))
            {
                Random rnd = new Random();
                Mx = rnd.Next() % 1000;
                My = rnd.Next() % 750;

            }

            {
                float dx = Mx - x;
                float dy = My - y;

                float length = MathF.Sqrt(dx * dx + dy * dy);

                dx /= length;
                dy /= length;

                if (findEnemy)
                {
                     dx *= 2;
                     dy *= 2;
                }

                    x += dx;
                    y += dy;
                gravityPoint.X = (int)(x - dx);
                gravityPoint.Y = (int)(y - dy - 10);
            }

        }

        public override bool Overlaps(BaseObject obj, Graphics g)
        {
            var path1 = GetGraphicsPath();
            var path2 = GetGraphicPathMarker();

            // применяем к объектам матрицы трансформации
            path1.Transform(GetTransform());
            path2.Transform(GetTransformMarker());

            // используем класс Region, который позволяет определить 
            // пересечение объектов в данном графическом контексте
            var region = new Region(path1);
            region.Intersect(path2); // пересекаем формы
            return !region.IsEmpty(g);
        }

        
    }
}
