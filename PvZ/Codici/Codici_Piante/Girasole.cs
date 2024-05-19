using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class Girasole : Pianta
    {
        // Immagine lista di girasole
        public static IntRect l_g = new IntRect(0, 0, 340, 170);
        public static Texture L_g = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_Girasole.png", l_g);
        public static readonly Sprite L_G = new Sprite(L_g);

        public static bool disponibile = true;
        public readonly int X, Y;
        public Timer Sun_On = new Timer(12000);

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
                        Sun_On.Close();
                        base.Vita = vita;
                    }
                }
            }
        }

        public Girasole(int x, int y) : base(x, y)
        {
            vita = 50;
            disponibile = false;
            attesa = new Timer(5000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            for (int i = 0; i < 8; i++)
                if (gioco.Lista_piante[i] != null)
                    if (gioco.Lista_piante[i] is Girasole)
                    {
                        gioco.n_soli -= gioco.Lista_piante[i].costo_soli;
                        break;
                    }

            X = x; Y = y;

            Sun_On.Elapsed += Sun_On_Elapsed;
            Sun_On.Enabled = true;

            rect = new IntRect(117, 9, 187, 212);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Girasole.png", rect);
            pianta.Texture = texture;
            pianta.Position += new Vector2f(15, 25);
            pianta.Scale = new Vector2f(0.3f, 0.3f);
        }

        public Girasole()
        {
            costo_soli = 50;
            costo_monete = 0;
        }

        // Fa comparire un nuovo sole
        private void Sun_On_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (gioco.LockSoli)
            {
                Sole s = new Sole(pianta.Position, 1)
                {
                    value_sun = 25
                };
            }
        }

        public override void GetInstace(int x, int y)
        {
            new Girasole(x, y);
        }

        public override void DisegnaLista(Vector2f posizione, Vector2f scala)
        {
            L_G.Position = posizione;
            L_G.Scale = scala;
            Gioco.Finestra.Draw(L_G);
        }

        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override Girasole GetInstace()
        {
            return new Girasole();
        }

        public override bool Disponibile
        {
            get => disponibile;
            set => disponibile = value;
        }

        public override void Stop()
        {
            Sun_On.Stop();
        }
    }
}
