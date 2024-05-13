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
    static class Paletta
    {
        public static Gioco gioco;

        public static IntRect rect = new IntRect(152, 7, 658, 707);
        public static Texture text = new Texture(@"..\..\..\Immagini\Mappa\Paletta.png", rect);
        public static readonly Sprite sprite = new Sprite(text)
        {
            Position = new Vector2f(351, 35),
            Scale = new Vector2f(0.07f, 0.07f),
            Origin = new Vector2f(329, 353)
        };

        public static bool presa = false;

        public static void pos()
        {
            sprite.Position = new Vector2f(351, 35);
        }

        public static void Rimuovi(int x, int y)
        {
            if (gioco.Mappa_piante[x, y] != null)
                gioco.Mappa_piante[x, y].Vita = -999;
            presa = false;
            pos();
        }
    }
}