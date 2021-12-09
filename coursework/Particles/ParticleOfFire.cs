using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace coursework.Particles
{
    class ParticleOfFire : BaseParticles
    {
        public Action<ParticleOfFire> onRelifefire;
        public ParticleOfFire(float x, float y):base(x,y)
        {
            Random rnd = new Random();
            Life = 10 + rnd.Next(20);
            Radius = 1 + rnd.Next() % 5;
            SpeedY = 2 - rnd.Next() % 4;
            SpeedX = 2 - rnd.Next() % 4;

        }

        public static Color MixColor(Color color1, Color color2, float k)
        {
            return Color.FromArgb(
                (int)(color2.A * k + color1.A * (1 - k)),
                (int)(color2.R * k + color1.R * (1 - k)),
                (int)(color2.G * k + color1.G * (1 - k)),
                (int)(color2.B * k + color1.B * (1 - k))
            );
        }
        public override void Draw(Graphics g)
        {
            float k = Math.Min(1f, Life / 30);

            // так как k уменьшается от 1 до 0, то порядок цветов обратный
            var color = MixColor(Color.FromArgb(255,251,95), Color.Red, k);
            var b = new SolidBrush(color);

            g.FillEllipse(b, X - Radius, Y - Radius, Radius * 2, Radius * 2);

            b.Dispose();
        }

    }
}
