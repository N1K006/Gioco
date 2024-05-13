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
    class Rovo : Pianta
    {
        // Immagine lista di Rovo
        public static IntRect l_ro = new IntRect(0, 0, 340, 170);
        public static Texture L_ro = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_Rovo.png", l_ro);
        public static readonly Sprite L_RO = new Sprite(L_ro);

        public static bool disponibile = true;
        public int danno = 3;
        private readonly int X, Y;
        private Timer Attack_On = new Timer(1000);

        public override int Vita 
        { 
            get => 99999;
            set
            {
                if (value == -999)
                {
                    Attack_On.Close();
                    base.Vita = 0;
                }

            } 
        }

        public Rovo(int x, int y) : base(x, y)
        {
            disponibile = false;
            attesa = new Timer(5000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            gioco.n_soli -= Program.piante_ottenute[6].costo_soli;

            X = x; Y = y;

            rect = new IntRect(52, 52, 308, 128);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Rovo.png", rect);
            pianta.Texture = texture;
            pianta.Position += new Vector2f(2, 65);
            pianta.Scale = new Vector2f(0.26f, 0.26f);

            Attack_On.Elapsed += Attack_On_Elapsed;
            Attack_On.Enabled = true;
        }

        public Rovo() 
        {
            costo_soli = 100;
            costo_monete = 100;
        }

        private void Attack_On_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (gioco.LockZombie)
                for (int i = 0; i < gioco.Mappa_zombie[Y].Count; i++)
                    if (gioco.Mappa_zombie[Y][i].sprite.Position.X <= pianta.Position.X + 308 * pianta.Scale.X && gioco.Mappa_zombie[Y][i].sprite.Position.X + 422 * gioco.Mappa_zombie[Y][i].sprite.Scale.X >= pianta.Position.X)
                        lock (gioco.Mappa_zombie[Y][i].LockVita)
                            gioco.Mappa_zombie[Y][i].Vita -= danno;
        }

        public override void GetInstace(int x, int y)
        {
            new Rovo(x, y);
        }

        public override void DisegnaLista(Vector2f posizione, Vector2f scala)
        {
            L_RO.Position = posizione;
            L_RO.Scale = scala;
                Gioco.Finestra.Draw(L_RO);
        }

        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override Rovo GetInstace()
        {
            return new Rovo();
        }

        public override bool Disponibile()
        {
            return disponibile;
        }
        public override void Stop()
        {
            Attack_On.Stop();
        }
    }
}