using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class Sole
    {
        public static Gioco gioco;
        bool preso = false;
        private Vector2f mov = default;
        public static List<Sole> soli = new List<Sole>();
        public static List<Sole> soliPresi = new List<Sole>();
        public Vector2f pos;
        public int y = 0;
        public int value_sun = 0;

        static IntRect s = new IntRect(0, 0, 360, 360);
        static Texture S = new Texture(@"..\..\..\Immagini\Piante\Sole.png", s);
        public Sprite sole = new Sprite(S);

        Timer Sun_Off;
        Timer Mov_Sun = new Timer(10);


        public Sole() // MAPPA
        {
            Dimensione_Sole();
            sole.Origin = new Vector2f(180, 180);
            soli.Add(this);

            Sun_Off = new Timer(15000);
            Sun_Off.Elapsed += Sun_Off_Elapsed;
            Sun_Off.Start();

            // posizione e movimento
            {
                Random r = new Random();
                pos.X = r.Next(285, 941);
                pos.Y = -65;

                y = r.Next(130, 520);

                Mov_Sun.Elapsed += Mov_Sun_Elapsed; ;
                Mov_Sun.Start();
            }
        }

        public Sole(Vector2f position, int sender /* GIRASOLE (1) O GIRASOLEGEMELLO (2)*/)
        {
            Dimensione_Sole();
            sole.Origin = new Vector2f(180, 180);
            soli.Add(this);

            Sun_Off = new Timer(10000);
            Sun_Off.Elapsed += Sun_Off_Elapsed;
            Sun_Off.Start();

            switch (sender)
            {
                case 1:
                    pos = position;
                    pos.X += 80;
                    pos.Y += 22;
                    y = (int)pos.Y + 22;
                    break;
                case 2:
                    pos = position;
                    Random x = new Random();
                    pos.X = x.Next((int)pos.X - 15, (int)pos.X + 70);

                    pos.Y += 30;
                    Random y_ = new Random();
                    pos.Y = y_.Next((int)pos.Y - 10, (int)pos.Y + 10);
                    y = (int)pos.Y + 25;
                    break;
            }
            Mov_Sun.Elapsed += Mov_Sun_Elapsed; ;
            Mov_Sun.Start();
        }

        private void Sun_Off_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (gioco.LockSoli)
                soli.Remove(this);
        }

        private void Mov_Sun_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (preso)
            {
                if (sole.Position.Y - Gioco.C_S.Position.Y < 8 && sole.Position.X - Gioco.C_S.Position.X < 8)
                    soliPresi.Remove(this);
                float x = sole.Position.X - Gioco.C_S.Position.X;
                float y = sole.Position.Y - Gioco.C_S.Position.Y;
                mov = new Vector2f(x / Convert.ToSingle(Math.Sqrt(x * x + y * y)), y / Convert.ToSingle(Math.Sqrt(x * x + y * y)));
                sole.Position -= mov * 14;
            }
            else
            {
                if (pos.Y >= y)
                    pos.Y = y;
                else
                {
                    pos.Y += 1;
                    sole.Position = pos;
                }
            }
            sole.Rotation += 0.7f;
        }

        public void Dimensione_Sole()
        {
            sole.Scale = new Vector2f(0.18f, 0.18f);
            sole.Position = new Vector2f(0, -200);
        }

        public void Preso()
        {
            preso = true;
            gioco.n_soli += value_sun;
            soli.Remove(this);
            soliPresi.Add(this);
        }

        public void Stop()
        {
            Sun_Off.Stop();
            Mov_Sun.Stop();
            soli.Remove(this);
        }
    }
}