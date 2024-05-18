using System;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Window;

namespace Plants_Vs_Zombies
{
    static class Home
    {
        static public RenderWindow Finestra = null;

        #region SFML
        // Immagine HOME
        public static IntRect _home = new IntRect(0, 0, 1279, 792);
        public static Texture _Home = new Texture(@"..\..\..\Immagini\Mappa\Home.png", _home);
        public static readonly Sprite HOME = new Sprite(_Home);

        // Immagine INDIETRO
        public static IntRect indietro = new IntRect(0, 0, 166, 166);
        public static Texture Indietro = new Texture(@"..\..\..\Immagini\Mappa\Indietro.png", indietro);
        public static readonly Sprite INDIETRO = new Sprite(Indietro);

        // Immagine INGRANAGGIO
        public static IntRect ingranaggio = new IntRect(16, 16, 493, 493);
        public static Texture Ingranaggio = new Texture(@"..\..\..\Immagini\Mappa\Impostazioni.png", ingranaggio);
        public static readonly Sprite INGRANAGGIO = new Sprite(Ingranaggio);
        #endregion

        #region Suoni
        static SoundBuffer suono_home = new SoundBuffer(@"..\..\..\Suoni\Home.wav");
        static Sound SUONO_HOME = new Sound(suono_home);
        #endregion

        static Text gioca;
        static Text shop;
        static Text seleziona_piante;
        public static Font font = new Font(@"..\..\..\Font\ComixLoud.ttf");

        static public int schermata = 0; //0 = principale, 1 = shop, 2 = piante

        public static Pianta[] piante = new Pianta[] { null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null};

        public static void home(ref Pianta[] piante)
        {
            Home.piante = piante;
            SUONO_HOME.Volume = 100;
            
            Finestra.SetVerticalSyncEnabled(true);
            Finestra.Closed += (sender, args) => Finestra.Close();
            Finestra.MouseButtonPressed -= MouseClick;
            Finestra.MouseButtonPressed += MouseClick;
            while (Finestra.IsOpen && Program.fase == 0)
            {
                if (SUONO_HOME.Status == SoundStatus.Stopped)
                    SUONO_HOME.Play();

                Finestra.Clear();
                Finestra.DispatchEvents();

                switch (schermata)
                {
                    case 0:
                        Disegna();
                        break;
                    case 1: //shop
                        Shop.shop();
                        Finestra.MouseButtonPressed -= Shop.MouseClick;
                        Finestra.MouseButtonPressed += MouseClick;
                        break;
                    case 2: //piante
                        Piante.SelezionaPiante(ref Home.piante);
                        Finestra.MouseButtonPressed -= Piante.MouseClick;
                        Finestra.MouseButtonPressed += MouseClick;
                        break;
                }
                Finestra.Display();
            }
            SUONO_HOME.Stop();
            piante = Home.piante;
        }

        public static void MouseClick(object sender, MouseButtonEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            Logger.WriteLine("X: " + x.ToString() + " " + "Y: " + y, 6);

            if (x >= 912 && x <= 1032 && y >= 525 && y <= 585 && schermata == 0 && Piante.stato) // tasto gioca (HOME)
                Program.fase = 1;
            else if (x >= 14 && x <= 134 && y >= 525 && y <= 585 && schermata == 0) // tasto shop (HOME)
                schermata = 1;
            else if (x >= 184 && x <= 858 && y >= 185 && y <= 454 && schermata == 0 && Program.piante_ottenute.Count > 0) // tasto seleziona piante (HOME)
                schermata = 2;
        }

        static public void Disegna()
        {
            if (schermata == 0)
            {
                HOME.Scale = new Vector2f((float)1045 / (float)HOME.Texture.Size.X, (float)600 / (float)HOME.Texture.Size.Y);
                Finestra.Draw(HOME);
                // TASTO PLAY
                {
                    RectangleShape rect_play = new RectangleShape(new Vector2f(120, 60));
                    rect_play.FillColor = new Color(100, 100, 100, 220);
                    rect_play.Position = new Vector2f(912, 525);
                    Finestra.Draw(rect_play);

                    gioca = new Text("PLAY", font, 20);
                    gioca.FillColor = new Color(255, 255, 255);
                    gioca.Position = new Vector2f(929, 544);
                    Finestra.Draw(gioca);
                }
                // TASTO SHOP
                {
                    RectangleShape rect_shop = new RectangleShape(new Vector2f(120, 60));
                    rect_shop.FillColor = new Color(100, 100, 100, 220);
                    rect_shop.Position = new Vector2f(14, 525);
                    Finestra.Draw(rect_shop);

                    shop = new Text("SHOP", font, 20);
                    shop.FillColor = new Color(255, 255, 255);
                    shop.FillColor = new Color(255, 255, 255);
                    shop.Position = new Vector2f(29, 544);
                    Finestra.Draw(shop);
                }
                // SELEZIONA PIANTE
                {
                    // Rettangolo nero (cornice)
                    {
                        RectangleShape rect = new RectangleShape(new Vector2f(690, 285));
                        rect.FillColor = new Color(0, 0, 0, 170);
                        rect.Origin = new Vector2f(rect.Size.X / 2, rect.Size.Y / 2);
                        rect.Position = new Vector2f(Finestra.Size.X / 2, (Finestra.Size.Y / 2) + 20);
                        Finestra.Draw(rect);
                    }
                    // Rettangolo verde
                    {
                        RectangleShape rect = new RectangleShape(new Vector2f(675, 270));
                        rect.FillColor = new Color(0, 207, 45, 210);
                        rect.Origin = new Vector2f(rect.Size.X / 2, rect.Size.Y / 2);
                        rect.Position = new Vector2f(Finestra.Size.X / 2, (Finestra.Size.Y / 2) + 20);
                        Finestra.Draw(rect);
                    }

                    seleziona_piante = new Text("PIANTE SELEZIONATE", font, 20);
                    seleziona_piante.FillColor = new Color(255, 255, 255);
                    seleziona_piante.Position = new Vector2f(336, 215);
                    Finestra.Draw(seleziona_piante);

                    // piante selezionate
                    {
                        Vector2f scala = new Vector2f(0.4f, 0.4f);
                        float larghezza = 340 * scala.X;
                        float altezza = 170 * scala.Y;

                        for (int y = 0; y < 2; y++)
                            for (int x = 0; x < 4; x++)
                                if (piante[4 * y + x] != null)
                                    piante[4 * y + x].DisegnaLista(new Vector2f(203 + (637 - larghezza) / 3 * x,
                                                                                275 + (161 - altezza) * y), scala);
                    }
                }
            }
        }
    }
}