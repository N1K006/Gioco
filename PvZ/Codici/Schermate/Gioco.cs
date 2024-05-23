using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class Gioco
    {
        public static RenderWindow Finestra;

        public object LockZombie = new object();
        public object LockSemi = new object();
        public object LockSoli = new object();
        public object LockMonete = new object();
        public object LockBoom = new object();

        public bool ricomincia = false;
        public int fase;

        #region Costruttore
        static object Lock = new object();
        static Gioco instance = null;
        private Gioco(Pianta[] piante)
        {
            instance = this;

            Pianta.gioco = instance;
            Paletta.gioco = instance;
            Sole.gioco = instance;
            Seme.gioco = instance;
            Zombie.gioco = instance;
            Moneta.gioco = instance;

            Lista_piante = piante;

            fase = 0;
            gioco();
        }

        public static void Start(Pianta[] piante)
        {
            lock (Lock)
            {
                if (instance == null)
                {
                    instance = new Gioco(piante);
                    Finestra.MouseButtonPressed -= instance.MouseClick;
                    Finestra.MouseMoved -= instance.MouseMoved;
                }
                instance = null;
            }
        }
        #endregion

        #region SFML
        // FONT
        public static Font numeri = new Font(@"..\..\..\Font\ComixLoud.ttf");

        //Immagine mappa
        public static Sprite mappa = new Sprite(new Texture(@"..\..\..\Immagini\Mappa\Mappa.png"), new IntRect(0, 0, 4180, 2400));

        // Immagine conta soli
        public static IntRect c_s = new IntRect(0, 0, 253, 101);
        public static Texture C_s = new Texture(@"..\..\..\Immagini\Mappa\Conta_soli.png", c_s);
        public static readonly Sprite C_S = new Sprite(C_s);

        // Immagine cerchio
        public static IntRect cerchioP = new IntRect(0, 0, 603, 603);
        public static Texture CerchioP = new Texture(@"..\..\..\Immagini\Mappa\Cerchio.png", cerchioP);
        public static readonly Sprite Cerchio_Paletta = new Sprite(CerchioP);

        // Immagine Conta Monete
        public static IntRect c_m = new IntRect(0, 0, 526, 148);
        public static Texture C_m = new Texture(@"..\..\..\Immagini\Mappa\Conta_Monete.png", c_m);
        public static readonly Sprite C_M = new Sprite(C_m);

        // Immagine Tasto Home
        public static IntRect t_h = new IntRect(0, 0, 799, 755);
        public static Texture T_h = new Texture(@"..\..\..\Immagini\Mappa\Tasto_Home.png", t_h);
        public static readonly Sprite T_H = new Sprite(T_h);
        #endregion

        #region Suoni
        static SoundBuffer suono_gioco = new SoundBuffer(@"..\..\..\Suoni\Gioco.wav");
        public static Sound SUONO_GIOCO = new Sound(suono_gioco);
        #endregion

        public Pianta[] Lista_piante;
        public Pianta[,] Mappa_piante = new Pianta[9, 5];

        Timer Sun_On_Map = new Timer(4000);

        public bool perso = false;
        public int yLista = 8;
        public int x, y;
        public int contatore = 5, n_soli = 50;
        public bool home = false, muto = false, esci = false;

        #region Zombie
        public static Zombie[] zombie = { new ZombieOrdinario(0f),
                                          new ZombieSegnaletico(0f),
                                          new ZombieSecchione(0f) };

        public List<Zombie>[] Mappa_zombie = new List<Zombie>[5];

        Timer Zombie_On = new Timer(5000); // velocita di spawn
        Timer Vel_Zombie = new Timer(5000); // tempo per aumentare la velocita di spawn

        Timer Diff = new Timer(30000); // tempo per cambiare gli zombie che spawnano
        public int difficolta = 0; // zombie che spawnano

        public int Difficolta
        {
            get => difficolta;
            set
            {
                difficolta = value;
                for (int i = 0; i < zombie.Length; i++)
                {
                    if (zombie[i].Probabilita == 0)
                    {
                        zombie[i].Probabilita = 100;
                        break;
                    }
                    else
                        zombie[i].Probabilita -= zombie[i].Probabilita / 6.5f;
                }
                switch (value)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                }
            }
        }

        void Vel_Zombie_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Zombie_On.Interval > 1000)
                Zombie_On.Interval -= 27.77;
        }

        void Diff_Elapsed(object sender, ElapsedEventArgs e)
        {
            Difficolta++;
        }

        void Zombie_On_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Difficolta != 0)
            {
                Random r = new Random();
                bool creato = false;
                for (int i = 0; i < zombie.Length && !creato; i++)
                    if (r.NextDouble() * 100 < zombie[i].GetInstance().Probabilita || i == zombie.Length - 1)
                    {
                        lock (LockZombie)
                        {
                            Zombie aux = zombie[i].GetInstance(new Random().Next(0, 5));
                        }
                        creato = true;
                        Logger.WriteLine(zombie[i].ToString(), 6);
                    }
            }
        }
        #endregion

        public void gioco()
        {
            Finale.Finestra = Finestra;
            Finale.gioco = this;

            Pausa.Finestra = Finestra;
            Pausa.gioco = this;

            for (int i = 0; i < 5; i++)
                Mappa_zombie[i] = new List<Zombie>();

            // Timer
            {
                Zombie_On.Elapsed += Zombie_On_Elapsed;
                Zombie_On.Enabled = true;

                Sun_On_Map.Elapsed += Sun_On_Map_Elapsed;
                Sun_On_Map.Enabled = true;

                Vel_Zombie.Elapsed += Vel_Zombie_Elapsed;
                Vel_Zombie.Enabled = true;

                Diff.Elapsed += Diff_Elapsed;
                Diff.Enabled = true;
            }

            Finestra.MouseButtonPressed -= Home.MouseClick;
            Finestra.MouseButtonPressed += MouseClick;
            Finestra.MouseMoved += MouseMoved;

            SUONO_GIOCO.Volume = 100;

            while (Finestra.IsOpen && Program.fase == 1)
            {
                //if (ricomincia || esci)
                //    break;

                if (SUONO_GIOCO.Status == SoundStatus.Stopped)
                    SUONO_GIOCO.Play();

                if (fase == 1)
                {
                    Finestra.MouseButtonPressed -= MouseClick;
                    Finestra.MouseMoved -= MouseMoved;

                    Zombie_On.Stop();
                    Sun_On_Map.Stop();
                    Vel_Zombie.Stop();
                    Diff.Stop();

                    Pausa.pausa();

                    Zombie_On.Start();
                    Sun_On_Map.Start();
                    Vel_Zombie.Start();
                    Diff.Start();

                    Finestra.MouseButtonPressed += MouseClick;
                    Finestra.MouseMoved += MouseMoved;
                }
                if (fase == 2)
                {
                    Finestra.MouseButtonPressed -= MouseClick;
                    Finestra.MouseMoved-= MouseMoved;

                    Zombie_On.Stop();
                    Sun_On_Map.Stop();
                    Vel_Zombie.Stop();
                    Diff.Stop();

                    Finale.Fine();

                    Zombie_On.Start();
                    Sun_On_Map.Start();
                    Vel_Zombie.Start();
                    Diff.Start();

                    Finestra.MouseButtonPressed += MouseClick;
                    Finestra.MouseMoved += MouseMoved;
                }

                Finestra.Clear();
                Disegna();
                Finestra.DispatchEvents();
                Finestra.Display();
            }
            Reset();
        }

        public void Reset()
        {
            SUONO_GIOCO.Stop();
            Sun_On_Map.Stop();
            Zombie_On.Stop();
            Vel_Zombie.Stop();
            Diff.Stop();

            // Utilità
            {
                // Semi
                while (Seme.semi.Count > 0)
                    Seme.semi[0].Stop();

                // Soli presi e soli non presi
                while (Sole.soli.Count() > 0)
                    Sole.soli[0].Stop();

                // Soli presi
                while (Sole.soliPresi.Count() > 0)
                    Sole.soliPresi[0].Stop();

                // Boom esplosioni
                while (Boom.esplosioni.Count() > 0)
                {
                    Boom.esplosioni[0]._boom.Stop();
                    Boom.esplosioni[0]._boom.Close();
                }

                // Zombi
                for (int i = 0; i < 5; i++)
                    while (Mappa_zombie[i].Count > 0)
                        Mappa_zombie[i][0].Vita = 0;

                // Monete
                while (Moneta.monete.Count() > 0)
                    Moneta.monete[0].Stop();
            }

            // Piante
            {
                // Porta disponibile la lista di piante
                foreach (Pianta p in Lista_piante)
                    if (p != null)
                        p.GetInstace().Disponibile = true;

                zombie = new Zombie[]{ new ZombieOrdinario(0f),
                                   new ZombieSegnaletico(0f),
                                   new ZombieSecchione(0f) };

                Difficolta = 0;
                difficolta = 0;
                contatore = 5;
                n_soli = 50;
                home = false;
                muto = false;
                fase = 0;
                instance = null;
            }

            // Piante
            for (int y = 0; y < 5; y++)
                for (int x = 0; x < 9; x++)
                    if (Mappa_piante[x, y] != null)
                        Mappa_piante[x, y].Vita = -999;

            Vel_Zombie = new Timer(5000);
            Diff = new Timer(30000);
            difficolta = 0;

            if (perso)
                Program.fase = 1;
            else
                Program.fase = 0;
        }
        public void MouseClick(object sender, MouseButtonEventArgs e)
        {
            x = e.X;
            y = e.Y;

            bool s = false;
            bool m = false;

            lock (LockSoli)
            {
                s = ClickSole();
            }
            lock (LockMonete)
            {
                m = ClickMoneta();
                if (m)
                    using (BinaryWriter wa = new BinaryWriter(File.Open(@"..\..\..\Salvataggio\Altro.txt", FileMode.Create), Encoding.UTF8, false))
                        wa.Write((Int32)Program.monete);
            }

            if (x > 24 && x < 140 && !home && !perso && !(s || m)) //lista selezionata
            {
                int Y = y - 10;
                int aux = Y % 65;
                Y /= 65;
                if (Y < 8)
                {
                    if (Lista_piante[Y] == null)
                        Y = 8;
                    else if (aux > 57 || y - 10 < 0 || !Lista_piante[Y].GetInstace().Disponibile)
                        Y = 8;
                }
                yLista = Y;
                Paletta.presa = false;
                Paletta.pos();
            }
            else if (x > 253 && y > 71 && !home && !perso && !(s || m)) //casella selezionata
            {
                int X, Y;
                X = (x - 254) / 81;
                Y = (y - 72) / 100;

                Logger.WriteLine(X + " " + Y, 6);

                if (X < 9 && Y < 5)
                {
                    if (Paletta.presa)
                        Paletta.Rimuovi(X, Y);
                    else if (X < 9)
                        PosizionaPianta(X, Y);
                }
            }
            else if (x >= 327 && x <= 375 && y >= 13 && y <= 60 && !home && !perso && !(s || m)) // paletta
            {
                Paletta.presa = true;
                yLista = 8;
            }
            else if (x >= 976 && x <= 1030 && y >= 16 && y <= 70 && !perso) // Tasto Pausa
            {
                Paletta.presa = false;
                Paletta.pos();
                yLista = 8;
                fase = 1;
                Finestra.MouseButtonPressed -= MouseClick;
                Finestra.MouseMoved -= MouseMoved;
            }
            else // altro
            {
                yLista = 8;
                Paletta.presa = false;
                Paletta.pos();
            }
        }

        public void MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (Paletta.presa)
                Paletta.sprite.Position = new Vector2f(e.X, e.Y);
        }

        void Sun_On_Map_Elapsed(object sender, ElapsedEventArgs e)
        {
            contatore++;
            if (contatore == 6)
            {
                contatore = 0;
                lock (LockSoli)
                {
                    Sole s = new Sole
                    {
                        value_sun = 25
                    };
                }
            }
        }

        bool ClickSole()
        {
            bool presa = false;
            lock (LockSoli)
                for (int i = 0; i < Sole.soli.Count; i++)
                    if (Sole.soli[i] != null)
                        if (x >= Sole.soli[i].pos.X - 32 && x <= Sole.soli[i].pos.X + 32 && y >= Sole.soli[i].pos.Y - 32 && y <= Sole.soli[i].pos.Y + 32)
                        {
                            Sole.soli[i].Preso();
                            i--;
                            presa = true;
                        }
            return presa;
        }

        bool ClickMoneta()
        {
            bool presa = false;
            lock (LockMonete)
                for (int i = 0; i < Moneta.monete.Count; i++)
                    if (Moneta.monete[i] != null)
                        if (x >= Moneta.monete[i].moneta.Position.X - 21 && x <= Moneta.monete[i].moneta.Position.X + 21 && y >= Moneta.monete[i].moneta.Position.Y - 21 && y <= Moneta.monete[i].moneta.Position.Y + 21)
                        {
                            Moneta.monete[i].Preso();
                            i--;
                            presa = true;
                        }
            return presa;
        }

        void PosizionaPianta(int x, int y)
        {
            if (Mappa_piante[x, y] == null && yLista != 8)
                if (Lista_piante[yLista].GetInstace().Disponibile && Lista_piante[yLista].GetInstace().costo_soli <= n_soli)
                    Lista_piante[yLista].GetInstace(x, y);
            yLista = 8;
        }

        void Disegna()
        {
            // Immagine mappa
            {
                mappa.Scale = new Vector2f(0.25f, 0.25f);
                Finestra.Draw(mappa);
            }
            T_H.Origin = new Vector2f(T_H.Texture.Size.X / 2, T_H.Texture.Size.X / 2);
            T_H.Scale = new Vector2f(0.07f, 0.07f);
            T_H.Position = new Vector2f(1005, 45);
            Finestra.Draw(T_H);
            // Immagine conta soli
            {
                C_S.Origin = new Vector2f(50, 50);
                C_S.Scale = new Vector2f(0.65f, 0.65f);
                C_S.Position = new Vector2f(155 + (C_S.Origin.X * C_S.Scale.X), 0 + (C_S.Origin.Y * C_S.Scale.Y));
                Finestra.Draw(C_S);  // Contatore soli

                Text n_sole = new Text(Convert.ToString(n_soli), numeri, 13)
                {
                    FillColor = Color.White,
                    Position = new Vector2f(235, 26)
                };
                Finestra.Draw(n_sole); //Numero soli
            }
            // Piante nella mappa
            {
                for (int y = 0; y < 5; y++)
                    for (int x = 0; x < 9; x++)
                        if (Mappa_piante[x, y] != null)
                            Finestra.Draw(Mappa_piante[x, y].pianta);
            }
            // Zombie nella mappa
            {
                lock (LockZombie)
                {
                    for (int i = 0; i < 5; i++)
                        for (int j = 0; j < Mappa_zombie[i].Count; j++)
                            if (Mappa_zombie[i][j] != null)
                                Finestra.Draw(Mappa_zombie[i][j].sprite);
                }
            }
            // Disegno delle immagini lista
            {
                for (int i = 0; i < 8; i++)
                {
                    if (Lista_piante[i] != null)
                        Lista_piante[i].DisegnaLista(new Vector2f(25, (65 * i) + 10), yLista == i ? new Vector2f(0.36f, 0.36f) :
                                                                                                  new Vector2f(0.34f, 0.34f));

                    if (Lista_piante[i] != null)
                        if (!Lista_piante[i].GetInstace().Disponibile)
                        {
                            RectangleShape rect = new RectangleShape(new Vector2f(115, 58))
                            {
                                FillColor = new Color(100, 100, 100, 150),
                                Position = new Vector2f(25, (65 * i) + 10)
                            };
                            Finestra.Draw(rect);
                        }
                }
            }
            // Immagine conta monete
            {
                C_M.Origin = new Vector2f(75, 75);
                C_M.Scale = new Vector2f(0.35f, 0.35f);
                C_M.Position = new Vector2f(10 + (C_M.Origin.X * C_M.Scale.X), 535 + (C_M.Origin.X * C_M.Scale.X));
                Finestra.Draw(C_M);  // Contatore soli

                Text num_monete = new Text(Convert.ToString(Program.monete), numeri, 13)
                {
                    FillColor = Color.White,
                    Position = new Vector2f(80, 554.5f)
                };
                Finestra.Draw(num_monete); // Numero monete
            }
            // Soli, monete e semi
            {
                lock (LockSemi)
                {
                    for (int i = 0; i < Seme.semi.Count; i++)
                        if (Seme.semi != null)
                            Finestra.Draw(Seme.semi[i].circle);
                }
                lock (LockSoli)
                {
                    for (int i = 0; i < Sole.soli.Count; i++)
                        if (Sole.soli != null)
                            Finestra.Draw(Sole.soli[i].sole);
                }
                lock (LockSoli)
                {
                    for (int i = 0; i < Sole.soliPresi.Count; i++)
                        if (Sole.soliPresi != null)
                            Finestra.Draw(Sole.soliPresi[i].sole);
                }
                lock (LockMonete)
                {
                    for (int i = 0; i < Moneta.monete.Count; i++)
                        if (Moneta.monete != null)
                            Finestra.Draw(Moneta.monete[i].moneta);
                }
                lock (LockMonete)
                {
                    for (int i = 0; i < Moneta.monetePrese.Count; i++)
                        if (Moneta.monetePrese != null)
                            Finestra.Draw(Moneta.monetePrese[i].moneta);
                }
                lock (LockBoom)
                {
                    if (!ricomincia)
                        for (int i = 0; i < Boom.esplosioni.Count; i++)
                            if (Boom.esplosioni[i] != null)
                                Finestra.Draw(Boom.esplosioni[i].BOOM);
                }
            }
            //Paletta
            {
                Cerchio_Paletta.Position = new Vector2f(351, 37);
                Cerchio_Paletta.Scale = new Vector2f(0.08f, 0.08f);
                Cerchio_Paletta.Origin = new Vector2f(301, 301);
                Finestra.Draw(Cerchio_Paletta); //Cerchio
                Finestra.Draw(Paletta.sprite); //Paletta
            }

        }
    }
}