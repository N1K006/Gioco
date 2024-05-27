using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using SFML.Graphics;
using SFML.System;

namespace Plants_Vs_Zombies
{
    abstract class Zombie
    {
        public static Gioco gioco;

        public object LockVel = new object(), LockVita = new object();
        public static IntRect rect;
        public static Texture texture;
        public Sprite sprite = new Sprite();

        public int fila;
        protected readonly float Vel = 30;
        public List<Rallentamento> rallentamenti = new List<Rallentamento>();

        protected int vita;
        public int danno;

        int x;
        public int X
        {
            get => x;
            set
            {
                if (value != x)
                {
                    if (value > 10)
                        value = 10;
                    lock (gioco.LockZombie)
                    {
                        gioco.Mappa_zombie[x, fila].Remove(this);
                        gioco.Mappa_zombie[value, fila].Add(this);
                    }
                    x = value;
                }
            }
        }
        virtual public int Vita 
        {
            get => vita;
            set
            {
                if (value <= 0)
                {
                    lock (gioco.LockZombie)
                        gioco.Mappa_zombie[X, fila].Remove(this);
                    for (int i = 0; i < 20; i++)
                        Logger.Write(X.ToString(), 6);
                    Logger.WriteLine("", 6);
                    GC.Collect();
                }
            }
        }
        
        public Zombie(int y, float vel)
        {
            Vel = vel;

            int Y = 80 + 100 * y;
            sprite.Position = new Vector2f(1093, Y);

            x = 10;
            gioco.Mappa_zombie[10, fila].Add(this);
        }
        public Zombie() { }
        abstract public float Probabilita { get; set; }
        abstract public Zombie GetInstance();
        abstract public Zombie GetInstance(int y);
        abstract public void Stop();
        abstract public void Start();
    }
}