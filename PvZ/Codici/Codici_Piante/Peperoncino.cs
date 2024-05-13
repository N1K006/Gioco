using System;
using SFML.Graphics;
using SFML.System;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class Peperoncino : Pianta
    {
        // Immagine lista di sparasemi
        public static IntRect l_p = new IntRect(0, 0, 340, 170);
        public static Texture L_p = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_Peperoncino.png", l_p);
        public static readonly Sprite L_P = new Sprite(L_p);

        public static bool disponibile = true;
        public readonly int X, Y;

        Timer brucia = new Timer(3000);

        public override int Vita
        {
            get => 1;
            set => vita = 1;
        }

        public Peperoncino(int x, int y) : base(x, y)
        {
            disponibile = false;
            attesa = new Timer(35000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            for (int i = 0; i < 8; i++)
                if (gioco.Lista_piante[i] != null)
                    if (gioco.Lista_piante[i] is Peperoncino)
                    {
                        gioco.n_soli -= gioco.Lista_piante[i].costo_soli;
                        break;
                    }

            X = x; Y = y;

            rect = new IntRect(174, 46, 202, 210);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Peperoncino.png", rect);
            pianta.Texture = texture;
            pianta.Position += new Vector2f(12, 25);
            pianta.Scale = new Vector2f(0.29f, 0.29f);

            brucia.Start();
            brucia.Elapsed += brucia_Elapsed;
        }

        public Peperoncino()
        {
            costo_soli = 150;
            costo_monete = 1150;
        }

        public override void GetInstace(int x, int y)
        {
            new Peperoncino(x, y);
        }

        public override void DisegnaLista(Vector2f posizione, Vector2f scala)
        {
            L_P.Position = posizione;
            L_P.Scale = scala;
                Gioco.Finestra.Draw(L_P);

        }
        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }
        void brucia_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (gioco.LockZombie)
            {
                for (int i = 0; i < gioco.Mappa_zombie[Y].Count; i++)
                    gioco.Mappa_zombie[Y][i].Vita = 0;
            }

            base.Vita = 0;
        }

        public override Peperoncino GetInstace()
        {
            return new Peperoncino();
        }

        public override bool Disponibile()
        {
            return disponibile;
        }
        public override void Stop()
        {
            brucia.Stop();
        }
    }
}