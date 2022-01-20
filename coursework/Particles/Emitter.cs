using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace coursework.Particles
{
    public class Emitter
    {
        public List<IImpactPoint> impactPoints = new List<IImpactPoint>();

        public List<BaseParticles> particles = new List<BaseParticles>();

        public float GravitationX = 0;
        public float GravitationY = 1;

        public float X;
        public float Y;

        public int Direction = 0; // вектор направления в градусах куда сыпет эмиттер
        public int Spreading = 360; // разброс частиц относительно Direction
        public int SpeedMin = 1; // начальная минимальная скорость движения частицы
        public int SpeedMax = 10; // начальная максимальная скорость движения частицы
        public int RadiusMin = 2; // минимальный радиус частицы
        public int RadiusMax = 10; // максимальный радиус частицы
        public int LifeMin = 10; // минимальное время жизни частицы
        public int LifeMax = 100; // максимальное время жизни частицы

        public Color ColorFrom = Color.Orange;
        public Color ColorTo = Color.FromArgb(0, Color.Red);

        public int ParticlesPerTick = 10;


        public int ParticlesCount = 500;

        public int emitterLife =0;

        public void UpdateState()
        {
            int particlesToCreate = ParticlesPerTick;

            foreach (var particle in particles)
            {
                particle.Life -= 1; // уменьшаю здоровье
                                    // если здоровье кончилось
                if (particle.Life < 0)
                {
                    ResetParticle(particle);

                    if (particlesToCreate > 0)
                    {
                        /* у нас как сброс частицы равносилен созданию частицы */
                        particlesToCreate -= 1; // поэтому уменьшаем счётчик созданных частиц на 1
                        ResetParticle(particle);
                    }
                }
                else
                {
                    foreach (var point in impactPoints)
                    {
                        float gX = point.X - particle.X;
                        float gY = point.Y - particle.Y;
                        float r2 = gX * gX + gY * gY;
                        float M = 100;

                        particle.SpeedX += (gX) * M / r2;
                        particle.SpeedY += (gY) * M / r2;
                    }

                    particle.SpeedX += GravitationX;
                    particle.SpeedY += GravitationY;

                    particle.X += particle.SpeedX;
                    particle.Y += particle.SpeedY;
                }
            }
            if(particles.Count < ParticlesCount)
            while (particlesToCreate >= 1)
            {
                particlesToCreate -= 1;
                var particle = CreateParticle();
                ResetParticle(particle);
                particles.Add(particle);
            }
        }

        public void Render(Graphics g)
        {
            // утащили сюда отрисовку частиц
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }
        }

        public virtual void ResetParticle(BaseParticles particle)
        {
            particle.Life = BaseParticles.rnd.Next(LifeMin, LifeMax);

            particle.X = X;
            particle.Y = Y;

            var direction = Direction
                + (double)BaseParticles.rnd.Next(Spreading)
                - Spreading / 2;

            var speed = BaseParticles.rnd.Next(SpeedMin, SpeedMax);

            particle.SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed);
            particle.SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed);

            particle.Radius = BaseParticles.rnd.Next(RadiusMin, RadiusMax);
        }

        public virtual BaseParticles CreateParticle()
        {
            var particle = new ParticleColorful()
            {
                MaxLife = this.LifeMax
            };
            particle.FromColor = ColorFrom;
            particle.ToColor = ColorTo;

            return particle;
        }
    }
}
