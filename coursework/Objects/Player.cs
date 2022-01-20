using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using coursework.Particles;


namespace coursework.Objects
{

    class Player : BaseObject
    {
        public Action<Marker> OnMarkerOverlap;

        public Action<Enemy> OnEnemyOverlap;

        public Action<Enemy> OnHit;

        public Action<Treatment> OnTreatment;

        public float vX, vY;
    
        public Bullet bullet = null;

        public Emitter emitter;

        public int bulletSpeed = 22;
        public Player(float x, float y, float angle) : base(x, y, angle)
        {
            int offset = 40;
            emitter = new Emitter
            {

                GravitationY = 0, // отключил гравитацию
                Direction = 0, // направление 0
                Spreading = 30, // немного разбрасываю частицы, чтобы было интереснее
                SpeedMin = 10, // минимальная скорость 10
                SpeedMax = 10, // и максимальная скорость 10
                ColorFrom = Color.GreenYellow, // цвет начальный
                ColorTo = Color.Green, // цвет конечный
                ParticlesPerTick = 10, // 3 частицы за тик генерю
                X = x, // x -- по центру экрана
                Y = y - offset, // y поднят вверх на offset
                emitterLife = 100
            };

            emitter.impactPoints.Add(new GravityPoint
            {
                Power = (int)Math.Pow((emitter.SpeedMax + emitter.SpeedMin) / 2, 2),
                X = emitter.X,
                Y = emitter.Y + offset,
            });
            
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
            g.DrawLine(new Pen(Color.Black, 2), 0, 0, 25, 0);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-size / 2, -size / 2, size, size);
            return path;
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);

            if (obj is Marker)
            {
                OnMarkerOverlap(obj as Marker);
            }
            if (obj is Enemy)
            {
                OnEnemyOverlap(obj as Enemy);
            }
            if(obj is Treatment)
            {
                OnTreatment(obj as Treatment);
            }
        }

        public override void renderParticles(Graphics g)
        {
            if (bullet != null)
                bullet.Draw(g);

            if (emitter.emitterLife > 0)
            {
                emitter.UpdateState();
                emitter.Render(g);
                emitter.emitterLife--;
            }
        }

        public bool shot()
        {
            if (bullet == null)
            {
                bullet = new Bullet(x, y, angle);
            }
            if (!bullet.alive())
            {
                for (int i = 0; i < bulletSpeed; i++)

                    bullet.shot();
                bullet = null;
                return false;
            }
            else
            {
                for (int i = 0; i < bulletSpeed; i++)

                    bullet.shot();

                return true;
            }
        }

        public GraphicsPath GetGraphicsPathBullet()
        {
            var path = base.GetGraphicsPath();
            var rec = new Rectangle(-3, -3, 6, 6);
            path.AddRectangle(rec);
            return path;
        }

        public bool hits(BaseObject obj, Graphics g)
        {
            // берем информацию о форме
            var path1 = bullet.GetGraphicsPathBullet();
            var path2 = obj.GetGraphicsPath();

            // применяем к объектам матрицы трансформации
            path1.Transform(bullet.GetTransform());
            path2.Transform(obj.GetTransform());

            // используем класс Region, который позволяет определить 
            // пересечение объектов в данном графическом контексте
            var region = new Region(path1);
            region.Intersect(path2); // пересекаем формы
            return !region.IsEmpty(g); // если полученная форма не пуста то значит было
        }
        public void hit(BaseObject obj)
        {
            if (obj is Enemy)
            {
                this.OnHit(obj as Enemy);
            }
        }

        
    }
}
