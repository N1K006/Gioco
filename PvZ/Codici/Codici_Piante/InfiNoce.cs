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
    class InfiNoce : Pianta
    {
        // Immagine lista di muro noce
        public static IntRect l_in = new IntRect(0, 0, 340, 170);
        public static Texture L_in = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_InfiNoce.png", l_in);
        public static readonly Sprite L_IN = new Sprite(L_in);

        public static bool disponibile = true;
        public readonly int X, Y;
        public int vitaAtt;

        Timer Recupero_vita = new Timer(500);

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
                        Recupero_vita.Close();
                        base.Vita = 0;
                    }
                }
            }
        }

        public InfiNoce(int x, int y) : base(x, y) //mappa
        {
            vita = 750; vitaAtt = vita;
            disponibile = false;
            attesa = new Timer(20000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            for (int i = 0; i < 8; i++)
                if (gioco.Lista_piante[i] != null)
                    if (gioco.Lista_piante[i] is InfiNoce)
                    {
                        gioco.n_soli -= gioco.Lista_piante[i].costo_soli;
                        break;
                    }

            X = x; Y = y;

            rect = new IntRect(116, 5, 183, 225);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Infi_Noce.png", rect);
            pianta.Texture = texture;
            pianta.Position += new Vector2f(2, 20);
            pianta.Scale = new Vector2f(0.33f, 0.33f);

            Recupero_vita.Elapsed += Recupero_vita_Elapsed;
            Recupero_vita.Enabled = true;
        }

        public InfiNoce()
        {
            costo_soli = 75;
            costo_monete = 650;
        } //lista

        public override void DisegnaLista(Vector2f posizione, Vector2f scala)
        {
            L_IN.Position = posizione;
            L_IN.Scale = scala;
            Gioco.Finestra.Draw(L_IN);
        }

        void Recupero_vita_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (vitaAtt <= vita - 1)
                vitaAtt += 1;
        }

        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override void GetInstace(int x, int y)
        {
            new InfiNoce(x, y);
        }

        public override InfiNoce GetInstace()
        {
            return new InfiNoce();
        }

        public override bool Disponibile()
        {
            return disponibile;
        }
        public override void Stop()
        {
            Recupero_vita.Stop();
        }
    }
}

