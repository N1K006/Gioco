﻿using System;
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
    class SemeInfuocato : Seme
    {
        public Timer Mov_Seme = new Timer(5);

        public SemeInfuocato(Vector2f p, int danno)
        {
            this.danno = danno;
            velocita = 4;

            circle = new CircleShape(10, 10)
            {
                FillColor = Color.Red,
                Origin = new Vector2f(5, 5),
                Position = new Vector2f(314 + p.X * 81, 122 + p.Y * 100)
            };

            fila = (int)p.Y;

            lock (gioco.LockSemi)
                semi.Add(this);
            Mov_Seme.Elapsed += Mov_Seme_Elapsed;
            Mov_Seme.Enabled = true;
        }

        protected override void Mov_Seme_Elapsed(object sender, ElapsedEventArgs e)
        {
            circle.Position = new Vector2f(circle.Position.X + velocita, circle.Position.Y);
            if (circle.Position.X >= 1060)
            {
                lock (gioco.LockSemi)
                    semi.Remove(this);
                return;
            }
            Zombie z = ZombieColpito();
            if (z != null)
            {
                lock (z.LockVita)
                    z.Vita -= danno;
                lock (gioco.LockSemi)
                    semi.Remove(this);
                Mov_Seme = null;
                fila = 5;
            }
        }

        public override void Stop()
        {
            Mov_Seme.Stop();
            Mov_Seme.Close();
            semi.Remove(this);
        }
    }
}