﻿using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Audio;
using System.Timers;
using System.Collections.Generic;

namespace Plants_Vs_Zombies
{
    static class Shop
    {
        static public RenderWindow Finestra;

        static public Pianta[] piante;
        static public Pianta p;

        public static void Disegna()
        {
            Home.INDIETRO.Position = new Vector2f(14, 14);
            Home.INDIETRO.Scale = new Vector2f(0.433f, 0.433f);
            Finestra.Draw(Home.HOME);
            Finestra.Draw(Home.INDIETRO);

            // TUTTE LE PIANTE NELLO SHOP
            {
                // Cornice
                {
                    RectangleShape rect = new RectangleShape(new Vector2f(750, 450));
                    rect.FillColor = new Color(0, 0, 0, 255);
                    rect.Origin = new Vector2f(rect.Size.X / 2, rect.Size.Y / 2);
                    rect.Position = new Vector2f((Finestra.Size.X / 2) + 140, (Finestra.Size.Y / 2) + 70);
                    Finestra.Draw(rect);
                }
                // Quadrate interno alla cornice
                {
                    RectangleShape rect = new RectangleShape(new Vector2f(730, 440));
                    rect.FillColor = new Color(140, 0, 160);
                    rect.Origin = new Vector2f(rect.Size.X / 2, rect.Size.Y / 2);
                    rect.Position = new Vector2f((Finestra.Size.X / 2) + 145, (Finestra.Size.Y / 2) + 70);
                    Finestra.Draw(rect);
                    
                    Vector2f scala = new Vector2f(0.38f, 0.38f);
                    float larghezza = 340 * scala.X;
                    float altezza = 170 * scala.Y;

                    int nx = 5;

                    for (int x = 0; x < nx; x++)
                        for (int y = 0; nx * y + x < Program.piante_ottenute.Count; y++)
                            Program.piante_ottenute[nx * y + x].DisegnaLista(new Vector2f(325 + (larghezza + 10) * x,
                                                                                 210 + (altezza + 15) * y), scala);
                    Text shop = new Text("SHOP", Home.font, 20);
                    shop.FillColor = new Color(255, 255, 255);
                    shop.Position = new Vector2f((Finestra.Size.X / 2) + 90, (Finestra.Size.Y / 2) - 135);
                    Finestra.Draw(shop);
                }
            }
            // PIANTA SELEZIONATA DALLO SHOP
            {
                Vector2f scala = new Vector2f(0.70f, 0.70f);
                float larghezza = 340 * scala.X;
                float altezza = 170 * scala.Y;

                // Cornice
                {
                    RectangleShape rect = new RectangleShape(new Vector2f(280, 450));
                    rect.FillColor = new Color(0, 0, 0, 255);
                    rect.Origin = new Vector2f(0, rect.Size.Y / 2);
                    rect.Position = new Vector2f(10, (Finestra.Size.Y / 2) + 70);
                    Finestra.Draw(rect);
                }
                // Quadrate interno alla cornice
                {
                    RectangleShape rect = new RectangleShape(new Vector2f(270, 440));
                    rect.FillColor = new Color(0, 207, 45);
                    rect.Origin = new Vector2f(0, rect.Size.Y / 2);
                    rect.Position = new Vector2f(15, (Finestra.Size.Y / 2) + 70);
                    Finestra.Draw(rect);
                    // Testi
                    {
                        Text testo_compra = new Text("COMPRA LA PIANTA", Home.font, 14);
                        testo_compra.FillColor = new Color(255, 255, 255);
                        testo_compra.Position = new Vector2f(35, (Finestra.Size.Y / 2) - 135);
                        Finestra.Draw(testo_compra);

                        Text monete_ = new Text("MONETE DISPONIBILI: ", Home.font, 12);
                        monete_.FillColor = new Color(255, 255, 255);
                        monete_.Position = new Vector2f(35, (Finestra.Size.Y / 2) - 65);
                        Finestra.Draw(monete_);
                    }
                    // Immagine conta monete
                    {
                        Gioco.C_M.Origin = new Vector2f(75, 75);
                        Gioco.C_M.Scale = new Vector2f(0.35f, 0.35f);
                        Gioco.C_M.Origin = new Vector2f(0, 0);
                        Gioco.C_M.Position = new Vector2f(50, (Finestra.Size.Y / 2) - 35);
                        Finestra.Draw(Gioco.C_M);  // Contatore soli

                        Text num_monete = new Text(Convert.ToString(Gioco.numero_monete), Gioco.numeri, 13);
                        num_monete.FillColor = Color.White;
                        num_monete.Position = new Vector2f(130, (Finestra.Size.Y / 2) - 15);
                        Finestra.Draw(num_monete); // Numero monete
                    }

                    // Quadrato per la pianta selezionata
                    {
                        {
                            RectangleShape rect_p = new RectangleShape(new Vector2f(larghezza, altezza));
                            rect_p.FillColor = new Color(100, 100, 100, 220);
                            rect_p.Position = new Vector2f(32, 360);
                            Finestra.Draw(rect_p);
                        }

                        if (p != null)
                            p.DisegnaLista(new Vector2f(32, 360), scala);

                        Gioco.C_M.Origin = new Vector2f(75, 75);
                        Gioco.C_M.Scale = new Vector2f(0.35f, 0.35f);
                        Gioco.C_M.Origin = new Vector2f(0, 0);
                        Gioco.C_M.Position = new Vector2f(22, (Finestra.Size.Y / 2) + 165);
                        Finestra.Draw(Gioco.C_M);  // Contatore monete

                        Text num_monete;
                        if (p != null)
                        {
                            num_monete = new Text(Convert.ToString(p.costo_monete), Gioco.numeri, 13);
                            num_monete.Position = new Vector2f(105, (Finestra.Size.Y / 2) + 185);
                            if (p.costo_monete > Gioco.numero_monete)
                                num_monete.FillColor = Color.Red;
                            else
                                num_monete.FillColor = Color.White;
                        }
                        else
                            num_monete = new Text("", Home.font, 13);
                        Finestra.Draw(num_monete); // Numero monete
                    }
  
                    // Tasto Compra
                    {
                        RectangleShape rect_c = new RectangleShape(new Vector2f(larghezza, 55));
                        rect_c.FillColor = new Color(100, 100, 100);
                        rect_c.Position = new Vector2f(32, 525);
                        Finestra.Draw(rect_c);

                        Text compra = new Text("COMPRA", Home.font, 20);
                        compra.Position = new Vector2f(80, 543);
                        if (p != null)
                            compra.FillColor = new Color(255, 255, 255);
                        else
                            compra.FillColor = new Color(150, 150, 150);
                        Finestra.Draw(compra);
                    }
                }
            }
        }

