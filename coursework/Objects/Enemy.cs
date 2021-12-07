using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;


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
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(
                new SolidBrush(Color.DeepSkyBlue),
                -size / 2, -size / 2,
                size, size
            );
            g.DrawEllipse(
                new Pen(Color.Black, 2),
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
                    x += dx * 2;
                    y += dy * 2;
                }
                else
                {
                    x += dx;
                    y += dy;
                }
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
