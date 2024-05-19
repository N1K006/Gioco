using System;
using SFML.Graphics;
using SFML.System;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class ZombieOrdinario : Zombie
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

        public ZombieOrdinario(int y) : base(y, 1)
        {
            lock (LockVita)
            {
                lock (gioco.LockZombie)
                {
                    gioco.Mappa_zombie[y].Add(this);
                }

                fila = y;
                vita = 60;
            }
            danno = 20;

            rect = new IntRect(30, 15, 422, 658);
            texture = new Texture(@"..\..\..\Immagini\Zombie\Zombie_ordinario\Zombie_ordinario.png", rect);
            sprite.Texture = texture;
            sprite.Scale = new Vector2f(-0.15f, 0.15f);
            {
                Mov_Zombie.Elapsed += Mov_Zombie_Elapsed;
                Mov_Zombie.Enabled = true;

                mangia.Elapsed += mangia_Elapsed;
                mangia.Enabled = true;
            } //Timer
        }

        public ZombieOrdinario(float prob)
        {
            Probabilita = prob;
        }

        public ZombieOrdinario() { }

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
                                int val = new Random().Next(0, 6);
                                if (val > 0)
                                {
                                    Moneta moneta = new Moneta(sprite.Position, val);
                                }
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
                    if(p.Vita - danno <= 0)
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

                Logger.WriteLine((sprite.Position.X).ToString(), 6);
                Logger.WriteLine((sprite.Position.X < p.pianta.Position.X + p.pianta.Texture.Size.X * Math.Abs(p.pianta.Scale.X)).ToString(), 6);

                if (sprite.Position.X > p.pianta.Position.X && sprite.Position.X < p.pianta.Position.X + 90)
                {
                    this.p = p; 
                    mangia = true; 
                    break;
                }
            }
            return mangia;
        }

        public override ZombieOrdinario GetInstance()
        {
            return new ZombieOrdinario();
        }

        public override ZombieOrdinario GetInstance(int y)
        {
            return new ZombieOrdinario(y);
        }
    }
}