        public static void shop()
        {
            Finestra.SetVerticalSyncEnabled(true);
            Finestra.Closed += (sender, args) => Finestra.Close();

            Finestra.MouseButtonPressed -= Home.MouseClick;
            Finestra.MouseButtonPressed += MouseClick;

            while (Finestra.IsOpen && Home.schermata == 2)
            {
                Finestra.Clear();
                Disegna();

                Finestra.DispatchEvents();
                Finestra.Display();
            }
        }

        public static void MouseClick(object sender, MouseButtonEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            Logger.WriteLine("X: " + x.ToString() + " " + "Y: " + y, 6);

            if (x >= 14 && x <= 86 && y >= 14 && y <= 86) // tasto indietro
                Home.schermata = 0;
            else if (x >= 32 && x <= 268 && y >= 524 && y <= 578) // Tasto compra
            {
                if (!Program.piante_ottenute.Contains(p) && Program.monete >= p.costo_monete)
                    Program.piante_ottenute.Add(p);
            }
            else if (x >= 325 && x <= 1010) // altre piante
            {
                int larghezza = (int)(340 * 0.38);
                int altezza = (int)(170 * 0.38);

                int X = x - 325;
                int auxX = X % (larghezza + 10);
                X /= (larghezza + 10);

                int Y = y - 210;
                int auxY = Y % (altezza + 15);
                Y /= (altezza + 15);

                int n = -1;

                if (auxX <= larghezza && auxY <= altezza && auxX > 0 && auxY > 0)
                {
                    n = 5 * Y + X;
                }

                if (n != -1 && n < Program.piante_ottenute.Count)
                    p = Program.piante_ottenute[n];
                else
                    p = null;
            }
        }
    }
}