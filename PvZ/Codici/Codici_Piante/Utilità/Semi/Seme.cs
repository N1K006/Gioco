using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Threading;
using System.Timers;

namespace Plants_Vs_Zombies
{
    abstract class Seme
    {
        public bool trasformato = false;
        public static Gioco gioco;
        public static List<Seme> semi = new List<Seme>();
        public CircleShape circle;
        public int danno;
        public int fila;
        public int velocita;

        abstract protected void Mov_Seme_Elapsed(object sender, ElapsedEventArgs e);
        abstract public void Stop();
    }
}