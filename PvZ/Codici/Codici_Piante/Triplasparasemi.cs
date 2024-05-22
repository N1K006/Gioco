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
    class Triplasparasemi : Pianta, IDisposable
    {
        // Immagine lista di Triplasparasemi
        public static IntRect l_ts = new IntRect(0, 0, 340, 170);
        public static Texture L_ts = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_Triplasparasemi.png", l_ts);
        public static readonly Sprite L_TS = new Sprite(L_ts);

        public static bool disponibile = true;
        public readonly int X, Y;
        public Timer Seme_On = new Timer(1500);

        public override int Vita
        {
            get
            {
                lock (LockVita)
                    return vita;
            }
            set
            {
                lock (LockVita)
                {
                    Seme_On.Close();
                    base.Vita = 0;
                }
            }
        }

        public Triplasparasemi(int x, int y) : base(x, y)
        {
            vita = 50;
            disponibile = false;
            attesa = new Timer(5000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            for (int i = 0; i < 8; i++)
                if (gioco.Lista_piante[i] != null)
                    if (gioco.Lista_piante[i] is Triplasparasemi)
                    {
                        gioco.n_soli -= gioco.Lista_piante[i].costo_soli;
                        break;
                    }

            X = x; Y = y;

            Seme_On.Elapsed += Seme_On_Elapsed;
            Seme_On.Enabled = true;

            rect = new IntRect(102, 14, 309, 264);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Triplasparasemi.png", rect);
            pianta.Texture = texture;
            pianta.Position += new Vector2f(0, 25);
            pianta.Scale = new Vector2f(0.29f, 0.29f);
        }
        public Triplasparasemi()
        {
            costo_soli = 300;
            costo_monete = /*58*/0;
        }

        public bool Spara()
        {
            bool spara = false;
            lock (gioco.LockZombie)
            {
                if (Y > 0)
                    for (int i = 0; i < gioco.Mappa_zombie[Y - 1].Count; i++)
                    {
                        Zombie z = gioco.Mappa_zombie[Y - 1][i];
                        if (z != null)
                            if (z.sprite.Position.X >= pianta.Position.X)
                                spara = true;
                    }
            }
            lock (gioco.LockZombie)
            {
                for (int i = 0; i < gioco.Mappa_zombie[Y].Count; i++)
                {
                    Zombie z = gioco.Mappa_zombie[Y][i];
                    if (z != null)
                        if (z.sprite.Position.X >= pianta.Position.X)
                            spara = true;
                }
            }
            lock (gioco.LockZombie)
            {
                if (Y < 4)
                    for (int i = 0; i < gioco.Mappa_zombie[Y + 1].Count; i++)
                    {
                        Zombie z = gioco.Mappa_zombie[Y + 1][i];
                        if (z != null)
                            if (z.sprite.Position.X >= pianta.Position.X)
                                spara = true;
                    }
            }
            return spara;
        }

        private void Seme_On_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Spara())
            {
                if (Y > 0)
                    lock (gioco.LockSemi)
                    {
                        SemeDefault s1 = new SemeDefault(new Vector2f(X, Y - 1), 10);
                    }
                lock (gioco.LockSemi)
                {
                    SemeDefault s2 = new SemeDefault(new Vector2f(X, Y), 10);
                }
                if (Y < 5)
                    lock (gioco.LockSemi)
                    {
                        SemeDefault s3 = new SemeDefault(new Vector2f(X, Y + 1), 10);
                    }
            }
        }

        public override void GetInstace(int x, int y)
        {
            new Triplasparasemi(x, y);
        }

        public override void DisegnaLista(Vector2f posizione, Vector2f scala)
        {
            L_TS.Position = posizione;
            L_TS.Scale = scala;
            Gioco.Finestra.Draw(L_TS);
        }
        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override Triplasparasemi GetInstace()
        {
            return new Triplasparasemi();
        }

        public override bool Disponibile
        {
            get => disponibile;
            set => disponibile = value;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            Seme_On.Stop();
        }
    }
}
