﻿using System;
using System.Collections.Generic;
using System.Text;

namespace coursework.Particles
{
    public class AntiGravityPoint : IImpactPoint
    {
        public int Power = 100; // сила отторжения


        // а сюда по сути скопировали с минимальными правками то что было в UpdateState
        public override void ImpactParticle(BaseParticles particle)
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;
            float r2 = (float)Math.Max(100, gX * gX + gY * gY);

            particle.SpeedX -= gX * Power / r2; // тут минусики вместо плюсов
            particle.SpeedY -= gY * Power / r2; // и тут
        }
    }
}
