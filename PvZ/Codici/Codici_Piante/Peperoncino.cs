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
            set
            {
                if (value == -999)
                {
                    brucia.Stop();
                    brucia.Close();
                    base.Vita = 0;
                }
            }
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

            rect = new IntRect(144, 3, 293, 239);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Peperoncino.png", rect);
            pianta.Texture = texture;
            pianta.Position += new Vector2f(12, 25);
            pianta.Scale = new Vector2f(0.29f, 0.29f);

            brucia.Start();
            brucia.Elapsed += brucia_Elapsed;
        }

        public Peperoncino()
        {
            costo_soli = 125;
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
                for(int i = 0; i < 11; i++)
                    while (gioco.Mappa_zombie[i, Y].Count > 0)
                        gioco.Mappa_zombie[i, Y][0].Vita = -999;

            new Boom(pianta.Position);
            Vita = -999;
        } 

        public override Peperoncino GetInstace()
        {
            return new Peperoncino();
        }

        public override bool Disponibile
        {
            get => disponibile;
            set => disponibile = value;
        }

        public override void Stop()
        {
            brucia.Stop();
        }
        public override void Start()
        {
            brucia.Start();
        }
    }
}