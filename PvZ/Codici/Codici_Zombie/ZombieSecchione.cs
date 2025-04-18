﻿using System;
using SFML.Graphics;
using SFML.System;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class ZombieSecchione : Zombie
    {
        Pianta p;
        Timer mangia = new Timer(750);

        Timer Mov_Zombie = new Timer(100);

        static private float prob;

        override public float Probabilita
        {
            get => prob;
            set => prob =
                value <= 100 && value >= 0 ? //condizione
                value : //se true
                prob; //se false
        }

        public ZombieSecchione(int y) : base(y, 1)
        {
            lock (LockVita)
            {
                lock (gioco.LockZombie)
                {
                    gioco.Mappa_zombie[y].Add(this);
                }

                fila = y;
                vita = 700;
            }
            danno = 20;

            rect = new IntRect(578, 716, 422, 890);
            texture = new Texture(@"..\..\..\Immagini\Zombie\Zombie_secchione\Zombie_secchione.png", rect);
            sprite.Texture = texture;
            sprite.Position = new Vector2f(sprite.Position.X, sprite.Position.Y - 23);
            sprite.Scale = new Vector2f(-0.120f, 0.121f);
            {
                Mov_Zombie.Elapsed += Mov_Zombie_Elapsed;
                Mov_Zombie.Enabled = true;

                mangia.Elapsed += mangia_Elapsed;
                mangia.Enabled = true;
            } //Timer
        }

        public ZombieSecchione(float prob)
        {
            Probabilita = prob;
        }

        public ZombieSecchione() { }

        public override int Vita
        {
            get => vita;
            set
            {
                lock (LockVita)
                {
                    vita = value;
                    if (value <= 0)
                    {
                        if (Program.fase != 0)
                            lock (gioco.LockZombie)
                            {
                                Moneta m = new Moneta(sprite.Position, new Random().Next(11, 16));
                            }
                        mangia.Stop();
                        mangia.Close();
                        Mov_Zombie.Stop();
                        Mov_Zombie.Close();
                        base.Vita = vita;
                    }
                }
            }
        }

        private void Mov_Zombie_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (sprite.Position.X >= 20)
            {
                lock (LockVel)
                {
                    sprite.Position -= new Vector2f(rallentamenti.Count > 0 ? Vel / 100 * (100 - RallMax()) : Vel, 0);
                }
            }
            else
            {
                lock (gioco.LockZombie)
                {
                    gioco.Mappa_zombie[fila].Remove(this);
                }
                ((Timer)sender).Stop();
                gioco.perso = true;
                gioco.Reset();
            }
            int RallMax()
            {
                int r;
                lock (LockVel)
                {
                    r = rallentamenti[0].ValRall;
                    for (int i = 0; i < rallentamenti.Count; i++)
                        if (rallentamenti[i].ValRall > r)
                            r = rallentamenti[i].ValRall;
                }
                return r;
            }
        }
        private void mangia_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Mangia())
            {
                Mov_Zombie.Stop();
                lock (p.LockVita)
                {
                    if (p.Vita - danno <= 0)
                        Mov_Zombie.Enabled = true;
                    p.Vita -= danno;
                }
            }
        }
        private bool Mangia()
        {
            bool mangia = false;
            for (int i = 8; i >= 0; i--)
            {
                Pianta p = gioco.Mappa_piante[i, fila];
                if (p == null)
                    continue;
                else if (p is Rovo)
                    continue;

                if (sprite.Position.X > p.pianta.Position.X && sprite.Position.X < p.pianta.Position.X + 90)
                {
                    this.p = p;
                    mangia = true;
                    break;
                }
            }
            return mangia;
        }

        public override ZombieSecchione GetInstance()
        {
            return new ZombieSecchione();
        }

        public override ZombieSecchione GetInstance(int y)
        {
            return new ZombieSecchione(y);
        }
    }
}