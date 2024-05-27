using System;
using SFML.Graphics;
using SFML.System;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class CiliegeEsplosive : Pianta
    {
        // Immagine lista di sparasemi
        public static IntRect l_ce = new IntRect(0, 0, 340, 170);
        public static Texture L_ce = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_CiliegieEsplosive.png", l_ce);
        public static readonly Sprite L_CE = new Sprite(L_ce);

        public static bool disponibile = true;
        public readonly int X, Y;

        Timer esplodi = new Timer(3000);

        public override int Vita
        {
            get => 1;
            set
            {
                if (value == -999)
                {
                    esplodi.Stop();
                    esplodi.Close();
                    base.Vita = 0;
                }
            }
        }

        public CiliegeEsplosive(int x, int y) : base(x, y)
        {
            disponibile = false;
            attesa = new Timer(35000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            for (int i = 0; i < 8; i++)
                if (gioco.Lista_piante[i] != null)
                    if (gioco.Lista_piante[i] is CiliegeEsplosive)
                    {
                        gioco.n_soli -= gioco.Lista_piante[i].costo_soli;
                        break;
                    }

            X = x; Y = y;

            rect = new IntRect(105, 12, 308, 224);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\CiliegieEsplosive.png", rect);
            pianta.Texture = texture;
            pianta.Position += new Vector2f(12, 25);
            pianta.Scale = new Vector2f(0.29f, 0.29f);

            esplodi.Start();
            esplodi.Elapsed += esplodi_Elapsed;
        }

        public CiliegeEsplosive()
        {
            costo_soli = 150;
            costo_monete = 1000;
        }

        public override void GetInstace(int x, int y)
        {
            new CiliegeEsplosive(x, y);
        }

        public override void DisegnaLista(Vector2f posizione, Vector2f scala)
        {
            L_CE.Position = posizione;
            L_CE.Scale = scala;
            Gioco.Finestra.Draw(L_CE);

        }
        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }
        void esplodi_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Y > 0) // fila sopra
                lock (gioco.LockZombie)
                {
                    while (gioco.Mappa_zombie[X, Y - 1].Count > 0)
                        gioco.Mappa_zombie[X, Y - 1][0].Vita = -999;
                    while (gioco.Mappa_zombie[X + 1, Y - 1].Count > 0)
                        gioco.Mappa_zombie[X + 1, Y - 1][0].Vita = -999;
                    while (gioco.Mappa_zombie[X + 2, Y - 1].Count > 0)
                        gioco.Mappa_zombie[X + 2, Y - 1][0].Vita = -999;
                }

            lock (gioco.LockZombie)
            {
                while (gioco.Mappa_zombie[X, Y].Count > 0)
                    gioco.Mappa_zombie[X, Y][0].Vita = -999;
                while (gioco.Mappa_zombie[X + 1, Y].Count > 0)
                    gioco.Mappa_zombie[X + 1, Y][0].Vita = -999;
                while (gioco.Mappa_zombie[X + 2, Y].Count > 0)
                    gioco.Mappa_zombie[X + 2, Y][0].Vita = -999;
            }

            if (Y < 4) // fila sotto
                lock (gioco.LockZombie)
                {
                    while (gioco.Mappa_zombie[X, Y + 1].Count > 0)
                        gioco.Mappa_zombie[X, Y + 1][0].Vita = -999;
                    while (gioco.Mappa_zombie[X + 1, Y + 1].Count > 0)
                        gioco.Mappa_zombie[X + 1, Y + 1][0].Vita = -999;
                    while (gioco.Mappa_zombie[X + 2, Y + 1].Count > 0)
                        gioco.Mappa_zombie[X + 2, Y + 1][0].Vita = -999;
                }

            new Boom(pianta.Position);
            Vita = -999;
        }

        public override CiliegeEsplosive GetInstace()
        {
            return new CiliegeEsplosive();
        }

        public override bool Disponibile
        {
            get => disponibile;
            set => disponibile = value;
        }
        public override void Stop()
        {
            esplodi.Stop();
        }
        public override void Start()
        {
            esplodi.Start();
        }
    }
}