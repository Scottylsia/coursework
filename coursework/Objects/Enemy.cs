using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using coursework.Particles;
using coursework;


namespace coursework.Objects
{
    public class Enemy : BaseObject
    {
        public float Mx;
        public float My;
        bool findEnemy = false;
        
        public Enemy(float x, float y, float angle):base(x,y,angle)
        {
           
            Mx = x;
            My = y;

            Random rnd = new Random();

            Emitter emitter = new Emitter
            {
                GravitationY = 0, // отключил гравитацию
                Direction = 0, // направление 0
                Spreading = 360, // немного разбрасываю частицы, чтобы было интереснее
                SpeedMin = 5, // минимальная скорость 10
                SpeedMax = 10, // и максимальная скорость 10
                ColorFrom = Color.FromArgb(255, 251, 95), // цвет начальный
                ColorTo = Color.Red, // цвет конечный
                ParticlesPerTick = 5, // 3 частицы за тик генерю
                ParticlesCount = 50,
                LifeMin = 5,
                LifeMax = 10,
                X = x, 
                Y = y, 
            };
            emitter.impactPoints.Add(new GravityPoint
            {
                Power = 5,
                X = x,
                Y = y - 10

            }); 
            emitters.Add(emitter);
        }

        public override void Render(Graphics g)
        {

              
            g.FillEllipse(
                new SolidBrush(Color.FromArgb(210, 105, 30)),
                -size / 2, -size / 2,
                size, size
            );
            g.DrawEllipse(
                new Pen(Color.FromArgb(128, 0, 0), 2),
                -size / 2, -size / 2,
                size, size
            );
            
            
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
                foreach (var em in emitters)
                {
                    em.X = x;
                    em.Y = y;
                    foreach(var gp in em.impactPoints)
                    {
                        gp.X = (int)(x);
                        gp.Y = (int)(y -  10);
                    }
                }
                
               // gravityPoint.X = (int)(x - dx);
                //gravityPoint.Y = (int)(y - dy - 10);
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
