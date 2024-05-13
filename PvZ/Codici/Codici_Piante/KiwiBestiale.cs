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
                        base.Vita = 0;
                    }
                }
            }
        }

        public KiwiBestiale(int x, int y) : base(x, y)
        {
            vita = 150;
            disponibile = false;
            attesa = new Timer(15000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
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
        }

        public KiwiBestiale()
        {
            costo_soli = 175;
            costo_monete = 1250;
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

        public override bool Disponibile()
        {
            return disponibile;
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
    }
}