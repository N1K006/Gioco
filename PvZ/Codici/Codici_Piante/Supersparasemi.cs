﻿using System;
using SFML.Graphics;
using SFML.System;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class Supersparasemi : Pianta
    {
        // Immagine lista di supersparasemi
        public static IntRect l_sup = new IntRect(0, 0, 340, 170);
        public static Texture L_sup = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_Supersparasemi.png", l_sup);
        public static readonly Sprite L_SUP = new Sprite(L_sup);

        public static bool disponibile = true;
        private readonly int X, Y;

        public Timer Seme_On = new Timer(1500);
        public Timer Seme_On_2 = new Timer(400);

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
                        Seme_On.Close();
                        Seme_On_2.Close();
                        base.Vita = 0;
                    }
                }
            }
        }

        public Supersparasemi(int x, int y) : base(x, y)
        {
            vita = 50;
            disponibile = false;
            attesa = new Timer(5000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            gioco.n_soli -= Program.piante_ottenute[1].costo_soli;

            X = x; Y = y;

            Seme_On.Elapsed += Seme_On_Elapsed;
            Seme_On.Enabled = true;

            rect = new IntRect(106, 10, 203, 212);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Supersparasemi.png", rect);
            pianta.Texture = texture;
            pianta.Position += new Vector2f(12, 25);
            pianta.Scale = new Vector2f(0.31f, 0.31f);
        }

        public Supersparasemi() 
        { 
            costo_soli = 200;
            costo_monete = 300;
        }

        public bool Spara()
        {
            bool spara = false;
            lock (gioco.LockZombie)
            {
                for (int i = 0; i < gioco.Mappa_zombie[Y].Count; i++)
                {
                    Zombie z = gioco.Mappa_zombie[Y][i];
                    if (z != null)
                        if (z.sprite.Position.X >= pianta.Position.X)
                            spara = true;
                }
            }
            return spara;
        }

        private void Seme_On_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Spara())
            {
                lock (gioco.LockSemi)
                {
                    SemeDefault s = new SemeDefault(new Vector2f(X, Y), 10);
                }
                Seme_On_2.Elapsed += Seme_On_2_Elapsed;
                Seme_On_2.Enabled = true;
            }
        }

        private void Seme_On_2_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (gioco.LockSemi)
            {
                SemeDefault s = new SemeDefault(new Vector2f(X, Y), 10);
            }
            Seme_On_2.Stop();
        }

        public override void DisegnaLista(Vector2f posizione, Vector2f scala)
        {
            L_SUP.Position = posizione;
            L_SUP.Scale = scala;
                Gioco.Finestra.Draw(L_SUP);
        }

        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override void GetInstace(int x, int y)
        {
            new Supersparasemi(x, y);
        }

        public override Supersparasemi GetInstace()
        {
            return new Supersparasemi();
        }

        public override bool Disponibile()
        {
            return disponibile;
        }
        public override void Stop()
        {
            Seme_On.Stop();
            Seme_On_2.Stop();
        }
    }
}