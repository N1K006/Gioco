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
    static class Finale
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
        public static void Fine()
        {
            Finestra.MouseButtonPressed += MouseClick;

            while (Finestra.IsOpen && gioco.fase == 2)
            {
                Disegna();
                Finestra.DispatchEvents();
                Finestra.Display();
            }
        }
        static void Disegna()
        {
            mappa.Scale = new Vector2f(0.25f, 0.25f);
            Finestra.Draw(mappa);

            T_H.Origin = new Vector2f(T_H.Texture.Size.X / 2, T_H.Texture.Size.X / 2);
            T_H.Scale = new Vector2f(0.07f, 0.07f);
            T_H.Position = new Vector2f(1005, 45);
            Finestra.Draw(T_H);

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
            // Scritta Hai perso
            Text perso = new Text("HAI PERSO", Home.font, 50)
            {
                FillColor = Color.White,
                Origin = new Vector2f(0, 0),
                Position = new Vector2f((Finestra.Size.X / 2) - 140, (Finestra.Size.Y / 2) - 90)
            };
            Finestra.Draw(perso);

            RectangleShape rect1 = new RectangleShape(new Vector2f(250, 200))
            {
                FillColor = new Color(30, 30, 30, 220),
                Position = new Vector2f(460, 317)
            };
            Finestra.Draw(rect1);

            RectangleShape rect2 = new RectangleShape(new Vector2f(250, 10))
            {
                FillColor = new Color(100, 100, 100, 220),
                Position = new Vector2f(460, 410)
            };
            Finestra.Draw(rect2);

            Text home = new Text("HOME", Home.font, 20)
            {
                FillColor = Color.White,
                Origin = new Vector2f(0, 0),
                Position = new Vector2f(532, 360)
            };
            Finestra.Draw(home);

            Text ricomincia = new Text("RICOMINCIA", Home.font, 20)
            {
                FillColor = Color.White,
                Origin = new Vector2f(0, 0),
                Position = new Vector2f(488, 455)
            };
            Finestra.Draw(ricomincia);
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
        public static void MouseClick(object sender, MouseButtonEventArgs e)
        {
            int x = e.X;
            int y = e.Y;


            if (x >= 460 && x <= 709 && y >= 316 && y <= 408) // Tasto Home quando hai perso
            {
                Home.schermata = 0;
                Program.fase = 0;
                gioco.fase = 0;
            }
            else if (x >= 460 && x <= 709 && y >= 420 && y <= 515) // Tasto Ricomincia quando hai perso
            {
                Home.ricomincia = true;
                Program.fase = 0;
                gioco.fase = 0;
            }
        }
    }
}
