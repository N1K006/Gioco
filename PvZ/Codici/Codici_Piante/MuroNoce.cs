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
    class MuroNoce : Pianta
    {
        // Immagine lista di muro noce
        public static IntRect l_mn = new IntRect(0, 0, 340, 170);
        public static Texture L_mn = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_MuroNoce.png", l_mn);
        public static readonly Sprite L_MN = new Sprite(L_mn);

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

        public MuroNoce(int x, int y) : base(x, y) //mappa
        {
            vita = 100;
            disponibile = false;
            attesa = new Timer(20000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            for (int i = 0; i < 8; i++)
                if (gioco.Lista_piante[i] != null)
                    if (gioco.Lista_piante[i] is MuroNoce)
                    {
                        gioco.n_soli -= gioco.Lista_piante[i].costo_soli;
                        break;
                    }

            X = x; Y = y;

            rect = new IntRect(116, 5, 183, 225);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\MuroNoce.png", rect);
            pianta.Texture = texture;
            pianta.Position += new Vector2f(12, 25);
            pianta.Scale = new Vector2f(0.3f, 0.3f);
        }
        public MuroNoce()
        {
            costo_soli = 50;
            costo_monete = 280;
        } //lista

        public override void GetInstace(int x, int y)
        {
            new MuroNoce(x, y);
        }

        public override void DisegnaLista(Vector2f posizione, Vector2f scala)
        {
            L_MN.Position = posizione;
            L_MN.Scale = scala;
                Gioco.Finestra.Draw(L_MN);
        }

        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override MuroNoce GetInstace()
        {
            return new MuroNoce();
        }

        public override bool Disponibile() 
        { 
            return disponibile;
        }
        public override void Stop() { }
    }
}