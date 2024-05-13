﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Timers;
using System.Reflection.Emit;

namespace Plants_Vs_Zombies
{
    class ZombieSegnaletico : Zombie
    {
        Pianta p;
        Timer mangia = new Timer(750);

        static private float prob;

        override public float Probabilita
        {
            get => prob;
            set => prob =
                value <= 100 && value >= 0 ? //condizione
                value : //se true
                throw new ArgumentException("è possibile assegnare solo valori compresi tra 0 e 100"); //se false
        }

        public ZombieSegnaletico(int y) : base(y, 1)
        {
            lock (LockVita)
            {
                lock (gioco.LockZombie)
                {
                    gioco.Mappa_zombie[y].Add(this);
                }

                fila = y;
                vita = 120;
            }
            danno = 10;

            rect = new IntRect(0, 3, 653, 1280);
            texture = new Texture(@"..\..\..\Immagini\Zombie\Zombie_segnaletico\Zombie_segnaletico.png", rect);
            sprite.Texture = texture;
            sprite.Position = new Vector2f(sprite.Position.X, sprite.Position.Y - 23);
            sprite.Scale = new Vector2f(-0.085f, 0.085f);
            {
                Timer Mov_Zombie = new Timer(100);
                Mov_Zombie.Elapsed += Mov_Zombie_Elapsed;
                Mov_Zombie.Enabled = true;

                mangia.Elapsed += mangia_Elapsed;
                mangia.Enabled = true;
            } //Timer
        }
        public ZombieSegnaletico(float prob)
        {
            Probabilita = prob;
        }
        public ZombieSegnaletico() { }

        public override int Vita
        {
            get => vita;
            set
            {
                lock (LockVita)
                {
                    vita = value;
                    if (value <= 0)
                        lock (gioco.LockZombie)
                        {
                            if (new Random().Next(0, 100) <= (1 / 3))
                            {
                                Moneta m = new Moneta(sprite.Position, new Random().Next(5, 11));
                                base.Vita = vita;
                            }
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
                lock (p.LockVita)
                    p.Vita -= danno;
        }
        private bool Mangia()
        {
            bool mangia = false;
            for (int i = 8; i >= 0; i--)
            {
                Pianta p = gioco.Mappa_piante[i, fila];
                if (p == null)
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

        public override ZombieSegnaletico GetInstance()
        {
            return new ZombieSegnaletico();
        }

        public override ZombieSegnaletico GetInstance(int y)
        {
            return new ZombieSegnaletico(y);
        }
    }
}