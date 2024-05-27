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
        protected Zombie ZombieColpito()
        {
            Zombie Z = null;

            if (Program.fase != 0)
                lock (gioco.LockZombie)
                    if (fila != 5)
                        for (int i = 0; i < 11; i++)
                            foreach (Zombie z in gioco.Mappa_zombie[i, fila])
                                if (Z == null && Tocca(z))
                                    if (z != null)
                                        Z = z;
                                    else if (Z != null)
                                        if (Z.sprite.Position.X > z.sprite.Position.X && Tocca(z))
                                            if (z != null)
                                                Z = z;
            return Z;

            bool Tocca(Zombie z)
            {
                if (z != null && z.sprite.Texture != null)
                    if (circle.Position.X - 5 < z.sprite.Position.X + (z.sprite.Texture.Size.X * Math.Abs(z.sprite.Scale.X) - 59) &&
                    circle.Position.X + 5 > z.sprite.Position.X - 50)
                        return true;
                return false;
            }
        }

    }
}