using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class Moneta
    {
        public static Gioco gioco;
        bool preso = false;
        private Vector2f mov = default;
        public static List<Moneta> monete = new List<Moneta>();
        public static List<Moneta> monetePrese = new List<Moneta>();
        public int y = 0;
        public int valore = 0;

        static IntRect m = new IntRect(0, 0, 360, 360);
        static Texture M = new Texture(@"..\..\..\Immagini\Mappa\Moneta.png", m);
        public Sprite moneta = new Sprite(M);

        Timer Elimina;
        Timer Muovi = new Timer(10);


        public Moneta(Vector2f pos, int valore)
        {
            this.valore = valore;

            moneta.Scale = new Vector2f(0.11f, 0.11f);
            moneta.Position = pos;

            moneta.Origin = new Vector2f(180, 180);
            lock (gioco.LockMonete)
                monete.Add(this);

            Elimina = new Timer(15000);
            Elimina.Elapsed += Elimina_Elapsed;
            Elimina.Start();

            // posizione e movimento
            {
                Random r = new Random();
                moneta.Position += new Vector2f(r.Next(-10, 50), 30);

                if(moneta.Position.X > 1040)
                    moneta.Position = new Vector2f(1040, moneta.Position.Y);

                y = (int)moneta.Position.Y + r.Next(0, 50);

                Muovi.Elapsed += Muovi_Elapsed;
                Muovi.Start();
            }
        }

        private void Elimina_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (gioco.LockMonete)
                monete.Remove(this);
        }

        private void Muovi_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (preso)
            {
                if (moneta.Position.Y - Gioco.C_M.Position.Y < 8 && moneta.Position.X - Gioco.C_M.Position.X < 8)
                    monetePrese.Remove(this);
                float x = moneta.Position.X - Gioco.C_M.Position.X;
                float y = moneta.Position.Y - Gioco.C_M.Position.Y;
                mov = new Vector2f(x / Convert.ToSingle(Math.Sqrt(x * x + y * y)), y / Convert.ToSingle(Math.Sqrt(x * x + y * y)));
                moneta.Position -= mov * 7;
            }
            else
            {
                if (moneta.Position.Y >= y)
                    moneta.Position = new Vector2f(moneta.Position.X, y);
                else
                {
                    moneta.Position += new Vector2f(0, 1);
                    moneta.Position = moneta.Position;
                }
            }
            moneta.Rotation += 0.2f;
        }

        public void Preso()
        {
            lock (gioco.LockMonete)
            {
                preso = true;
                Program.monete += valore;
                monete.Remove(this);
                monetePrese.Add(this);
            }
        }
        public void Stop()
        {
            Muovi.Stop();
            Elimina.Stop();
            monete.Remove(this);
        }
    }
}