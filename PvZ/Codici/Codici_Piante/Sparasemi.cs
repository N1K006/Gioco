using System;
using SFML.Graphics;
using SFML.System;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class Sparasemi : Pianta
    {
        // Immagine lista di sparasemi
        public static IntRect l_ss = new IntRect(0, 0, 340, 170);
        public static Texture L_ss = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_Sparasemi.png", l_ss);
        public static readonly Sprite L_SS = new Sprite(L_ss);

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
                    vita = value;
                    if (vita <= 0)
                    {
                        Seme_On.Close();
                        base.Vita = 0;
                    }
                }
            }
        }

        public Sparasemi(int x, int y) : base(x, y)
        {
            vita = 50;
            disponibile = false;
            attesa = new Timer(5000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            gioco.n_soli -= Program.piante_ottenute[0].costo_soli;

            X = x; Y = y;

            Seme_On.Elapsed += Seme_On_Elapsed;
            Seme_On.Enabled = true;

            rect = new IntRect(174, 46, 202, 210);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Sparasemi.png", rect);
            pianta.Texture = texture;
            pianta.Position += new Vector2f(12, 25);
            pianta.Scale = new Vector2f(0.29f, 0.29f);
        }

        public Sparasemi()
        {
            costo_soli = 100;
            costo_monete = 0;
        }

        public bool Spara()
        {
            bool spara = false;
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
            return spara;
        }

        private void Seme_On_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Spara())
            {
                lock (gioco.LockSemi)
                {
                    SemeDefault s = new SemeDefault(new Vector2f(X, Y), 10);
                }
            }
        }

        public override void GetInstace(int x, int y)
        {
            new Sparasemi(x, y);
        }

        public override void DisegnaLista(Vector2f posizione, Vector2f scala)
        {
            L_SS.Position = posizione;
            L_SS.Scale = scala;
                Gioco.Finestra.Draw(L_SS);

        }
        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override Sparasemi GetInstace()
        {
            return new Sparasemi();
        }

        public override bool Disponibile()
        {
            return disponibile;
        }
        public override void Stop()
        {
            Seme_On.Stop();
        }
    }
}