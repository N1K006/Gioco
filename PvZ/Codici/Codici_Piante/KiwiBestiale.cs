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
    class KiwiBestiale : Pianta
    {
        // Immagine lista di muro noce
        public static IntRect l_kb = new IntRect(0, 0, 340, 170);
        public static Texture L_kb = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_KiwiBestiale.png", l_kb);
        public static readonly Sprite L_KB = new Sprite(L_kb);

        public static bool disponibile = true;
        public readonly int X, Y;

        Timer Attack_On = new Timer(1500);
        int danno;
        int fase = 1;

        

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
                        Attack_On.Stop();
                        Attack_On.Close();
                        base.Vita = 0;
                    }
                    else if (vita < 50)
                        fase = 3;
                    else if (vita < 100)
                        fase = 2;
                }
            }
        }

        public KiwiBestiale(int x, int y) : base(x, y)
        {
            vita = 150;
            danno = 7;

            disponibile = false;
            attesa = new Timer(15000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;

            Attack_On.Enabled = true;
            Attack_On.Elapsed += Attack_On_Elapsed;

            for (int i = 0; i < 8; i++)
                if (gioco.Lista_piante[i] != null)
                    if (gioco.Lista_piante[i] is KiwiBestiale)
                    {
                        gioco.n_soli -= gioco.Lista_piante[i].costo_soli;
                        break;
                    }
            rect = new IntRect(116, 5, 183, 225);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\KiwiBestiale.png", rect);
            pianta.Texture = texture;
            pianta.Scale = new Vector2f(0.3f, 0.3f);
        }

        public KiwiBestiale()
        {
            costo_soli = 0;
            costo_monete = 0;
        }

        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override void DisegnaLista(Vector2f posizione, Vector2f scala)
        {
            L_KB.Position = posizione;
            L_KB.Scale = scala;
                Gioco.Finestra.Draw(L_KB);
        }

        public override bool Disponibile
        {
            get => disponibile;
            set => disponibile = value;
        }

        public override void GetInstace(int x, int y)
        {
            new KiwiBestiale(x, y);
        }

        public override KiwiBestiale GetInstace()
        {
            return new KiwiBestiale();
        }
        public override void Stop() { }
        void Attack_On_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Y > 1 && fase == 3) // due file sopra
            {
                lock (gioco.LockZombie)
                {
                    for (int i = 0; i < gioco.Mappa_zombie[Y - 1].Count; i++)
                        if (Math.Abs(gioco.Mappa_zombie[Y - 1][i].sprite.Position.X - pianta.Position.X) < 100 * fase)
                        {
                            Logger.WriteLine(gioco.Mappa_zombie[Y - 1][i].Vita.ToString(), 6);
                            gioco.Mappa_zombie[Y - 1][i].Vita -= danno * fase;
                            i--;
                        }
                }
            }

            if (Y > 0 && fase > 1) // fila sopra
            {
                lock (gioco.LockZombie)
                {
                    for (int i = 0; i < gioco.Mappa_zombie[Y - 1].Count; i++)
                        if (Math.Abs(gioco.Mappa_zombie[Y - 1][i].sprite.Position.X - pianta.Position.X) < 100 * fase)
                        {
                            Logger.WriteLine(gioco.Mappa_zombie[Y - 1][i].Vita.ToString(), 6);
                            gioco.Mappa_zombie[Y - 1][i].Vita -= danno * fase;
                            i--;
                        }
                }
            }

            lock (gioco.LockZombie)
            {
                for (int i = 0; i < gioco.Mappa_zombie[Y].Count; i++)
                    if (Math.Abs(gioco.Mappa_zombie[Y][i].sprite.Position.X - pianta.Position.X) < 100 * fase)
                    {
                        Logger.WriteLine(gioco.Mappa_zombie[Y - 1][i].Vita.ToString(), 6);
                        gioco.Mappa_zombie[Y][i].Vita -= danno * fase;
                        i--;
                    }
            }

            if (Y < 4 && fase > 1) // fila sotto
            {
                lock (gioco.LockZombie)
                {
                    for (int i = 0; i < gioco.Mappa_zombie[Y + 1].Count; i++)
                        if (Math.Abs(gioco.Mappa_zombie[Y + 1][i].sprite.Position.X - pianta.Position.X) < 100 * fase)
                        {
                            Logger.WriteLine(gioco.Mappa_zombie[Y - 1][i].Vita.ToString(), 6);
                            gioco.Mappa_zombie[Y + 1][i].Vita -= danno * fase;
                            i--;
                        }
                }
            }

            if (Y < 3 && fase == 3) // 2 file sotto
            {
                lock (gioco.LockZombie)
                {
                    for (int i = 0; i < gioco.Mappa_zombie[Y + 1].Count; i++)
                        if (Math.Abs(gioco.Mappa_zombie[Y + 1][i].sprite.Position.X - pianta.Position.X) < 100 * fase)
                        {
                            Logger.WriteLine(gioco.Mappa_zombie[Y - 1][i].Vita.ToString(), 6);
                            gioco.Mappa_zombie[Y + 1][i].Vita -= danno * fase;
                            i--;
                        }
                }
            }
        }
    }
}