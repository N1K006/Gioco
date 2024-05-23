using System;
using SFML.Graphics;
using SFML.System;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class Supersparasemi : Pianta
    {
        // Immagine lista di sparasemi
        public static IntRect l_sup = new IntRect(0, 0, 340, 170);
        public static Texture L_sup = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_Supersparasemi.png", l_sup);
        public static readonly Sprite L_SUP = new Sprite(L_sup);

        public static bool disponibile = true;
        public readonly int X, Y;
        public Timer Seme_On = new Timer(750);

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

        public Supersparasemi(int x, int y) : base(x, y)
        {
            vita = 50;
            disponibile = false;
            attesa = new Timer(5000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            for (int i = 0; i < 8; i++)
                if (gioco.Lista_piante[i] != null)
                    if (gioco.Lista_piante[i] is Supersparasemi)
                    {
                        gioco.n_soli -= gioco.Lista_piante[i].costo_soli;
                        break;
                    }

            X = x; Y = y;

            Seme_On.Elapsed += Seme_On_Elapsed;
            Seme_On.Enabled = true;

            rect = new IntRect(106, 10, 203, 212);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Supersparasemi.png", rect);
            pianta.Texture = texture;
            pianta.Position += new Vector2f(12, 40);
            pianta.Scale = new Vector2f(0.31f, 0.31f);
        }

        public Supersparasemi()
        {
            costo_soli = 200;
            costo_monete = 300;
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
            new Supersparasemi(x, y);
        }

        public override void DisegnaLista(Vector2f posizione, Vector2f scala)
        {
            L_SUP.Position = posizione;
            L_SUP.Scale = scala;
            Gioco.Finestra.Draw(L_SUP);

        }
        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override Supersparasemi GetInstace()
        {
            return new Supersparasemi();
        }

        public override bool Disponibile
        {
            get => disponibile;
            set => disponibile = value;
        }
        public override void Stop()
        {
            Seme_On.Stop();
        }
    }
}