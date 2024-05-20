using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;

namespace Plants_Vs_Zombies
{
    static class Program
    {
        public const int LARGHEZZA = 1045;
        public const int ALTEZZA = 600;

        static VideoMode Schermo = new VideoMode(LARGHEZZA, ALTEZZA);
        static RenderWindow Finestra = new RenderWindow(Schermo, "Plants VS Zombies");

        public static Pianta[] all = { new Sparasemi(),
                                          new Supersparasemi(),
                                          new Triplasparasemi(),
                                          new Sparabrina(),
                                          new Girasole(),
                                          new GirasoleGemello(),
                                          new Rovo(),
                                          new MuroNoce(),
                                          new InfiNoce(),
                                          new CiliegeEsplosive(),
                                          new Peperoncino(),
                                          new KiwiBestiale() };

        public static List<Pianta> piante_ottenute = new();

        public static Pianta[] piante = new Pianta[8];

        public static int monete = 0;

        public static int fase = 0;

        static void Main()
        {
            Logger.Grade = Logger.Grades.Trace;

            Home.Finestra = Finestra;
            Shop.Finestra = Finestra;
            Piante.Finestra = Finestra;
            Gioco.Finestra = Finestra;

            Finestra.SetVerticalSyncEnabled(true);
            Finestra.Closed += (sender, args) => Finestra.Close();

            Salvataggio.Carica();

            if (!piante_ottenute.Contains(all[0])) // aggiunge la sparasemi se nen ce l'ha
                piante_ottenute.Add(all[0]);
            if (!piante_ottenute.Contains(all[4])) // aggiunge il girasole se nen ce l'ha
                piante_ottenute.Add(all[4]);


            while (Finestra.IsOpen)
            {
                Finestra.Clear();
                switch (fase)
                {
                    case 0:
                        Home.home(ref piante);
                        Finestra.MouseButtonPressed -= Home.MouseClick;
                        Salvataggio.Salva();
                        break;
                    case 1:
                        Gioco.Start(piante);
                        Salvataggio.Salva();
                        break;
                }
                Finestra.DispatchEvents();
                Finestra.Display();
            }
        }
    }
}