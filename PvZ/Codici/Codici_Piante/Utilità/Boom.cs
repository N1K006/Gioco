using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class Boom
    {
        // Immagine scritta boom 
        static IntRect boom = new IntRect(13, 12, 737, 458);
        static Texture _Boom = new Texture(@"..\..\..\Immagini\Mappa\Boom.png", boom);
        public Sprite BOOM = new Sprite(_Boom);

        public static List<Boom> esplosioni = new List<Boom>();
        public Timer stop_boom;
        public Timer _boom;

        public Boom(Vector2f position)
        {
            esplosioni.Add(this);
            BOOM.Origin = new Vector2f(160, 40);
            BOOM.Scale = new Vector2f(0.10f, 0.10f);
            BOOM.Position += new Vector2f(10, 15);
            BOOM.Position = position;

            stop_boom = new Timer(1400);
            stop_boom.Elapsed += Stop_boom_Elapsed;
            stop_boom.Start();

            _boom = new Timer(10);
            _boom.Elapsed += _boom_Elapsed;
            _boom.Start();
        }

        private void _boom_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (BOOM.Scale.X < 0.20f && BOOM.Scale.Y < 0.20f)
                BOOM.Scale += new Vector2f(0.005f, 0.005f); 
            else
            {
                _boom.Stop();
                _boom.Close();
            }
            BOOM.Rotation += 0.3f;
        }

        private void Stop_boom_Elapsed(object sender, ElapsedEventArgs e)
        {
            _boom.Stop();
            _boom.Close();
            stop_boom.Stop();
            stop_boom.Close();
            esplosioni.Remove(this);
        }
    }
}