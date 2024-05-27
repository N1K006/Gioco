using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plants_Vs_Zombies
{
    static class Pausa
    {
        public static RenderWindow Finestra;
        public static Gioco gioco;

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
        public static void pausa()
        {
            Finestra.MouseButtonPressed += MouseClick; 

            while (Finestra.IsOpen && gioco.fase == 1)
            {
                Disegna();
                Finestra.DispatchEvents();
                Finestra.Display();
            }
        }
        public static void MouseClick(object sender, MouseButtonEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            if (x >= 437 && x <= 605 && y >= 175 && y <= 223) // Tasto riprendi
            {
                gioco.fase = 0;
            }
            else if (x >= 437 && x <= 605 && y >= 385 && y <= 435) // Tasto Home
            {
                Home.schermata = 0;
                Program.fase = 0;
                gioco.fase = 0;
            }
            else if (x >= 437 && x <= 606 && y >= 315 && y <= 363) // Tasto Muta
            {
                gioco.muto = !gioco.muto;

                if (gioco.muto)
                    Gioco.SUONO_GIOCO.Volume = 0;
                else
                    Gioco.SUONO_GIOCO.Volume = 100;
            }
            else if (x >= 437 && x <= 606 && y >= 245 && y <= 295) // Tasto Ricomincia
            {
                Home.ricomincia = true;
                gioco.fase = 0;
                Program.fase = 0;
            }
        }
        static void Disegna()
        {
            mappa.Scale = new Vector2f(0.25f, 0.25f);
            Finestra.Draw(mappa);

            // Disegno delle immagini lista
            {
                for (int i = 0; i < 8; i++)
                {
                    if (gioco.Lista_piante[i] != null)
                        gioco.Lista_piante[i].DisegnaLista(new Vector2f(25, (65 * i) + 10), gioco.yLista == i ? new Vector2f(0.36f, 0.36f) :
                                                                                                  new Vector2f(0.34f, 0.34f));

                    if (gioco.Lista_piante[i] != null)
                        if (!gioco.Lista_piante[i].GetInstace().Disponibile)
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

            //Home
            {
                T_H.Origin = new Vector2f(T_H.Texture.Size.X / 2, T_H.Texture.Size.X / 2);
                T_H.Scale = new Vector2f(0.07f, 0.07f);
                T_H.Position = new Vector2f(1005, 45);
                Finestra.Draw(T_H);

                // Quadrato sfondo
                {
                    RectangleShape rect = new RectangleShape(new Vector2f(220, 300))
                    {
                        FillColor = new Color(100, 100, 100, 220),
                        Position = new Vector2f(Finestra.Size.X / 2, Finestra.Size.Y / 2)
                    };
                    rect.Origin = new Vector2f(rect.Size.X / 2, rect.Size.Y / 2);
                    Finestra.Draw(rect);
                }
                // Tasto Riprendi
                {
                    RectangleShape rect = new RectangleShape(new Vector2f(170, 50))
                    {
                        FillColor = new Color(30, 30, 30, 220),
                        Position = new Vector2f(Finestra.Size.X / 2, (Finestra.Size.Y / 2) - 100)
                    };
                    rect.Origin = new Vector2f(rect.Size.X / 2, rect.Size.Y / 2);
                    Finestra.Draw(rect);

                    Text home = new Text("RIPRENDI", Home.font, 15)
                    {
                        FillColor = Color.White,
                        Origin = new Vector2f(0, 0),
                        Position = new Vector2f((Finestra.Size.X / 2) - 60, (Finestra.Size.Y / 2) - 105)
                    };
                    Finestra.Draw(home);
                }
                // Tasto Ricomincia
                {
                    RectangleShape rect = new RectangleShape(new Vector2f(170, 50))
                    {
                        FillColor = new Color(30, 30, 30, 220),
                        Position = new Vector2f(Finestra.Size.X / 2, (Finestra.Size.Y / 2) - 30)
                    };
                    rect.Origin = new Vector2f(rect.Size.X / 2, rect.Size.Y / 2);
                    Finestra.Draw(rect);

                    Text home = new Text("RICOMINCIA", Home.font, 15)
                    {
                        FillColor = Color.White,
                        Origin = new Vector2f(0, 0),
                        Position = new Vector2f((Finestra.Size.X / 2) - 74, (Finestra.Size.Y / 2) - 38)
                    };
                    Finestra.Draw(home);
                }
                // Tasto Muta
                {
                    {
                        RectangleShape rect = new RectangleShape(new Vector2f(170, 50))
                        {
                            FillColor = new Color(30, 30, 30, 220),
                            Position = new Vector2f(Finestra.Size.X / 2, (Finestra.Size.Y / 2) + 40)
                        };
                        rect.Origin = new Vector2f(rect.Size.X / 2, rect.Size.Y / 2);
                        Finestra.Draw(rect);
                    }
                    // Quadratino per muto
                    {
                        RectangleShape rect = new RectangleShape(new Vector2f(20, 20))
                        {
                            Position = new Vector2f((Finestra.Size.X / 2) + 62, (Finestra.Size.Y / 2) + 40)
                        };
                        rect.Origin = new Vector2f(rect.Size.X / 2, rect.Size.Y / 2);

                        if (gioco.muto)
                            rect.FillColor = Color.Red;
                        else
                            rect.FillColor = Color.Green;
                        Finestra.Draw(rect);
                    }

                    Text home = new Text("VOLUME", Home.font, 15)
                    {
                        FillColor = Color.White,
                        Origin = new Vector2f(0, 0),
                        Position = new Vector2f((Finestra.Size.X / 2) - 70, (Finestra.Size.Y / 2) + 32)
                    };
                    Finestra.Draw(home);
                }
                // Tasto Home
                {
                    RectangleShape rect = new RectangleShape(new Vector2f(170, 50))
                    {
                        FillColor = new Color(30, 30, 30, 220),
                        Position = new Vector2f(Finestra.Size.X / 2, (Finestra.Size.Y / 2) + 110)
                    };
                    rect.Origin = new Vector2f(rect.Size.X / 2, rect.Size.Y / 2);
                    Finestra.Draw(rect);

                    Text home = new Text("HOME", Home.font, 15)
                    {
                        FillColor = Color.White,
                        Origin = new Vector2f(0, 0),
                        Position = new Vector2f((Finestra.Size.X / 2) - 35, (Finestra.Size.Y / 2) + 105)
                    };
                    Finestra.Draw(home);
                }
                // Immagine conta soli
                {
                    C_S.Origin = new Vector2f(50, 50);
                    C_S.Scale = new Vector2f(0.65f, 0.65f);
                    C_S.Position = new Vector2f(155 + (C_S.Origin.X * C_S.Scale.X), 0 + (C_S.Origin.Y * C_S.Scale.Y));
                    Finestra.Draw(C_S);  // Contatore soli

                    Text n_sole = new Text(Convert.ToString(gioco.n_soli), numeri, 13)
                    {
                        FillColor = Color.White,
                        Position = new Vector2f(235, 26)
                    };
                    Finestra.Draw(n_sole); //Numero soli
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
}
