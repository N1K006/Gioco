using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Timers;
using Plants_Vs_Zombies.Codici.Oggetti;

namespace Plants_Vs_Zombies
{
    class GirasoleGemello : Pianta
    {
        // Immagine lista di girasole gemello
        public static IntRect l_gg = new IntRect(0, 0, 340, 170);
        public static Texture L_gg = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_GirasoleGemello.png", l_gg);
        public static readonly Sprite L_GG = new Sprite(L_gg);

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
                        base.Vita = 0;
                    }
                }
            }
        }

        public GirasoleGemello(int x, int y) : base(x, y)
        {
            vita = 50;
            disponibile = false;
            attesa = new Timer(10000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed; 
            for (int i = 0; i < 8; i++)
                if (gioco.Lista_piante[i] != null)
                    if (gioco.Lista_piante[i] is GirasoleGemello)
                    {
                        gioco.n_soli -= gioco.Lista_piante[i].costo_soli;
                        break;
                    }

            X = x; Y = y;

            Sun_On.Elapsed += Sun_On_Elapsed;
            Sun_On.Enabled = true;

            rect = new IntRect(76, 0, 259, 231);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\GirasoleGemello.png", rect);
            pianta.Texture = texture;
            pianta.Position += new Vector2f(5, 25);
            pianta.Scale = new Vector2f(0.3f, 0.3f);
        }

        public GirasoleGemello()
        {
            costo_soli = 150;
            costo_monete = 300;
        }

        private void Sun_On_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (gioco.LockSoli)
            {
                Sole s1 = new Sole(pianta.Position, 2);
                s1.value_sun = 25;
            }
            lock (gioco.LockSoli)
            {
                Sole s2 = new Sole(pianta.Position, 2);
                s2.value_sun = 25;
            }
        }

        public override void GetInstace(int x, int y)
        {
            new GirasoleGemello(x, y);
        }

        public override void DisegnaLista(Vector2f posizione, Vector2f scala)
        {
            L_GG.Position = posizione;
            L_GG.Scale = scala;
            Gioco.Finestra.Draw(L_GG);
        }
        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override GirasoleGemello GetInstace()
        {
            return new GirasoleGemello();
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
