using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Audio;
using System.Timers;
using System.Collections.Generic;

namespace Plants_Vs_Zombies
{
    static class Impostazioni
    {
        static public RenderWindow Finestra;

        public static void Disegna()
        {
            Home.INDIETRO.Position = new Vector2f(14, 14);
            Home.INDIETRO.Scale = new Vector2f(0.433f, 0.433f);
            Finestra.Draw(Home.HOME);
            Finestra.Draw(Home.INDIETRO);
        }

        public static void impostazioni()
        {
            Finestra.SetVerticalSyncEnabled(true);
            Finestra.Closed += (sender, args) => Finestra.Close();

            Finestra.MouseButtonPressed -= Home.MouseClick;
            Finestra.MouseButtonPressed += MouseClick;

            while (Finestra.IsOpen && Home.schermata == 1)
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
        }
    }
}