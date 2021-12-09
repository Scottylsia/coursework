using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using coursework.Objects;
using coursework.Particles;

namespace coursework
{

    public partial class Form1 : Form
    {
        List<BaseObject> objects = new List<BaseObject>();
       // List<BaseParticles> particles = new List<BaseParticles>();
        Player player;
        Marker marker;
        int hitPoint = 10;
        List<ParticleOfFire> particleOfFires = new List<ParticleOfFire>();
        Point gravityPoint;

        public Form1()
        {
            InitializeComponent();
            Random rnd = new Random();
            player = new Player(field.Width / 2, field.Height / 2, 0);

            player.OnOverlap += (p, obj) =>
            {

            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            player.OnEnemyOverlap += (e) =>
            {
                hitPoint--;
                objects.Remove(e);
                objects.Add(new Enemy(rnd.Next() % field.Width, rnd.Next() % field.Height, 0));
            };

            marker = new Marker(field.Width / 2 + 50, field.Height / 2 + 50, 0);

            objects.Add(player);
            objects.Add(marker);
            objects.Add(new Enemy(rnd.Next() % field.Width, rnd.Next() % field.Height, 0));
            objects.Add(new Enemy(rnd.Next() % field.Width, rnd.Next() % field.Height, 0));
            objects.Add(new Enemy(rnd.Next() % field.Width, rnd.Next() % field.Height, 0));
            objects.Add(new Enemy(rnd.Next() % field.Width, rnd.Next() % field.Height, 0));


            gravityPoint = new Point(10, 10);
            for (var i = 0; i < 20; i++)
            {
                particleOfFires.Add(new ParticleOfFire(0, 0));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void field_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);

            updatePlayer();

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
                if (obj is Enemy)
                {
                    obj.artificialIntelligence(player, g);
                    obj.renderParticles(g);
                }
            }
            foreach (var obj in objects)
            {

                g.Transform = obj.GetTransform();
                obj.Render(g);
 

            }

        }

        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.x - player.x;
                float dy = marker.y - player.y;

                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                /*                player.x += dx * 2;
                               // player.y += dy * 2;*/

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                player.angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;


            player.x += player.vX;
            player.y += player.vY;


        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            field.Invalidate();
        }

        private void field_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker);
            }
            marker.x = e.X;
            marker.y = e.Y;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {

            }
        }
    }
}
