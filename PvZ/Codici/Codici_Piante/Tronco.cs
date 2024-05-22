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
    class Tronco : Pianta
    {
        // Immagine lista di muro noce
        public static IntRect l_mn = new IntRect(0, 0, 340, 170);
        public static Texture L_mn = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_Tronco.png", l_mn);
        public static readonly Sprite L_MN = new Sprite(L_mn);

        public static bool disponibile = true;
        public readonly int X, Y;

        Timer fuoco = new Timer(30);
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
                        fuoco.Stop();
                        fuoco.Close();
                        base.Vita = 0;
                    }
                }
            }
        }

        public Tronco(int x, int y) : base(x, y) //mappa
        {
            vita = 100;
            disponibile = false;
            attesa = new Timer(20000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            for (int i = 0; i < 8; i++)
                if (gioco.Lista_piante[i] != null)
                    if (gioco.Lista_piante[i] is Tronco)
                    {
                        gioco.n_soli -= gioco.Lista_piante[i].costo_soli;
                        break;
                    }

            X = x; Y = y;

            rect = new IntRect(7, 14, 229, 393);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Tronco.png", rect);
            pianta.Texture = texture;
            pianta.Position += new Vector2f(12, 10);
            pianta.Scale = new Vector2f(0.24f, 0.24f);

            fuoco.Enabled = true;
            fuoco.Elapsed += fuoco_Elapsed;
        }
        void fuoco_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (gioco.LockSemi)
            {
                for (int i = 0; i < Seme.semi.Count; i++)
                {
                    Seme s = Seme.semi[i];
                    if (s.fila == Y)
                    {
                        if (s.circle.Position.X - pianta.Position.X < 82 && s.circle.Position.X - pianta.Position.X > 0 && !s.trasformato)
                        {
                            Vector2f pos = s.circle.Position;
                            if (s is SemeDefault)
                            {
                                SemeInfuocato auxS = new SemeInfuocato(new Vector2f((float)((pos.X - 314) / 81), (float)((pos.Y - 122)/ 100)), s.danno * 2);
                                s.Stop();
                                auxS.trasformato = true;
                            }
                            else if (s is SemeBrina)
                            {
                                SemeDefault auxS = new SemeDefault(new Vector2f((float)((pos.X - 314) / 81), (float)((pos.Y - 122) / 100)), s.danno);
                                s.Stop();
                                auxS.trasformato = true;
                            }
                        }
                    }
                }
            }
        }
        public Tronco()
        {
            //costo_soli = 175;
            costo_soli = 0;
            costo_monete = 750;
        } //lista

        public override void GetInstace(int x, int y)
        {
            new Tronco(x, y);
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

        public override Tronco GetInstace()
        {
            return new Tronco();
        }

        public override bool Disponibile
        {
            get => disponibile;
            set => disponibile = value;
        }
        public override void Stop() { }
    }
}